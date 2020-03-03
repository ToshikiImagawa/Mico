using System.Collections.Generic;

namespace Mico.Internal
{
    internal class RegisterInfoPool : IPool<RegisterInfo>
    {
        private readonly Queue<RegisterInfo> _cache = new Queue<RegisterInfo>();

        public RegisterInfo Spawn()
        {
            return _cache.Count > 0 ? _cache.Dequeue() : new RegisterInfo(this);
        }

        public void Despawn(object value)
        {
            if (value is RegisterInfo registerInfo)
            {
                _cache.Enqueue(registerInfo);
                return;
            }

            MicoAssert.Throw("Assert!");
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}