using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChameleonGame.Model;
using Xunit;

namespace ChameleonGame.Test
{
    public class PieceTests
    {
        [Fact]
        public void Piece_SetCurrentCell_Correct()
        {
            Cell cell2 = new Cell(1, 1, CellColor.Green);
            Piece piece = new Piece(Player.Red, 0, 0);

            piece.SetCurrentCell(cell2);

            Assert.Equal(cell2.Row, piece.Row);
            Assert.Equal(cell2.Col, piece.Col);
        }

        [Fact]
        public void Piece_ChangeColor_Correct()
        {
            Piece piece = new Piece(Player.Red, 0, 0);
            piece.IncrementDelay();

            piece.ChangeColor();

            Assert.Equal(Player.Green, piece.Owner);
            Assert.Equal(0, piece.ColorChangeDelay);
        }

        [Fact]
        public void Piece_IncrementDelay_Correct()
        {
            Piece piece = new Piece(Player.Red, 0, 0);

            piece.IncrementDelay();

            Assert.Equal(1, piece.ColorChangeDelay);
        }

        [Fact]
        public void Piece_ResetDelay_Correct()
        {
            Piece piece = new Piece(Player.Red, 0, 0);
            piece.IncrementDelay();

            piece.ResetDelay();

            Assert.Equal(0, piece.ColorChangeDelay);
        }
    }
}
