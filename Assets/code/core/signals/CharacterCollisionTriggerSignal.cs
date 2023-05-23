using UnityEngine;

namespace Core.Signals
{
    public struct CharacterCollisionTriggerSignal
    {
        public Collider TriggeredCollider { get; private set; }
        public bool IsEnter { get; private set; }

        public CharacterCollisionTriggerSignal( Collider triggeredCollider, bool isEnter )
        {
            TriggeredCollider = triggeredCollider;
            IsEnter = isEnter;
        }
    }
}