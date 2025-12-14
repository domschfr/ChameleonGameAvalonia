namespace ChameleonGame.Persistance
{

    [Serializable]
    public class ChameleonDataException : Exception
    {
        public ChameleonDataException()
        {
        }

        public ChameleonDataException(string? message) : base(message)
        {
        }

        public ChameleonDataException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
