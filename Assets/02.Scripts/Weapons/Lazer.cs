using System.Collections;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    
    public LineRenderer LazerLine;
    
    private Vector3 _lazerDirection;
    private float _lazerSpeed = 30f;
    private float _lazerLength;
    // 레이저 발사
    public void LazerShoot(Vector3 startPosition, Vector3 endPosition)
    {
        LazerLine.gameObject.SetActive(true); 
        SetLazerPosition(startPosition, endPosition);
        UpdatePosition();
        
        _lazerLength = Vector3.Distance(startPosition, endPosition);
        _lazerDirection = (endPosition - startPosition).normalized;
        
        StartCoroutine(LazerMove());
        
    }

    private void DecreaseStart()
    {
        _startPosition += _lazerDirection * _lazerSpeed * Time.deltaTime;
        UpdatePosition();
    }
    // 레이저 처음 끝
    public void UpdatePosition()
    {
        LazerLine.SetPosition(0, _startPosition);
        LazerLine.SetPosition(1, _endPosition);
    }

    public void SetLazerPosition(Vector3 start, Vector3 end)
    {
        _startPosition = start;
        _endPosition = end;
    }
    // 레이저 이동
    private IEnumerator LazerMove()
    {
        while (_lazerLength > 0)
        {
            _lazerLength -= Time.deltaTime * _lazerSpeed;
            DecreaseStart();
            yield return null;
        }
        LazerLine.gameObject.SetActive(false);
    }
    
    
}
