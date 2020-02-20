// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mico.Unity
{
    public sealed class GameObjectContext : MonoContext, IContextHelper
    {
        [SerializeField] private MonoContext parentContext;
        private DiContainer _container;

        public override DiContainer Container => _container ?? (_container = new DiContainer(parentContext.Container));

        void IContextHelper.SetParentContext(MonoContext defaultParentContext)
        {
            var parentGameObjectContext = this.GetComponentInParentOnly<MonoContext>();
            parentContext = parentGameObjectContext != null ? parentGameObjectContext : defaultParentContext;
            parentContext.AddContextChild(this);
        }

        void IContextHelper.Compile()
        {
            Precompile(Container);
        }

        void IContextHelper.Inject()
        {
            foreach (var component in GetComponentsInChildren<Component>())
            {
                if (component is MonoContext) continue;
                if (ContextChildren.Any(context =>
                    context.gameObject == component.gameObject ||
                    component.transform.IsChildOf(context.transform))) continue;
                Container.Inject(component);
            }
        }
    }

    internal interface IContextHelper
    {
        void SetParentContext(MonoContext defaultParentContext);
        void Compile();
        void Inject();
    }

    internal interface IContext
    {
        DiContainer Container { get; }
    }

    [DisallowMultipleComponent]
    public abstract class MonoContext : MonoBehaviour, IContext
    {
        [SerializeField] private MonoInstaller[] installers = new MonoInstaller[0];
        private readonly HashSet<MonoContext> _contextChildren = new HashSet<MonoContext>();
        protected MonoContext[] ContextChildren { get; private set; }
        public abstract DiContainer Container { get; }

        protected void Precompile(DiContainer container)
        {
            ContextChildren = _contextChildren.ToArray();
            _contextChildren.Clear();
            foreach (var installer in installers)
            {
                installer.InstallRegisters(container);
            }
        }

        internal void AddContextChild(MonoContext context)
        {
            if (_contextChildren.Contains(context))
                MicoAssert.Throw($"Disallow multiple component! : {context.FullName()}");
            _contextChildren.Add(context);
        }
    }
}