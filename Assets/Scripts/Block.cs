using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
  [SerializeField]
  private BlockData blockData;

  [SerializeField]
  private Transform visual;

  public BlockData GetBlockData()
  {
    return blockData;
  }

  public void MagnetVisualToSlot(Transform slot)
  {
    visual.SetParent(slot, true);
    visual.localPosition = Vector3.zero;
    visual.localRotation = Quaternion.identity;
    visual.localScale = Vector3.one;
  }

  public void ReturnVisual()
  {
    visual.SetParent(transform, true);
    visual.localPosition = Vector3.zero;
    visual.localRotation = Quaternion.identity;
    visual.localScale = Vector3.one;
  }
}
