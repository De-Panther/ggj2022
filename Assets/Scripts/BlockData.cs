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
    blockMinusCube,
    blockPlusCircle,
    blockMinusCircle
  }

  [SerializeField]
  private Material material;
  [SerializeField]
  private Material inverseMaterial;
  [SerializeField]
  private ShapeType shape;
  [SerializeField]
  private ShapeType inverseShape;
  [SerializeField]
  private Mesh visual;
  [SerializeField]
  private Mesh inverseVisual;
  [SerializeField]
  private Vector3 visualScale;
  [SerializeField]
  private Vector3 inverseVisualScale;

  public Material GetMaterial()
  {
    return material;
  }

  public Material GetInverseMaterial()
  {
    return inverseMaterial;
  }

  public ShapeType GetShape()
  {
    return shape;
  }

  public ShapeType GetInverseShape()
  {
    return inverseShape;
  }

  public Mesh GetVisual()
  {
    return visual;
  }

  public Mesh GetInverseVisual()
  {
    return inverseVisual;
  }

  public Vector3 GetVisualScale()
  {
    return visualScale;
  }

  public Vector3 GetInverseVisualScale()
  {
    return inverseVisualScale;
  }

  public bool IsColorInverse(BlockData blockData)
  {
    return inverseMaterial == blockData.inverseMaterial;
  }

  public bool IsShapeInverse(BlockData blockData)
  {
    return inverseShape == blockData.inverseShape && inverseShape != ShapeType.block;
  }
}
