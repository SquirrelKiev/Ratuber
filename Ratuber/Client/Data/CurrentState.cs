using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Ratuber.Client.Data
{
    public static class CurrentState
    {
        public static TubeClient client;
        public static Random random { get; private set; } = new();

        public static List<MicrophoneManager> MicrophoneManagers { get; private set; } = new();
    }
}
