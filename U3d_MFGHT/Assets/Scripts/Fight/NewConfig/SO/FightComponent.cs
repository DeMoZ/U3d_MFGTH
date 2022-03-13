using Sirenix.OdinInspector;
using UnityEngine;

namespace Fight.NewConfig.SO
{
    public class FightComponent : //MonoBehaviour
        SerializedMonoBehaviour
    {
        [HorizontalGroup] [SerializeField] [TableList]
        private ActionMap _actionMap;

        [HorizontalGroup]
        [Button(nameof(NewMap))]
        private void NewMap()
        {
            Debug.Log("New Map Created");
        }
    }
}