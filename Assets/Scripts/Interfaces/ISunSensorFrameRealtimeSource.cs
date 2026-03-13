using Sunsensor;
using System;

namespace Assets.Scripts.Interfaces
{
    public interface ISunSensorFrameRealtimeSource : IDisposable
    {
        event Action<Frame> FrameReceived;
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
