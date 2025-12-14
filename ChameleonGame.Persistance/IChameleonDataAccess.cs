namespace ChameleonGame.Persistance
{
    public interface IChameleonDataAccess
    {
        void SaveGame(String path, ChameleonBoardDTO board, PlayerDTO currentPlayer);
        ChameleonBoardDTO LoadGame(String path, out PlayerDTO currentPlayer);
    }
}
