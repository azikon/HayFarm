using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public struct GameStateChangeSignal
{
    public Enums.GameStates State { get; set; }

    public GameStateChangeSignal( Enums.GameStates state )
    {
        State = state;
    }
}