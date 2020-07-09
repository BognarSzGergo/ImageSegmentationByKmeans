using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSegmentationByKmeans
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Input file name: ");
            string InpFile = Console.ReadLine();

            Console.Write("Output file name: ");
            string OutFile = Console.ReadLine();

            ImageSegmentation ImageSeg = new ImageSegmentation();
            ImageSeg.Compute(InpFile, OutFile);

            Console.ReadKey();
        }
    }
}
