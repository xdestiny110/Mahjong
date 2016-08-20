using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mahjong;

namespace MahjongTest
{
    [TestClass]
    public class TileOperationTest
    {
        [TestMethod]
        public void tileStr2TileArray_test()
        {
            string testStr = "";
            int[] excepted = new int[34], actual = null;

            testStr = "11234m";
            excepted = new int[34];
            excepted[0] = 2; excepted[1] = 1; excepted[2] = 1; excepted[3] = 1;
            TileOperation.TileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "56p";
            excepted = new int[34];
            excepted[13] = 1; excepted[14] = 1;
            TileOperation.TileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "11112233m";
            excepted = new int[34];
            excepted[0] = 4;excepted[1] = 2;excepted[2] = 2;
            TileOperation.TileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "11234m56p789s1117h";
            excepted = new int[34];
            excepted[0] = 2; excepted[1] = 1; excepted[2] = 1; excepted[3] = 1;
            excepted[13] = 1; excepted[14] = 1;
            excepted[24] = 1; excepted[25] = 1; excepted[26] = 1;
            excepted[27] = 3; excepted[33] = 1;
            TileOperation.TileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "1235h";
            excepted = new int[34];
            excepted[27] = 1; excepted[28] = 1; excepted[29] = 1; excepted[31] = 1;
            TileOperation.TileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "7h";
            excepted = new int[34];
            excepted[33] = 1;
            TileOperation.TileStr2TileArray(testStr, out actual);
            CollectionAssert.AreEqual(excepted, actual, testStr + " test failed");

            testStr = "^&$&*^";
            try
            {
                TileOperation.TileStr2TileArray(testStr, out actual);
            }
            catch(Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());                
            }            

            testStr = "123456789";
            try
            {
                TileOperation.TileStr2TileArray(testStr, out actual);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());
            }

            testStr = "s";
            try
            {
                TileOperation.TileStr2TileArray(testStr, out actual);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());
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
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanKokushi(tileArray), testStr + " test failed");

            testStr = "19m19p129s1234567h";
            excepted = 0;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanKokushi(tileArray), testStr + " test failed");

            testStr = "789m49p19s1234567h";
            excepted = 2;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanKokushi(tileArray), testStr + " test failed");

            testStr = "123456789m1h2579s";
            excepted = 9;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanKokushi(tileArray), testStr + " test failed");

            testStr = "123456789m25788s";
            excepted = 11;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanKokushi(tileArray), testStr + " test failed");

            testStr = "19m";
            excepted = 100;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanKokushi(tileArray), testStr + " test failed");

        }

        [TestMethod]
        public void scanChitoitsu_test()
        {
            string testStr = "";
            int[] tileArray = null;
            int excepted = 0;

            testStr = "1155m7788p1122s55h";
            excepted = -1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanChitoitsu(tileArray), testStr + " test failed");

            testStr = "1155m7788p1122s57h";
            excepted = 0;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanChitoitsu(tileArray), testStr + " test failed");

            testStr = "11155m7788p1122s7h";
            excepted = 0;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanChitoitsu(tileArray), testStr + " test failed");

            testStr = "111555m7788p1122s";
            excepted = 1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanChitoitsu(tileArray), testStr + " test failed");

            testStr = "111155m7788p1122s";
            excepted = 1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanChitoitsu(tileArray), testStr + " test failed");

            testStr = "12356p333s11m5556h";
            excepted = 3;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanChitoitsu(tileArray), testStr + " test failed");

            testStr = "19m";
            excepted = 100;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.ScanChitoitsu(tileArray), testStr + " test failed");
        }

        [TestMethod]
        public void calculateShanten_test()
        {
            string testStr = "";
            int[] tileArray = null;
            int excepted = 0;

            testStr = "123m456p789s11122h";
            excepted = -1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "11112233m";
            excepted = -1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "1144p5577m4466s33h";
            excepted = -1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "19m19p19s12334567h";
            excepted = -1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "22233m123s456789p";
            excepted = -1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "11123456788999p";
            excepted = -1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "11122245679999s";
            excepted = 0;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "233789m456s111p11h";
            excepted = 0;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "111345677m15s456s";
            excepted = 1;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "11345677m159s456p";
            excepted = 2;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "1589m1358s13588p4h";
            excepted = 5;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

            testStr = "1589m1358s258p457h";
            excepted = 6;
            TileOperation.TileStr2TileArray(testStr, out tileArray);
            Assert.AreEqual(excepted, TileOperation.CalculateShanten(tileArray), testStr + " test failed");

        }

        [TestMethod]
        public void tileArr2TileStr()
        {            
            string expected = "123456789m57h";
            int[] tileArr;

            TileOperation.TileStr2TileArray(expected, out tileArr);
            Assert.AreEqual<string>(expected, TileOperation.TileArr2String(tileArr));
            

        }
    }    
}
