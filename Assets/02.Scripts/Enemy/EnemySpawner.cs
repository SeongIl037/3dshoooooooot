using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool pool;
    
    private float _spawnTime = 2;
    private float _spawnTimer = 0;
    
    private Vector3 _spawnPosition;
    private float _spawnDistance = 10f;
    private bool _isSpawning = false;
    
    private GameObject _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        SpawnCondition();
        
        if (_isSpawning)
        {
            _spawnTimer += Time.deltaTime;
            
            Spawn();
        }
    }

    private void SpawnCondition()
    {
        
        if (Vector3.Distance(transform.position, _player.transform.position) < _spawnDistance)
        {
            _isSpawning = true;
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) > _spawnDistance)
        {
            _isSpawning = false;
        }
    }
    
    private void Spawn()
    {
        if (_spawnTimer >= _spawnTime)
        {
            _spawnPosition = this.transform.position + new Vector3(Random.Range(-2,2), 0, Random.Range(-2,2));
            pool.MakeObject(_spawnPosition);
            _spawnTimer = 0;
        }
    }
}
