using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI BombCount;
    public GameObject ThrowBar;
    public Slider ThrowPowerSlider;
    public Slider StaminaBar;
    public TextMeshProUGUI BulletCount;
    public Image ReloadBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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