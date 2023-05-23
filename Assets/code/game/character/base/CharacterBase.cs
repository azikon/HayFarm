using UnityEngine;

public abstract class CharacterBase : ICharacterBase
{
    public string Type { get; set; }
    public float Damage { get; set; }
    public float MoveSpeed { get; set; }
    public float TurnSpeed { get; set; }
    public string PrefabPath { get; set; }
    public Transform PrefabParent { get; set; }
    public Transform ModelBody { get; set; }

    public virtual void Initialize() { }

    public void LoadModel()
    {
        GameObject prefabObject = Resources.Load<GameObject>( PrefabPath );
        GameObject instatiated = UnityEngine.Object.Instantiate( prefabObject );
        instatiated.transform.SetParent( PrefabParent.transform, false );

        ModelBody = instatiated.transform;

        OnModelLoaded();
    }

    public abstract void OnModelLoaded();
}