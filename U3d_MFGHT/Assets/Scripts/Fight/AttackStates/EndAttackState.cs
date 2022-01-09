using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EndAttackState : AbstractAttackState
{
    public EndAttackState(Ctx ctx) : base(ctx)
    {
        Debug.Log("EndAttackState ctx");
        //Debug.Log($"Start _ctx.CurrentAttackSequences == null {_ctx.CurrentAttackSequences == null}");
        //Debug.Log($"_ctx.CurrentAttackSequences.Count == {_ctx.CurrentAttackSequences.Count}");
        _ctx.CurrentAttackConfig = _ctx.CurrentAttackSequences[0]._attacks[0].AttackConfig;
        var positionType = _ctx.CurrentAttackConfig.GetToLocalPosition();
        var mapPoint =
            _ctx.AttackMap.RHEndPoints.First(p => p.AttackPointPosition == positionType); // and you better be found

        _currentTween = _ctx.BodyParts.RHTarget.DOMove(mapPoint.transform.position, TimeAttack)
            .SetEase(Ease.InOutQuint);

        WaitForSwingEnd();
    }

    protected async override void OnSwipe(Swipe swipe)
    {
        if (swipe.SwipeState == SwipeStates.None)
        {
            Debug.Log("EndAttackState received skip (None state)");
            return;
        }
        
        Debug.Log($"EndAttackState received on swipe {swipe.SwipeState}; {swipe.SwipeDirection}");

        _currentSwipe = swipe;
        if (_currentTween.IsActive() && _currentTween.IsPlaying())
            await _currentTween.AsyncWaitForCompletion();

        switch (swipe.SwipeState)
        {
            case SwipeStates.Start:
                //_ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.LongEnd);
                break;
            case SwipeStates.None:

                break;
            default:
                Debug.LogError($"EndAttackState received no state for {swipe.SwipeState}; {swipe.SwipeDirection}");

                break;
        }
    }

    private async void WaitForSwingEnd()
    {
        await _currentTween.AsyncWaitForCompletion();
        //_ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.ShortEnd); // TODO: go to ShortEnd. For now it is to default
        _ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.Default);
    }
}