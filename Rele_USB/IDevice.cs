﻿using System;
using Models;

namespace Interfaces
{
    public interface IDevice : IDisposable
    {
        DeviceStatus Status { get; set; }
        string Port { get; set; }
        int Baud { get; set; }
        bool Init();
        bool Reset();
    }
}