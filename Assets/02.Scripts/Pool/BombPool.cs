using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    public List<GameObject> BombPrefab;
    public int BombCount = 3;
    [SerializeField]
    private List<GameObject> _bomb;

    private void Start()
    {
        _bomb = new List<GameObject>(BombPrefab.Count * BombCount);
        foreach (GameObject bomb in BombPrefab)
        {
            for (int i = 0; i < BombCount; i++)
            {
                GameObject bombPrefab = Instantiate(bomb);
                
                bombPrefab.transform.SetParent(this.transform);
                bombPrefab.SetActive(false);
                
                _bomb.Add(bombPrefab);
                
            }
        }
        
    }

    public GameObject MakeBomb(Vector3 position)
    {
        foreach (GameObject bomb in _bomb)
        {
            if (bomb.activeInHierarchy == false)
            {
                bomb.transform.position = position;
                
                bomb.SetActive(true);

                return bomb;
            }

        }
        return null;
    }
}
