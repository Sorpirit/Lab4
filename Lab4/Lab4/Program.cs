using System;
using System.IO;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wav scaler.)");
            Console.WriteLine("Input format: [Input file] [Output file] [Scale]");
            string input = Console.ReadLine();
            string[] commands = input.Split(' ');
            if (commands.Length != 3)
            {
                Console.WriteLine("Wrong input format");
            }
            string inPath = commands[0];
            string outPut = commands[1];
            double scaleFactor = double.Parse(commands[2]);
            
            Track track = new Track();
            using (FileStream stream = new FileStream(inPath, FileMode.OpenOrCreate))
            {
                Console.WriteLine("Reading wav file...");
                track.Load(stream);
            }
            
            track.ScaleTrack(scaleFactor);

            using (FileStream stream = new FileStream(outPut, FileMode.OpenOrCreate))
            {
                Console.WriteLine("Writing to wav file...");
                track.Save(stream);
            }

        }
    }
}