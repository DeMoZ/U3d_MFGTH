using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fight.NewConfig.SO
{
    [CreateAssetMenu(menuName = "SoActions/ActionMap")]
    public class ActionMap : SerializedScriptableObject
    {
        [TableList(ShowIndexLabels = true)] [SerializeField]
        private List<ActionSequence> _actionSequences = new List<ActionSequence>();

        private void OnValidate()
        {
            foreach (var sequence in _actionSequences)
            {
                foreach (var actionSet in sequence.ActionSets)
                {
                    foreach (var actionMove in actionSet.ActionMoves)
                    {
                        actionMove.StartOffset = Mathf.Clamp(actionMove.StartOffset, 0, actionSet.Duration - 0.001f);
                        actionMove.Duration = Mathf.Clamp(actionMove.Duration, 0.001f, actionSet.Duration - actionMove.StartOffset);
                    }
                }
            }
        }
    }

    [Serializable]
    public class ActionSequence
    {
        [TableList(ShowIndexLabels = false)] [LabelText("@LabelName()")] [SerializeField]
        private List<ActionSet> _actionSets = new List<ActionSet>();

        public List<ActionSet> ActionSets
        {
            get => _actionSets;
            set => _actionSets = value;
        }

        private string LabelName()
        {
            var label = "[ ";
            foreach (var action in _actionSets)
                label += Util.DirectionSymbol(action.Direction) + " ";

            label += "]";

            return label;
        }
    }

    [Serializable]
    public class ActionSet
    {
        [SerializeField] [VerticalGroup("Base")] [TableColumnWidth(85, false)] [HideLabel]
        private SwipeDirections _direction;

        [SerializeField] [VerticalGroup("Base")] [LabelText("Dur."), Tooltip("Duration")] [SuffixLabel("sec.", true)]
        private float _duration = 1f;

        [SerializeField] [VerticalGroup("Moves")] [TableList] [LabelText("Moves in one action")]
        private List<ActionMove> _actionMoves = new List<ActionMove>();

        public SwipeDirections Direction => _direction;
        public float Duration => _duration;

        public List<ActionMove> ActionMoves
        {
            get => _actionMoves;
            set => _actionMoves = value;
        }
    }

    [Serializable]
    public class ActionMove
    {
        [VerticalGroup("Timing")] [SerializeField] [TableColumnWidth(100, false)] [LabelWidth(35)]
        [LabelText("Offset"), Tooltip("Start Offset")] [SuffixLabel("sec.", true)]
        private float _startOffset = 0;
        [VerticalGroup("Timing")] [SerializeField][LabelWidth(35)]
        [LabelText("Dur."), Tooltip("Duration")] [SuffixLabel("sec.", true)]
        private float _duration = 1;
        
        [VerticalGroup("Points")] [SerializeField] [LabelText("From"), LabelWidth(30)] [TableColumnWidth(220, false)]
        private Vector3 _fromLocalPoint;
        [VerticalGroup("Points")] [SerializeField] [LabelText("To"), LabelWidth(30)]
        private Vector3 _toLocalPoint;

        [VerticalGroup("Curves")] [SerializeField] [TableColumnWidth(100, false)]
        private AnimationCurve _durationCurve;
        [VerticalGroup("Curves")] [SerializeField]
        private AnimationCurve _amplitudeCurve;

        [VerticalGroup("Target")] [SerializeField] [TableColumnWidth(100, false)]
        private MovePart _movePart;
        
        [VerticalGroup("Target")] [SerializeField] [TableColumnWidth(100, false)]
        private MoveType _moveType;
        
        public float StartOffset
        {
            get => _startOffset;
            set => _startOffset = value;
        }

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        [Button] [VerticalGroup("Buttons")]
        private void Show()
        {
            
        }

        [Button] [VerticalGroup("Buttons")]
        private void Clear()
        {
            
        }
        
        /*
         [HorizontalGroup("g1")] [VerticalGroup("v1")]
            [HorizontalGroup("g2")][VerticalGroup("v1")]
         */
    }

    public class Util
    {
        public static string DirectionSymbol(SwipeDirections swipeDirection)
        {
            return swipeDirection switch
            {
                SwipeDirections.None => "\u22A1", // 0
                SwipeDirections.ToLeft => "\u2190", // <        
                SwipeDirections.ToUp => "\u2191", // ^        
                SwipeDirections.ToRight => "\u2192", // >        
                SwipeDirections.ToDown => "\u2193", // v        
                SwipeDirections.Thrust => "\u00D7", // x
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static void Log()
        {
            Debug.Log(""
                      + "\u00D7" // x
                      + "\u22A1" // 0
                      + "\u2190" // <
                      + "\u2191" // ^
                      + "\u2192" // >
                      + "\u2193" // v
                      + "\u2194" // <>
                      + "\u2195" // ^v
                      + "\u2196" // /
                      + "\u2197" // \ 
                      + "\u2198" // /
                      + "\u2199" // \
            );
        }
    }
}