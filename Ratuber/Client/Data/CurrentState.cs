using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Ratuber.Client.Data
{
    internal static class CurrentState
    {
        public static TubeClient client;

        public static List<MicrophoneManager> MicrophoneManagers { get; private set; } = new();
    }
}
