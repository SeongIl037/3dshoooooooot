using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class UIManager : Singletone<UIManager>
{
    public Slider PlayerHealthSlider;
    public Slider ThrowPowerSlider;
    public Slider StaminaBar;

    // 피격 이펙트 관련
    private float _subDuration = 0.05f;
    private float _subAlpha = 0.2f;
    private float _hitAlpha = 1;
    private float _hitEffectDuration = 21f;
    
    // 감소 시간 조절
    private float _creaseTime = 1f;
    
    public TextMeshProUGUI BombCount;
    public TextMeshProUGUI BulletCount;

    public GameObject ThrowBar;
    
    public Image HitEffect;
    public Image SubHitEffect;
    public Image ReloadBar;

    // 게임 시작 중간 끝
    public TextMeshProUGUI CurrentGameState;
    private float _stateTimer = 0.1f;
    
    
    // 슬라이더 값 조절 공통 메서드
    public void SliderRefresh(Slider slider, float value)
    {
        slider.value = value;
    }
    
    public void HealthSliderRefresh(Slider slider, float value)
    {
        StartCoroutine(SliderRefresh_Coroutine(slider, value));
    }

    private IEnumerator SliderRefresh_Coroutine(Slider slider, float value)
    {
        float elapsed = 0f;
        float duration = _creaseTime;
        float currentValue = slider.value;                             
       
         while (elapsed < duration)
         {
             elapsed += Time.deltaTime;
             float time = elapsed / duration;
             slider.value = Mathf.Lerp(currentValue, value, time);        
             yield return null;
         }
         
         slider.value = value;
    }
    // 폭탄 개수 조절 메서드
    public void BombRefresh(int current, int max)
    {
        BombCount.text = $"폭탄 개수 : {current}/{max}";
    }
    
    // 폭탄 리프레시
    public void BulletRefresh(int current, int max)
    {
        BulletCount.text = $"총알 : {current}/{max}";
    }
    // 재장전 UI
    public void ReloadBarRefresh(float value, float max, bool on)
    {
        ReloadBar.gameObject.SetActive(on);
        ReloadBar.fillAmount = value / max;
    }
    // 플레이어 피격 이펙트
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
    
    // 게임 시작 중간 끝 설정
    public void ReadyGame()
    {
        StartCoroutine(ReadyGame_Coroutine());
    }

    private IEnumerator ReadyGame_Coroutine()
    {
        yield return new WaitForSecondsRealtime(_stateTimer);
        CurrentGameState.text = "Ready.";
        yield return new WaitForSecondsRealtime(_stateTimer);
        CurrentGameState.text = "Ready..";
        yield return new WaitForSecondsRealtime(_stateTimer);
        CurrentGameState.text = "Ready...";
        yield return new WaitForSecondsRealtime(_stateTimer);
        CurrentGameState.text = "Go!";
        yield return new WaitForSecondsRealtime(1);
        CurrentGameState.gameObject.SetActive(false);
        GameManager.instance.RunGame();
    }

    public void OverGame()
    {
        CurrentGameState.text = "Game Over!";
        CurrentGameState.gameObject.SetActive(true);
    }

}