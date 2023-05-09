using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Ratuber.Client.Data;
using System;
using System.Linq;

namespace Ratuber.Client
{
    public class MicrophoneManager : UniqueObject
    {
        public const double minDb = -120d;

        public double RmsVal { get; private set; }
        public double DbValRaw { get; private set; } = minDb;
        public double DbVal { get; private set; } = minDb;

        public bool IsTalking { get; private set; }

        private byte[] sampleData;

        private Microphone currentMicrophone;
        private bool isBufferReady = false;
        private double previousDb = minDb;
        public MicrophoneConfig MicConfig { get; private set; }

        public Microphone CurrentMicrophone => currentMicrophone;

        public MicrophoneManager Initialize(MicrophoneConfig micConfig)
        {
            MicConfig = micConfig;

            ResetMicrophone(MicConfig.GetMicrophone());

            return this;
        }

        public void ResetMicrophone(Microphone microphone)
        {
            isBufferReady = false;

            currentMicrophone = microphone;

            MicConfig.microphoneName = microphone.Name;

            if (currentMicrophone.State == MicrophoneState.Stopped)
                currentMicrophone.Start();

            currentMicrophone.BufferReady += CurrentMicrophone_BufferReady;
        }

        private void CurrentMicrophone_BufferReady(object sender, EventArgs e)
        {
            sampleData = new byte[CurrentMicrophone.GetSampleSizeInBytes(CurrentMicrophone.BufferDuration)];

            isBufferReady = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!isBufferReady) return;

            currentMicrophone.GetData(sampleData);

            double sum = 0;
            for (var i = 0; i < sampleData.Length; i += 2)
            {
                double sample = BitConverter.ToInt16(sampleData, i) / 32768.0;
                sum += sample * sample;
            }

            RmsVal = Math.Sqrt(sum / (sampleData.Length / 2));
            DbValRaw = 20 * Math.Log10(RmsVal);

            DbVal = Math.Max(Math.Max(DbValRaw, previousDb), minDb);

            previousDb = DbVal;
            previousDb += MicConfig.dbDecay * gameTime.ElapsedGameTime.TotalSeconds;
        }



        public void RenderMicrophoneEditor()
        {
            var validMics = MicrophoneConfig.GetValidMicrophones(CurrentMicrophone);

            var currentMicrophone = Array.IndexOf(validMics, CurrentMicrophone);

            ImGui.Combo($"Microphone##{GetUniqueId()}", ref currentMicrophone, validMics.Select(x => x.Name).ToArray(), validMics.Length);

            if (CurrentMicrophone != validMics[currentMicrophone])
            {
                ResetMicrophone(validMics[currentMicrophone]);
            }

            ImGui.ProgressBar((float)RmsVal, new System.Numerics.Vector2(0, 0), RmsVal.ToString("F2"));
            ImGui.SameLine();
            ImGui.Text("Microphone Level (RMS)");

            ImGui.ProgressBar((float)((DbVal + Math.Abs(minDb)) / Math.Abs(minDb)),
                new System.Numerics.Vector2(0, 0),
                DbVal.ToString("F1"));
            ImGui.SameLine();
            ImGui.Text("Microphone Level (DbFS)");

            ImGui.SliderFloat($"Db Reference##{GetUniqueId()}", ref MicConfig.dbReference, (float)minDb, 0);
        }
    }
}