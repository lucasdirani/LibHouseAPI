namespace LibHouse.Business.Notifiers
{
    public class Notification
    {
        public string Message { get; }
        public string Title { get; }

        public Notification(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public override string ToString() =>
            $"Notification {Title}: {Message}";
    }
}