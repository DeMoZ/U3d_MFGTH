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

        _currentSwipe = new Swipe();
        _ctx.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);

        TargetDefaultPosition();
    }

    private void OnSwipe(Swipe swipe)
    {
        Debug.Log($"PlayerFightPm Received on swipe  {swipe.SwipeState},{swipe.SwipeDirection}");

        // --> TODO: calculation depends on current attack state
        // return if something , or even do something different
        // <--

        // TODO: DELETE THIS. It brake all the mechanic. Created in test purposes 
        if (swipe.SwipeState == SwipeStates.None)
        {
            _currentSwipe = swipe;
            //.RHStartPoints.First(p=>p.AttackPointPosition==AttackPointPositions.Default).transform.position,

            MoveToDefaultPosition();
            //.OnComplete(myFunction);
            //BrakeRoutines();

            Debug.LogWarning("return");
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
                    MoveToStartHitPosition(swipe);
                }
                else if (swipe.SwipeState == SwipeStates.End)
                {
                    MoveToDefaultPosition();
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

    private void MoveToDefaultPosition()
    {
        Debug.LogWarning("MoveToDefaultPosition");
        _currentTween = _ctx.BodyParts.RHTarget.DOMove(_ctx.AttackMap.RHDefaultPoint.transform.position, 0.3f)
            .SetEase(Ease.OutQuint);
    }

    private void MoveToStartHitPosition(Swipe swipe)
    {
        Debug.LogWarning($"MoveToStartHitPosition {swipe.SwipeDirection}");
        _currentSwipe = swipe;
        
        // TODO: attention !!! here I take all sequences by first direction. In future here should be orderign is sequenses. 
        var sequience = _ctx.AttackScheme._attackSequences.Where(s => 
            s._attacks[0].SewipeDireciton == swipe.SwipeDirection).ToList();

        if (sequience.Count == 0)
        {
            // return or brake whatever we where in the middle
            return;
        }

        var positionType = sequience[0]._attacks[0].AttackConfig.GetFromLocalPosition();
        //Debug.Log(positionType);
        var mapPoint = _ctx.AttackMap.RHStartPoints.First(p => p.AttackPointPosition == positionType); // and you better be found
        
        _currentTween = _ctx.BodyParts.RHTarget.DOMove(mapPoint.transform.position, 0.3f)
            .SetEase(Ease.OutQuint);

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