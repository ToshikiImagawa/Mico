using System;
using UnityEngine;

namespace Mico.Context
{
    [IgnoreInjection]
    [DisallowMultipleComponent]
    public abstract class Context : MonoBehaviour
    {
        [SerializeField] private MonoInstaller[] installers = new MonoInstaller[0];
        public abstract DiContainer Container { get; }

        internal void Compile()
        {
            foreach (var installer in installers)
            {
                if (installer != null) installer.InstallRegisters(Container);
            }
            Container.Compile();
        }

        protected abstract void OnDestroy();
    }
}