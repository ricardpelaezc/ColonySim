using UnityEngine;
using UnityEngine.AI;

public class ColonistIdleState : ColonistState
{
    public ColonistIdleState(Colonist colonist, Animator animator, ColonistStateMachine stateMachine, string animBool) : base(colonist, animator, stateMachine, animBool)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_colonist.Moving)
        {
            _stateMachine.ChangeState(_colonist.WalkState);
        }
        else if (_colonist.Building)
        {
            _stateMachine.ChangeState(_colonist.BuildState);
        }
    }
}
