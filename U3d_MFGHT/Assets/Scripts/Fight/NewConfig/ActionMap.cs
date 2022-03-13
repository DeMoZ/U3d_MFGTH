using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;

namespace Fight.NewConfig
{
    public class ActionMap : MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveCommand<float> OnUpdate;
            public ReactiveCommand<(MovePart part, Vector3? pos, Quaternion? rot)> OnCalculated;
        }

        //[SerializeField] private CompositeAction[] _actions;

        [TableList(ShowIndexLabels = true)] [SerializeField, OdinSerialize]
        private ActionsSequence[] _actionsSequences;

        private ReactiveCommand<float> _onUpdate;

        /*public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;


            _ctx.OnUpdate.Subscribe(OnUpdate).AddTo(this);
            _ctx.OnCalculated.Subscribe(OnCalculated).AddTo(this);

            foreach (var action in _actions)
            {
                var actionCtx = new CompositeAction.Ctx
                {
                    OnUpdate = _onUpdate,
                    OnCalculated = _ctx.OnCalculated
                };
                action.SetCtx(actionCtx);
            }
        }*/

        private Ctx _ctx;

        private void OnValidate()
        {
            foreach (var sequence in _actionsSequences)
            {
                if (sequence!=null)
                {
                    sequence.Buttons = new List<ActionButton>();
                    foreach (var action in sequence.Actions)
                    {
                        if(action)
                            sequence.Buttons.Add(new ActionButton());
                        else
                            sequence.Buttons.Add(null);
                    }
                }
            }
        }

        public void OnUpdate(float deltaTime)
        {
            _onUpdate.Execute(deltaTime);
        }

        private void OnCalculated((MovePart part, Vector3? localPosition, Quaternion? localRotation) calculated)
        {
            Debug.Log($"<color=yellow>!!!</color> {this} OnCalculated");
            if (calculated.localPosition.HasValue)
            {
            }

            if (calculated.localRotation.HasValue)
            {
            }
        }
    }
}