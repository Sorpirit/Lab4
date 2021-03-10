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

            string path = @"Audio.wav";
            string outPut = @"Audio2.wav";
            Track tr = new Track();
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                tr.Load(stream);
                Console.WriteLine(tr);
            }

            Console.WriteLine(tr.ChunkSize);

            tr.ScaleTrack(.2);

            using (FileStream stream = new FileStream(outPut, FileMode.OpenOrCreate))
            {
                tr.Save(stream);
            }
            
            Console.WriteLine(tr.ChunkSize);

            Console.ReadKey();
        }

        //private static int[] ScaleArr(int scale,int sample,int[] inputArr)
        //{
        //    int[] resultArr = new int[inputArr.Length * scale];
        //    for (int i = 0; i < inputArr.Length - sample; i+=sample)
        //    {
        //        for (int j = 0; j < scale; j++)
        //        {
        //            for (int k = 0; k < sample; k++)
        //            {
        //                resultArr[i * scale + j * sample + k] = inputArr[i+k];
        //            }
        //        }
        //    }

        //    return resultArr;
        //}

            return resultArr;
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