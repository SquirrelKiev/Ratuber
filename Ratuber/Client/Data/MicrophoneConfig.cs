using Microsoft.Xna.Framework.Audio;
using System;
using System.Linq;

namespace Ratuber.Client.Data
{
    public class MicrophoneConfig : UniqueObject
    {
        public float dbReference = -65f;
        public float dbDecay = -95f;
        public string microphoneName = string.Empty;

        public Microphone GetMicrophone()
        {
            var mic = Microphone.All.FirstOrDefault(x => x.Name == microphoneName);

            mic ??= Microphone.Default;

            return mic;
        }
        
        public static Microphone[] GetValidMicrophones(Microphone forcedMicrophone = null)
        {
            return Microphone.All
                .Where(potentialMic =>
                {
                    foreach (var existingMic in CurrentState.MicrophoneManagers)
                    {
                        if (existingMic.CurrentMicrophone == potentialMic && forcedMicrophone != potentialMic)
                        {
                            return false;
                        }
                    }

                    return true;
                }).ToArray();
        }
    }
}
