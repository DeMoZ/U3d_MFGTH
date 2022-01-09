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
    protected const float TimeAttack = 1f;

    public struct Ctx
    {
        public Swipe CurrentSwipe;
        public AttackConfig CurrentAttackConfig;
        public AttackScheme AttackScheme;
        public BodyParts BodyParts;
        public AttackMapView AttackMap;
        public List<AttackSequence> CurrentAttackSequences;
        public IReactiveCommand<Swipe> OnSwipe;
        
        public Action<List<AttackSequence>> OnAttackSequences;
        public Action<AttackStatesTypes> OnAttackStateChanged;
    }

    protected Ctx _ctx;
    protected Swipe _currentSwipe;
    protected Tween _currentTween;
    protected bool _firstCallSkipped;
    private CompositeDisposable _toDispose;

    protected abstract void OnSwipe(Swipe swipe);

    public AbstractAttackState(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();
        _ctx.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);
    }

    /*protected bool NeedToSkipOnFirstCall()
    {
        if (_firstCallSkipped)
            return false;

        _firstCallSkipped = true;
        return true;
    }*/
    
    protected List<AttackSequence> GetSequencesByDirection(List<AttackSequence> sequences, int attackNumber) => 
        sequences.Where(s => s._attacks[attackNumber].SewipeDireciton == _currentSwipe.SwipeDirection).ToList();

    public void Dispose()
    {
        _toDispose.Dispose();
    }
}