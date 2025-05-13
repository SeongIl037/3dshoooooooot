using UnityEngine;

public abstract class EnemyBase
{
    protected StateMachine _stateMachine;

    public EnemyBase (StateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }
    
    public virtual void OnStateEnter()
    {
    }
    
    public virtual void OnState()
    {
    }
    
    public virtual void OnStateExit()
    {
    }
}
