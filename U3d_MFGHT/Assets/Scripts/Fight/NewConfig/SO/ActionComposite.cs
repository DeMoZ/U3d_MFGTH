using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Fight.NewConfig.SO
{
    [CreateAssetMenu(menuName = "SoActions/ActionComposite")]
    public class ActionComposite : SerializedScriptableObject
    {
        [OdinSerialize, TableColumnWidth(50)] private List<bool> _a;
        [OdinSerialize, TableColumnWidth(50)] private List<bool> _b;
        [OdinSerialize, TableColumnWidth(50)] private List<bool> _c;

        [TableColumnWidth(10), Button("Play")]
        private void Play()
        {
            
        }
    }
}