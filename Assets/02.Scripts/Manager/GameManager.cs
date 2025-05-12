using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singletone<GameManager>
{
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        Over
        
    }
    private bool _gameStart = false;
    private float _gameOverDuration = 20f;
    private GameState _gameState = GameState.Ready;
    
    private void Start()
    {
        ReadyGame();
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.Ready:
                ReadyGame();
                break;
            case GameState.Run:
                RunGame();
                break;
            case GameState.Over:
                OverGame();
                break;
        }
    }

    public void ReadyGame()
    {
        if (_gameStart)
        {
            return;
        }
        _gameState = GameState.Ready;
        Time.timeScale = 0;          
        _gameStart = true;
        Debug.Log("Ready");
        UIManager.instance.ReadyGame();
    }
    public void RunGame()
    {
        _gameState = GameState.Run;
        Time.timeScale = 1;

    }

    public void OverGame()
    {
        if (!_gameStart)
        {
            return;
        }
        _gameStart = false;
        StartCoroutine(OverGame_Coroutine());
        _gameState = GameState.Over;
    }

    private IEnumerator OverGame_Coroutine()
    {
        for (int i = 1; i < _gameOverDuration; i++)
        {
            Time.timeScale -=  1/ _gameOverDuration;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
        UIManager.instance.OverGame();
        Time.timeScale = 0;
    }

    public void Pause()
    {
        PopupManager.instance.Open(EPopupType.UI_OptionPopup,closeCallback: Continue);
        _gameState = GameState.Pause;
        Cursor.lockState = CursorLockMode.None;
        //옵션 팝업을 활성화한다.
        Time.timeScale = 0;
    }
    public void Continue()
    {
        _gameState = GameState.Run;
        Cursor.lockState = CursorLockMode.Locked;
        //옵션 팝업을 활성화한다.
        Time.timeScale = 1;
        
    }

    public void Restart()
    {
        _gameState = GameState.Run;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        // 보고 있는 씬의 번호를 가져오는 방법
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        // 다시 시작을 했더니 게임이 망가지는 경우가 있다
        // 싱글톤일 경우
    }
    
}
