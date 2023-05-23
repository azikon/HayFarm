using Core.Managers;
using Core.Signals;

using UnityEngine;

using Zenject;

public class CharacterFarmer : CharacterBase, IFixedTickable
{
    private Rigidbody _ridigbody;
    private Transform _model;
    private Animator _animator;

    private CharacterCollisionTrigger _collisionTrigger;

    private Vector3 _movePosition;
    private bool _isMoving = false;

    public CharacterFarmer( string prefabPath, Transform prefabParent, float moveSpeed, float turnSpeed )
    {
        PrefabPath = prefabPath;
        PrefabParent = prefabParent;
        MoveSpeed = moveSpeed;
        TurnSpeed = turnSpeed;

        GameManager.Container.Resolve<TickableManager>().AddFixed( this, -10 );
        GameManager.Container.Resolve<SignalBus>().Subscribe<CharacterMovementDirectionSignal>( HandleMovement );

        LoadModel();
    }

    public override void OnModelLoaded()
    {
        _ridigbody = GameManager.Container.Resolve<SceneObjectsManager>().RootCharacterMain.GetComponent<Rigidbody>();
        _model = ModelBody.GetChild( 0 );
        _animator = _model.GetComponent<Animator>();

        _collisionTrigger = GameManager.Container.Resolve<SceneObjectsManager>().RootCharacterMain.GetComponent<CharacterCollisionTrigger>();
        _collisionTrigger.EnterTriggerEvent.AddListener( OnTriggerEnter );
        _collisionTrigger.ExitTriggerEvent.AddListener( OnTriggerExit );
    }

    public void FixedTick()
    {
        if ( _isMoving )
        {
            _ridigbody.velocity = _movePosition * MoveSpeed;
            _model.rotation = Quaternion.Slerp( _model.rotation, Quaternion.LookRotation( -_movePosition ), TurnSpeed * Time.fixedDeltaTime );
            _isMoving = false;
        }
        else
        {
            AnimationStateRun( 0f );
        }
    }

    private void HandleMovement( CharacterMovementDirectionSignal movementVector )
    {
        _movePosition = new Vector3( movementVector.DirectionVector.x, 0, movementVector.DirectionVector.y );
        if ( Mathf.Approximately( _movePosition.sqrMagnitude, 0 ) == false )
        {
            if ( _isMoving == false )
            {
                AnimationStateRun( 1f );
            }
            _isMoving = true;
        }
    }

    private void AnimationStateRun( float speed )
    {
        _animator.SetFloat( "Speed_f", speed );
    }

    private void OnTriggerEnter( Collider collider )
    {
        GameManager.Container.Resolve<SignalBus>().TryFire<CharacterCollisionTriggerSignal>( new CharacterCollisionTriggerSignal( collider, true ) );
    }

    private void OnTriggerExit( Collider collider )
    {
        GameManager.Container.Resolve<SignalBus>().TryFire<CharacterCollisionTriggerSignal>( new CharacterCollisionTriggerSignal( collider, false ) );
    }
}





//    public GameObject prefabBox;
//    public GameObject boxParent;
//    public BF_MowingManager mowingManager;
//    public int lastMowingmarkersPos;

//    public float animSpeedScale = 1f;
//    public float animPathsFirstScale = 1f;
//    public float animPathsSecondScale = 1f;

//    public bool animStarted;

//    void Update()
//    {
//        if ( lastMowingmarkersPos != mowingManager.markersPos.Count )
//        {
//            lastMowingmarkersPos = mowingManager.markersPos.Count;
//            CreateGrassBox();
//        }
//    }

//    private void CreateGrassBox()
//    {
//        if ( animStarted )
//        {
//            return;
//        }
//        animStarted = true;
//        GameObject box = Instantiate( prefabBox );
//        box.transform.SetParent( boxParent.transform, false );
//        box.transform.localPosition = Vector3.zero;


//        box.transform.DOScale( new Vector3( 0.5f, 0.5f, 0.5f ), animSpeedScale )
//        .OnComplete( () =>
//        {
//            Vector3[] paths = new Vector3[]
//            {
//                Vector3.zero,
//                new Vector3( 0f, 0f, 0f ),
//                new Vector3( 0f, 3.5f, 0f ),
//                new Vector3( 0f, 2.29f, 0f ),
//                new Vector3( 0f, 2.9f, 0f ),
//                new Vector3( 0f, 2.29f, 0f ),
//                new Vector3( 0f, 2.6f, 0f ),
//                new Vector3( 0f, 2.29f, 0f ),
//            };

//            box.transform.DOLocalPath( paths, animPathsFirstScale )
//            .OnComplete( () =>
//            {
//                Vector3[] paths = new Vector3[]
//                {
//                    new Vector3( 0f, 2.29f, 0f ),
//                    new Vector3( 0f, 2.71f, 0.59f ),
//                    new Vector3( 0f, 2.77f, 1.45f ),
//                    new Vector3( 0f, 2.24f, 2.32f ),
//                    new Vector3( 0f, 1.42f, 2.55f ),
//                    new Vector3( 0.289f, 0.65f, 2.38f ),
//                    new Vector3( 0.589f, 0.23f, 2.21f ),
//                    new Vector3( 0.589f, 0.23f, 1.883f ),
//                };

//                box.transform.DOLocalPath( paths, animPathsSecondScale )
//                .OnComplete( () =>
//                {
//                    box.transform.DOScale( new Vector3( 0.1f, 0.1f, 0.1f ), animSpeedScale );
//                    animStarted = false;
//                } );
//            } );
//        } );
//    }
//}