
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // 다음 씬을 비동기 방식으로 로드하고 싶다.
    // 또한 로딩 진행률을 시각적으로 표현하고 싶다.
    // % 프로그레스 바와 %별 텍스트를 이용해서 만든다.
    // 속성:
    // - 다음 씬 번호(인덱스)
    public int NextSceneIndex = 2;
    // 프로그래스 슬라이더 바
    public Slider ProgressSlider;
    // 프로그레스 텍스트
    public  TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
       AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
       ao.allowSceneActivation = false; // 비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

       while (ao.isDone == false)
       {
           // 비동기로 실행할 코드를 입력한다.
           ProgressSlider.value = ao.progress; // => 0~1의 값을 나타낸다.
           ProgressText.text = $"{ao.progress * 100}";
           if (ao.progress <= 0f)
           {
               ProgressText.text = " “어두운 기척이 다가오고 있다...”";
           }
           else if (ao.progress <= 0.2f)
           {
               ProgressText.text = " 탄약을 확인하라. 이 도시는 너를 반기지 않는다.";
           }
           else if (ao.progress <= 0.4f)
           {
               ProgressText.text = "그들은 멈추지 않아. 도망치든 싸우든 선택은 너의 몫.";
           }
           else if (ao.progress <= 0.6f)
           {
               ProgressText.text = "소리 하나에 몰려든다. 조용히 움직여.";
           }
           else if (ao.progress <= 0.8f)
           {
               ProgressText.text = "마지막 생존자는 네가 될 수도 있다… 혹은 그들도 될 수 있다.";
           }

           if (ao.progress >= 0.9f)
           {
               ProgressText.text = "지옥의 문이 열렸다. 이제 네 차례야.";
               ao.allowSceneActivation = true;
           }
           yield return null;
       }
    }
}
