using UnityEngine;
using UnityEngine.AI;

public class ColonistWalkState : ColonistState
{
    private Vector3 _lastPosition;
    private float _timer;
    public ColonistWalkState(Colonist colonist, Animator animator, ColonistStateMachine stateMachine, string animBool) : base(colonist, animator, stateMachine, animBool)
    {
    }
    public override void Exit()
    {
        _timer = 0;
        if (_colonist.BuildAssigned)
        {
            _colonist.Building = true;
        }
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_colonist.Moving)
        {
            _stateMachine.ChangeState(_colonist.IdleState);
        }
        if (_lastPosition != _colonist.transform.position)
        {
            _lastPosition = _colonist.transform.position;
        }
        else
        {
            if (_timer > 1)
            {
                _colonist.RecalculatePath();
                _timer = 0;
            }
            _timer += Time.deltaTime;
        }
    }
}
