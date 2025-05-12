using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider HealthBar;
    private float _creaseTime = 0.3f;
    // 빌보드 
    
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    // 체력 셋팅
    public void SetHealth(int health)
    {
     HealthBar.maxValue = health;
     HealthBar.value = health;
    }
    
    // 체력바 리프레시
    public void HealthbarRefresh(int health)
    {
        StartCoroutine(HealthbarRefresh_Coroutine(health));
    }
    
    private IEnumerator HealthbarRefresh_Coroutine(float value)
    {
        float elapsed = 0f;
        float duration = _creaseTime;
        float currentHealth = HealthBar.value;  
 
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float time = elapsed / duration;
            HealthBar.value = Mathf.Lerp(currentHealth, value, time);        
            yield return null;
        }
         
        HealthBar.value = value;
    }
}
