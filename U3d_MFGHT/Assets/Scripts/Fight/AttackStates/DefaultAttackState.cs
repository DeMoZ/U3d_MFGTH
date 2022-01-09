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
        //--> base?
        _currentSwipe = swipe;
        if (_currentTween.IsActive() && _currentTween.IsPlaying())
            await _currentTween.AsyncWaitForCompletion();
        //<--
        
        _ctx.CurrentAttackSequences = GetSequencesByDirection(_ctx.AttackScheme._attackSequences, 0);

        if (_ctx.CurrentAttackSequences.Count == 0)
        {
            Debug.Log("_ctx.CurrentAttackSequences.Count == 0; stay in default");
            return; // stay in default state
        }

        /*Debug.Log($"_ctx.CurrentAttackSequences.Count == {_ctx.CurrentAttackSequences.Count}");
        Debug.Log($" Default _ctx.CurrentAttackSequences == null {_ctx.CurrentAttackSequences == null}");*/

        _ctx.OnAttackSequences.Invoke(_ctx.CurrentAttackSequences);
        _ctx.OnAttackStateChanged.Invoke(AttackStatesTypes.Start);
    }
}