using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaveReaderTest
{
    public class SplitChannelsTest
    {
        private readonly int BitsPerByte = 8;
        public short NumberOfChannels { get; set; }
        public short BitsPerSample { get; set; }
        public byte[] Input { get; set; }
        public short[][] Output { get; set; }
        public int GetNumberOfFrames()
        {
            return (Input.Length * BitsPerByte) / (NumberOfChannels * BitsPerSample);
        }
    }
}
