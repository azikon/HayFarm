using Core.Managers;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PanelMainMenu : UIBasePanel
    {
        [SerializeField]
        private Button buttonPlay;

        [SerializeField]
        private Button buttonIndox;
        [SerializeField]
        private Button buttonRewards;
        [SerializeField]
        private Button buttonChat;
        [SerializeField]
        private Button buttonRanking;
        [SerializeField]
        private Button buttonQuests;
        [SerializeField]
        private Button buttonStage;
        [SerializeField]
        private Button buttonPass;


        public override void Initialize()
        {
            buttonPlay.onClick.AddListener( OnClickPlay );

            buttonIndox.onClick.AddListener( OnClickInbox );
            buttonRewards.onClick.AddListener( OnClickRewards );
            buttonChat.onClick.AddListener( OnClickChat );
            buttonRanking.onClick.AddListener( OnClickRanking );
            buttonQuests.onClick.AddListener( OnClickQuests );
            buttonStage.onClick.AddListener( OnClickStage );
            buttonPass.onClick.AddListener( OnClickPass );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        private void OnClickPlay()
        {
            GameManager.Container.Resolve<StateManager>().ChangeState( Enums.GameStates.InGame );
        }

        private void OnClickInbox()
        {
            GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_inbox" ).Show();
        }

        private void OnClickRewards()
        {
            GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_reward_month" ).Show();
        }

        private void OnClickChat()
        {
            GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_chat" ).Show();
        }

        private void OnClickRanking()
        {
            GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_ranking" ).Show();
        }

        private void OnClickQuests()
        {
            GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_missions" ).Show();
        }

        private void OnClickStage()
        {
            GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_stage_select" ).Show();
        }

        private void OnClickPass()
        {
            GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_pass" ).Show();
        }
    }
}