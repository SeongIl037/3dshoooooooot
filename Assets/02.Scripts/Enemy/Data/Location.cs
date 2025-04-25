using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "Scriptable Objects/Location")]
public class Location : ScriptableObject
{
    public Vector3[] PatrolLocation;
}
