using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Fight.NewConfig.SO
{
    [CreateAssetMenu(menuName = "SoActions/ActionSequenceSO")]
    public class ActionSequenceSO : SerializedScriptableObject
    {
        [OdinSerialize, TableColumnWidth(120, false)]
        private List<SwipeDirections> _swipeDirections;

        [OdinSerialize, TableColumnWidth(50)] // 
        private List<ActionComposite> _actionComposite;

        private void OnValidate()
        {
            if (_swipeDirections.Count > _actionComposite.Count)
            {
                for (int i = 0; i < _swipeDirections.Count; i++)
                    if (i >= _actionComposite.Count)
                    {
                        _swipeDirections = _swipeDirections.GetRange(0, i - 1);
                        break;
                    }
            }
            else if (_swipeDirections.Count < _actionComposite.Count)
                for (int i = 0; i < _actionComposite.Count; i++)
                    if (i >= _swipeDirections.Count)
                        _swipeDirections.Add(SwipeDirections.None);
        }
    }
}