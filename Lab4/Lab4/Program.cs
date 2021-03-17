using System;
using System.IO;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * TODO
             * 
             * Serialisation + 
             * Desirialisation + 
             *
             * Scaling + Interpolation 
             *
             * Main
             */

            string path = @"Test.wav";
            string outPut = @"Test2.wav";
            Track tr = new Track();
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                tr.Load(stream);
                Console.WriteLine(tr);
            }

            Console.WriteLine(tr.ChunkSize);

            tr.ScaleTrack(.5);

            using (FileStream stream = new FileStream(outPut, FileMode.OpenOrCreate))
            {
                tr.Save(stream);
            }
            
            Console.WriteLine(tr.ChunkSize);

            Console.ReadKey();
        }
    }
}