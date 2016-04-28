using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Common
{
    class CanvasImg
    {
        public CanvasBitmap Bmp { get; private set; }

        public GaussianBlurEffect GaussianBlurCache { get; set; }

        public ScaleEffect ScaleEffect { get; set; }
        public string Src { get; private set; }
        public double Height
        {
            get
            {
                if (Bmp != null) return Bmp.SizeInPixels.Height;
                return -1;
            }
        }

        public double Width
        {
            get
            {
                if (Bmp != null) return Bmp.SizeInPixels.Width;
                return -1;
            }
        }

        public float Scale { get; set; }
        public float Opacity { get; set; }
        public bool Loaded { get; set; }
        public CanvasImg(string source)
        {
            Src = source;
            Opacity = 0;
            GaussianBlurCache = new GaussianBlurEffect()
            {
                BlurAmount = 4.0f,
                BorderMode = EffectBorderMode.Soft
            };
            ScaleEffect= new ScaleEffect()
            {
                CenterPoint = new System.Numerics.Vector2(0, 0)
            };
        }

        public async Task Initialize(ICanvasResourceCreator cac)
        {
            Bmp = await CanvasBitmap.LoadAsync(cac, new Uri(Src));
            Loaded = true;
        }

    }
}
