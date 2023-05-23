using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PanelStageSelect : UIBasePanel
    {
        [SerializeField]
        private Button buttonClose;

        public override void Initialize()
        {
            buttonClose.onClick.AddListener( () => { Hide(); } );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }
    }
}