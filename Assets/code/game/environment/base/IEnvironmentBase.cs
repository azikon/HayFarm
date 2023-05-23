using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvironmentBase
{
    public string Type { get; set; }
    public string PrefabPath { get; }
    public Transform PrefabParent { get; }
}