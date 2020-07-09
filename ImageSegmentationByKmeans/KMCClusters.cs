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
    class KMCClusters : IEnumerable<KMCFrame>
    {
        private readonly System.Random rand = new System.Random();
        private static HashSet<KMCFrame> m_Clusters = new HashSet<KMCFrame>();
        public void Init(string Filename, Int32 Distance, Int32 Offset)
        {
            LockedBitmap FrameBuffer = new LockedBitmap(Filename);
            
            List<KMCPoint<int>> Centroids = new List<KMCPoint<int>>();
            
            this.Generate(ref Centroids, FrameBuffer, Distance, Offset);
            
            KMCPoint<int> Mean = this.GetMean(FrameBuffer, Centroids);
            
            m_Clusters.Add(new KMCFrame(FrameBuffer, Centroids, Mean));
        }
        public void Generate(ref List<KMCPoint<int>> Centroids, LockedBitmap ImageFrame, Int32 Distance, Int32 Offset)
        {
            Int32 Size = ImageFrame.Width * ImageFrame.Height;
            ImageFrame.LockBits();

            for (Int32 IterCount = 0; IterCount < Size; IterCount++)
            {
                Int32 Rand_X = rand.Next(0, ImageFrame.Width);
                Int32 Rand_Y = rand.Next(0, ImageFrame.Height);
                
                KMCPoint<int> RandPoint = new KMCPoint<int>(Rand_X,
                              Rand_Y, ImageFrame.GetPixel(Rand_X, Rand_Y));

                if (!this.IsValidColor(Centroids, RandPoint, Offset) &&
                    !this.IsValidDistance(Centroids, RandPoint, Distance))
                {
                    if (!Centroids.Contains(RandPoint))
                    {
                        Centroids.Add(RandPoint);
                    }
                }
            }

            ImageFrame.UnlockBits();
        }
        private bool IsValidDistance(List<KMCPoint<int>> Points, KMCPoint<int> Target, Int32 Distance)
        {
            Int32 Index = -1; bool Exists = false;
            while (++Index < Points.Count() && !Exists)

                Exists = ((Math.Abs(Target.X - Points.ElementAt(Index).X) <= Distance) ||
                          (Math.Abs(Target.Y - Points.ElementAt(Index).Y) <= Distance)) ? true : false;

            return Exists;
        }
        private bool IsValidColor(List<KMCPoint<int>> Points, KMCPoint<int> Target, Int32 Offset)
        {
            Int32 Index = -1; bool Exists = false;
            while (++Index < Points.Count() && !Exists)
                Exists = (Math.Sqrt(Math.Pow(Math.Abs(Points[Index].Clr.R - Target.Clr.R), 2) +
                                    Math.Pow(Math.Abs(Points[Index].Clr.G - Target.Clr.G), 2) +
                                    Math.Pow(Math.Abs(Points[Index].Clr.B - Target.Clr.B), 2))) <= Offset ? true : false;

            return Exists;
        }
        public KMCPoint<int> GetMean(LockedBitmap FrameBuffer, List<KMCPoint<int>> Centroids)
        { 

            double Mean_X = 0, Mean_Y = 0;

            for (Int32 Index = 0; Index < Centroids.Count(); Index++)
            {
                Mean_X += Centroids[Index].X / (double)Centroids.Count();
                Mean_Y += Centroids[Index].Y / (double)Centroids.Count();
            }

            Int32 X = Convert.ToInt32(Mean_X);
            Int32 Y = Convert.ToInt32(Mean_Y);

            FrameBuffer.LockBits();
            Color Clr = FrameBuffer.GetPixel(X, Y);
            FrameBuffer.UnlockBits();

            return new KMCPoint<int>(X, Y, Clr);
        }
        public void Add(LockedBitmap FrameImage, List<KMCPoint<int>> Centroids, KMCPoint<int> Center)
        {
            m_Clusters.Add(new KMCFrame(FrameImage, Centroids, Center));
        }

        public KMCFrame this[Int32 Index]
        {
            get { return m_Clusters.ElementAt(Index); }
        }

        public IEnumerator<KMCFrame> GetEnumerator()
        {
            return m_Clusters.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
