using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    Player_One,
    Player_Two
}

[CreateAssetMenu(fileName = "GamePlayGrid", menuName = "GameGrid")]
public class GameGrid : ScriptableObject
{
    #region OBJECT_REFERENCES

    [SerializeField] GameObject[,] _gameGrid;
    [SerializeField] GameObject[,] _player_One;
    [SerializeField] GameObject[,] _player_Two;
    [SerializeField] GameObject _gameGridPrefab;
    [SerializeField] Transform _gridSpawnPoint;
    [SerializeField] Camera _gameCamera;

    [SerializeField] Material _pathMaterial;
    [SerializeField] Material _areaMaterial;
    [SerializeField] Material _playerMaterial;

    [SerializeField, HideInInspector] int _gridrow;
    [SerializeField, HideInInspector] int _gridcol;
    [SerializeField] float _gridspace;

    #endregion

    #region GRID_CREATION

    public void CreateGrid()
    {
        Vector3 spawnPosition = _gridSpawnPoint.position;
        _gameGrid = new GameObject[_gridrow, _gridcol];
        Transform parent = GameObject.Find("GridPath").transform;

        for (int i = 0; i < _gridrow; i++)
        {
            for (int j = 0; j < _gridcol; j++)
            {
                Vector3 m = new Vector3(i, 0, j);
                var x = Instantiate(_gameGridPrefab, (spawnPosition + m) * _gridspace, Quaternion.identity);
                x.gameObject.GetComponent<Renderer>().material = _pathMaterial;
                x.name = i.ToString() + " , " + j.ToString();
                x.transform.parent = parent;
                _gameGrid[i, j] = x;
            }
        }
        CameraControl();
        _player_One = CreatePlayers(Player.Player_One);
        _player_Two = CreatePlayers(Player.Player_Two);

        DisableUnwantedBlocks(new GameObject[]
        {
            _player_Two[0, 0], _player_Two[0, 1],
            _player_One[0, 1], _player_One[0, 2]
        });
        EnablePlayer_N_AreaMaterials(_player_One, _player_Two);

        parent.position = new Vector3(0, 0, parent.position.z + _gridspace);
    }

    private GameObject[,] CreatePlayers(Player player)
    {
        Vector3 spawnPosition = _gameGrid[Mathf.FloorToInt((this._gridrow) / 2),
                                           player == Player.Player_Two ?
                                           this._gridcol - 4 : 0].transform.position;
        int _gridrow = 4, _gridcol = 3;

        Transform parent = player == Player.Player_One ?
                                        GameObject.Find("Player_One_Grid").transform :
                                        GameObject.Find("Player_Two_Grid").transform;

        GameObject[,] _playerGrid = new GameObject[_gridrow, _gridcol];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 m = new Vector3(i, 0, j);
                var x = Instantiate(_gameGridPrefab, (spawnPosition + m) * _gridspace, Quaternion.identity);
                _playerGrid[i, j] = x;
                x.name = i.ToString() + " , " + j.ToString();
                x.transform.parent = parent;
            }
        }
        return _playerGrid;
    }

    private void DisableUnwantedBlocks(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
    }
    private void EnablePlayer_N_AreaMaterials(GameObject[, ] playerOne, GameObject[, ] playerTwo)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 0 || i == 3)
                {
                    playerOne[i, j].GetComponent<Renderer>().material = _areaMaterial;
                    playerTwo[i, j].GetComponent<Renderer>().material = _areaMaterial;
                }
                else if ((i == 1 || i == 2) && (j == 0 || j == 2))
                {
                    playerOne[i, j].GetComponent<Renderer>().material = _areaMaterial;
                    playerTwo[i, j].GetComponent<Renderer>().material = _areaMaterial;
                }
                else
                {
                    playerOne[i, j].GetComponent<Renderer>().material = _playerMaterial;
                    playerTwo[i, j].GetComponent<Renderer>().material = _playerMaterial;
                }
            }
        }
    }
    
    public Queue<BlockCoordinate> GetWalkinQueue(Player _player)
    {
        return new Queue<BlockCoordinate>();
    }

    #endregion

    #region CAMERA_CONTROL

    private void CameraControl()
    {
        var _myCam = Instantiate(_gameCamera);

        int _row = Mathf.FloorToInt(_gridrow / 2);
        int _col = Mathf.FloorToInt(_gridcol / 2);
        Vector3 _camPosition = new Vector3(
                                            _gameGrid[_row, _col].transform.position.x,
                                            _gameGrid[_row, _col].transform.position.y + (_gridcol * 1.5f),
                                            //_gameGrid[_row, _col].transform.position.z
                                            0
                                            );
        _myCam.transform.position = _camPosition;
        _myCam.transform.LookAt(_gameGrid[_row, _col - 1].transform);
    }

    #endregion
}
