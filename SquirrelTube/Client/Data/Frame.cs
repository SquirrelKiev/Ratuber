using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquirrelTube.Client.Data
{
    public class Frame : IDisposable
    {
        public string texturePath;
        public Texture2D texture { get; private set; }

        public void Dispose()
        {
            texture.Dispose();
        }
    }
}
