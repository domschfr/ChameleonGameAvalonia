using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChameleonGame.Model;
using Xunit;

namespace ChameleonGame.Test
{
    public class CellTests
    {
        [Fact]
        public void Cell_ChangePiece_Correct()
        {
            Cell cell = new Cell(0, 0, CellColor.Red);
            Piece piece = new Piece(Player.Red, 0, 0);

            cell.ChangePiece(piece);

            Assert.Equal(piece, cell.Piece);
            Assert.Equal(Player.Red, cell.Piece!.Owner);
        }

        [Fact]
        public void Cell_RemovePiece_Correct()
        {
            Cell cell = new Cell(0, 0, CellColor.Red);
            Piece piece = new Piece(Player.Red, 0, 0);
            cell.ChangePiece(piece);

            cell.ChangePiece(null);

            Assert.Null(cell.Piece);
        }
    }
}
