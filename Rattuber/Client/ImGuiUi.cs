using ImGuiNET;
using Rattuber.Client.Data;

namespace Rattuber.Client
{
    public static class ImGuiConfigWindow
    {
        public static void ConfigLayout()
        {
            ImGui.Begin("Config");
            if (ImGui.BeginTabBar("Config"))
            {
                if (ImGui.BeginTabItem("Microphone Settings"))
                {
                    RenderAllMicrophoneConfigs();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Layer Editor"))
                {
                    for (int i = 0; i < Config.CurrentConfig.Layers.Count; i++)
                    {
                        var layer = Config.CurrentConfig.Layers[i];

                        if (ImGui.TreeNode($"Layer {i}##{layer.GetUniqueId()}"))
                        {
                            layer.RenderLayerEditor();

                            if (ImGui.Button("Remove Layer"))
                            {
                                Config.CurrentConfig.Layers.Remove(layer);
                            }

                            ImGui.TreePop();
                        }
                    }

                    if (ImGui.Button("Add Layer"))
                    {
                        Config.CurrentConfig.Layers.Add(new Layer());
                    }
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
            ImGui.End();
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