using System.Collections.Generic;

namespace Ratuber.Client.Data
{
    internal static class CurrentState
    {
        public static List<MicrophoneManager> MicrophoneManagers { get; set; } = new();
    }
}
