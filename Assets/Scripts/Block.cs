using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
  public static System.Action OnInUseDropped;

  [SerializeField]
  private BlockData blockData;
  [SerializeField]
  private bool isRight;
  [SerializeField]
  private Visual visual;
  [SerializeField]
  private Transform visualTransform;
  [SerializeField]
  private MouseDragObject mouseDragObject;
  [SerializeField]
  private Rigidbody _rigidbody;
  [SerializeField]
  private BoxCollider boxCollider;

  private bool inUse = false;
  private bool isHeld = false;

  void OnEnable()
  {
    mouseDragObject.OnHeld += HandleOnHeld;
  }

  void OnDisable()
  {
    mouseDragObject.OnHeld -= HandleOnHeld;
  }

  public void HandleOnHeld(bool held)
  {
    isHeld = held;
    if (inUse && !isHeld)
    {
      OnInUseDropped?.Invoke();
    }
  }

  public bool InUse()
  {
    return inUse;
  }

  public bool IsHeld()
  {
    return isHeld;
  }

  public void Init(BlockData blockData, bool isRight, bool useFixedPos)
  {
    mouseDragObject.UseFixedPos(useFixedPos);
    this.blockData = blockData;
    this.isRight = isRight;
    if (isRight)
    {
      visual.shapeTransform.localScale = blockData.GetVisualScale();
      visual.shapeRenderer.sharedMaterial = blockData.GetMaterial();
      visual.shapeMeshFilter.sharedMesh = blockData.GetVisual();
    }
    else
    {
      visual.shapeTransform.localScale = blockData.GetInverseVisualScale();
      visual.shapeRenderer.sharedMaterial = blockData.GetInverseMaterial();
      visual.shapeMeshFilter.sharedMesh = blockData.GetInverseVisual();
    }
  }

  public BlockData GetBlockData()
  {
    return blockData;
  }

  public bool IsRight()
  {
    return isRight;
  }

  public void MagnetVisualToSlot(Transform slot)
  {
    inUse = true;
    boxCollider.enabled = false;
    _rigidbody.isKinematic = true;
    transform.SetPositionAndRotation(slot.position, slot.rotation);
    boxCollider.enabled = true;
    visual.shapeRenderer.enabled = false;
  }

  public void ReturnVisual()
  {
    inUse = false;
    boxCollider.enabled = false;
    _rigidbody.isKinematic = false;
    boxCollider.enabled = true;
    visual.shapeRenderer.enabled = true;
  }

  public void Consume()
  {
    Destroy(gameObject);
  }
}
