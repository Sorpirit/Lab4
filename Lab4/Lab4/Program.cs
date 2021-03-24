using System;
using System.IO;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter input file name: ");
            string inputPath = Console.ReadLine() ?? throw new ArgumentNullException();
            if (!File.Exists(inputPath) || !inputPath.EndsWith(".wav"))
            {
                throw new ArgumentException("There was an error with the input file");
            }

            Console.Write("Enter output file name: ");
            string outputPath = Console.ReadLine() ?? throw new ArgumentNullException();
            if (!outputPath.EndsWith(".wav"))
            {
                throw new ArgumentException("There was an error with the output file");
            }

            Console.Write("Enter scale factor: ");
            if (!double.TryParse(Console.ReadLine(), out double scaleFactor))
            {
                throw new InvalidCastException();
            }

            Track track = new Track();

            Console.WriteLine("Reading wav file...");
            using (FileStream stream = new FileStream(inputPath, FileMode.OpenOrCreate))
            {
                track.Load(stream);
            }
            
            track.ScaleTrack(scaleFactor);

            Console.WriteLine("Writing to wav file...");
            using (FileStream stream = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                track.Save(stream);
            }

            Console.ReadKey();
        }
    }
}