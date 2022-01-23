using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;

internal interface IAttackState
{
    public void Dispose();
}

public abstract class AbstractAttackState : IAttackState, IDisposable
{
    protected const float TimeToDefault = 0.2f;
    protected const float TimeToStart = 0.3f;
    protected const float TimeAttack = 7f;

    public struct Ctx
    {
        public IReactiveCommand<Swipe> OnSwipe;
    }

    protected Ctx _ctx;
    protected Swipe _currentSwipe;
    protected Tween _currentTween;
    private CompositeDisposable _toDispose;

    protected abstract void OnSwipe(Swipe swipe);

    public AbstractAttackState(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();
        _ctx.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);
    }
    
    protected List<AttackSequence> GetSequencesByDirection(List<AttackSequence> sequences, int attackNumber) => 
        sequences.Where(s => s._attacks[attackNumber].SewipeDireciton == _currentSwipe.SwipeDirection).ToList();

    public void Dispose()
    {
        _toDispose.Dispose();
    }
}