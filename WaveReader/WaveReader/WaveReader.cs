using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WaveReaderDLL
{
    public class WaveReader
    {
        private static readonly int BitsPerByte = 8;
        private static readonly int MaxBits = 8;
        public Int32[][] Samples { get; private set; }
        public int CompressionCode { get; private set; }
        public int NumberOfChannels { get; private set; }
        public int SampleRate { get; private set; }
        public int BytesPerSecond { get; private set; }
        public int BitsPerSample { get; private set; }
        public int BlockAlign { get; private set; }
        public int Frames { get; private set; }
        public double TimeLength { get; private set; }

        /// <summary>
        /// Reads a Wave file from the input stream, but doesn't close the stream
        /// </summary>
        /// <param name="stream">Input WAVE file stream</param>
        public WaveReader(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                binaryReader.ReadChars(4); //"RIFF"
                int length = binaryReader.ReadInt32();
                binaryReader.ReadChars(4); //"WAVE"
                string chunkName = new string(binaryReader.ReadChars(4)); //"fmt "
                int chunkLength = binaryReader.ReadInt32();
                this.CompressionCode = binaryReader.ReadInt16(); //1 for PCM/uncompressed
                this.NumberOfChannels = binaryReader.ReadInt16();
                this.SampleRate = binaryReader.ReadInt32();
                this.BytesPerSecond = binaryReader.ReadInt32();
                this.BlockAlign = binaryReader.ReadInt16();
                this.BitsPerSample = binaryReader.ReadInt16();
                if ((MaxBits % BitsPerSample) != 0)
                {
                    throw new Exception("The input stream uses an unhandled SignificantBitsPerSample parameter");
                }
                binaryReader.ReadChars(chunkLength - 16);
                chunkName = new string(binaryReader.ReadChars(4));
                try
                {
                    while (chunkName.ToLower() != "data")
                    {
                        binaryReader.ReadChars(binaryReader.ReadInt32());
                        chunkName = new string(binaryReader.ReadChars(4));
                    }
                }
                catch
                {
                    throw new Exception("Input stream misses the data chunk");
                }
                chunkLength = binaryReader.ReadInt32();
                this.Frames = 8 * chunkLength / this.BitsPerSample / this.NumberOfChannels;
                this.TimeLength = ((double)this.Frames) / ((double)this.SampleRate);
                this.Samples = SplitChannels(binaryReader, this.NumberOfChannels, this.BitsPerSample, this.Frames);
            }
        }

        public static Int32[][] SplitChannels(BinaryReader binaryReader, int numberOfChannels, int bitsPerSample, int numberOfFrames)
        {
            var samples = new Int32[numberOfChannels][];
            for (int channel = 0; channel < numberOfChannels; channel++)
            {
                samples[channel] = new Int32[numberOfFrames];
            }
            int readedBits = 0;
            int numberOfReadedBits = 0;
            for (int frame = 0; frame < numberOfFrames; frame++)
            {
                for (int channel = 0; channel < numberOfChannels; channel++)
                {
                    while (numberOfReadedBits < bitsPerSample)
                    {
                        readedBits |= Convert.ToInt32(binaryReader.ReadByte()) << numberOfReadedBits;
                        numberOfReadedBits += BitsPerByte;
                    }
                    int numberOfExcessBits = numberOfReadedBits - bitsPerSample;
                    samples[channel][frame] = readedBits >> numberOfExcessBits;
                    readedBits = readedBits % (1 << numberOfExcessBits);
                    numberOfReadedBits = numberOfExcessBits;
                }
            }
            return samples;
        }
    }
}