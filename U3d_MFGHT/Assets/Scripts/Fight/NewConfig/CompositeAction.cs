using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Fight.NewConfig
{
    [Serializable]
    public class CompositeAction : MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveCommand<float> OnUpdate;
            public ReactiveCommand<(MovePart part, Vector3? pos, Quaternion? rot)> OnCalculated;
        }

        [SerializeField] private string _name = default;
        [Space] //
        [SuffixLabel("sec.", true)]
        [SerializeField] private float _duration = 1;
        [SerializeField] private AnimationCurve _durationCurve;
        [SerializeField] private SwipeDirections _direction;
        [SerializeField] private MoveSet[] _moves = { };
       
        private Ctx _ctx;
        private float _timer;
        private float _timerScalled;

        private ReactiveCommand<float> _onUpdate;
        private List<Transform> _testTargets;
        public float Duration => _duration;
        
        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            _timer = 0;
            _timerScalled = 0;

            _onUpdate = new ReactiveCommand<float>();
            _ctx.OnUpdate.Subscribe(OnUpdate).AddTo(this);

            foreach (var set in _moves)
            {
                var moveCtx = new MoveSet.Ctx
                {
                    OnUpdate = _onUpdate,
                    OnCalculated = _ctx.OnCalculated
                };
                set.SetCtx(moveCtx);
            }
        }
        
        private void OnValidate()
        {
            _duration = _duration <= 0 ? 0.01f : _duration;
            foreach (var set in _moves)
            {
                if (set != null)
                    set.SetDuration(_duration);
            }
        }

        public float testTIme = 0;
        private void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            _timerScalled = _durationCurve.Evaluate(_timer / _duration);

            testTIme = _duration * _timerScalled;

            _onUpdate.Execute(_duration * _timerScalled);
        }

        private void TestOnCalculated((MovePart part, Vector3? localPosition, Quaternion? localRotation) calculated)
        {
            foreach (var tt in _testTargets.Where(tt => tt.name.Equals("TestTarget_" + calculated.part)))
            {
                if (calculated.localPosition.HasValue)
                    tt.localPosition = calculated.localPosition.Value;

                if (calculated.localRotation.HasValue)
                    tt.localRotation = calculated.localRotation.Value;
            }
        }

        [Button("Test Play")]
        private async void Play()
        {
            _timer = 0;
            _timerScalled = 0;
            
            _onUpdate = new ReactiveCommand<float>();
            var onCalculated = new ReactiveCommand<(MovePart, Vector3?, Quaternion?)>();
            onCalculated.Subscribe(TestOnCalculated).AddTo(this);

            _testTargets = new List<Transform>();
            foreach (var set in _moves)
            {
                var target = _testTargets.FirstOrDefault(t => t.name == "TestTarget_" + set.MovePart);
                if (target == null)
                {
                    target = new GameObject().transform;
                    target.name = "TestTarget_" + set.MovePart;
                    target.gameObject.AddComponent<GizmoSelected>();
                    target.parent = transform;
                    
                    _testTargets.Add(target);
                }

                var moveCtx = new MoveSet.Ctx
                {
                    OnUpdate = _onUpdate,
                    OnCalculated = onCalculated
                };
                set.SetCtx(moveCtx);
            }
            
            int delay = 1;
            while (_timer < _duration)
            {
                await Task.Delay(delay);
                OnUpdate((float)delay/18);
            }

            foreach (var tt in _testTargets) 
                DestroyImmediate(tt.gameObject);
            
            _testTargets = new List<Transform>();
        }
    }
}