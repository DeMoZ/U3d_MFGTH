using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;

internal interface IAttackState
{
    public void Dispose();
}

public abstract class AbstractAttackState : IAttackState, IDisposable
{
    protected const float TimeToDefault = 0.2f;
    protected const float TimeToStart = 0.3f;
    protected const float TimeAttack = 7f;

    public struct Ctx
    {
        public IReactiveCommand<Swipe> OnSwipe;
        public BodyParts BodyParts;
        public AttackMapView AttackMap;
    }

    protected Ctx _ctx;
    protected Swipe _currentSwipe;
    protected Tween _currentTween;
    private CompositeDisposable _toDispose;

    protected abstract void OnSwipe(Swipe swipe);

    public AbstractAttackState(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();
        _ctx.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);
    }

    protected List<AttackSequence> GetSequencesByDirection(List<AttackSequence> sequences, int attackNumber)
    {
        var seq =  sequences.Where(s => s._attacks[attackNumber].SewipeDireciton == _currentSwipe.SwipeDirection).ToList();
        Debug.Log($"From sequences amount {sequences.Count} selected amount {seq.Count}");
        return seq;
    }

    protected void TweenBlend(AttackConfig config, AttackStatesTypes attackStatesType)
    {
        float timerScaled;
        float timer = 0;
        float amplitude;
        Vector3 straightLocalPosition;
        Vector3 position;

        var destinationPoints = DestinationPoints(attackStatesType);

        var positionType = config.GetToLocalPosition();
        var mapPoint = destinationPoints.First(p => p.AttackPointPosition == positionType);
        var toPoint = mapPoint.transform.localPosition;
        var fromPoint = _ctx.BodyParts.RHTarget.localPosition;

        var duration = attackStatesType switch
        {
            AttackStatesTypes.Default => TimeToDefault,
            AttackStatesTypes.Start => TimeToStart,
            AttackStatesTypes.End => TimeAttack,
            _ => throw new NotImplementedException($"The state for {attackStatesType} wasn't implemented")
        };

        _currentTween = DOTween.To(() => timer, x => timer = x, 1, duration).OnUpdate(() =>
        {
            timerScaled = config.GetSpeedCurve().Evaluate(timer);
            straightLocalPosition = Vector3.Lerp(fromPoint, toPoint, timerScaled);
            amplitude = Mathf.Sin(timerScaled * Mathf.PI) * config.GetAmplitude();
            position = straightLocalPosition + Vector3.forward * amplitude;
            _ctx.BodyParts.RHTarget.localPosition = position;
        });
    }

    private List<PointView> DestinationPoints(AttackStatesTypes attackStatesType)
    {
        List<PointView> destinationPoints = new List<PointView>();


        switch (attackStatesType)
        {
            case AttackStatesTypes.Default:
                destinationPoints.Add(_ctx.AttackMap.RHDefaultPoint);
                break;
            case AttackStatesTypes.Start:
                destinationPoints = _ctx.AttackMap.RHStartPoints;
                break;
            /*case AttackStatesTypes.PowerUp:
                destinationPoints = _ctx.AttackMap.RHStartPoints;
                break;*/
            case AttackStatesTypes.End:
                destinationPoints = _ctx.AttackMap.RHEndPoints;
                break;
            /*case AttackStatesTypes.ShortEnd:
                destinationPoints = _ctx.AttackMap.RHEndPoints;
                break;*/
            /*case AttackStatesTypes.LongEnd:
                destinationPoints = _ctx.AttackMap.RHEndPoints;
                break;*/
            default:
                throw new ArgumentOutOfRangeException(nameof(attackStatesType), attackStatesType, null);
        }

        return destinationPoints;
    }

    public void Dispose()
    {
        _toDispose.Dispose();
    }

    public static char SwipeDirectionChar(SwipeDirections direction)
    {
        return direction switch
        {
            SwipeDirections.None => 'O',
            SwipeDirections.ToRight => '>',
            SwipeDirections.ToLeft => '<',
            SwipeDirections.ToUp => '^',
            SwipeDirections.ToDown => 'v',
            SwipeDirections.Thrust => 'x',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static string SwipeDirectionFromTo(SwipeDirections from, SwipeDirections to) =>
        $"{SwipeDirectionChar(from)} : {SwipeDirectionChar(to)}";
}