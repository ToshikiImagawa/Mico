// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using UnityEngine;

namespace MicoSample
{
    public class MicoSampleInjected : MonoBehaviour
    {
        [InjectField] private IMicoSampleLogger _logger = default;

        private void Awake()
        {
            _logger.Debug("MicoSampleInjectable Awake", this);
            _logger.Error("MicoSampleInjectable Awake", this);
        }
    }
}