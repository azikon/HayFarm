using UnityEngine;
using UnityEngine.Events;

public class CharacterCollisionTrigger : MonoBehaviour
{
    public CollisionEvent EnterTriggerEvent;

    public CollisionEvent ExitTriggerEvent;

    private void Awake()
    {
        EnterTriggerEvent = new CollisionEvent();
        ExitTriggerEvent = new CollisionEvent();
    }

    private void OnTriggerEnter( Collider other )
    {
        EnterTriggerEvent.Invoke( other );
    }

    void OnTriggerExit( Collider other )
    {
        ExitTriggerEvent.Invoke( other );
    }
}

[System.Serializable]
public class CollisionEvent : UnityEvent<Collider>
{
}