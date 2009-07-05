using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaveReaderTest
{
    public class SplitChannelsTest
    {
        private readonly int BitsPerByte = 8;
        public int NumberOfChannels { get; set; }
        public int BitsPerSample { get; set; }
        public byte[] Input { get; set; }
        public Int32[][] Output { get; set; }
        public int GetNumberOfFrames()
        {
            return (Input.Length * BitsPerByte) / (NumberOfChannels * BitsPerSample);
        }
    }
}
