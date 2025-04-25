using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class UIManager : Singletone<UIManager>
{
    public Image HitEffect;
    public Image SubHitEffect;
    private float _subDuration = 0.05f;
    private float _subAlpha = 0.2f;
    private float _hitAlpha = 1;
    private float _hitEffectDuration = 21f;
    public TextMeshProUGUI BombCount;
    public GameObject ThrowBar;
    public Slider ThrowPowerSlider;
    public Slider StaminaBar;
    public TextMeshProUGUI BulletCount;
    public Image ReloadBar;


    public void SliderRefresh(Slider slider, float value)
    {
        slider.value = value;
    }

    public void BombRefresh(int current, int max)
    {
        BombCount.text = $"폭탄 개수 : {current}/{max}";
    }
    public void BulletRefresh(int current, int max)
    {
        BulletCount.text = $"총알 : {current}/{max}";
    }

    public void ReloadBarRefresh(float value, float max, bool on)
    {
        ReloadBar.gameObject.SetActive(on);
        ReloadBar.fillAmount = value / max;
    }

    public void PlayerHit()
    {
        HitEffect.color = new Color(1f, 1f, 1f, 1f);
        SubHitEffect.color = new Color(1f, 0 , 0, _subAlpha);
        _hitAlpha = 1;
        StartCoroutine(DecreaseAlpha_Coroutine());
    }

    private IEnumerator DecreaseAlpha_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < _hitEffectDuration; i++)
        {
            _hitAlpha -= _subDuration;
            HitEffect.color = new Color(1f,0f,0f,_hitAlpha);
            SubHitEffect.color = new Color( 1f, 0f, 0f, _hitAlpha *_subAlpha );
            yield return new WaitForSeconds(_subDuration);
        }

    }

}