﻿using System;

namespace Interfaces
{
    public interface IKeyboard : IDevice
    {
        event Action<int> PressedKey;

        void SetLight(int value);
        long DeviceId { get; }

        void Enable();
        void Disable();
    }
}