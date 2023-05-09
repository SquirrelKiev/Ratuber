using System.Collections.Generic;

namespace SquirrelTube.Client.Data
{
    internal static class CurrentState
    {
        public static List<MicrophoneManager> MicrophoneManagers { get; set; } = new();
    }
}
