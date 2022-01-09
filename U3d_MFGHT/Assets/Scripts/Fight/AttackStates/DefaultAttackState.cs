using System;
using DG.Tweening;
using UnityEngine;

public class DefaultAttackState : AbstractAttackState
{
    public DefaultAttackState(Ctx ctx) : base(ctx)
    {
        Debug.Log("ToDefault ctx");

        _currentTween = _ctx.BodyParts.RHTarget.DOMove(_ctx.AttackMap.RHDefaultPoint.transform.position, TimeToDefault)
            .SetEase(Ease.OutQuint);
    }

    protected override async void OnSwipe(Swipe swipe)
    {
        if (swipe.SwipeState == SwipeStates.None)
        {
            Debug.Log("DefaultAttackState received skip (None state)");
            return;
        }
        //--> base?
        _currentSwipe = swipe;
        if (_currentTween.IsActive() && _currentTween.IsPlaying())
            await _currentTween.AsyncWaitForCompletion();
        //<--
        switch (swipe.SwipeState)
        {
            case SwipeStates.Start:
                _ctx.CurrentAttackSequences = GetSequencesByDirection(_ctx.AttackScheme._attackSequences, 0);
                Debug.Log($"DefaultAttackState _ctx.CurrentAttackSequences.Count == {_ctx.CurrentAttackSequences.Count}");
                //Debug.Log($" Default _ctx.CurrentAttackSequences == null {_ctx.CurrentAttackSequences == null}");
                _ctx.OnAttackSequences.Invoke(_ctx.CurrentAttackSequences);
                _ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.Start);
                break;
            default:
                Debug.LogError($"DefaultAttackState received no state for {swipe.SwipeState}; {swipe.SwipeDirection}");
                break;
        }
    }
}