using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Fight.NewConfig
{
    [Serializable]
    public class MoveSet : MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveCommand<float> OnUpdate;
            public ReactiveCommand<(MovePart part, Vector3? pos, Quaternion? rot)> OnCalculated;
        }

        [SerializeField] private MovePart _movePart = default;
        [SerializeField] private MoveType _moveType = default;
        [SerializeField] private float _duration = 1;
        [SerializeField] private AnimationCurve _durationCurve;

        [SerializeField, Tooltip("Limited by parent configuration")] [MinMaxSlider(0, "@_duration", true)]
        private Vector2 _actionTime = new Vector2(0, 1);

        [SerializeField] private AnimationCurve _amplitudeCurve = default;
        [SerializeField] private bool _simpleStartEnd = true;

        [SerializeField, ShowIf(nameof(_simpleStartEnd))]
        private Transform _startPoint = default;

        [SerializeField, ShowIf(nameof(_simpleStartEnd))]
        private Transform _endPoint = default;

        [SerializeField, HideIf(nameof(_simpleStartEnd))]
        private Transform[] _startEndPoints = default;

        /*[Space] [SerializeField] private Transform _holdPoint = default;
        [SerializeField] private Transform _endPointShort = default;
        [SerializeField] private Transform _endPointLong = default;

        [Space] [SerializeField] private Transform _tipStartPoint = default;
        [Space] [SerializeField] private Transform _tipEndPoint = default;
        */

        //[Space] [SerializeField] private bool _show = default;

        private Ctx _ctx;

        public MovePart MovePart => _movePart;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.OnUpdate.Subscribe(OnUpdate).AddTo(this);
        }

        private void OnValidate()
        {
            if (transform.parent.TryGetComponent<CompositeAction>(out var composite))
                _duration = composite.Duration;

            _simpleStartEnd = true; // TODO make it possible to work with _startEndPointsArray
        }

        public void SetDuration(float duration)
        {
            _duration = duration;
            _actionTime.y = Mathf.Clamp(_actionTime.y, 0.1f, _duration);
        }

        private Vector3 _straight;
        private float _amplitude;
        private float _timeCut;
        private float _timeScaled;

        private Vector3? _localPosition;
        private Quaternion? _localRotation;

        private void OnUpdate(float time)
        {
            if (time < _actionTime.x) return;
            if (time > _actionTime.y) return;

            _timeCut = (time - _actionTime.x) / (_actionTime.y - _actionTime.x);
            _timeScaled = _durationCurve.Evaluate(_timeCut);

            _localPosition = null;

            if (_moveType == MoveType.Position || _moveType == MoveType.PositionAndRotation)
            {
                _straight = Vector3.Lerp(_startPoint.localPosition, _endPoint.localPosition, _timeScaled);
                _amplitude = Mathf.Sin(_timeScaled * Mathf.PI) * _amplitudeCurve.Evaluate(_timeScaled);

                _localPosition = _straight + Vector3.forward * _amplitude;
            }

            _localRotation = null;

            if (_moveType == MoveType.Rotation || _moveType == MoveType.PositionAndRotation)
                _localRotation = Quaternion.Lerp(_startPoint.localRotation, _endPoint.localRotation, _timeScaled);

            _ctx.OnCalculated.Execute((_movePart, _localPosition, _localRotation));
        }

        [Button("Add Start End points")]
        private void AddStartEndPoints()
        {
            if (!_startPoint)
                _startPoint = CreatePoint(PointType.Start);

            if (!_endPoint)
                _endPoint = CreatePoint(PointType.End);
        }

        private Transform CreatePoint(PointType pointType)
        {
            Transform point;
            point = new GameObject().transform;
            point.name = pointType + "Point";

            point.localScale = Vector3.one * 0.1f;
            point.parent = transform;
            point.localPosition = Vector3.zero;

            point.gameObject.AddComponent<GizmoSelected>();
            return point;
        }
    }
}