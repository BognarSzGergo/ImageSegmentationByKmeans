using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSegmentationByKmeans
{
    class KMCFrame
    {
        private LockedBitmap m_Frame = null;
        private KMCPoint<Int32> m_Center;
        private List<KMCPoint<int>> m_Centroids = null;

        public KMCFrame(LockedBitmap Frame, List<KMCPoint<int>> Centroids, KMCPoint<int> Center)
        {
            this.Frame = Frame;
            this.m_Centroids = Centroids; this.Center = Center;
        }
        
        public LockedBitmap Frame
        {
            get { return m_Frame; }
            set { m_Frame = value; }
        }
        
        public List<KMCPoint<int>> Centroids
        {
            get { return m_Centroids; }
            set { m_Centroids = value; }
        }
        
        public KMCPoint<Int32> Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }

    }
}
