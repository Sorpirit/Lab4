using System;
using System.Collections.Generic;
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
            double absScale = Math.Abs(scale);

            if (NumChannels == 1)
            {
                data = ScaleTrack(data, absScale);
            }
            else if (NumChannels == 2)
            {
                int channelLength = data.Length / 2;
                byte[] rightChannel = new byte[channelLength];
                byte[] leftChannel = new byte[channelLength];

                for (int i = 0; i < channelLength; i++)
                {
                    leftChannel[i] = data[2 * i];
                    rightChannel[i] = data[2 * i + 1];
                }

                rightChannel = ScaleTrack(rightChannel, absScale);
                leftChannel = ScaleTrack(leftChannel, absScale);

                data = new byte[rightChannel.Length + leftChannel.Length];

                for (int i = 0; i < leftChannel.Length; i++)
                {
                    data[2 * i] = leftChannel[i];
                    data[2 * i + 1] = rightChannel[i];
                }
            }

            if (scale < 0)
            {
                int bytesPerSample = BitsPerSample / 8;
                byte[,] samples = new byte[data.Length / bytesPerSample, bytesPerSample];

                for (int i = 0; i < data.Length / bytesPerSample; i++)
                {
                    for (int j = 0; j < bytesPerSample; j++)
                    {
                        samples[i, j] = data[i * bytesPerSample + j];
                    }
                }

                byte[] reversedData = new byte[data.Length];

                for (int i = samples.GetLength(0) - 1; i >= 0; i--)
                {
                    for (int j = 0; j < bytesPerSample; j++)
                    {
                        reversedData[2 * samples.GetLength(0) - 2 * i - 2 + j] = samples[i, j];
                    }
                }

                data = reversedData;
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
                double placeInInput = Interpolate(0, inputSamples - 1, 0, outputSamples - 1, i / sampleSize);

                int prevSampleIndex = (int)placeInInput;
                int nextSampleIndex = (int)placeInInput + 1;

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

        public void RepeatTrack(double scale)
        {
            byte[] newData = new byte[(int)(data.Length * scale)];

            int integerScale = (int) scale;

            for (int i = 0; i < integerScale; i++)
            {
                Array.Copy(data, 0, newData, i * data.Length, data.Length);
            }

            double realScale = scale % 1;

            for (int i = 0; i < Math.Round(data.Length * realScale); i++)
            {
                newData[integerScale * data.Length + i] = data[i];
            }

            data = newData;

            SubChunk2Size = data.Length;
            ChunkSize = 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size);
        }

        private double Interpolate(double y0, double y1, double x0, double x1, double x)
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