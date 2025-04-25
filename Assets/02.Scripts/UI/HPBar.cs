using System;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    
    public Slider HealthBar;

    public void SetHealth(int health)
    {
     HealthBar.maxValue = health;
     HealthBar.value = health;
    }

    private void Update()
    {
     transform.rotation = Camera.main.transform.rotation;
    }

    public void HealthbarRefresh(int health)
    {
        HealthBar.value = health;
    }
}
