using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameleonGame.Persistance
{
    public record ChameleonBoardDTO
    {
        public int Size { get; set; }
        public List<PieceDTO> Pieces { get; set; } = [];
    }

    public record PieceDTO
    {
        public PlayerDTO Owner { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int ColorChangeDelay { get; set; }
    }
    public enum PlayerDTO 
    {
        Red,
        Green
    }
}
