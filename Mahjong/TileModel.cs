﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mahjong
{
    public class TileModel : Exception
    {

        //mahjong tile struct

        private int[] tiles = new int[34];
        public int[] Tiles
        {
            get
            {
                return tiles;
            }
        }

        public TileModel(string tileStr)
        {            
            tileStr2TileArray(tileStr, out tiles);
        }

        //m is man, p is pin, s is sou, h1 is east, h2 is south, h3 is west, h4 is north, h5 is bai, h6 is fang, h7 is zhong
        private readonly Dictionary<char, int> tileDict = new Dictionary<char, int>()
        {
            {'m',0 },
            {'p',9 },
            {'s',18 },
            {'h',27 },          
        };
        private void tileStr2TileArray(string tileStr, out int[] tileArray)
        {
            Regex r1 = new Regex("[1-9]"), r2 = new Regex("[msph]");
            if (!r1.IsMatch(tileStr) || !r2.IsMatch(tileStr))
                throw new MahjongException(MahjongErrorCode.TilesStrError);

            tileStr = tileStr.ToLower();
            int tileNum = 0, index = 0; ;
            string[] tileStrArr = r2.Split(tileStr);
            var matches = r2.Matches(tileStr);
            tileArray = new int[34];

            foreach (Match match in matches)
            {
                for (int i = index; i < match.Index; i++)
                {
                    tileArray[tileDict[match.Value[0]] + tileStr[i] - '1']++;
                    tileNum++;
                }
                index = match.Index + 1;
            }

            if (tileNum < 2 || tileNum > 14 || (tileNum - 2) % 3 != 0)
                throw new MahjongException(MahjongErrorCode.TilesNumError);

            if (tileArray.Any(_ => _ > 4))
                throw new MahjongException(MahjongErrorCode.TilesNumError);

        }

    }
}