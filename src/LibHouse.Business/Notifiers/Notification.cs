namespace LibHouse.Business.Notifiers
{
    public class Notification
    {
        public string Message { get; }
        public string Code { get; }
        public string Title { get; }

        public Notification(string message, string code, string title)
        {
            Message = message;
            Code = code;
            Title = title;
        }

        public override string ToString() =>
            $"Notification {Code} - {Title}: {Message}";
    }
}