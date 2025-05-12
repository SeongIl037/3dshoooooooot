using System.Collections;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    private MaterialPropertyBlock _block;
    private int _colorID;
    public SkinnedMeshRenderer[] ZombieSkinnedMeshRenderers;

    private void Start()
    {
        _block = new MaterialPropertyBlock();
        _colorID = Shader.PropertyToID("_BaseColor");
        ZombieSkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void HitFlash()
    {
        StartCoroutine(HitFlash_Coroutine());
    }
    private IEnumerator HitFlash_Coroutine()
    {
        foreach (SkinnedMeshRenderer skin in ZombieSkinnedMeshRenderers )
        {
            skin.GetPropertyBlock(_block);
            _block.SetColor(_colorID, Color.red);
            skin.SetPropertyBlock(_block);
        }
        yield return new WaitForSeconds(0.2f);
        
        foreach (SkinnedMeshRenderer skin in ZombieSkinnedMeshRenderers )
        {
            skin.GetPropertyBlock(_block);
            _block.SetColor(_colorID, Color.white);
            skin.SetPropertyBlock(_block);
        }
    }

}
