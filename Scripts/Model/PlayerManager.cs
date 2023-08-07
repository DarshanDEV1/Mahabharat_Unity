using System.Collections.Generic;
using UnityEngine;

public enum MoveState
{
    MyHome,
    MyTown,
    Road,
    EnemyTown,
    EnemyHome
}

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] GameGrid _gameGrid;
    [SerializeField] MoveState _currentMoveState;

    [SerializeField] Stack<BlockCoordinate> _visitedBlockStack = new Stack<BlockCoordinate>();
}

[System.Serializable]
public struct BlockCoordinate
{
    public int _row;
    public int _col;
}
