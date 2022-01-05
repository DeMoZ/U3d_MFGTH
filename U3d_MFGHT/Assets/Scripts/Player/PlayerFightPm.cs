using System;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class PlayerFightPm : IDisposable
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

    /*private SwipeStates _currentSwipeState;
    private SwipeDirections _currentSwipeDirection;*/

    private Swipe _currentSwipe;
    private Tween _currentTween;

    public PlayerFightPm(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();

        _ctx.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);

        TargetDefaultPosition();
    }

    private void OnSwipe(Swipe swipe)
    {
        Debug.LogWarning($"PlayerFightPm Received on swipe  {swipe.SwipeState},{swipe.SwipeDirection}");

        // --> TODO: calculation depends on current attack state
        // return if something , or even do something different
        // <--
        
        // TODO: DELETE THIS. It brake all the mechanic. Created in test purposes 
        if (swipe.SwipeState == SwipeStates.None)
        {
            _currentSwipe = swipe;
            //.RHStartPoints.First(p=>p.AttackPointPosition==AttackPointPositions.Default).transform.position,
            Tween myTween = _ctx.BodyParts.RHTarget.DOMove(_ctx.AttackMap.RHDefaultPoint.transform.position, 2)
                .SetEase(Ease.OutQuint);
            //.OnComplete(myFunction);
            //BrakeRoutines();
            return;
        }

        switch (_currentSwipe.SwipeState)
        {
            case SwipeStates.None:
                if (swipe.SwipeState == SwipeStates.Start)
                    MoveToStartHitPosition(swipe);
                break;
            case SwipeStates.Start:
                if (swipe.SwipeState == SwipeStates.Change)
                {
                }
                else if (swipe.SwipeState == SwipeStates.End)
                {
                }

                break;
            case SwipeStates.Change:
                // i dont apply change state on _currentState
                break;
            case SwipeStates.End:
                // TODO: to think out
                // should be the ending of the fight some how calculated
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (swipe.SwipeDirection)
        {
            case SwipeDirections.None:
                break;
            case SwipeDirections.ToRight:
                break;
            case SwipeDirections.ToLeft:
                break;
            case SwipeDirections.ToUp:
                break;
            case SwipeDirections.ToDown:
                break;
            case SwipeDirections.Thrust:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // for now just usual routine
    }

    private void MoveToStartHitPosition(Swipe swipe)
    {
        _currentSwipe = swipe;
        // tween to start hit position
    }

    private void TargetDefaultPosition()
    {
        SetLocalPosition(_ctx.BodyParts.RHTarget, _ctx.AttackMap.RHDefaultPoint);
    }

    private void SetLocalPosition(Transform obj, PointView toPoint)
    {
        obj.localPosition = toPoint.transform.localPosition;
    }

    public void Dispose()
    {
        _toDispose.Dispose();
    }

    // replace with task is required
    /*private IEnumerator IEBlend()
    {
        var timer = 0f;
        float timerScaled;
        float amplitude;
        Vector3 straightPosition;
        Vector3 position;
        while (timer < _blendTime)
        {
            yield return null;
            /*             
                Vector3 shoulderToStraight = straightPosition - _shoulder.position;
                Vector3 fromTo = _toPoint.position - _fromPoint.position;                
                Vector3 cross = Vector3.Cross(shoulderToStraight, fromTo);
                Vector3 rotatedVector = Quaternion.AngleAxis(-90, cross) * fromTo;                
                Vector3 position = straightPosition + rotatedVector.normalized * amplitude;              
            #1#
            timerScaled = _speedCurve.Evaluate(timer / _blendTime);
            amplitude = Mathf.Sin(timerScaled * Mathf.PI) * _amplitude;
            straightPosition = Vector3.Lerp(_fromPoint.position, _toPoint.position, timerScaled);
            position = straightPosition + _playerTransfrom.forward * amplitude;

            _target.position = position;
            timer += Time.deltaTime;
        }

        _target.position = _toPoint.position;
        _blendRoutine = null;
    }*/
}