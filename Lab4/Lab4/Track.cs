using System;
using System.IO;

namespace Lab4
{
    public class Track
    {
        private Int32 id;
        private Int32 chunkSize;
        private Int32 format;
        
        private Int32 subchunk1Id;
        private Int32 subchunk1Size;
        private Int16 audioFormat;
        private Int16 numChannels;
        private Int32 sampleRate;
        private Int32 byteRate;
        private Int16 blockAlign;
        private Int16 bitsPerSample;

        private Int32 subchunk2Id;
        private Int32 subchunk2Size;
        private byte[] data;
        
        public void ScaleTrack(int scale)
        {

            byte[] newData = new byte[data.Length*scale];
 
            int sampleSize=bitsPerSample/8;
            
            for (int i = 0; i < data.Length - sampleSize; i+=sampleSize)
            {
                for (int j = 0; j < scale; j++)
                {
                    for (int k = 0; k < sampleSize; k++)
                    {
                        newData[i * scale + j * sampleSize + k] = data[i+k];
                    }
                }
            }
            
            
            data = newData;
            OnDataChanged();
        }

        public override string ToString()
        {
            byte[] arr = BitConverter.GetBytes(id);
            byte[] arr1 = BitConverter.GetBytes(subchunk1Id);
            byte[] arr2 = BitConverter.GetBytes(subchunk2Id);
            return System.Text.Encoding.Default.GetString(arr) + System.Text.Encoding.Default.GetString(arr1) + System.Text.Encoding.Default.GetString(arr2);
        }
        
        private void OnDataChanged()
        {
            /*
            subchunk2Size == NumSamples * NumChannels * BitsPerSample/8
                This is the number of bytes in the data.
                You can also think of this as the size
                of the read of the subchunk following this
            */
            
            int NumSamples = data.Length * 8 / bitsPerSample;
            subchunk2Size = NumSamples * numChannels * bitsPerSample / 8;
            chunkSize = 4 + (8 + subchunk1Size) + (8 + subchunk2Size);
        }

        public void Deserialize(FileStream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                //Head
                id = reader.ReadInt32();
                chunkSize = reader.ReadInt32();
                format = reader.ReadInt32();
                
                //Block 1
                subchunk1Id = reader.ReadInt32();
                subchunk1Size = reader.ReadInt32();
                audioFormat = reader.ReadInt16();
                numChannels = reader.ReadInt16();
                sampleRate = reader.ReadInt32();
                byteRate = reader.ReadInt32();
                blockAlign = reader.ReadInt16();
                bitsPerSample = reader.ReadInt16(); 
                
                //Block 2
                subchunk2Id = reader.ReadInt32();
                subchunk2Size = reader.ReadInt32();
                data = reader.ReadBytes((int) reader.BaseStream.Length);
            }
        }

        public void Serialize(FileStream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                //HEAD
                writer.Write(id);
                writer.Write(chunkSize);
                writer.Write(format);
                
                //Block 1
                writer.Write(subchunk1Id);
                writer.Write(subchunk1Size);
                writer.Write(audioFormat);
                writer.Write(numChannels);
                writer.Write(sampleRate);
                writer.Write(byteRate);
                writer.Write(blockAlign);
                writer.Write(bitsPerSample);
                
                //Block 2
                writer.Write(subchunk2Id);
                writer.Write(subchunk2Size);
                writer.Write(data);

            }
        }

    }
    
    
}