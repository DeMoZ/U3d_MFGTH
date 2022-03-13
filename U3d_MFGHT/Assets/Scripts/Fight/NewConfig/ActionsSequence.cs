using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Fight.NewConfig
{
    [Serializable]
    public class ActionsSequence {
        [SerializeField] private List<CompositeAction> _actions;
        [SerializeField] private List<ActionButton> _buttons;

        public List<CompositeAction> Actions
        {
            get => _actions;
            set => _actions = value;
        }

        public List<ActionButton> Buttons
        {
            get => _buttons;
            set => _buttons = value;
        }


        [Button("PlayAll")]
        private void PlayAll()
        {
            //
        }
        
        /*[SerializeField]
        private string b;                              // Will be serialized by Unity and shown in the inspector.

        [OdinSerialize]                                // Will be serialized by Odin and shown in the inspector.
        private string c;                              // But a good rule is to let Unity serialize it whenever you can.

        [SerializeField, OdinSerialize]
        private string d;                              // WARNING: This will be serialized by both Unity and Odin

        [OdinSerialize]
        public string e;                               // WARNING: This will be serialized by both Unity and Odin

        [OdinSerialize]
        private Dictionary<string, AudioClip> h;       // Will be serialized by Odin and shown in the inspector.

        [SerializeField]
        private Dictionary<string, AudioClip> i;       // Will be serialized by Odin and shown in the inspector.
          
        [ShowInInspector]                              
        private Dictionary<string, AudioClip> j;       // This will be shown in the inspecotr, but will not be serialized.

        [OdinSerialize]
        private AudioClip k { get; set; }              // Will be serialized by Odin and shown in the inspector.

        [ShowInInspector]
        public AudioClip l { get; set; } */              // This will be shown in the inspecotr, but will not be serialized.
    }

    [Serializable]
    public class ActionButton
    {
        private bool _exist;
        public ActionButton()
        {
            _exist = true;
        }
        [ShowIf(nameof(_exist)), Button("Play")]
        private void Play()
        {
            Debug.Log("Play");
        }
    }
}