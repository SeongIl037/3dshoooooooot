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
        transform.DOScale(new Vector3(7f,7f, 7f), 2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            transform.localScale = new Vector3(0, 0, 0);
        });
    }
}
