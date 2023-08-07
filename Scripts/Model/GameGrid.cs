using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePlayGrid", menuName = "GameGrid")]
public class GameGrid : ScriptableObject
{
    [SerializeField] GameObject[,] _gameGrid;
    [SerializeField] GameObject _gameGridPrefab;
}
