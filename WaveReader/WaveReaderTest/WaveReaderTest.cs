using WaveReaderDLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Linq;

namespace WaveReaderTest
{
    
    
    /// <summary>
    ///This is a test class for WaveReaderTest and is intended
    ///to contain all WaveReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WaveReaderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SplitChannels
        ///</summary>
        [TestMethod()]
        public void SplitChannelsTest()
        {
            var splitChannelsTests = new SplitChannelsTest[]
            {
                new SplitChannelsTest()
                {
                    BitsPerSample = 8,
                    NumberOfChannels = 2,
                    Input = new byte[]
                    {
                        0x15,
                        0x51
                    },
                    Output = new Int32[][]
                    {
                        new Int32[]
                        {
                            0x15
                        },
                        new Int32[]
                        {
                            0x51
                        }
                    }
                },
                new SplitChannelsTest()
                {
                    BitsPerSample = 4,
                    NumberOfChannels = 1,
                    Input = new byte[]
                    {
                        0x15,
                        0x51
                    },
                    Output = new Int32[][]
                    {
                        new Int32[]
                        {
                            0x01,
                            0x05,
                            0x05,
                            0x01
                        }
                    }
                },
                new SplitChannelsTest()
                {
                    BitsPerSample = 16,
                    NumberOfChannels = 1,
                    Input = new byte[]
                    {
                        0x15,
                        0x51
                    },
                    Output = new Int32[][]
                    {
                        new Int32[]
                        {
                            0x5115
                        }
                    }
                },
                new SplitChannelsTest()
                {
                    BitsPerSample = 32,
                    NumberOfChannels = 1,
                    Input = new byte[]
                    {
                        0x12,
                        0x34,
                        0x56,
                        0x78
                    },
                    Output = new Int32[][]
                    {
                        new Int32[]
                        {
                            0x78563412
                        }
                    }
                }
            };
            foreach (var splitChannelsTest in splitChannelsTests)
            {
                using (var stream = new MemoryStream(splitChannelsTest.Input))
                using (var binaryReader = new BinaryReader(stream))
                {
                    var actual = WaveReader.SplitChannels(binaryReader, splitChannelsTest.NumberOfChannels, splitChannelsTest.BitsPerSample, splitChannelsTest.GetNumberOfFrames());
                    Assert.AreEqual(splitChannelsTest.Output.Length, actual.Length);
                    for (var channel = 0; channel < splitChannelsTest.NumberOfChannels; channel++)
                    {
                        Assert.IsTrue(splitChannelsTest.Output[channel].SequenceEqual(actual[channel]));
                    }
                }
            }
        }
    }
}