using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static void TileStr2TileArray(string tileStr, out int[] tileArray)
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

            if (tileArray.Any(_ => _ > 4))
                throw new MahjongException(MahjongErrorCode.TilesNumError);
        }

        public static int ScanChitoitsu(int[] tileArray)
        {
            //check chitoitsu
            if (tileArray.Aggregate((sum, v) => sum + v) != 14)
                return 100;

            int completed_pairs = 0, pairs = 0;
            completed_pairs += tileArray.Count(_ => _ >= 2);
            pairs += tileArray.Count(_ => _ != 0);
            return (6 - completed_pairs + ((pairs < 7) ? 7 - pairs : 0));
        }

        public static int ScanKokushi(int[] tileArray)
        {
            //check kokushi
            if (tileArray.Aggregate((sum, v) => sum + v) != 14)
                return 100;

            HashSet<int> kokushiTable = new HashSet<int>() { 0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 32, 33 };
            int completedTerminals = 0, terminals = 0; ;
            completedTerminals += kokushiTable.Count(_ => tileArray[_] >= 2);
            terminals += kokushiTable.Count(_ => tileArray[_] != 0);
            return 13 - terminals - ((completedTerminals == 0) ? 0 : 1);
        }

        public static void MergeTileArr(ref int[] tileArr1, int[] tileArr2, bool isPlus)
        {
            int sign = (isPlus) ? 1 : -1;
            for (int i = 0; i < tileArr2.Length; i++)
            {
                if (tileArr2[i] != 0)
                    tileArr1[i] += tileArr2[i] * sign;
            }

            var tileNum = tileArr1.Aggregate((sum, v) => { return sum + v; });
            if (tileNum > 14 || tileNum < 1 || tileArr1.Any(_ => { return _ > 4 || _ < 0; }))
                throw new MahjongException(MahjongErrorCode.TilesNumError);

        }

        public static string TileArr2String(int[] tileArr)
        {
            if (tileArr == null || !tileArr.Any(_ => { return _ > 0 && _ <= 4; }))
                throw new MahjongException(MahjongErrorCode.TilesNumError);

            StringBuilder result = new StringBuilder();
            for(int i = 0; i < tileArr.Length; i++)
            {
                if (i == 9 && !string.IsNullOrEmpty(result.ToString()) && result[result.Length - 1] >= '1' && result[result.Length - 1] <= '9')
                    result.Append('m');
                if (i == 18 && !string.IsNullOrEmpty(result.ToString()) && result[result.Length - 1] >= '1' && result[result.Length - 1] <= '9')
                    result.Append('p');
                if (i == 27 && !string.IsNullOrEmpty(result.ToString()) && result[result.Length - 1] >= '1' && result[result.Length - 1] <= '9')
                    result.Append('s');

                for (int j = 0; j < tileArr[i]; j++)
                    result.Append(i % 9 + 1);
            }

            var ch = result[result.Length - 1];
            if (ch >= '1' && ch <= '7')
                result.Append('h');

            return result.ToString();
        }
        
        private static int melds = 0;
        private static int jidahai = 0;
        private static int characters = 0;
        private static int isolated = 0;
        private static int pairs = 0;
        private static int tatsu = 0;
        private static int shanten = 8;

        public static int CalculateShanten(int[] tileArray)
        {
            init();
            int tileCount = tileArray.Aggregate((sum, v) => sum + v);
            int minShanten =
                Math.Min(ScanChitoitsu(tileArray), ScanKokushi(tileArray));
            calculateHornorTiles(tileArray);
            //tile that already chii, hon and ga
            melds += (int)Math.Floor((14 - tileCount) / 3.0);
            for(int i = 0; i < 27; i++)
            {
                if (tileArray[i] == 4)
                    characters |= (1 << i);
            }

            DFS(ref tileArray, 0);
            return Math.Min(shanten,minShanten);
        }

        private static void init()
        {
            melds = 0;
            jidahai = 0;
            pairs = 0;
            characters = 0;
            isolated = 0;
            tatsu = 0;
            shanten = 8;
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
                        _number |= (1 << (i - 27));
                        _isolated |= (1 << (i - 27));
                        break;
                    case 3:
                        melds++;
                        break;
                    case 2:
                        pairs++;
                        break;
                    case 1:
                        _isolated |= (1 << (i - 27));
                        break;
                }
            }

            jidahai = (jidahai > 0 && tileArray.Length % 3 == 2) ? jidahai + 1 : jidahai;
            if (_isolated != 0)
            {
                isolated |= (1 << 27);
                if ((_number | _isolated) == _number)
                    characters |= (1 << 27);
            }
        }

        private static void DFS(ref int[] tileArr,int depth)
        {
            while (tileArr[depth] <= 0)
            {
                depth++;
                if (depth >= 27)
                    break;
            }

            //dfs end condition
            if (depth >= 27)
            {
                int temp = 8 - melds * 2 - pairs;
                int mentsuKouho = melds + tatsu;
                if (pairs > 0)
                    mentsuKouho += pairs - 1;
                else
                {
                    if (characters > 0 && isolated > 0 && (characters | isolated) == characters)
                        temp += 1;
                }

                if (mentsuKouho > 4)
                    temp += mentsuKouho - 4;
                if (temp >= 0 && temp < jidahai)
                    temp = jidahai;

                shanten = (shanten > temp) ? temp : shanten;
                return;
            }

            switch (tileArr[depth])
            {
                case 4:
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Set, 1);
                    if (checkTatauPossible(tileArr, depth, depth + 1))
                    {
                        if (checkTatauPossible(tileArr, depth, depth + 2))
                        {
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, 1);
                            DFS(ref tileArr, depth + 1);
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, -1);
                        }
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, -1);
                    }
                    if (checkTatauPossible(tileArr, depth, depth + 2))
                    {
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, -1);
                    }
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Isolated, 1);
                    DFS(ref tileArr, depth + 1);
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Isolated, -1);
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Set, -1);

                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Pair, 1);
                    if (checkTatauPossible(tileArr, depth, depth + 1))
                    {
                        if (checkTatauPossible(tileArr, depth, depth + 2))
                        {
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, 1);
                            DFS(ref tileArr, depth);
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, -1);
                        }
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, -1);
                    }
                    if (checkTatauPossible(tileArr, depth, depth + 2))
                    {
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, -1);
                    }
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Pair, -1);
                    break;
                case 3:
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Set, 1);
                    DFS(ref tileArr, depth + 1);
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Set, -1);

                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Pair, 1);
                    if (checkTatauPossible(tileArr, depth, depth + 1))
                    {
                        if (checkTatauPossible(tileArr, depth, depth + 2))
                        {
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, 1);
                            DFS(ref tileArr, depth);
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, -1);
                        }
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, -1);
                    }
                    if (checkTatauPossible(tileArr, depth, depth + 2))
                    {
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, -1);
                    }
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Pair, -1);

                    if (checkTatauPossible(tileArr, depth, depth + 1) &&
                        checkTatauPossible(tileArr, depth, depth + 2) &&
                        tileArr[depth + 1] >= 2 && tileArr[depth + 2] >= 2)
                    {
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, 2);
                        DFS(ref tileArr, depth);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, -2);
                    }
                    break;
                case 2:
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Pair, 1);
                    DFS(ref tileArr, depth + 1);
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Pair, -1);
                    if(checkTatauPossible(tileArr, depth,depth+1) && checkTatauPossible(tileArr, depth, depth + 2))
                    {
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, 1);
                        DFS(ref tileArr, depth);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, -1);
                    }
                    break;
                case 1:
                    if (checkTatauPossible(tileArr, depth, depth + 1))
                    {
                        if (checkTatauPossible(tileArr, depth, depth + 2))
                        {
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, 1);
                            DFS(ref tileArr, depth + 1);
                            tileTypeOperation(ref tileArr, depth, TilesCombineType.Syuntsu, -1);
                        }
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu1, -1);
                    }
                    if (checkTatauPossible(tileArr, depth, depth + 2))
                    {
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, 1);
                        DFS(ref tileArr, depth + 1);
                        tileTypeOperation(ref tileArr, depth, TilesCombineType.Tatsu2, -1);
                    }
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Isolated, 1);
                    DFS(ref tileArr, depth + 1);
                    tileTypeOperation(ref tileArr, depth, TilesCombineType.Isolated, -1);
                    break;
            }
        }

        private enum TilesCombineType
        {
            Set, Pair, Syuntsu, Tatsu1, Tatsu2, Isolated
        }

        private static void tileTypeOperation(ref int[] tileArr, int tileIdx, TilesCombineType type, int signFlag)
        {
            signFlag = Math.Sign(signFlag);
            switch (type)
            {
                case TilesCombineType.Set:
                    tileArr[tileIdx] -= 3 * signFlag;
                    melds += signFlag;
                    break;
                case TilesCombineType.Pair:
                    tileArr[tileIdx] -= 2 * signFlag;
                    pairs += signFlag;
                    break;
                case TilesCombineType.Syuntsu:                   
                    tileArr[tileIdx] -= signFlag;
                    tileArr[tileIdx+1] -= signFlag;
                    tileArr[tileIdx+2] -= signFlag;
                    melds += signFlag;
                    break;
                case TilesCombineType.Tatsu1:
                    tileArr[tileIdx] -= signFlag;
                    tileArr[tileIdx + 1] -= signFlag;
                    tatsu += signFlag;
                    break;
                case TilesCombineType.Tatsu2:
                    tileArr[tileIdx] -= signFlag;
                    tileArr[tileIdx + 2] -= signFlag;
                    tatsu += signFlag;
                    break;
                case TilesCombineType.Isolated:
                    tileArr[tileIdx] -= signFlag;
                    isolated |= (1 << tileIdx);
                    break;
                default:
                    break;
            }
        }

        private static bool checkTatauPossible(int[] tileArr ,int idx1, int idx2)
        {
            return (idx1 < idx2 && idx1 % 9 < idx2 % 9 && tileArr[idx1] > 0 && tileArr[idx2] > 0);
        }

    }
}
