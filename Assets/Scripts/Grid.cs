using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject _tilePrefab;

    public static Grid instance;

    public static int _gridSize_x;
    public static int _gridSize_y;
    public static int _numOfMines;

    public static TileBlock[][] _tiles;

    static List<TileBlock> _mines;

    public static int _leftTiles;
    [SerializeField] int leftTiles;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        leftTiles = _leftTiles;
    }

    public void Init()
    {
        Malloc();
        MakeGridTiles();
        DeployMines();
        CountMinesNearby();
    }

    void Malloc()
    {
        _tiles = new TileBlock[_gridSize_x][];
        _mines = new List<TileBlock>();
        for (int i = 0; i < _gridSize_x; i++)
            _tiles[i] = new TileBlock[_gridSize_y];
    }

    void MakeGridTiles()
    {
        for(int i=0; i < _gridSize_x; i++)
            for(int j=0; j < _gridSize_y; j++)
            {
                Vector3 pos = new Vector3(i,0,j);
                GameObject go = Instantiate(_tilePrefab, pos, Quaternion.Euler(0, 180, 0), this.transform);
                _tiles[i][j] = go.GetComponent<TileBlock>();
                _tiles[i][j].pos_x = i;
                _tiles[i][j].pos_y = j;
            }
        _leftTiles = _gridSize_x * _gridSize_y;
    }

    void DeployMines()
    {
        for(int i=0; i<_numOfMines; i++)
        {
            int rand = Random.Range(0, _gridSize_x * _gridSize_y);
            var targetBlock = _tiles[rand / _gridSize_x][rand % _gridSize_y].GetComponent<TileBlock>();
            if(targetBlock.isMine)
            {
                i--;
                continue;
            }    
            else
            {
                _tiles[rand / _gridSize_x][rand % _gridSize_y].GetComponent<TileBlock>().SetTileAsMineTile();
                _mines.Add(_tiles[rand / _gridSize_x][rand % _gridSize_y]);
            }
        }
    }

    void CountMinesNearby()
    {
        for(int i=0; i<_gridSize_x; i++)
            for(int j=0; j<_gridSize_y; j++)
            {
                if (_tiles[i][j].isMine == true)
                    continue;
                int mineCount = 0;
                for(int x = Mathf.Max(0, i-1); x<= Mathf.Min(_gridSize_x-1, i+1); x++)
                    for (int y = Mathf.Max(0, j - 1); y <= Mathf.Min(_gridSize_y-1, j + 1); y++)
                    {
                        if (_tiles[x][y].isMine == true)
                            mineCount++;
                    }
                _tiles[i][j].SetNearbyMineCount(mineCount);
            }
    }

    public void CheckGameClear()
    {
        if(_leftTiles == _numOfMines)
            GameManager.instance.GameClear();
    }
}
