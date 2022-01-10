using DG.Tweening;
using UnityEngine;

public class DefaultAttackState : AbstractAttackState
{
    public DefaultAttackState(Ctx ctx) : base(ctx)
    {
        Debug.Log("<color=#00FF00> DefaultState ctx</color>");
        _currentTween = _ctx.BodyParts.RHTarget.DOMove(_ctx.AttackMap.RHDefaultPoint.transform.position, TimeToDefault)
            .SetEase(Ease.OutQuint);
    }

    protected override async void OnSwipe(Swipe swipe)
    {
        if (swipe.SwipeState == SwipeStates.None)
        {
            Debug.Log("DefaultState received skip (None state)");
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
                Debug.Log("DefaultState -> StartState");
                _ctx.CurrentAttackSequences.Clear();
                var sequences = GetSequencesByDirection(_ctx.AttackScheme._attackSequences, 0);
                foreach (var sequence in sequences) 
                    _ctx.CurrentAttackSequences.Add(sequence);

                Debug.Log($"<color=#FF0000>_ctx.CurrentAttackSequences.Count</color> = {_ctx.CurrentAttackSequences.Count}");
                _ctx.OnAttackStateChanged.Execute(AttackStatesTypes.Start);
                break;
            default:
                Debug.LogError($"DefaultAttackState received no state for {swipe.SwipeState}; {swipe.SwipeDirection}");
                break;
        }
    }
}