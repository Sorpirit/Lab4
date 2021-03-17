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
                Array.Copy(data, rightChannel, channelLength);
                Array.Copy(data, channelLength, leftChannel, 0, channelLength);
                rightChannel = ScaleTrack(rightChannel, scale);
                leftChannel = ScaleTrack(leftChannel, scale);
                data = new byte[rightChannel.Length + leftChannel.Length];
                Array.Copy(rightChannel, data, rightChannel.Length);
                Array.Copy(leftChannel, 0, data, rightChannel.Length, leftChannel.Length);
            }

            SubChunk2Size = data.Length;
            ChunkSize = 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size);
        }

        private byte[] ScaleTrack(byte[] input, double scale)
        {
            int newDataLength = (int)(input.Length * scale);

            byte[] newData = new byte[newDataLength];

            int sampleSize = BitsPerSample / 8;

            for (int i = 0; i <= (int)((input.Length - 1 - 2 * sampleSize) * scale); i += sampleSize)
            {
                byte[] previousSample = new byte[sampleSize];
                byte[] nextSample = new byte[sampleSize];
                Array.Copy(input, (int)(i / scale), previousSample, 0, sampleSize);
                Array.Copy(input, (int)(i / scale + sampleSize), nextSample, 0, sampleSize);

                byte[] currentSample = new byte[sampleSize];

                for (int k = 0; k < sampleSize; k++)
                {
                    currentSample[k] = (byte)(previousSample[k] + (nextSample[k] - previousSample[k]) * (i / (i + sampleSize * scale)));
                }

                Array.Copy(currentSample, 0, newData, i, sampleSize);
            }

            return newData;
        }

        public override string ToString()
        {
            byte[] arr = BitConverter.GetBytes(Id);
            byte[] arr1 = BitConverter.GetBytes(SubChunk1Id);
            byte[] arr2 = BitConverter.GetBytes(SubChunk2Id);
            return System.Text.Encoding.Default.GetString(arr) + System.Text.Encoding.Default.GetString(arr1) + System.Text.Encoding.Default.GetString(arr2);
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