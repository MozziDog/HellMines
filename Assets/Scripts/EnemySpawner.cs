using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;

    public bool _enableEnemySpawnAtAllDirection;

    [Space(10)]

    public int _maxEnemyCount;
    public float _spawnInterval;
    public float _spareDistance;

    private float _lastSpawnTime;

    

    [SerializeField] private List<GameObject> _enemyPool;
    private int _enemyPoolIterator = 0;
    // Start is called before the first frame update
    void Start()
    {
        _lastSpawnTime = Time.time;
        SetPool();
    }

    private void SetPool()
    {
        if(_enemyPool == null)
            _enemyPool = new List<GameObject>();

        for(int i=0; i<_maxEnemyCount; i++)
        {
            var spawned = Instantiate(_enemyPrefab);
            spawned.SetActive(false);
            _enemyPool.Add(spawned);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if( _lastSpawnTime + _spawnInterval < Time.time)
        {
            _lastSpawnTime = Time.time;
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPosition = FindSpawnPosition();
        GameObject nextEnemy = GetNextPoolEntry();
        if(nextEnemy != null)
        {
            nextEnemy.transform.position = spawnPosition;
            nextEnemy.SetActive(true);
            nextEnemy.GetComponent<Enemy>().OnSpawn();
        }
    }

    private GameObject GetNextPoolEntry()
    {
        int startPoint = _enemyPoolIterator;
        while(true)
        {
            _enemyPoolIterator = (_enemyPoolIterator + 1)%_maxEnemyCount;
            if (_enemyPool[_enemyPoolIterator].activeSelf == false)
                return _enemyPool[_enemyPoolIterator];
            if(startPoint == _enemyPoolIterator)
                break;
        }
        return null;
    }

    private Vector3 FindSpawnPosition()
    {
        if(_enableEnemySpawnAtAllDirection)
            switch (Random.Range(0,4))
            {
            case 0:// 合率
                return new Vector3(Random.Range(0, Grid._gridSize_x), 1, Grid._gridSize_y + _spareDistance);
            case 1:// 悼率
                return new Vector3(Grid._gridSize_x + _spareDistance, 1, Random.Range(0, Grid._gridSize_y));
            case 2:// 巢率
                return new Vector3(Random.Range(0, Grid._gridSize_x), 1, - _spareDistance);
            case 3:// 辑率
                return new Vector3(- _spareDistance, 1, Random.Range(0, Grid._gridSize_y));
            default:
                return Vector3.zero;
            }
        else
            return new Vector3(Random.Range(0, Grid._gridSize_x), 1, Grid._gridSize_y + _spareDistance);
    }
}
