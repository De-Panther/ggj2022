using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksCombiner : MonoBehaviour
{
  public static System.Action OnCombine;

  [SerializeField]
  private Transform[] towerBaseRight;
  [SerializeField]
  private Transform[] towerBaseLeft;

  [SerializeField]
  private Transform slotRight;
  [SerializeField]
  private Transform slotLeft;
  [SerializeField]
  private Visual visualPrefab;
  [SerializeField]
  private BoxCollider boxCollider;

  private bool slotRightOpen = true;
  private bool slotLeftOpen = true;

  private Block slotRightBlock;
  private Block slotLeftBlock;

  private BlockData slotRightBlockData;
  private BlockData slotLeftBlockData;

  private int floorsCount = 0;

  private List<Visual> visualsRight = new List<Visual>();
  private List<Visual> visualsLeft = new List<Visual>();

  private Transform _transform;

  Vector3 center;
  Vector3 halfSize;

  void Awake()
  {
    _transform = transform;
  }

  void Start()
  {
    center = _transform.TransformPoint(boxCollider.center);
    halfSize = boxCollider.size * 0.5f;
  }

  void OnEnable()
  {
    Block.OnInUseDropped += HandleOnInUseDropped;
  }

  void OnDisable()
  {
    Block.OnInUseDropped -= HandleOnInUseDropped;
  }

  Visual GetVisualRight(int floor)
  {
    if (visualsRight.Count > floor)
    {
      return visualsRight[floor];
    }
    Visual visual = Instantiate(visualPrefab);
    visualsRight.Add(visual);
    return visual;
  }

  Visual GetVisualLeft(int floor)
  {
    if (visualsLeft.Count > floor)
    {
      return visualsLeft[floor];
    }
    Visual visual = Instantiate(visualPrefab);
    visualsLeft.Add(visual);
    return visual;
  }

  void HandleOnInUseDropped()
  {
    TryToCombine();
  }

  void TryToCombine()
  {
    if (slotRightBlock != null && slotLeftBlock != null
      && slotRightBlock.InUse() && !slotRightBlock.IsHeld()
      && slotLeftBlock.InUse() && !slotLeftBlock.IsHeld())
    {
      var visualRight = GetVisualRight(floorsCount);
      var visualLeft = GetVisualLeft(floorsCount);
      if (slotRightBlockData.IsColorInverse(slotLeftBlockData)
          ||slotRightBlockData.IsShapeInverse(slotLeftBlockData))
      {
        int towerBase = floorsCount%3;
        visualRight.transform.SetPositionAndRotation(towerBaseRight[towerBase].position + new Vector3(0, 0.05f * floorsCount, 0), towerBaseRight[towerBase].rotation);
        visualLeft.transform.SetPositionAndRotation(towerBaseLeft[towerBase].position + new Vector3(0, 0.05f * floorsCount, 0), towerBaseLeft[towerBase].rotation);
        visualRight.shapeRenderer.enabled = true;
        visualLeft.shapeRenderer.enabled = true;
        floorsCount = Mathf.Max(0, floorsCount + 1);
      }
      else
      {
        visualRight.shapeRenderer.enabled = false;
        visualLeft.shapeRenderer.enabled = false;
        //floorsCount = Mathf.Max(0, floorsCount - 1);
      }

      slotRightBlock.Consume();
      slotLeftBlock.Consume();

      slotRightOpen = true;
      slotLeftOpen = true;
      slotRightBlock = null;
      slotLeftBlock = null;
      slotRightBlockData = null;
      slotLeftBlockData = null;

      OnCombine?.Invoke();
      StartCoroutine(WaitCheckStay());
    }
  }

  IEnumerator WaitCheckStay()
  {
    yield return null;
    CheckStay();
  }

  private void CheckStay()
  {
    var colliders = Physics.OverlapBox(center, halfSize, _transform.rotation, 1);
    for (int i = 0; i < colliders.Length; i++)
    {
      if (colliders[i]?.gameObject != null)
      {
        OnTriggerEnter(colliders[i]);
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    Block block = other.GetComponent<Block>();
    if (block != null)
    {
      if (slotRightOpen && block.IsRight())
      {
        slotRightBlock = block;
        slotRightBlockData = block.GetBlockData();
        block.MagnetVisualToSlot(slotRight);
        var visual = GetVisualRight(floorsCount);
        visual.transform.SetPositionAndRotation(slotRight.position, slotRight.rotation);
        visual.shapeTransform.localScale = slotRightBlockData.GetVisualScale();
        visual.shapeRenderer.sharedMaterial = slotRightBlockData.GetMaterial();
        visual.shapeMeshFilter.sharedMesh = slotRightBlockData.GetVisual();
        visual.shapeRenderer.enabled = true;
        slotRightOpen = false;
      }
      else if (slotLeftOpen && !block.IsRight())
      {
        slotLeftBlock = block;
        slotLeftBlockData = block.GetBlockData();
        block.MagnetVisualToSlot(slotLeft);
        var visual = GetVisualLeft(floorsCount);
        visual.transform.SetPositionAndRotation(slotLeft.position, slotLeft.rotation);
        visual.shapeTransform.localScale = slotLeftBlockData.GetInverseVisualScale();
        visual.shapeRenderer.sharedMaterial = slotLeftBlockData.GetInverseMaterial();
        visual.shapeMeshFilter.sharedMesh = slotLeftBlockData.GetInverseVisual();
        visual.shapeRenderer.enabled = true;
        slotLeftOpen = false;
      }
    }
    TryToCombine();
  }

  private void OnTriggerExit(Collider other)
  {
    Block block = other.GetComponent<Block>();
    if (block != null)
    {
      if (slotRightBlock == block)
      {
        slotRightBlock = null;
        slotRightBlockData = null;
        block.ReturnVisual();
        var visual = GetVisualRight(floorsCount);
        visual.shapeRenderer.enabled = false;
        slotRightOpen = true;
      }
      else if (slotLeftBlock == block)
      {
        slotLeftBlock = null;
        slotLeftBlockData = null;
        block.ReturnVisual();
        var visual = GetVisualLeft(floorsCount);
        visual.shapeRenderer.enabled = false;
        slotLeftOpen = true;
      }
    }
    CheckStay();
  }
}
