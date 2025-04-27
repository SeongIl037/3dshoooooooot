using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singletone<GameManager>
{
    public enum GameState
    {
        Ready,
        Run,
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
    
}
