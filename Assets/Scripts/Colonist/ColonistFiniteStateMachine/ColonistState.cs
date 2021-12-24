using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ColonistState
{
    protected Colonist _colonist;
    private Animator _animator;
    protected ColonistStateMachine _stateMachine;
    private string _animatorBoolParamater;

    private AnimatorClipInfo[] _animatorClipInfos;
    private AnimatorTransitionInfo _animatorTransitionInfo;
    private float _animationDuration;
    private float _transitionDuration;
    protected float _transitionNormalizedTime => _animatorTransitionInfo.normalizedTime;
    private bool _animationStarted;
    private float _timer;
    protected bool _animationFinished;
    protected bool _transitionFinished;

    public ColonistState(Colonist colonist, Animator animator, ColonistStateMachine stateMachine, string animatorBoolParamater)
    {
        _colonist = colonist;
        _stateMachine = stateMachine;
        _animatorBoolParamater = animatorBoolParamater;
        _animator = animator;
    }
    public virtual void Enter()
    {
        _animatorClipInfos = _animator.GetCurrentAnimatorClipInfo(0);
        _animatorTransitionInfo = _animator.GetAnimatorTransitionInfo(0);

        _animator.SetBool(_animatorBoolParamater, true);
    }
    public virtual void Exit()
    {
        _animationStarted = false;
        _animationFinished = false;
        _transitionFinished = false;
        _timer = 0;

        _animator.SetBool(_animatorBoolParamater, false);
    }

    public virtual void LogicUpdate()
    {
        if (_animatorClipInfos.Length > 0 && !_animationStarted)
        {
            _animationDuration = _animatorClipInfos[0].clip.length;
            _transitionDuration = _animatorTransitionInfo.duration;
            _animationStarted = true;
        }

        if (_animationStarted)
        {
            if (!_transitionFinished)
            {
                if (_timer >= _transitionDuration)
                {
                    _transitionFinished = true;
                }
            }
            if (!_animationFinished)
            {
                if (_timer >= _animationDuration)
                {
                    _animationFinished = true;
                }
            }
            _timer += Time.deltaTime;
        }
    }
    public virtual void PhysicsUpdate()
    {
    }
}
