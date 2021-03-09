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

    }
}