using System;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISunVectorRealtimeSource : IDisposable
    {
        event Action<Vector3> VectorReceived;
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
