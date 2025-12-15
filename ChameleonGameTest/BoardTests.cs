using ChameleonGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameleonGame.Test
{
    public class BoardTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(-1)]
        public void Board_Constructor_InvalidSize(int invalidSize)
        {
            Assert.Throws<ArgumentException>(() => new ChameleonBoard(invalidSize));
        }

        [Fact]
        public void Board_InitializeCells_3x3_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(3);
            Assert.Equal(CellColor.Red, board[0, 0].Color);
            Assert.Equal(CellColor.Red, board[0, 1].Color);
            Assert.Equal(CellColor.Red, board[0, 2].Color);
            Assert.Equal(CellColor.Red, board[1, 0].Color);
            Assert.Equal(CellColor.Gray, board[1, 1].Color);
            Assert.Equal(CellColor.Green, board[1, 2].Color);
            Assert.Equal(CellColor.Green, board[2, 0].Color);
            Assert.Equal(CellColor.Green, board[2, 1].Color);
            Assert.Equal(CellColor.Green, board[2, 2].Color);
        }

        [Fact]
        public void Board_InitializeCells_5x5_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(5);
            Assert.Equal(CellColor.Red, board[0, 0].Color);
            Assert.Equal(CellColor.Red, board[0, 1].Color);
            Assert.Equal(CellColor.Red, board[0, 2].Color);
            Assert.Equal(CellColor.Red, board[0, 3].Color);
            Assert.Equal(CellColor.Red, board[0, 4].Color);
            Assert.Equal(CellColor.Red, board[1, 0].Color);
            Assert.Equal(CellColor.Green, board[1, 1].Color);
            Assert.Equal(CellColor.Green, board[1, 2].Color);
            Assert.Equal(CellColor.Green, board[1, 3].Color);
            Assert.Equal(CellColor.Green, board[1, 4].Color);
            Assert.Equal(CellColor.Red, board[2, 0].Color);
            Assert.Equal(CellColor.Green, board[2, 1].Color);
            Assert.Equal(CellColor.Gray, board[2, 2].Color);
            Assert.Equal(CellColor.Red, board[2, 3].Color);
            Assert.Equal(CellColor.Green, board[2, 4].Color);
            Assert.Equal(CellColor.Red, board[3, 0].Color);
            Assert.Equal(CellColor.Red, board[3, 1].Color);
            Assert.Equal(CellColor.Red, board[3, 2].Color);
            Assert.Equal(CellColor.Red, board[3, 3].Color);
            Assert.Equal(CellColor.Green, board[3, 4].Color);
            Assert.Equal(CellColor.Green, board[4, 0].Color);
            Assert.Equal(CellColor.Green, board[4, 1].Color);
            Assert.Equal(CellColor.Green, board[4, 2].Color);
            Assert.Equal(CellColor.Green, board[4, 3].Color);
            Assert.Equal(CellColor.Green, board[4, 4].Color);
        }

        [Fact]
        public void Board_InitializeCells_7x7_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(7);
            Assert.Equal(CellColor.Red, board[0, 0].Color);
            Assert.Equal(CellColor.Red, board[0, 1].Color);
            Assert.Equal(CellColor.Red, board[0, 2].Color);
            Assert.Equal(CellColor.Red, board[0, 3].Color);
            Assert.Equal(CellColor.Red, board[0, 4].Color);
            Assert.Equal(CellColor.Red, board[0, 5].Color);
            Assert.Equal(CellColor.Red, board[0, 6].Color);
            Assert.Equal(CellColor.Red, board[1, 0].Color);
            Assert.Equal(CellColor.Green, board[1, 1].Color);
            Assert.Equal(CellColor.Green, board[1, 2].Color);
            Assert.Equal(CellColor.Green, board[1, 3].Color);
            Assert.Equal(CellColor.Green, board[1, 4].Color);
            Assert.Equal(CellColor.Green, board[1, 5].Color);
            Assert.Equal(CellColor.Green, board[1, 6].Color);
            Assert.Equal(CellColor.Red, board[2, 0].Color);
            Assert.Equal(CellColor.Green, board[2, 1].Color);
            Assert.Equal(CellColor.Red, board[2, 2].Color);
            Assert.Equal(CellColor.Red, board[2, 3].Color);
            Assert.Equal(CellColor.Red, board[2, 4].Color);
            Assert.Equal(CellColor.Red, board[2, 5].Color);
            Assert.Equal(CellColor.Green, board[2, 6].Color);
            Assert.Equal(CellColor.Red, board[3, 0].Color);
            Assert.Equal(CellColor.Green, board[3, 1].Color);
            Assert.Equal(CellColor.Red, board[3, 2].Color);
            Assert.Equal(CellColor.Gray, board[3, 3].Color);
            Assert.Equal(CellColor.Green, board[3, 4].Color);
            Assert.Equal(CellColor.Red, board[3, 5].Color);
            Assert.Equal(CellColor.Green, board[3, 6].Color);
            Assert.Equal(CellColor.Red, board[4, 0].Color);
            Assert.Equal(CellColor.Green, board[4, 1].Color);
            Assert.Equal(CellColor.Green, board[4, 2].Color);
            Assert.Equal(CellColor.Green, board[4, 3].Color);
            Assert.Equal(CellColor.Green, board[4, 4].Color);
            Assert.Equal(CellColor.Red, board[4, 5].Color);
            Assert.Equal(CellColor.Green, board[4, 6].Color);
            Assert.Equal(CellColor.Red, board[5, 0].Color);
            Assert.Equal(CellColor.Red, board[5, 1].Color);
            Assert.Equal(CellColor.Red, board[5, 2].Color);
            Assert.Equal(CellColor.Red, board[5, 3].Color);
            Assert.Equal(CellColor.Red, board[5, 4].Color);
            Assert.Equal(CellColor.Red, board[5, 5].Color);
            Assert.Equal(CellColor.Green, board[5, 6].Color);
            Assert.Equal(CellColor.Green, board[6, 0].Color);
            Assert.Equal(CellColor.Green, board[6, 1].Color);
            Assert.Equal(CellColor.Green, board[6, 2].Color);
            Assert.Equal(CellColor.Green, board[6, 3].Color);
            Assert.Equal(CellColor.Green, board[6, 4].Color);
            Assert.Equal(CellColor.Green, board[6, 5].Color);
            Assert.Equal(CellColor.Green, board[6, 6].Color);
        }

        [Fact]
        public void Board_InitializePieces_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(3);
            board.InitializePieces();
            Assert.NotNull(board[0, 0].Piece);
            Assert.Equal(Player.Red, board[0, 0].Piece!.Owner);
            Assert.NotNull(board[0, 1].Piece);
            Assert.Equal(Player.Red, board[0, 0].Piece!.Owner);
            Assert.NotNull(board[0, 2].Piece);
            Assert.Equal(Player.Red, board[0, 0].Piece!.Owner);
            Assert.NotNull(board[1, 0].Piece);
            Assert.Equal(Player.Red, board[0, 1].Piece!.Owner);
            Assert.Null(board[1, 1].Piece);
            Assert.NotNull(board[1, 2].Piece);
            Assert.Equal(Player.Green, board[1, 2].Piece!.Owner);
            Assert.NotNull(board[2, 0].Piece);
            Assert.Equal(Player.Green, board[2, 0].Piece!.Owner);
            Assert.NotNull(board[2, 1].Piece);
            Assert.Equal(Player.Green, board[2, 1].Piece!.Owner);
            Assert.NotNull(board[2, 2].Piece);
            Assert.Equal(Player.Green, board[2, 2].Piece!.Owner);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void Board_ValidCell_Correct(int row, int col)
        {
            ChameleonBoard board = new ChameleonBoard(3);

            Assert.True(board.ValidCell(board[row, col]));
        }
        
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(3, 0)]
        [InlineData(0, 3)]
        public void Board_InvalidCells_Correct(int row, int col)
        {
            ChameleonBoard board = new ChameleonBoard(3);

            Assert.False(board.ValidCell(new Cell(row, col, CellColor.Red)));
        }

        [Fact]
        public void Board_TryMovePiece_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(3);
            board.InitializePieces();
            Player currentPlayer = Player.Red;

            // Valid move
            bool moveResult = board.TryMovePiece(board[1, 0], board[1, 1], currentPlayer);

            Assert.True(moveResult);
            Assert.Null(board[1, 0].Piece);
            Assert.NotNull(board[1, 1].Piece);
            Assert.Equal(Player.Red, board[1, 1].Piece!.Owner);

            // Invalid move: moving opponent's piece
            moveResult = board.TryMovePiece(board[2, 0], board[1, 0], currentPlayer);

            Assert.False(moveResult);
            Assert.NotNull(board[2, 0].Piece);
            Assert.Null(board[1, 0].Piece);
            Assert.Equal(Player.Green, board[2, 0].Piece!.Owner);

            // Invalid move: moving from empty cell
            moveResult = board.TryMovePiece(board[1, 0], board[1, 1], currentPlayer);

            Assert.False(moveResult);
            Assert.NotNull(board[1, 1].Piece);
            Assert.Null(board[1, 0].Piece);

            // Invalid move: moving to occupied cell
            moveResult = board.TryMovePiece(board[0, 1], board[0, 2], currentPlayer);

            Assert.False(moveResult);
            Assert.NotNull(board[0, 1].Piece);
            Assert.NotNull(board[0, 2].Piece);

            // Invalid move: moving to non-adjacent cell
            moveResult = board.TryMovePiece(board[0, 1], board[1, 0], currentPlayer);

            Assert.False(moveResult);
            Assert.NotNull(board[0, 1].Piece);

            // Invalid move: moving to same cell
            moveResult = board.TryMovePiece(board[0, 1], board[0, 1], currentPlayer);

            Assert.False(moveResult);
            Assert.NotNull(board[0, 1].Piece);

            // Valid move: switch player and move
            currentPlayer = Player.Green;
            moveResult = board.TryMovePiece(board[2, 0], board[1, 0], currentPlayer);

            Assert.True(moveResult);
            Assert.Null(board[2, 0].Piece);
            Assert.NotNull(board[1, 0].Piece);
            Assert.Equal(Player.Green, board[1, 0].Piece!.Owner);
        }

        [Fact]
        public void Board_TryJump_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(3);
            board.InitializePieces();
            Player currentPlayer = Player.Red;

            _ = board.TryMovePiece(board[1, 2], board[1, 1], Player.Green); // Move Green piece to allow jump

            // Valid jump
            bool jumpResult = board.TryJump(board[1, 0], board[1, 2], currentPlayer);

            Assert.True(jumpResult);
            Assert.NotNull(board[1, 2].Piece);
            Assert.Equal(Player.Red, board[1, 2].Piece!.Owner);
            Assert.Null(board[1, 0].Piece);
            Assert.Null(board[1, 1].Piece);

            _ = board.TryMovePiece(board[0, 1], board[1, 1], Player.Red);

            // Invalid jump: jumping opponent's piece
            jumpResult = board.TryJump(board[1, 2], board[1, 0], Player.Green);

            Assert.False(jumpResult);
            Assert.NotNull(board[1, 2].Piece);
            Assert.NotNull(board[1, 1].Piece);
            Assert.Null(board[1, 0].Piece);

            // Invalid jump: jumping over own piece
            jumpResult = board.TryJump(board[1, 2], board[1, 0], Player.Red);

            Assert.False(jumpResult);
            Assert.NotNull(board[1, 2].Piece);
            Assert.NotNull(board[1, 1].Piece);
            Assert.Null(board[1, 0].Piece);

            // Valid jump: switch player and jump
            jumpResult = board.TryJump(board[2, 1], board[0, 1], Player.Green);

            Assert.True(jumpResult);
            Assert.NotNull(board[0, 1].Piece);
            Assert.Equal(Player.Green, board[0, 1].Piece!.Owner);
            Assert.Null(board[1, 1].Piece);
            Assert.Null(board[2, 1].Piece);

            // Invalid jump: jumping over empty cell
            jumpResult = board.TryJump(board[1, 2], board[1, 0], currentPlayer);

            Assert.False(jumpResult);
            Assert.NotNull(board[1, 2].Piece);
            Assert.Null(board[1, 1].Piece);
            Assert.Null(board[1, 0].Piece);

            // Invalid jump: jumping to occupied cell
            jumpResult = board.TryJump(board[0, 0], board[0, 2], currentPlayer);

            Assert.False(jumpResult);
            Assert.NotNull(board[0, 0].Piece);
            Assert.NotNull(board[0, 2].Piece);

            _ = board.TryMovePiece(board[2, 2], board[2, 1], Player.Green);
            _ = board.TryMovePiece(board[2, 1], board[1, 1], Player.Green);

            // Invalid jump: jumping to non-adjacent cell
            jumpResult = board.TryJump(board[0, 0], board[2, 2], currentPlayer);

            Assert.False(jumpResult);
            Assert.NotNull(board[0, 0].Piece);
            Assert.Null(board[2, 2].Piece);
            Assert.NotNull(board[1, 1].Piece);

            // Invalid jump: jumping to same cell
            jumpResult = board.TryJump(board[0, 0], board[0, 0], currentPlayer);

            Assert.False(jumpResult);
            Assert.NotNull(board[0, 0].Piece);
        }

        [Fact]
        public void Board_PerformColorChange_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(3);
            board[1, 0].ChangePiece(new Piece(Player.Red, 1, 0));
            board[1, 1].ChangePiece(new Piece(Player.Red, 1, 1));
            board[1, 2].ChangePiece(new Piece(Player.Red, 1, 2));
            board[0, 0].ChangePiece(new Piece(Player.Green, 0, 0));

            board.HandleColorChange(board[1, 0].Piece!, board[1, 0]);
            board.HandleColorChange(board[1, 1].Piece!, board[1, 1]);
            board.HandleColorChange(board[1, 2].Piece!, board[1, 2]);
            board.HandleColorChange(board[0, 0].Piece!, board[0, 0]);

            // Increment delays
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    board[i, j].Piece?.IncrementDelay();
                }
            }

            board[1, 2].Piece!.IncrementDelay(); // Now delay is 2 for this piece
            board[0, 0].Piece!.IncrementDelay();

            // Perform color change
            board.PerformColorChange();

            // Check that pieces with delay >= 1 have changed color
            Assert.NotNull(board[1, 0].Piece);
            Assert.Equal(Player.Red, board[1, 0].Piece!.Owner);
            Assert.Equal(1, board[1, 0].Piece!.ColorChangeDelay);
            Assert.NotNull(board[1, 1].Piece);
            Assert.Equal(Player.Red, board[1, 1].Piece!.Owner);
            Assert.Equal(1, board[1, 1].Piece!.ColorChangeDelay);
            Assert.NotNull(board[1, 2].Piece);
            Assert.Equal(Player.Green, board[1, 2].Piece!.Owner);
            Assert.Equal(0, board[1, 2].Piece!.ColorChangeDelay);
            Assert.NotNull(board[0, 0].Piece);
            Assert.Equal(Player.Red, board[0, 0].Piece!.Owner);
            Assert.Equal(0, board[0, 0].Piece!.ColorChangeDelay);
        }

        [Fact]
        public void Board_PlayerHasPieces_Correct()
        {
            ChameleonBoard board = new ChameleonBoard(3);
            board.InitializePieces();

            Assert.True(board.PlayerHasPieces(Player.Red));
            Assert.True(board.PlayerHasPieces(Player.Green));

            // Remove all Red pieces
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board[i, j].Piece?.Owner == Player.Red)
                    {
                        board[i, j].ChangePiece(null);
                    }
                }
            }

            Assert.False(board.PlayerHasPieces(Player.Red));
            Assert.True(board.PlayerHasPieces(Player.Green));

            // Remove all Green pieces
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board[i, j].Piece?.Owner == Player.Green)
                    {
                        board[i, j].ChangePiece(null);
                    }
                }
            }

            Assert.False(board.PlayerHasPieces(Player.Red));
            Assert.False(board.PlayerHasPieces(Player.Green));
        }
    }
}
