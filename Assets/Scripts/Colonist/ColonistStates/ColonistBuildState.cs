using UnityEngine;
using UnityEngine.AI;

public class ColonistBuildState : ColonistState
{
    public ColonistBuildState(Colonist colonist, Animator animator, ColonistStateMachine stateMachine, string animBool) : base(colonist, animator, stateMachine, animBool)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _colonist.transform.rotation = Quaternion.LookRotation(_colonist.AssignedBuildDraft.transform.position - _colonist.transform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_colonist.InsideBuildDraft)
        {
            _colonist.RecalculatePath();
            _colonist.InsideBuildDraft = false;
            _stateMachine.ChangeState(_colonist.IdleState);
        }
        if (_transitionFinished)
        {
            if (!(_colonist.AssignedBuildDraft is null))
            {
                if (!_colonist.AssignedBuildDraft.IsBuilt)
                {
                    _colonist.SurfaceGen.AddSurface(_colonist.AssignedBuildDraft.transform);
                    _colonist.AssignedBuildDraft.ColorFade(_colonist.BuildRatio);
                }
                else
                {
                    _colonist.InsideBuildDraft = false;
                    _colonist.DeassingBuildDraft();
                    ColonistManager.BuildDrafts.Remove(_colonist.AssignedBuildDraft);
                    _stateMachine.ChangeState(_colonist.IdleState);
                }
            }
        }
    }
}
