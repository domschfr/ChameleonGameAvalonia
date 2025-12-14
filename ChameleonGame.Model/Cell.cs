namespace ChameleonGame.Model
{
    public enum CellColor
    {
        Red,
        Green,
        Gray
    }

    public class Cell(int row, int col, CellColor color)
    {
        public int Row { get; } = row;
        public int Col { get; } = col;
        public CellColor Color { get; } = color;
        public Piece? Piece { get; private set; } = null;

        public void ChangePiece(Piece? newPiece)
        {
            Piece = newPiece;
        }
    }
}
