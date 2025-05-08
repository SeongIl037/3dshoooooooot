using UnityEngine;

public class cull1 : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera camera = Camera.main;
            
            camera.cullingMask = 1 << ~LayerMask.NameToLayer("Nose");
        }
    }
}
