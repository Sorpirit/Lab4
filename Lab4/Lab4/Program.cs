﻿using System;
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

            string path = @"C:\Users\danvu\Desktop\BestSounds\file_example_WAV_10MGMono.wav";
            string outPut = @"C:\Users\danvu\Desktop\BestSounds\testWav1.wav";
            Track tr = new Track();
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                tr.Deserialize(stream);
                Console.WriteLine(tr);

            }

            tr.ScaleTrack(3);
            using (FileStream stream = new FileStream(outPut, FileMode.OpenOrCreate))
            {
                tr.Serialize(stream);
            }
        }

        private static int[] ScaleArr(int scale,int sample,int[] inputArr)
        {
            int[] resultArr = new int[inputArr.Length * scale];
            for (int i = 0; i < inputArr.Length - sample; i+=sample)
            {
                for (int j = 0; j < scale; j++)
                {
                    for (int k = 0; k < sample; k++)
                    {
                        resultArr[i * scale + j * sample + k] = inputArr[i+k];
                    }
                }
            }

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