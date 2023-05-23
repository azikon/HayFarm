using System.Collections;

using UnityEngine;

public static class StaticCoroutine
{
    private class CoroutineHolder : MonoBehaviour { }

    private static CoroutineHolder _runner;
    private static CoroutineHolder Runner
    {
        get
        {
            if ( _runner == null )
            {
                _runner = new GameObject( "Static Coroutine" ).AddComponent<CoroutineHolder>();
                Object.DontDestroyOnLoad( _runner );
            }
            return _runner;
        }
    }

    public static Coroutine StartCoroutine( IEnumerator coroutine )
    {
        return Runner.StartCoroutine( coroutine );
    }

    public static void Delay( System.Action returnMethod, float time )
    {
        StartCoroutine( DelayAndCall( returnMethod, time ) );
    }

    public static void Delay( this MonoBehaviour _monoBehaviour, System.Action returnMethod, float time )
    {
        _monoBehaviour.StartCoroutine( DelayAndCall( returnMethod, time ) );
    }

    static IEnumerator DelayAndCall( System.Action returnMethod, float time )
    {
        yield return new WaitForSeconds( time );
        returnMethod();
    }
}