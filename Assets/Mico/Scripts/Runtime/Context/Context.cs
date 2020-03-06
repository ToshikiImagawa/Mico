using System.Collections.Generic;
using Mico.Context.Internal;
using UnityEngine;

namespace Mico.Context
{
    [IgnoreInjection, DisallowMultipleComponent]
    public abstract class Context : MonoBehaviour, IContext
    {
        [SerializeField] private MonoInstaller[] installers = new MonoInstaller[0];
        private DiContainer _container;

        public DiContainer Container => _container;
        internal abstract IContext ParentContext { get; }
        IContext IContext.ParentContext => ParentContext;
        IEnumerable<IInstaller> IContext.Installers => installers;

        void IContext.SetContainer(DiContainer container)
        {
            _container = container;
        }

        protected virtual void OnDestroy()
        {
            _container?.Dispose();
            installers = null;
        }
    }
}