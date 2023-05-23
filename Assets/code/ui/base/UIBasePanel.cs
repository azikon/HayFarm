using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace UI
{
    [Serializable]
    public abstract class UIBasePanel : UI.UIBase
    {
        protected UIBasePanel()
        {
        }

        [field: SerializeField]
        public Transform Panel { get; set; }

        public void Show()
        {
            IsShowed = true;
            Panel?.gameObject.SetActive( true );
            OnShow();
        }

        public void Hide()
        {
            Panel?.gameObject.SetActive( false );
            OnHide();
            IsShowed = false;
        }

        public abstract void OnShow();
        public abstract void OnHide();
    }
}