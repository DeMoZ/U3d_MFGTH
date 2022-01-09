using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EndAttackState : AbstractAttackState
{
    public EndAttackState(Ctx ctx) : base(ctx)
    {
        Debug.Log("EndAttackState ctx");

      
        //Debug.Log($"Start _ctx.CurrentAttackSequences == null {_ctx.CurrentAttackSequences == null}");
        Debug.Log($"_ctx.CurrentAttackSequences.Count == {_ctx.CurrentAttackSequences.Count}");
        _ctx.CurrentAttackConfig = _ctx.CurrentAttackSequences[0]._attacks[0].AttackConfig;
        var positionType = _ctx.CurrentAttackConfig.GetToLocalPosition();
        var mapPoint = _ctx.AttackMap.RHEndPoints.First(p => p.AttackPointPosition == positionType); // and you better be found

        _currentTween = _ctx.BodyParts.RHTarget.DOMove(mapPoint.transform.position, TimeAttack)
            .SetEase(Ease.InOutQuint);

        WaitForSwingEnd();
    }

    protected override void OnSwipe(Swipe swipe)
    {        
        Debug.Log($"EndAttackState reseived on swipe {swipe.SwipeState}; {swipe.SwipeDirection}");

        
        
    }

    private async void WaitForSwingEnd()
    {
        await _currentTween.AsyncWaitForCompletion();
        _ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.Default);
    }
}