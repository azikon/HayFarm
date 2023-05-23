using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public interface ICharacterBase
{
    public string Type { get; set; }
    public float Damage { get; set; }
    public float MoveSpeed { get; set; }
    public float TurnSpeed { get; set; }
    public string PrefabPath { get; }
    public Transform PrefabParent { get; }
    //public Transform CurrentTransform { get; set; }
    public Transform ModelBody { get; set; }
}