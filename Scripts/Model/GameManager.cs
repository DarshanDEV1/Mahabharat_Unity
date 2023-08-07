using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameGrid _gameGrid;
    [SerializeField] Player _playerTurn;

    private void Awake()
    {
        _gameGrid.CreateGrid();
    }
}
