using System;
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
            TileOperation.tileStr2TileArray(tileStr, out tiles);
        }        

        private int calculateShanten(int[] tileArray)
        {
            int shanten = 8;
            return 0;
        }       
    }
    
}
