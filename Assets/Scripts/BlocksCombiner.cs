using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksCombiner : MonoBehaviour
{
  [SerializeField]
  private Transform slotRight;
  [SerializeField]
  private Transform slotLeft;

  private bool slotRightOpen = true;
  private bool slotLeftOpen = true;

  private Block slotRightBlock;
  private Block slotLeftBlock;

  private BlockData slotRightBlockData;
  private BlockData slotLeftBlockData;

  private void OnTriggerEnter(Collider other)
  {
    Block block = other.GetComponent<Block>();
    if (block != null)
    {
      if (slotRightOpen)
      {
        slotRightBlock = block;
        slotRightBlockData = block.GetBlockData();
        block.MagnetVisualToSlot(slotRight);
        slotRightOpen = false;
      }
      else if (slotLeftOpen)
      {
        slotLeftBlock = block;
        slotLeftBlockData = block.GetBlockData();
        block.MagnetVisualToSlot(slotLeft);
        slotLeftOpen = false;
      }
    }
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
        slotRightOpen = true;
      }
      else if (slotLeftBlock == block)
      {
        slotLeftBlock = null;
        slotLeftBlockData = null;
        block.ReturnVisual();
        slotLeftOpen = true;
      }
    }
  }
}
