using System;

using UnityEngine;

using Zenject;

namespace Core.Managers
{
    public abstract class BaseManagerBehaviour : MonoBehaviour, IInitializable, IDisposable
    {
        [Inject] protected DiContainer diContainer;

        public delegate void StartComplete();

        public void Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {

        }

        public void Load()
        {
            OnLoad();
        }

        protected virtual void OnLoad()
        {
        }

        public virtual void Dispose()
        {

        }
    }
}