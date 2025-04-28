using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> ObjectPrefab;
    public int ObjectCount = 3;
    [FormerlySerializedAs("_Enemy")] [SerializeField]
    private List<GameObject> _objects;

    private void Start()
    {
        _objects = new List<GameObject>(ObjectPrefab.Count * ObjectCount);
        foreach (GameObject bomb in ObjectPrefab)
        {
            for (int i = 0; i < ObjectCount; i++)
            {
                GameObject enemyPrefab = Instantiate(bomb);
                
                enemyPrefab.transform.SetParent(this.transform);
                enemyPrefab.SetActive(false);
                
                _objects.Add(enemyPrefab);
                
            }
        }
        
    }

    public GameObject MakeObject(Vector3 position)
    {
        foreach (GameObject obj in _objects)
        {
            if (obj.activeInHierarchy == false)
            {
                obj.transform.position = position;
                
                obj.SetActive(true);

                return obj;
            }

        }
        return null;
    }
}
