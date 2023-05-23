using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentBase : IEnvironmentBase
{
    public string Type { get; set; }
    public string PrefabPath { get; set; }
    public Transform PrefabParent { get; set; }

    public virtual void Initialize() { }

    public void LoadModel()
    {
        GameObject prefabObject = Resources.Load<GameObject>( PrefabPath );
        GameObject instatiated = UnityEngine.Object.Instantiate( prefabObject );
        instatiated.transform.SetParent( PrefabParent.transform, false );

        OnModelLoaded();
    }

    public abstract void OnModelLoaded();
}