using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singletone<UIManager>
{
    public TextMeshProUGUI BombCount;
    public GameObject ThrowBar;
    public Slider ThrowPowerSlider;
    public Slider StaminaBar;
    public TextMeshProUGUI BulletCount;
    public Image ReloadBar;
    
    
    public void StaminaRefresh(float value)
    {
        StaminaBar.value = value;
    }

    public void BombRefresh(int current, int max)
    {
        BombCount.text = $"폭탄 개수 : {current}/{max}";
    }

    public void ThrowPlusPowerRefresh(float value)
    {
        ThrowPowerSlider.value = value;
    }

    public void BulletRefresh(int current, int max)
    {
        BulletCount.text = $"총알 : {current}/{max}";
    }

    public void ReloadBarRefresh(float value, float max, bool on)
    {
        ReloadBar.gameObject.SetActive(on);
        ReloadBar.fillAmount = value/max;
    }
}