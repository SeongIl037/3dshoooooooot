using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPool : MonoBehaviour
{
    public List<GameObject> EnemyPrefab;
    public int EnemyCount = 3;
    [FormerlySerializedAs("_Enemy")] [SerializeField]
    private List<GameObject> _enemy;

    private void Start()
    {
        _enemy = new List<GameObject>(EnemyPrefab.Count * EnemyCount);
        foreach (GameObject bomb in EnemyPrefab)
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                GameObject enemyPrefab = Instantiate(bomb);
                
                enemyPrefab.transform.SetParent(this.transform);
                enemyPrefab.SetActive(false);
                
                _enemy.Add(enemyPrefab);
                
            }
        }
        
    }

    public GameObject MakeEnemy(Vector3 position)
    {
        foreach (GameObject enemy in _enemy)
        {
            if (enemy.activeInHierarchy == false)
            {
                enemy.transform.position = position;
                
                enemy.SetActive(true);

                return enemy;
            }

        }
        return null;
    }
}
