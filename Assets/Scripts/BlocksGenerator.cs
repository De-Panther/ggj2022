using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksGenerator : MonoBehaviour
{
  public enum GeneratorStates
  {
    off,
    pause,
    blitz,
    zen
  }

  [SerializeField]
  private Transform portalRight;
  [SerializeField]
  private Transform portalLeft;
  [SerializeField]
  private float generationRate = 1f;
  [SerializeField]
  private int maxPairsOnTable = 10;
  [SerializeField]
  private BlockData[] blocksData;
  [SerializeField]
  private Block baseBlock;

  private GeneratorStates state = GeneratorStates.off;

  private float nextGenerate = 0f;
  private int pairsGenerated = 0;

  void OnEnable()
  {
    BlocksCombiner.OnCombine += HandleOnCombine;
  }

  void OnDisable()
  {
    BlocksCombiner.OnCombine -= HandleOnCombine;
  }

  void Start()
  {
    StartZen();
  }

  void Update()
  {
    switch (state)
    {
      case GeneratorStates.blitz:
      case GeneratorStates.zen:
        GenerateUpdate();
        break;
    }
  }

  void GenerateUpdate()
  {
    if (pairsGenerated < maxPairsOnTable && Time.time >= nextGenerate)
    {
      nextGenerate = Time.time + generationRate;
      var right = Instantiate(baseBlock, portalRight.position, Quaternion.identity);
      right.Init(blocksData[Random.Range(0, blocksData.Length)], true);
      var left = Instantiate(baseBlock, portalLeft.position, Quaternion.identity);
      left.Init(blocksData[Random.Range(0, blocksData.Length)], false);
      pairsGenerated++;
    }
  }

  public void StartBlitz()
  {
    state = GeneratorStates.blitz;
  }

  public void StartZen()
  {
    state = GeneratorStates.zen;
  }

  void HandleOnCombine()
  {
    pairsGenerated--;
  }
}
