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
    class ImageSegmentation
    {
        private const Int32 m_Distance = 5;
        private const Int32 m_OffsetClr = 50;

        private static KMCClusters m_Clusters = new KMCClusters();
        public ImageSegmentation() { }
        public void Compute(string InputFile, string OutputFile)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            DirectoryInfo dir_info = new DirectoryInfo("Clusters");

            if (dir_info.Exists == false) dir_info.Create();

            m_Clusters.Init(InputFile, m_Distance, m_OffsetClr);

            LockedBitmap ResultBitmap = new LockedBitmap(m_Clusters[0].Frame.Width, m_Clusters[0].Frame.Height);

            Int32 FrameIndex = 0;

            for (Int32 Index = 0; Index < m_Clusters.Count(); Index++)
            {

                List<KMCPoint<int>> Centroids = m_Clusters[Index].Centroids.ToList();
                LockedBitmap FrameBuffer = new LockedBitmap(m_Clusters[Index].Frame.m_Bitmap);


                FrameBuffer.Save("Clusters\\Cluster_" + FrameIndex + ".jpg");

                FrameBuffer.LockBits();

                for (Int32 Cnt = 0; Cnt < Centroids.Count(); Cnt++)
                {
                    Int32 Width = FrameBuffer.Width;
                    Int32 Height = FrameBuffer.Height;

                    LockedBitmap TargetFrame = new LockedBitmap(FrameBuffer.Width, FrameBuffer.Height);

                    TargetFrame.LockBits();

                    for (Int32 Row = 0; Row < FrameBuffer.Width; Row++)
                    {
                        for (Int32 Col = 0; Col < Height; Col++)
                        {
                            double OffsetClr = GetEuclClr(new KMCPoint<int>(Row, Col, FrameBuffer.GetPixel(Row, Col)),
                                                          new KMCPoint<int>(Centroids[Cnt].X, Centroids[Cnt].Y, Centroids[Cnt].Clr));

                            if (OffsetClr <= 50)
                            {
                                TargetFrame.SetPixel(Row, Col, Centroids[Cnt].Clr);
                            }

                            else TargetFrame.SetPixel(Row, Col, Color.FromArgb(255, 255, 255));
                        }
                    }

                    TargetFrame.UnlockBits();

                    List<KMCPoint<int>> TargetCnts = new List<KMCPoint<int>>();
                    TargetCnts.Add(Centroids[0]);

                    KMCPoint<int> Mean = m_Clusters.GetMean(TargetFrame, TargetCnts);

                    if (Mean.X != m_Clusters[Index].Center.X && Mean.Y != m_Clusters[Index].Center.Y)
                        m_Clusters.Add(TargetFrame, TargetCnts, Mean);

                    FrameIndex++;
                }

                FrameBuffer.UnlockBits();
            }

            ResultBitmap.LockBits();

            for (Int32 Index = 0; Index < m_Clusters.Count(); Index++)
            {
                LockedBitmap FrameOut = new LockedBitmap(m_Clusters[Index].Frame.m_Bitmap);

                FrameOut.LockBits();

                FrameOut.Save("temp_" + Index + ".jpg");

                int Width = FrameOut.Width, Height = FrameOut.Height;

                for (Int32 Row = 0; Row < Width; Row++)
                {
                    for (Int32 Col = 0; Col < Height; Col++)
                    {
                        if (FrameOut.GetPixel(Row, Col) != Color.FromArgb(255, 255, 255))
                        {
                            ResultBitmap.SetPixel(Row, Col, FrameOut.GetPixel(Row, Col));
                        }
                    }
                }

                FrameOut.UnlockBits();
            }

            ResultBitmap.UnlockBits();

            ResultBitmap.Save(OutputFile);

            watch.Stop(); 
            var elapsedMs = watch.ElapsedMilliseconds;

            TimeSpan ts = TimeSpan.FromMilliseconds(elapsedMs);

            Console.WriteLine("***Done***\n" + ts.ToString(@"hh\:mm\:ss"));
        }

        public double GetEuclD(KMCPoint<int> Point1, KMCPoint<int> Point2)
        {
            return Math.Sqrt(Math.Pow(Point1.X - Point2.X, 2) +
                             Math.Pow(Point1.Y - Point2.Y, 2));
        }
        public double GetEuclClr(KMCPoint<int> Point1, KMCPoint<int> Point2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(Point1.Clr.R - Point2.Clr.R), 2) +
                             Math.Pow(Math.Abs(Point1.Clr.G - Point2.Clr.G), 2) +
                             Math.Pow(Math.Abs(Point1.Clr.B - Point2.Clr.B), 2));
        }

    }
}
