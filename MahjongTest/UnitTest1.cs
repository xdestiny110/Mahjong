using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mahjong;

namespace MahjongTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void tileStr2TileArray_test()
        {
            string testStr = "";
            int[] excepted = new int[34], actual = null;

            testStr = "11234m";
            excepted = new int[34];
            excepted[0] = 2; excepted[1] = 1; excepted[2] = 1; excepted[3] = 1;
            TileOperation.tileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "56p";
            excepted = new int[34];
            excepted[13] = 1; excepted[14] = 1;
            TileOperation.tileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "11234m56p789s1117h";
            excepted = new int[34];
            excepted[0] = 2; excepted[1] = 1; excepted[2] = 1; excepted[3] = 1;
            excepted[13] = 1; excepted[14] = 1;
            excepted[24] = 1; excepted[25] = 1; excepted[26] = 1;
            excepted[27] = 3; excepted[33] = 1;
            TileOperation.tileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "1235h";
            excepted = new int[34];
            excepted[27] = 1; excepted[28] = 1; excepted[29] = 1; excepted[31] = 1;

            testStr = "^&$&*^";
            try
            {
                TileOperation.tileStr2TileArray(testStr, out actual);
            }
            catch(Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());                
            }            

            testStr = "123456789";
            try
            {
                TileOperation.tileStr2TileArray(testStr, out actual);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());
            }

            testStr = "s";
            try
            {
                TileOperation.tileStr2TileArray(testStr, out actual);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());
            }

            testStr = "1h";
            try
            {
                TileOperation.tileStr2TileArray(testStr, out actual);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesNumError.ToString());
            }

        }

        [TestMethod]
        public void scanKokushi_test()
        {
            string testStr = "";
            int[] tileArray = null;
            int excepted = 0;

            testStr = "19m19p19s12234567h";
            excepted = -1;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanKokushi(tileArray), testStr + " test failed");

            testStr = "19m19p129s1234567h";
            excepted = 0;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanKokushi(tileArray), testStr + " test failed");

            testStr = "789m49p19s1234567h";
            excepted = 2;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanKokushi(tileArray), testStr + " test failed");

            testStr = "123456789m1h2579s";
            excepted = 9;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanKokushi(tileArray), testStr + " test failed");

            testStr = "123456789m25788s";
            excepted = 11;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanKokushi(tileArray), testStr + " test failed");

            testStr = "19m";
            excepted = -1;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            try
            {
                Assert.AreEqual(excepted, TileOperation.scanKokushi(tileArray), testStr + " test failed");
            }
            catch(Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesNumError.ToString(), testStr + " test failed");
            }
            
        }

        [TestMethod]
        public void scanChitoitsu_test()
        {
            string testStr = "";
            int[] tileArray = null;
            int excepted = 0;

            testStr = "1155m7788p1122s55h";
            excepted = -1;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanChitoitsu(tileArray), testStr + " test failed");

            testStr = "1155m7788p1122s57h";
            excepted = 0;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanChitoitsu(tileArray), testStr + " test failed");

            testStr = "11155m7788p1122s7h";
            excepted = 0;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanChitoitsu(tileArray), testStr + " test failed");

            testStr = "111555m7788p1122s";
            excepted = 1;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanChitoitsu(tileArray), testStr + " test failed");

            testStr = "111155m7788p1122s";
            excepted = 1;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanChitoitsu(tileArray), testStr + " test failed");

            testStr = "12356p333s11m5556h";
            excepted = 3;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.scanChitoitsu(tileArray), testStr + " test failed");

            testStr = "19m";
            excepted = -1;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            try
            {
                Assert.AreEqual(excepted, TileOperation.scanChitoitsu(tileArray), testStr + " test failed");
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesNumError.ToString(), testStr + " test failed");
            }

        }

        [TestMethod]
        public void calculateShanten_test()
        {
            string testStr = "";
            int[] tileArray = null;
            int excepted = 0;

            testStr = "123m456p789s11122h";
            excepted = -1;
            TileOperation.tileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.calculateShanten(tileArray), testStr + " test failed");
        }
    }
}
