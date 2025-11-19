namespace ConsoleApp.DependencyInversion
{
    internal class EMail : IMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public void SendEmail()
        {
            // Logic to send email
            Console.WriteLine($"Sending Email to {To}: {Subject} - {Body}");
        }

        public void SendMessage()
        {
            SendEmail();
        }
    }
}
