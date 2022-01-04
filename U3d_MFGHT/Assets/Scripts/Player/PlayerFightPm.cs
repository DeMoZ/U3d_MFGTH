using System;
using UniRx;
using UnityEngine;

public class PlayerFightPm : IDisposable
{
    public struct Ctx
    {
        public BodyParts BodyParts;
        public AttackMapView AttackMap;
        public IReactiveCommand<Swipe> OnSwipe;
    }

    private Ctx _ctx;
    private CompositeDisposable _toDispose;

    public PlayerFightPm(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();
        
        _ctx.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);

        TargetDefaultPosition();
    }

    private void OnSwipe(Swipe swipe)
    {
        Debug.LogWarning($"PlayerFightPm Received on swipe  {swipe.SwipeStates},{swipe.SwipeDirection}");
        
       // --> TODO: calculation depends on current attack state
       // return if something , or even do something different
       // <--
        
       // for now just usual routine
    }

    private void TargetDefaultPosition()
    {
         SetLocalPosition(_ctx.BodyParts.RHTarget,_ctx.AttackMap.RHDefaultPoint);
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