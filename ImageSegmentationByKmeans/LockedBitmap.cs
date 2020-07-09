using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections;
using System.Windows;
using System.Collections.Concurrent;
using System.IO;
using System.Drawing.Imaging;

namespace ImageSegmentationByKmeans
{
    class LockedBitmap
    {
        public Bitmap m_Bitmap = null;

        private Rectangle m_rect;
        private IntPtr m_BitmapPtr;
        private byte[] m_Pixels = null;
        private BitmapData m_BitmapInfo = null;

        public Int32 Width { get { return m_Bitmap.Width; } }
        public Int32 Height { get { return m_Bitmap.Height; } }

        public LockedBitmap(string filename)
        {
            if (m_Bitmap == null)
            {
                m_Bitmap = new Bitmap(filename);
                m_rect = new Rectangle(new Point(0, 0), m_Bitmap.Size);
            }
        }
        public LockedBitmap(Int32 Width, Int32 Height)
        {
            if (m_Bitmap == null)
            {
                m_Bitmap = new Bitmap(Width, Height);
                m_rect = new Rectangle(new Point(0, 0), m_Bitmap.Size);
            }
        }
        public LockedBitmap(Bitmap bitmap)
        {
            if (m_Bitmap == null)
            {
                m_Bitmap = new Bitmap(bitmap);
                m_rect = new Rectangle(new Point(0, 0), m_Bitmap.Size);
            }
        }
        public static implicit operator LockedBitmap(Bitmap bitmap)
        {
            return new LockedBitmap(bitmap);
        }
        public void LockBits()
        {
            m_BitmapInfo = m_Bitmap.LockBits(m_rect, System.Drawing.Imaging.
                ImageLockMode.ReadWrite, m_Bitmap.PixelFormat);

            m_BitmapPtr = m_BitmapInfo.Scan0;
            m_Pixels = new byte[Math.Abs(m_BitmapInfo.Stride) * m_Bitmap.Height];
            System.Runtime.InteropServices.Marshal.Copy(m_BitmapPtr, m_Pixels,
                0, Math.Abs(m_BitmapInfo.Stride) * m_Bitmap.Height);
        }
        public void UnlockBits()
        {
            m_BitmapPtr = m_BitmapInfo.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(m_Pixels, 0,
                m_BitmapPtr, Math.Abs(m_BitmapInfo.Stride) * m_Bitmap.Height);

            m_Bitmap.UnlockBits(m_BitmapInfo);
        }
        public Color GetPixel(Int32 Row, Int32 Col)
        {
            Int32 Channel = System.Drawing.Bitmap.GetPixelFormatSize(m_BitmapInfo.PixelFormat);
            Int32 Pixel = (Row + Col * m_Bitmap.Width) * (Channel / 8);

            Int32 Red = 0, Green = 0, Blue = 0, Alpha = 0;

            if (Channel == 32)
            {
                Blue = m_Pixels[Pixel];
                Green = m_Pixels[Pixel + 1];
                Red = m_Pixels[Pixel + 2];
                Alpha = m_Pixels[Pixel + 3];
            }

            else if (Channel == 24)
            {
                Blue = m_Pixels[Pixel];
                Green = m_Pixels[Pixel + 1];
                Red = m_Pixels[Pixel + 2];
            }

            else if (Channel == 16)
            {
                Blue = m_Pixels[Pixel];
                Green = m_Pixels[Pixel + 1];
            }

            else if (Channel == 8)
            {
                Blue = m_Pixels[Pixel];
            }

            return (Channel != 8) ? Color.FromArgb(Red, Green, Blue) : Color.FromArgb(Blue, Blue, Blue);
        }
        public void SetPixel(Int32 Row, Int32 Col, Color Clr)
        {
            Int32 Channel = System.Drawing.Bitmap.GetPixelFormatSize(m_BitmapInfo.PixelFormat);
            Int32 Pixel = (Row + Col * m_Bitmap.Width) * (Channel / 8);

            if (Channel == 32)
            {
                m_Pixels[Pixel] = Clr.B;
                m_Pixels[Pixel + 1] = Clr.G;
                m_Pixels[Pixel + 2] = Clr.R;
                m_Pixels[Pixel + 3] = Clr.A;
            }

            else if (Channel == 24)
            {
                m_Pixels[Pixel] = Clr.B;
                m_Pixels[Pixel + 1] = Clr.G;
                m_Pixels[Pixel + 2] = Clr.R;
            }

            else if (Channel == 16)
            {
                m_Pixels[Pixel] = Clr.B;
                m_Pixels[Pixel + 1] = Clr.G;
            }

            else if (Channel == 8)
            {
                m_Pixels[Pixel] = Clr.B;
            }
        }
        public void Save(string filename)
        {
            m_Bitmap.Save(filename);
        }
    }
}
