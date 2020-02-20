// Mico.Unity C# reference source
// Copyright (c) 2016-2020 COMCREATE. All rights reserved.

using System.Linq;
using UnityEngine;

namespace Mico.Unity
{
    public sealed class MainGameObjectContext : MonoContext, IContextHelper
    {
        private static MainGameObjectContext _main;
        public override DiContainer Container { get; } = new DiContainer();

        private void Awake()
        {
            if (_main == null)
            {
                _main = this;
                var contextSceneAll = this.GetComponentsInScene<IContextHelper>();
                foreach (var context in contextSceneAll) context.SetParentContext(this);
                foreach (var context in contextSceneAll) context.Compile();
                foreach (var context in contextSceneAll) context.Inject();
                return;
            }

            MicoAssert.Throw("");
        }

        private void OnDestroy()
        {
            if (_main == this) _main = null;
        }

        public void SetParentContext(MonoContext defaultParentContext)
        {
        }

        public void Compile()
        {
            Precompile(Container);
        }

        public void Inject()
        {
            foreach (var component in this.GetComponentsInScene<Component>())
            {
                if (component is MonoContext) continue;
                if (ContextChildren.Any(context =>
                    context.gameObject == component.gameObject ||
                    component.transform.IsChildOf(context.transform))) continue;
                Container.Inject(component);
            }
        }
    }
}