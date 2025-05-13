using UnityEngine;

public class StateMachine
{
    //행동을 할 스크립트 받기
    public EnemyBase CurrentState { get; private set; }

    public void Initialize(EnemyBase startState)
    {
        CurrentState = startState;
        // 행동 시작
        CurrentState.OnStateEnter();
    }
    // 행동 변경
    public void ChangeState(EnemyBase newstate)
    {
        // 행동 하던거 종료
        CurrentState.OnStateExit();
        CurrentState = newstate;
        // 행동 시작
        CurrentState.OnStateEnter();
    }
    // 계속해서 할 동작
    public void Update()
    {
        CurrentState?.OnState();
    }
}
