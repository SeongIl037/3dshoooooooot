using System.Drawing;
using UnityEngine;
using DG.Tweening;
public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 10;
    private float _currentSize;
    public float SizeChange = 5f;
    private float _changeTime = 1f;
    private float _sizeMax = 40f;
    
    private void Update()
    {
        Vector3 newPosition = Target.position;
        newPosition.y += YOffset;
        transform.position = newPosition;
        
        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        
        transform.eulerAngles = newEulerAngles;
    }

    public void OthorIncrease()
    {
        _currentSize = this.GetComponent<Camera>().orthographicSize;
        float newSize = _currentSize - SizeChange;
        this.GetComponent<Camera>().DOOrthoSize(Mathf.Max(newSize, SizeChange), _changeTime).SetEase(Ease.OutCirc);
    }

    public void OthorDecrease()
    {
        _currentSize = this.GetComponent<Camera>().orthographicSize;
        float newSize = _currentSize + SizeChange;
        this.GetComponent<Camera>().DOOrthoSize(Mathf.Min(newSize,_sizeMax), _changeTime).SetEase(Ease.OutCirc);
    }
}
