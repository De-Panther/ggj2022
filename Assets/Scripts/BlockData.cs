using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "GGJ Game/BlockData", order = 1)]
public class BlockData : ScriptableObject
{
  public enum ShapeType
  {
    block,
    blockPlusTriangle,
    blockMinusTriangle,
    blockPlusCube,
    blockMinusCube
  }

  [SerializeField]
  private Color color;
  [SerializeField]
  private ShapeType shape;
  [SerializeField]
  private ShapeType inverseShape;
  [SerializeField]
  private Block prefab;

  public Color GetColor()
  {
    return color;
  }

  public Color GetInverseColor()
  {
    return new Color(1f-color.r, 1f-color.g, 1f-color.b);
  }

  public ShapeType GetShape()
  {
    return shape;
  }

  public ShapeType GetInverseShape()
  {
    return inverseShape;
  }
}
