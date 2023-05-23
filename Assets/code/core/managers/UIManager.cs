using System;
using System.Collections.Generic;

using UI;

using Zenject;

namespace Core.Managers
{
    public sealed class UIManager : BaseManager
    {

        private Dictionary<string, UI.UIBase> _collectionUI;
        private List<UIBase> _openedUI;

        protected override void OnLoad()
        {
            GameManager.Container.Resolve<SignalBus>().Subscribe<GameStateChangeSignal>( HandleState );

            LoadData();

            base.OnLoad();
        }

        private void LoadData()
        {
            _collectionUI = new Dictionary<string, UI.UIBase>();
            _openedUI = new List<UIBase>();

            UI.UIBase[] foundUIArray = GameManager.Container.Resolve<SceneObjectsManager>().RootUI.GetComponentsInChildren<UI.UIBase>();
            for ( int i = 0; i < foundUIArray.Length; i++ )
            {
                foundUIArray[ i ].Initialize();
                Add( foundUIArray[ i ] );
            }
        }

        private void HandleState( GameStateChangeSignal changedState )
        {
            if ( changedState.State == Enums.GameStates.Loading )
            {
                Get<UIBasePanel>( "ui_panel_loading" ).Show();
            }
            else if ( changedState.State == Enums.GameStates.MainMenu )
            {
                Get<UIBasePanel>( "ui_panel_character_joystick" ).Hide();

                Get<UIBasePanel>( "ui_panel_loading" ).Hide();
                Get<UIBasePanel>( "ui_panel_main_menu" ).Show();

                Get<UIBasePanel>( "ui_panel_user_level" ).Show();
                Get<UIBasePanel>( "ui_panel_user_currency" ).Show();
                Get<UIBasePanel>( "ui_panel_settings" ).Show();
                Get<UIBasePanel>( "ui_panel_inventory" ).Show();
            }
            else if ( changedState.State == Enums.GameStates.InGame )
            {
                Get<UIBasePanel>( "ui_panel_main_menu" ).Hide();
                Get<UIBasePanel>( "ui_panel_character_joystick" ).Show();
                Get<UIBasePanel>( "ui_panel_inventory_gather" ).Show();
                Get<UIBasePanel>( "ui_panel_play_pause" ).Show();
            }
            else if ( changedState.State == Enums.GameStates.IsPause )
            {
                Get<UIBasePanel>( "ui_panel_character_joystick" ).Hide();
            }
        }

        public UIBase Get( string name )
        {
            if ( string.IsNullOrEmpty( name ) == true )
            {
                throw new ArgumentNullException( "UI name " );
            }
            if ( _collectionUI.ContainsKey( name ) == true )
            {
                return _collectionUI[ name ];
            }
            return null;
        }

        public T Get<T>( string uiName ) where T : class
        {
            return Get( uiName ) as T;
        }

        public void Add( UI.UIBase ui )
        {
            if ( null == ui )
            {
                throw new ArgumentNullException( "UI" );
            }
            lock ( _collectionUI )
            {
                if ( _collectionUI.ContainsKey( ui.Name ) == true )
                {
                    throw new ArgumentException( "UI is already registered", ui.Name );
                }
                _collectionUI.Add( ui.Name, ui );
            }
        }

        public void Remove( string name )
        {
            if ( string.IsNullOrEmpty( name ) == true )
            {
                throw new ArgumentNullException( "UI name" );
            }
            lock ( _collectionUI )
            {
                if ( _collectionUI.ContainsKey( name ) == true )
                {
                    _collectionUI.Remove( name );
                }
            }
        }

        public void AddOpened( UIBase uIBase )
        {
            if ( _openedUI.Contains( uIBase ) == false )
            {
                _openedUI.Add( uIBase );
            }
        }

        public void RemoveOpened( UIBase uIBase )
        {
            if ( _openedUI.Contains( uIBase ) )
            {
                _openedUI.Remove( uIBase );
            }
        }

        public void Show( UIBase uIBase, bool addToOpened = false )
        {
            if ( null != uIBase )
            {
                if ( addToOpened == true )
                {
                    AddOpened( uIBase );
                }
            }
        }

        public void Hide( UIBase uIBase, bool removeFromOpened = false )
        {
            if ( null != uIBase )
            {
                if ( removeFromOpened == true )
                {
                    RemoveOpened( uIBase );
                }
            }
        }

        public void HideAll()
        {
            if ( null != _openedUI )
            {
                _openedUI.ForEach( x =>
                {
                    ( x as UIBasePanel ).Hide();
                } );
                _openedUI.Clear();
            }
        }

        public override void Dispose()
        {
        }
    }
}