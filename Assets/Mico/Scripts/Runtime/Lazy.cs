// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;

namespace Mico
{
    internal class Lazy<T>
    {
        private readonly Func<T> _getter;
        private bool _initialized;
        private T _catch;

        public T Value
        {
            get
            {
                if (!_initialized)
                {
                    _catch = _getter();
                }

                _initialized = true;
                return _catch;
            }
        }

        public Lazy(Func<T> getter)
        {
            _getter = getter;
            _initialized = false;
        }
    }


    internal class Lazy
    {
        private readonly Func<object> _getter;
        private bool _initialized;
        private object _catch;

        public object Value
        {
            get
            {
                if (!_initialized)
                {
                    _catch = _getter();
                }

                _initialized = true;
                return _catch;
            }
        }

        public Lazy(Func<object> getter)
        {
            _getter = getter;
            _initialized = false;
        }
    }
}