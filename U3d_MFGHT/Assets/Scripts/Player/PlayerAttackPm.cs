using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class PlayerAttackPm : IDisposable
{
    public struct Ctx
    {
        public Transform PlayerTransform;
        public AttackScheme AttackScheme;
        public BodyParts BodyParts;
        public AttackMapView AttackMap;
        public ReactiveCommand<Swipe> OnSwipe;
    }

    private Ctx _ctx;
    private CompositeDisposable _toDispose;

    //private Swipe _currentSwipe;
    private Tween _currentTween;
    private IAttackState _currentAttackState;
    private List<AttackSequence> _currentAttackSequences;
    
    private ReactiveProperty<AttackConfig> _currentAttackConfig;
    
    private ReactiveCommand<AttackStatesTypes> _onAttackStateChange;
    public PlayerAttackPm(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();

        //_currentSwipe = new Swipe();
        _currentAttackConfig = new ReactiveProperty<AttackConfig>();
        _currentAttackSequences = new List<AttackSequence>();
        
        _onAttackStateChange = new ReactiveCommand<AttackStatesTypes>();
        _onAttackStateChange.Subscribe(OnStateChanged).AddTo(_toDispose);
        _onAttackStateChange.Execute(AttackStatesTypes.Default);
    }

    private void OnStateChanged(AttackStatesTypes state)
    {
        // Debug.Log($"<color=#FF0000>_ctx.CurrentAttackSequences.Count</color> = {_currentAttackSequences.Count}");
        
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

    public void Dispose()
    {
        _toDispose.Dispose();
    }

    private DefaultAttackState CreateDefaultAttackState()
    {
        var attackStateCtx = new DefaultAttackState.Ctx
        {
            CurrentAttackSequences = _currentAttackSequences,
            AttackScheme = _ctx.AttackScheme,
            BodyParts = _ctx.BodyParts,
            AttackMap = _ctx.AttackMap,

            OnAttackStateChanged = _onAttackStateChange,
            OnSwipe = _ctx.OnSwipe,
        };
        return new DefaultAttackState(attackStateCtx);
    }

    private StartAttackState CreateStartAttackState()
    {
        var attackStateCtx = new StartAttackState.Ctx
        {
            CurrentAttackConfig = _currentAttackConfig,
            CurrentAttackSequences = _currentAttackSequences,
            AttackScheme = _ctx.AttackScheme,
            BodyParts = _ctx.BodyParts,
            AttackMap = _ctx.AttackMap,

            OnAttackStateChanged = _onAttackStateChange,
            OnSwipe = _ctx.OnSwipe,
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
            PlayerTransform = _ctx.PlayerTransform,
            CurrentAttackConfig = _currentAttackConfig,
            CurrentAttackSequences = _currentAttackSequences,
            BodyParts = _ctx.BodyParts,
            AttackMap = _ctx.AttackMap,

            OnAttackStateChanged = _onAttackStateChange,
            OnSwipe = _ctx.OnSwipe,
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