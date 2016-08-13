using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mahjong
{
    public static class TileOperation
    {
        //m is man, p is pin, s is sou, h1 is east, h2 is south, h3 is west, h4 is north, h5 is white, h6 is green, h7 is red
        private static readonly Dictionary<char, int> tileDict = new Dictionary<char, int>()
        {
            {'m',0 },
            {'p',9 },
            {'s',18 },
            {'h',27 },
        };

        /// <summary>
        /// Change string to int[]
        /// </summary>
        /// <param name="tileStr">input string. Such as 123p456m789s11122h</param>
        /// <param name="tileArray">tile represent as int[]</param>
        public static void tileStr2TileArray(string tileStr, out int[] tileArray)
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

        public static int scanChitoitsu(int[] tileArray)
        {
            //check chitoitsu
            if (tileArray.Aggregate((sum, v) => sum + v) != 14)
                throw new MahjongException(MahjongErrorCode.TilesNumError);

            int completed_pairs = 0, pairs = 0;
            completed_pairs += tileArray.Count(_ => _ >= 2);
            pairs += tileArray.Count(_ => _ != 0);
            return (6 - completed_pairs + ((pairs < 7) ? 7 - pairs : 0));
        }

        public static int scanKokushi(int[] tileArray)
        {
            //check kokushi
            if (tileArray.Aggregate((sum, v) => sum + v) != 14)
                throw new MahjongException(MahjongErrorCode.TilesNumError);

            HashSet<int> kokushiTable = new HashSet<int>() { 0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 32, 33 };
            int completedTerminals = 0, terminals = 0; ;
            completedTerminals += kokushiTable.Count(_ => tileArray[_] >= 2);
            terminals += kokushiTable.Count(_ => tileArray[_] != 0);
            return 13 - terminals - ((completedTerminals == 0) ? 0 : 1);
        }


        private static int melds = 0;
        private static int jidahai = 0;
        private static int number = 0;
        private static int isolated = 0;
        private static int pairs = 0;

        public static int calculateShanten(int[] tileArray)
        {
            init();
            int tileCount = tileArray.Aggregate((sum, v) => sum + v);
            int minShanten =
                Math.Min(scanChitoitsu(tileArray), scanKokushi(tileArray));
            calculateHornorTiles(tileArray);
            //tile that already chii, hon and ga
            int initMentsu = (int)Math.Floor((14 - tileCount) / 3.0);


            return 0;
        }

        private static void init()
        {
            melds = 0;
            jidahai = 0;
            pairs = 0;
            number = 0;
            isolated = 0;
        }

        private static void calculateHornorTiles(int[] tileArray)
        {
            //check honors
            int _number = 0;
            int _isolated = 0;

            for (int i = 27; i < 34; i++)
            {
                switch (tileArray[i])
                {
                    case 4:
                        melds++;
                        jidahai++;
                        _number |= 1 << (i - 27);
                        _isolated |= 1 << (i - 27);
                        break;
                    case 3:
                        melds++;
                        break;
                    case 2:
                        pairs++;
                        break;
                    case 1:
                        _isolated |= 1 << (i - 27);
                        break;
                }
            }

            jidahai = (jidahai > 0 && tileArray.Length % 3 == 2) ? jidahai + 1 : jidahai;
            if (_isolated != 0)
            {
                isolated |= 1 << 27;
                if ((_number | _isolated) == _number)
                    number |= 1 << 27;
            }
        }

        private static void DFS(int depth)
        {

        }

        private enum TilesCombineType
        {
            Set, Pair, Syuntsu, Tatsu1, Tatsu2, Isolated
        }

        private static void typeOperation(ref int[] tileArr, int tileIdx, TilesCombineType type, int signFlag)
        {
            signFlag = Math.Sign(signFlag);
            switch (type)
            {
                case TilesCombineType.Set:
                    tileArr[tileIdx] += 3 * signFlag;
                    melds += signFlag;
                    break;
                case TilesCombineType.Pair:
                    tileArr[tileIdx] += 2 * signFlag;
                    pairs += signFlag;
                    break;
                case TilesCombineType.Syuntsu:
                    for(int i = tileIdx; i < tileIdx + 3; i++)
                    {
                        tileArr[i] += signFlag;
                        melds += signFlag;
                    }
                    break;
                case TilesCombineType.Tatsu1:
                    break;
                case TilesCombineType.Tatsu2:
                    break;
                case TilesCombineType.Isolated:
                    break;
                default:
                    break;
            }
        }

    }
}
