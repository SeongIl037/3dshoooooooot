using UnityEngine;
using DG.Tweening;
public class AttackRange : MonoBehaviour
{
    private Vector3 startPos;

    private void OnEnable()
    {
        Increase();
    }

    private void Increase()
    {
        transform.DOScale(new Vector3(4f, 4f, 4f), 2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
