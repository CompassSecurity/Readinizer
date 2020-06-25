namespace Readinizer.Frontend.Messages
{
    public class SnackbarMessage
    {
        public string Message { get; }

        public SnackbarMessage(string message)
        {
            Message = message;
        }
    }
}
