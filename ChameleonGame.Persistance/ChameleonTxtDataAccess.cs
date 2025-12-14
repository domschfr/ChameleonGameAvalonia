using System.Text;

namespace ChameleonGame.Persistance
{
    public class ChameleonTxtDataAccess : IChameleonDataAccess
    {
        public void SaveGame(String path, ChameleonBoardDTO board, PlayerDTO currentPlayer)
        {
            try
            {
                StringBuilder sb = new();
                _ = sb.Append($"{(currentPlayer == PlayerDTO.Red ? 'r' : 'g')}\t{board.Size.ToString()}\n");

                foreach (PieceDTO piece in board.Pieces)
                {
                    _ = sb.Append($"{piece.Row}\t{piece.Col}\t{(piece.Owner == PlayerDTO.Red ? 'r' : 'g')}\t{piece.ColorChangeDelay}\n");
                }

                using StreamWriter streamWriter = new(path);
                streamWriter.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                throw new ChameleonDataException(ex.Message, ex);
            }
        }

        public ChameleonBoardDTO LoadGame(String path, out PlayerDTO currentPlayer)
        {
            try
            {
                using StreamReader streamReader = new(path);
                string[] boardInfo = (streamReader.ReadLine() ?? String.Empty).Split('\t');
                currentPlayer = boardInfo[0] == "r" ? PlayerDTO.Red : PlayerDTO.Green;
                _ = int.TryParse(boardInfo[1], out int size);

                List<PieceDTO> pieces = [];

                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine() ?? String.Empty;
                    string[] pieceString = line.Split('\t');

                    _ = int.TryParse(pieceString[0], out int i);
                    _ = int.TryParse(pieceString[1], out int j);
                    PlayerDTO owner = pieceString[2] == "r" ? PlayerDTO.Red : PlayerDTO.Green;
                    _ = int.TryParse(pieceString[3], out int delay);

                    PieceDTO piece = new()
                    {
                        Owner = owner,
                        Row = i,
                        Col = j,
                        ColorChangeDelay = delay
                    };

                    pieces.Add(piece);
                }

                return new ChameleonBoardDTO()
                {
                    Size = size,
                    Pieces = pieces
                };
            }
            catch (Exception ex)
            {

                throw new ChameleonDataException(ex.Message, ex);
            }
        }
    }

}
