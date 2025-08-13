using System;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    internal interface ISunVectorRealtimeSource : IDisposable
    {
        event Action<Vector3> DataReceived;
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
