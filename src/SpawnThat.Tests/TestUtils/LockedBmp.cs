using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace SpawnThat.TestUtils
{
    public unsafe class LockedBmp : IDisposable
    {
        public Bitmap Bmp { get; }
        public int Stride { get; }
        public int Width { get; }
        public int Height { get; }

        public BitmapData Lock { get; }
        public byte* FirstPixel { get; }
        public int BytesPrPixel { get; } = 4;

        public LockedBmp(Bitmap bmp, ImageLockMode lockMode = ImageLockMode.ReadWrite)
        {
            Bmp = bmp;

            Lock = Bmp.LockBits(new Rectangle(0, 0, Bmp.Width, Bmp.Height), lockMode, Bmp.PixelFormat);

            FirstPixel = (byte*)Lock.Scan0.ToPointer();
            Stride = Lock.Stride;
            Width = Lock.Width;
            Height = Lock.Height;
        }

        public void Dispose()
        {
            Bmp.UnlockBits(Lock);
        }
    }
}
