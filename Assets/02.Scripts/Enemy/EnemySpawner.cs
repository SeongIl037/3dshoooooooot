using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool pool;
    
    private float _spawnTime = 5;
    private float _spawnTimer = 0;
    
    private Vector3 _spawnPosition;
    
    
    
    private void Spawn()
    {
        if (_spawnTimer >= _spawnTime)
        {
            pool.MakeEnemy(_spawnPosition);
        }
    }
}
