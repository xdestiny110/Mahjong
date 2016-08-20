using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mahjong;

namespace MahjongTest
{
    [TestClass]
    class PlayerModelTest
    {
        [TestMethod]
        public void PlayerModelInit_Test()
        {
            int[] excepted = new int[34];

            PlayerModel player = new PlayerModel("1234567m444s135h");
            player.DrawTile("1s");
            excepted[0] = 1; excepted[1] = 1; excepted[2] = 1; excepted[3] = 1; excepted[4] = 1; excepted[5] = 1; excepted[6] = 1;
            excepted[18] = 1;excepted[21] = 3;
            excepted[27] = 1;excepted[29] = 1;excepted[31] = 1;
            CollectionAssert.AreEqual(excepted, player.Tiles);

            player.DiscardTile("7m");
            excepted[6] = 0;
            CollectionAssert.AreEqual(excepted, player.Tiles);
        }

        [TestMethod]
        public void PlayerModelCalculate_Test()
        {

        }
    }
}
