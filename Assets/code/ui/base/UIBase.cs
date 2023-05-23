using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace UI
{
    [Serializable]
    public abstract class UIBase : MonoBehaviour
    {
        [field: SerializeField]
        public string Name { get; set; }
        [field: SerializeField]
        public bool IsInited { get; set; }

        [field: SerializeField]
        public bool IsShowed { get; set; }

        public virtual void Initialize() { }
    }
}