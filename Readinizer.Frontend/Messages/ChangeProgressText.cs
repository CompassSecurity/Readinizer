namespace Readinizer.Frontend.Messages
{
    public class ChangeProgressText
    {
        public string ProgressText { get; set; }

        public ChangeProgressText(string progressText)
        {
            ProgressText = progressText;
        }
    }
}
