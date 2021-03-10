using System;
using System.IO;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("XD");
            
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

            string path = @"C:\Users\danvu\Desktop\BestSounds\tone4.wav";
            string outPut = @"C:\Users\danvu\Desktop\BestSounds\testWav.wav";
            Track tr = new Track();
            using (FileStream stream = new FileStream(path,FileMode.OpenOrCreate))
            {
                tr.Deserialize(stream);
                Console.WriteLine(tr);
                
            }
            tr.ScaleTrack(2);
            using (FileStream stream = new FileStream(outPut,FileMode.OpenOrCreate))
            {
                tr.Serialize(stream);
            }
        }
        /*    
        public override string ToString()
        {
            byte[] arr = BitConverter.GetBytes(id);
            byte[] arr1 = BitConverter.GetBytes(subchunk1Id);
            byte[] arr2 = BitConverter.GetBytes(subchunk2Id);
            return System.Text.Encoding.Default.GetString(arr) + System.Text.Encoding.Default.GetString(arr1) + System.Text.Encoding.Default.GetString(arr2);
        }
        */
    }
}