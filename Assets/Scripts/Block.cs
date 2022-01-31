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

  private bool inUse = false;
  private bool isHeld = false;
  private Transform usedSlot;

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
      if (usedSlot != null)
      {
        transform.SetPositionAndRotation(usedSlot.position, usedSlot.rotation);
      }
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
    usedSlot = slot;
    inUse = true;
    visual.shapeRenderer.enabled = false;
  }

  public void ReturnVisual()
  {
    usedSlot = null;
    inUse = false;
    visual.shapeRenderer.enabled = true;
  }

  public void Consume()
  {
    Destroy(gameObject);
  }
}
