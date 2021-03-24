using System;
using System.IO;
using System.Linq;

namespace Lab4
{
    public class Track
    {
        public Int32 Id { get; private set; }
        public Int32 ChunkSize { get; private set; }
        public Int32 Format { get; private set; }

        public Int32 SubChunk1Id { get; private set; }
        public Int32 SubChunk1Size { get; set; }
        public Int16 AudioFormat { get; private set; }
        public Int16 NumChannels { get; private set; }
        public Int32 SampleRate { get; private set; }
        public Int32 ByteRate { get; private set; }
        public Int16 BlockAlign { get; private set; }
        public Int16 BitsPerSample { get; private set; }

        public Int32 SubChunk2Id { get; private set; }
        public Int32 SubChunk2Size { get; private set; }

        private byte[] data;

        public void ScaleTrack(double scale)
        {
            if (NumChannels == 1)
            {
                data = ScaleTrack(data, scale);
            }
            else if (NumChannels == 2)
            {
                int channelLength = data.Length / 2;
                byte[] rightChannel = new byte[channelLength];
                byte[] leftChannel = new byte[channelLength];

                for (int i = 0; i < channelLength; i ++)
                {
                    leftChannel[i] = data[2*i];
                    rightChannel[i] = data[2*i + 1];
                }

                rightChannel = ScaleTrack(rightChannel, scale);
                leftChannel = ScaleTrack(leftChannel, scale);

                data = new byte[rightChannel.Length + leftChannel.Length];

                for (int i = 0; i < leftChannel.Length; i++)
                {
                    data[2 * i] = leftChannel[i];
                    data[2 * i + 1] = rightChannel[i];
                }
            }

            SubChunk2Size = data.Length;
            ChunkSize = 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size);
        }

        private byte[] ScaleTrack(byte[] input, double scale)
        {
            int sampleSize = BitsPerSample / 8;

            int inputSamples = input.Length / sampleSize;
            int outputSamples = (int)(scale * inputSamples);

            byte[] newData = new byte[outputSamples * sampleSize];

            for (int i = 0; i < newData.Length - sampleSize; i += sampleSize)
            {
                int placeInInput = Interpolate(0, inputSamples - 1, 0, outputSamples - 1, i / sampleSize);

                int prevSampleIndex = placeInInput;
                int nextSampleIndex = placeInInput + 1;

                byte[] previousSample = new byte[sampleSize];
                byte[] nextSample = new byte[sampleSize];
                Array.Copy(input, prevSampleIndex * sampleSize, previousSample, 0, sampleSize);
                Array.Copy(input, nextSampleIndex * sampleSize, nextSample, 0, sampleSize);

                byte[] currentSample = new byte[sampleSize];

                for (int k = 0; k < sampleSize; k++)
                {
                    currentSample[k] = (byte)(Interpolate(previousSample[k], nextSample[k], prevSampleIndex, nextSampleIndex, placeInInput));
                }

                Array.Copy(currentSample, 0, newData, i, sampleSize);
            }

            return newData;
        }


        private int Interpolate(double y0, double y1, double x0, double x1, double x)
        {
            return (int)(y0 + (y1 - y0) * (x - x0) / (x1 - x0));
        }

        public void Load(FileStream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                //Head
                Id = reader.ReadInt32();
                ChunkSize = reader.ReadInt32();
                Format = reader.ReadInt32();

                //Block 1
                SubChunk1Id = reader.ReadInt32();
                SubChunk1Size = reader.ReadInt32();
                AudioFormat = reader.ReadInt16();
                NumChannels = reader.ReadInt16();
                SampleRate = reader.ReadInt32();
                ByteRate = reader.ReadInt32();
                BlockAlign = reader.ReadInt16();
                BitsPerSample = reader.ReadInt16();

                //Block 2
                SubChunk2Id = reader.ReadInt32();
                SubChunk2Size = reader.ReadInt32();
                data = reader.ReadBytes((int)reader.BaseStream.Length);

                Console.WriteLine(Id);
                Console.WriteLine(ChunkSize);
                Console.WriteLine(Format);
                Console.WriteLine();
                Console.WriteLine(SubChunk1Id);
                Console.WriteLine(SubChunk1Size);
                Console.WriteLine(AudioFormat);
                Console.WriteLine(NumChannels);
                Console.WriteLine(SampleRate);
                Console.WriteLine(ByteRate);
                Console.WriteLine(BlockAlign);
                Console.WriteLine(BitsPerSample);
                Console.WriteLine();
                Console.WriteLine(SubChunk2Id);
                Console.WriteLine(SubChunk2Size);
                Console.WriteLine(data.Length);
            }
        }

        public void Save(FileStream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                //HEAD
                writer.Write(Id);
                writer.Write(ChunkSize);
                writer.Write(Format);

                //Block 1
                writer.Write(SubChunk1Id);
                writer.Write(SubChunk1Size);
                writer.Write(AudioFormat);
                writer.Write(NumChannels);
                writer.Write(SampleRate);
                writer.Write(ByteRate);
                writer.Write(BlockAlign);
                writer.Write(BitsPerSample);

                //Block 2
                writer.Write(SubChunk2Id);
                writer.Write(SubChunk2Size);
                writer.Write(data);
            }
        }
    }
}