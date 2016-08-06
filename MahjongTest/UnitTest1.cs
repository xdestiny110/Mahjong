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
            int[] excepted = new int[34];

            testStr = "11234m";
            excepted = new int[34];
            excepted[0] = 2; excepted[1] = 1; excepted[2] = 1; excepted[3] = 1;
            TileModel model = new TileModel(testStr);
            CollectionAssert.AreEqual(excepted, model.Tiles, testStr + " test failed");

            testStr = "56p";
            excepted = new int[34];
            excepted[13] = 1; excepted[14] = 1;
            model = new TileModel(testStr);
            CollectionAssert.AreEqual(excepted, model.Tiles, testStr + " test failed");

            testStr = "11234m56p789s1117h";
            excepted = new int[34];
            excepted[0] = 2; excepted[1] = 1; excepted[2] = 1; excepted[3] = 1;
            excepted[13] = 1; excepted[14] = 1;
            excepted[24] = 1; excepted[25] = 1; excepted[26] = 1;
            excepted[27] = 3; excepted[33] = 1;
            model = new TileModel(testStr);
            CollectionAssert.AreEqual(excepted, model.Tiles, testStr + " test failed");

            testStr = "1235h";
            excepted = new int[34];
            excepted[27] = 1; excepted[28] = 1; excepted[29] = 1; excepted[31] = 1;

            testStr = "^&$&*^";
            try
            {
                model = new TileModel(testStr);
            }
            catch(Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());                
            }            

            testStr = "123456789";
            try
            {
                model = new TileModel(testStr);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());
            }

            testStr = "s";
            try
            {
                model = new TileModel(testStr);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesStrError.ToString());
            }

            testStr = "1h";
            try
            {
                model = new TileModel(testStr);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, MahjongErrorCode.TilesNumError.ToString());
            }

        }
    }
}
