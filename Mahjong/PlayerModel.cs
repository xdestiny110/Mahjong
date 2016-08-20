using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mahjong
{
    public class PlayerModel
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

        private Dictionary<int, List<int>> calculateResult = new Dictionary<int, List<int>>();

        public PlayerModel(string tileStr)
        {
            TileOperation.TileStr2TileArray(tileStr, out tiles);
        }

        public void DrawTile(string tile)
        {
            var singleTile = new int[34];
            TileOperation.TileStr2TileArray(tile, out singleTile);
            TileOperation.MergeTileArr(ref tiles, singleTile, true);
        }

        public void DiscardTile(string tile)
        {
            var singleTile = new int[34];
            TileOperation.TileStr2TileArray(tile, out singleTile);
            TileOperation.MergeTileArr(ref tiles, singleTile, false);
        }

        private int calculateShanten()
        {
            calculateResult.Clear();
            int shantenNow = TileOperation.CalculateShanten(tiles);
            
            for(int i = 0; i < 34; i++)
            {
                for(int j = 0; j < 34; j++)
                {
                    if (i == j) continue;


                }
            }

            return 0;
        }       
    }
    
}
