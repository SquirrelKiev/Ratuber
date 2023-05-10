using ImGuiNET;
using Ratuber.Client.Data;

namespace Ratuber.Client
{
    public static class ImGuiConfigWindow
    {
        public static void ConfigLayout()
        {
            ImGui.ShowDemoWindow();

            ImGui.SetNextWindowSizeConstraints(new System.Numerics.Vector2(375, 400), new System.Numerics.Vector2(float.MaxValue, float.MaxValue));

            ImGui.Begin("Config");
            if (ImGui.BeginTabBar("Config"))
            {
                if (ImGui.BeginTabItem("Microphone Settings"))
                {
                    RenderAllMicrophoneConfigs();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Layer Group Editor"))
                {
                    RenderAllLayerGroupEditors();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
            ImGui.End();
        }

        private static void RenderAllLayerGroupEditors()
        {
            for (int i = 0; i < Config.CurrentConfig.LayerGroups.Count; i++)
            {
                var layerGroup = Config.CurrentConfig.LayerGroups[i];

                ImGui.Text(i.ToString());

                ImGui.SameLine();

                ImGuiHelpers.MoveElementArrows(Config.CurrentConfig.LayerGroups, layerGroup, true);

                ImGui.SameLine();

                if (ImGui.TreeNode($"{layerGroup.GetUniqueId()}", $"Layer Group - {layerGroup.name}"))
                {
                    layerGroup.RenderLayerGroupEditor();

                    if (ImGui.Button("Remove Layer Group"))
                    {
                        Config.CurrentConfig.LayerGroups.Remove(layerGroup);
                    }

                    ImGui.TreePop();
                }
            }

            if (ImGui.Button("Add Layer"))
            {
                Config.CurrentConfig.LayerGroups.Add(new LayerGroup().Initialize(CurrentState.client.GraphicsDevice, CurrentState.client.guiRenderer));
            }
        }

        private static void RenderAllMicrophoneConfigs()
        {
            for (int i = 0; i < Config.CurrentConfig.MicConfigs.Count; i++)
            {
                if (ImGui.TreeNode($"Microphone Config {i}"))
                {
                    CurrentState.MicrophoneManagers[i].RenderMicrophoneEditor();

                    if (ImGui.Button("Remove Microphone"))
                    {
                        Config.CurrentConfig.MicConfigs.Remove(Config.CurrentConfig.MicConfigs[i]);

                        Config.ConfigUpdated();
                    }
                    ImGui.TreePop();
                }
            }

            var validMics = MicrophoneConfig.GetValidMicrophones();

            if (validMics.Length > 0 && ImGui.Button("Add Microphone"))
            {
                var config = new MicrophoneConfig
                {
                    microphoneName = validMics[0].Name
                };

                Config.CurrentConfig.MicConfigs.Add(config);

                Config.ConfigUpdated();
            }
        }
    }
}