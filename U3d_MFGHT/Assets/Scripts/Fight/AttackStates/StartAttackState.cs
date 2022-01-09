using System.Linq;
using DG.Tweening;
using UnityEngine;

public class StartAttackState : AbstractAttackState
{
    public StartAttackState(Ctx ctx) : base(ctx)
    {
        Debug.Log("StartAttackState ctx");

        Debug.Log($"_ctx.CurrentAttackSequences.Count == {_ctx.CurrentAttackSequences.Count}");
        _ctx.CurrentAttackConfig = _ctx.CurrentAttackSequences[0]._attacks[0].AttackConfig;
        var positionType = _ctx.CurrentAttackConfig.GetFromLocalPosition();
        var mapPoint =
            _ctx.AttackMap.RHStartPoints.First(p => p.AttackPointPosition == positionType); // and you better be found

        _currentTween = _ctx.BodyParts.RHTarget.DOMove(mapPoint.transform.position, TimeToStart)
            .SetEase(Ease.InOutQuint);
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
                Debug.Log(
                    $"Start attack received change attack direction to {swipe.SwipeState}; {swipe.SwipeDirection}");
                _ctx.CurrentAttackSequences = GetSequencesByDirection(_ctx.AttackScheme._attackSequences, 0);
                _ctx.OnAttackSequences.Invoke(_ctx.CurrentAttackSequences);
                _ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.Start);
                break;
            case SwipeStates.End:

                _ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.End);
                break;
            default:
                Debug.LogError($"StartAttackState received no state for {swipe.SwipeState}; {swipe.SwipeDirection}");
                break;
        }
    }
}