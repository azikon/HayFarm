using Core.Managers;

using DG.Tweening;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelUserLevel : UIBasePanel
    {
        [SerializeField]
        private int _userLevelValue;
        [SerializeField]
        private int _userExperienceValue;
        [SerializeField]
        private int _userMaxExperienceValue;

        [SerializeField]
        private TMP_Text _userLevel;
        [SerializeField]
        private TMP_Text _userExperience;
        [SerializeField]
        private Slider _userExperienceSlider;

        [SerializeField]
        private Button buttonOpenUserInfo;

        [SerializeField]
        private TMP_Text _userName;

        [SerializeField]
        private Transform _parentAnimatedItems;

        [SerializeField]
        private Transform _targetAnimatedItems;

        private UserInfoData _userInfoData;

        private readonly List<int> _experienceCollection = new List<int>
        {
            0, 11, 7, 9, 28, 50, 190, 370, 490, 790, 960, 1180, 1550, 1790, 2270, 2880,
            3270, 4120, 4740, 5610, 6320, 7750, 9140, 10830, 12200, 14000, 15700, 18300,
            20500, 23400, 26100, 29400, 32400, 35800, 38700, 42200, 45200, 48900, 54900,
            61800, 67900, 75000, 81500, 94400, 100800, 110900, 119500, 129500, 138000,
            148400, 157000, 168000, 179000, 190000, 201000, 212000, 223000, 234000,
            245000, 256000, 267000, 278000, 289000, 300000, 3017000, 3028000, 3039000, 3050000,
            3061000, 3072000, 3083000, 3094000, 3105000, 3116000, 3127000, 3138000, 3149000, 3160000,
            3171000, 3182000, 3193000, 3204000, 3215000, 3226000, 3237000, 3248000, 3259000, 3270000,
            3281000, 3292000, 3303000, 3314000, 3325000, 3336000, 3347000, 3358000, 3369000, 3380000,
            3391000, 3402000, 3413000, 3424000, 3435000, 3446000, 3457000, 3468000, 3479000, 3490000,
            3501000, 3512000, 3523000, 3534000, 3545000, 3556000, 3567000, 3578000, 3589000, 3600000,
            3611000, 3622000, 3633000, 3644000, 3655000, 3666000, 3677000, 3688000, 3699000, 3710000,
            3721000, 3732000, 3743000, 3754000, 3765000, 3776000, 3787000, 3798000, 3809000, 3820000,
            3831000, 3842000, 3853000, 3864000, 3875000, 3886000, 3897000, 3908000, 3919000, 3930000,
            3941000, 3952000, 3963000, 3974000, 3985000, 3996000, 4007000, 4018000, 4029000, 4040000,
            4051000, 4062000, 4073000, 4084000, 4095000, 4106000, 4117000, 4128000, 4139000, 4150000,
            4161000, 4172000, 4183000, 4194000, 4205000, 4216000, 4227000, 4238000, 4249000, 4260000,
            4271000, 4282000, 4293000, 4304000, 4315000, 4326000, 4337000, 4348000, 4359000, 4370000,
            4381000, 4392000, 4403000, 4414000, 4425000, 4436000, 4447000, 4458000, 4469000, 4480000,
            4491000, 4502000, 4513000, 4524000, 4535000, 4546000, 4557000, 4568000, 4579000, 4590000,
            4601000, 4612000, 4623000, 4634000, 4645000, 4656000, 4667000, 4678000, 4689000, 4700000,
            4711000, 4722000, 4733000, 4744000, 4755000, 4766000, 4777000, 4788000, 4799000, 4810000,
            4821000, 4832000, 4843000, 4854000, 4865000, 4876000, 4887000, 4898000, 4909000, 4920000,
            4931000, 4942000, 4953000, 4964000, 4975000, 4986000, 4997000, 5008000, 5019000, 5030000,
            5041000, 5052000, 5063000, 5074000, 5085000, 5096000, 5107000,
        };

        public override void Initialize()
        {
            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );
            GameManager.Container.Resolve<SignalBus>().Subscribe<UserExperienceChangeSignal>( HandleExperienceChange );

            buttonOpenUserInfo.onClick.AddListener( () =>
            {
                GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_user_info" ).Show();
            } );

            base.Initialize();
        }

        public override void OnShow()
        {
            UpdateData();
        }

        public override void OnHide()
        {
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( UserInfoData ).Name == localDataSignal.TypeName )
            {
                _userInfoData = localDataSignal.Data as UserInfoData;
                UpdateData();
            }
        }

        private void HandleExperienceChange( UserExperienceChangeSignal experienceChange )
        {
            int addExperience = experienceChange.AddExperience;

            StaticCoroutine.Delay
            ( 
                () =>
                {
                    AddExperienceAnimation( "prefabs/ui/ui_panel_user_level/ui_panel_level_item_animated", addExperience, () =>
                    {
                        if ( _userInfoData.UserExperience + addExperience >= _experienceCollection[ _userInfoData.UserLevel ] )
                        {
                            int newLevelExperience = _experienceCollection[ _userInfoData.UserLevel ] - _userInfoData.UserExperience;
                            _userInfoData.UserLevel += 1;
                            _userInfoData.UserExperience = 0;
                            _userInfoData.UserExperience += addExperience - newLevelExperience;
                        }
                        else
                        {
                            _userInfoData.UserExperience += addExperience;
                        }
                        UpdateData();
                    } );
                },
                Random.Range( 0.05f, 0.15f )
             );
        }

        private void UpdateData()
        {
            if ( IsShowed == true )
            {
                _userName.text = _userInfoData.UserName;
                _userLevel.text = _userInfoData.UserLevel.ToString();
                _userExperience.text = _userInfoData.UserExperience.ToString() + "/" + _experienceCollection[ _userInfoData.UserLevel ].ToString();
                _userExperienceSlider.value = ( float )_userInfoData.UserExperience / ( float )_experienceCollection[ _userInfoData.UserLevel ];
            }
        }

        private void AddExperienceAnimation( string prefabPath, int addedExperience, System.Action returnMethod )
        {
            Transform item = Instantiate( Resources.Load<GameObject>( prefabPath ) ).transform;
            item.SetParent( _parentAnimatedItems, false );

            item.Find( "text_experience" ).GetComponent<TMP_Text>().text = "+" + addedExperience.ToString();

            Camera envCamera = GameManager.Container.Resolve<SceneObjectsManager>().EnvironmentCamera;
            Transform character = GameManager.Container.Resolve<SceneObjectsManager>().RootCharacterMain.transform;

            Vector3 startWorldPosition = envCamera.WorldToScreenPoint( character.position );
            Vector3 targetScreenPosition = _targetAnimatedItems.position;

            item.position = startWorldPosition;
            Vector3[] path = GetAnimationPath( startWorldPosition, targetScreenPosition );
            ( item as RectTransform ).DOPath( path, 1f ).OnComplete( () => { returnMethod(); Destroy( item.gameObject ); } );
        }

        Vector3[] GetAnimationPath( Vector3 startWorldPosition, Vector3 targetScreenPosition )
        {
            Vector3[] path = new Vector3[ 3 ];
            path[ 0 ] = startWorldPosition;
            path[ 0 ] = new Vector3( path[ 0 ].x, path[ 0 ].y, 0.0f );
            path[ 1 ] = new Vector3( path[ 0 ].x + 280f, path[ 0 ].y - 120f, 0.0f );
            path[ 2 ] = targetScreenPosition;
            path[ 2 ] = new Vector3( path[ 2 ].x, path[ 2 ].y, 0.0f );
            return path;
        }
    }
}