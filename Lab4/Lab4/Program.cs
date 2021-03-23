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

            string path = @"C:\Users\danvu\Documents\GitHubPP\Lab4\Lab4\Wavs\tone5.wav";
            string outPut = @"Test2.wav";
            Track tr = new Track();
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                tr.Load(stream);
                Console.WriteLine(tr);
            }

            Console.WriteLine(tr.ChunkSize);

            tr.ScaleTrack(2.28);

            using (FileStream stream = new FileStream(outPut, FileMode.OpenOrCreate))
            {
                tr.Save(stream);
            }
            
            Console.WriteLine(tr.ChunkSize);

            Console.ReadKey();
        }
    }
}