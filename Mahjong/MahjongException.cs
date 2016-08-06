using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahjong
{

    public enum MahjongErrorCode
    {
        OK,
        TilesNumError,
        TilesStrError,
        MemoryError
    }

    class MahjongException : Exception
    {
        public string Msg { get; private set; }
        public MahjongException(MahjongErrorCode code):base(code.ToString())
        {
            Msg = code.ToString();
        }
        public static void CheckErrorCode(MahjongErrorCode code)
        {
            if (code != MahjongErrorCode.OK)
            {
                throw new MahjongException(code);
            }
        }        
    }
}
