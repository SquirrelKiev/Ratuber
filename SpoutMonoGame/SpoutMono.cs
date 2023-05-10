using Microsoft.Xna.Framework.Graphics;
using Spout.Interop;
using System.Reflection;
using System;

namespace Spout.MonoGame
{
    public static class SpoutMono
    {

        public static void SendTexture2D(SpoutSender sender, RenderTarget2D texture2D)
        {
            var info = typeof(RenderTarget2D).GetField("glTexture", BindingFlags.Instance | BindingFlags.NonPublic);
            var glTexture = (uint)(int)info.GetValue(texture2D);

            sender.SendTexture(glTexture, 3553, (uint)texture2D.Width, (uint)texture2D.Height, false, 0);
        }
    }
}
