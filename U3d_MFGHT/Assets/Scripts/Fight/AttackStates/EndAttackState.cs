using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class EndAttackState : AbstractAttackState
{
    public new struct Ctx
    {
        //public Swipe CurrentSwipe;
        public BodyParts BodyParts;
        public AttackMapView AttackMap;
        public List<AttackSequence> CurrentAttackSequences;
        public ReactiveProperty<AttackConfig> CurrentAttackConfig;
        public ReactiveCommand<AttackStatesTypes> OnAttackStateChanged;
        public ReactiveCommand<Swipe> OnSwipe;
    }

    private Ctx _ctx;
    
    public EndAttackState(Ctx ctx) : base(new AbstractAttackState.Ctx{OnSwipe = ctx.OnSwipe})
    {
        _ctx = ctx;
    
        Debug.Log("<color=#00FF00>EndAttackState ctx</color>");
        var positionType = _ctx.CurrentAttackConfig.Value.GetToLocalPosition();
        Debug.Log($"<color=#FF0000>_ctx.CurrentAttackSequences.Count</color> = {_ctx.CurrentAttackSequences.Count}");
        var mapPoint = _ctx.AttackMap.RHEndPoints.First(p => p.AttackPointPosition == positionType);

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
        _ctx.OnAttackStateChanged.Execute(AttackStatesTypes.Default);
    }
}