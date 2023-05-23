using UnityEngine;

namespace Core.Signals
{
    public struct CharacterMovementDirectionSignal
    {
        public Vector3 DirectionVector { get; set; }

        public CharacterMovementDirectionSignal( Vector3 directionVector )
        {
            DirectionVector = directionVector;
        }
    }
}