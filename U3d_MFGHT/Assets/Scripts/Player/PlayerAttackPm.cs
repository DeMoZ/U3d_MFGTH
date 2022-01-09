using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class PlayerAttackPm : IDisposable
{
    public struct Ctx
    {
        public AttackScheme AttackScheme;
        public BodyParts BodyParts;
        public AttackMapView AttackMap;
        public IReactiveCommand<Swipe> OnSwipe;
    }

    private Ctx _ctx;
    private CompositeDisposable _toDispose;

    private Swipe _currentSwipe;
    private Tween _currentTween;
    private List<AttackSequence> _currentAttackSequences;
    private AttackConfig _currentAttackConfig;
    private IAttackState _currentAttackState;

    private Action<AttackStatesTypes> _onAttackStateChange;
    private Action<List<AttackSequence>> _onAttackSequences;

    public PlayerAttackPm(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();

        _currentSwipe = new Swipe();
        _onAttackSequences = OnAttackSequences;
        _onAttackStateChange = OnStateChanged;

        OnStateChanged(AttackStatesTypes.Default);
    }

    private void OnAttackSequences(List<AttackSequence> sequences) =>
        _currentAttackSequences = sequences;

    private void OnStateChanged(AttackStatesTypes state)
    {
        Debug.Log($" OnStateChanged _ctx.CurrentAttackSequences == null {_currentAttackSequences == null}");
        _currentAttackState?.Dispose();
        switch (state)
        {
            case AttackStatesTypes.Default:
                _currentAttackState = CreateDefaultAttackState();
                break;
            case AttackStatesTypes.Start:
                _currentAttackState = CreateStartAttackState();
                break;
            /*case AttackStatesTypes.PowerUp:
                _currentAttackState = CreatePowerUpAttackState();
                break;*/
            case AttackStatesTypes.End:
                _currentAttackState = CreateEndAttackState();
                break;
            /*case AttackStatesTypes.ShortEnd:
                _currentAttackState = CreateShortEndAttackState();
                break;
            case AttackStatesTypes.LongEnd:
                _currentAttackState = CreateLongEndAttackState();
                break;*/
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void SetLocalPosition(Transform obj, PointView toPoint)
    {
        obj.localPosition = toPoint.transform.localPosition;
    }

    public void Dispose()
    {
        _toDispose.Dispose();
    }

    private DefaultAttackState CreateDefaultAttackState()
    {
        var attackStateCtx = new DefaultAttackState.Ctx
        {
            CurrentSwipe = _currentSwipe,
            CurrentAttackConfig = _currentAttackConfig,
            CurrentAttackSequences = _currentAttackSequences,
            AttackScheme = _ctx.AttackScheme,
            BodyParts = _ctx.BodyParts,
            AttackMap = _ctx.AttackMap,

            OnAttackStateChanged = _onAttackStateChange,
            OnSwipe = _ctx.OnSwipe,
            
            OnAttackSequences = _onAttackSequences
        };
        return new DefaultAttackState(attackStateCtx);
    }

    private StartAttackState CreateStartAttackState()
    {
        var attackStateCtx = new StartAttackState.Ctx
        {
            CurrentSwipe = _currentSwipe,
            CurrentAttackConfig = _currentAttackConfig,
            CurrentAttackSequences = _currentAttackSequences,
            AttackScheme = _ctx.AttackScheme,
            BodyParts = _ctx.BodyParts,
            AttackMap = _ctx.AttackMap,

            OnAttackStateChanged = _onAttackStateChange,
            OnSwipe = _ctx.OnSwipe,
            OnAttackSequences = _onAttackSequences
        };

        return new StartAttackState(attackStateCtx);
    }

    private void CreatePowerUpAttackState()
    {
    }

    private EndAttackState CreateEndAttackState()
    {
        var attackStateCtx = new EndAttackState.Ctx
        {
            CurrentSwipe = _currentSwipe,
            CurrentAttackConfig = _currentAttackConfig,
            CurrentAttackSequences = _currentAttackSequences,
            AttackScheme = _ctx.AttackScheme,
            BodyParts = _ctx.BodyParts,
            AttackMap = _ctx.AttackMap,

            OnAttackStateChanged = _onAttackStateChange,
            OnSwipe = _ctx.OnSwipe,
            OnAttackSequences = _onAttackSequences
        };

        return new EndAttackState(attackStateCtx);
    }

    private void CreateShortEndAttackState()
    {
    }

    private void CreateLongEndAttackState()
    {
    }
}