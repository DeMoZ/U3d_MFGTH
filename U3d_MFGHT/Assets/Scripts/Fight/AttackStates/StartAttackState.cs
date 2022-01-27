using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class StartAttackState : AbstractAttackState
{
    public new struct Ctx
    {
        public BodyParts BodyParts;
        public AttackMapView AttackMap;
        public AttackScheme AttackScheme;
        public List<AttackSequence> CurrentAttackSequences;
        public ReactiveProperty<AttackConfig> CurrentAttackConfig;
        public ReactiveCommand<AttackStatesTypes> OnAttackStateChanged;
        public ReactiveCommand<Swipe> OnSwipe;
    }

    private Ctx _ctx;

    public StartAttackState(Ctx ctx) : base(new AbstractAttackState.Ctx
    {
        OnSwipe = ctx.OnSwipe,
        BodyParts = ctx.BodyParts,
        AttackMap = ctx.AttackMap
    })
    {
        _ctx = ctx;

        Debug.Log("<color=#00FF00>StartAttackState ctx</color>");
        Debug.Log($"<color=#FF0000>_ctx.CurrentAttackSequences.Count</color> = {_ctx.CurrentAttackSequences.Count}");

        _ctx.CurrentAttackConfig.Value = _ctx.CurrentAttackSequences[0]._attacks[0].AttackConfig;
        var positionType = _ctx.CurrentAttackConfig.Value.GetFromLocalPosition();
        var mapPoint = _ctx.AttackMap.RHStartPoints.First(p => p.AttackPointPosition == positionType);

        _currentTween = _ctx.BodyParts.RHTarget.DOMove(mapPoint.transform.position, TimeToStart)
            .SetEase(Ease.InOutQuint);
        // TODO TweenBlend(_ctx.CurrentAttackConfig.Value, AttackStatesTypes.Start);
    }

    protected async override void OnSwipe(Swipe swipe)
    {
        if (swipe.SwipeState == SwipeStates.None)
        {
            Debug.Log("StartAttackState received skip (None state)");
            return;
        }

        // TODO: HOLD!!!!! need to implement somehow
        Debug.Log($"StartAttackState.OnSwipe received {swipe.SwipeState}; {swipe.SwipeDirection}");

        _currentSwipe = swipe;
        if (_currentTween.IsActive() && _currentTween.IsPlaying())
            await _currentTween.AsyncWaitForCompletion();

        switch (swipe.SwipeState)
        {
            case SwipeStates.Change:
                Debug.Log($"Start attack received change  direction to {swipe.SwipeState}; {swipe.SwipeDirection}");
                _ctx.CurrentAttackSequences.Clear();
                _ctx.CurrentAttackSequences.AddRange(GetSequencesByDirection(_ctx.AttackScheme._attackSequences, 0));
                _ctx.OnAttackStateChanged.Execute(AttackStatesTypes.Start);
                break;
            case SwipeStates.End:
                _ctx.OnAttackStateChanged.Execute(AttackStatesTypes.End);
                break;
            default:
                Debug.LogError($"StartAttackState received no state for {swipe.SwipeState}; {swipe.SwipeDirection}");
                break;
        }
    }
}