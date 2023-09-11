using System;

namespace SandKing.Engine.Core
{
    public class GameTimer
    {
        public GameTimer()
        {
            FrameEndTickCount64 = Environment.TickCount64;
        }

        public double DeltaTime { get; private set; }
        public double TotalSecondsElapsed { get; private set; }
        private long FrameEndTickCount64 { get; set; }

        public void Update()
        {
            DeltaTime = (Environment.TickCount64 - FrameEndTickCount64) / 1000.0;
            TotalSecondsElapsed += DeltaTime;
            FrameEndTickCount64 = Environment.TickCount64;
        }
    }
}
