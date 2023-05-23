using System;

using Zenject;

namespace Core.Managers
{
    public abstract class BaseManager : IInitializable, IDisposable
    {
        [Inject] private protected DiContainer _container;

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