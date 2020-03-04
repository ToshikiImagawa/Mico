// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context
{
    [IgnoreInjection]
    public sealed class GameObjectContext : Context
    {
        [SerializeField] private Context parentContext;
        private DiContainer _container;
        public Context ParentContext => parentContext;
        public override DiContainer Container => _container ?? (_container = new DiContainer(parentContext.Container));

        internal void SetParentContext(Context defaultParentContext)
        {
            if (parentContext != null) return;
            var parentGameObjectContext = this.GetComponentInParentOnly<Context>();
            parentContext = parentGameObjectContext != null ? parentGameObjectContext : defaultParentContext;
        }

        protected override void OnDestroy()
        {
            _container?.Dispose();
        }
    }
}