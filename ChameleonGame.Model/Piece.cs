namespace ChameleonGame.Model
{
    public class Piece
    {
        public Player Owner { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public int ColorChangeDelay { get; private set; } = 0;

        public Piece(Player owner, int row, int col)
        {
            Owner = owner;
            Row = row;
            Col = col;
        }

        public Piece(Player owner, int row, int col, int delay)
        {
            Owner = owner;
            Row = row;
            Col = col;
            ColorChangeDelay = delay;
        }

        public void SetCurrentCell(Cell cell)
        {
            Row = cell.Row;
            Col = cell.Col;
        }

        public void ChangeColor()
        {
            Owner = Owner == Player.Red ? Player.Green : Player.Red;
            ResetDelay();
        }

        public void IncrementDelay()
        {
            ColorChangeDelay++;
        }

        public void ResetDelay()
        {
            ColorChangeDelay = 0;
        }
    }
}
