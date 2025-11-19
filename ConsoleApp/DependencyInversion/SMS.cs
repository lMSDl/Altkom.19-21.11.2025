namespace ConsoleApp.DependencyInversion
{
    internal class SMS : IMessage
    {
        public string Content { get; set; }
        public string PhoneNumber { get; set; }

        public void SendMessage()
        {
            SendSms();
        }

        public void SendSms()
        {
            // Logic to send SMS
            Console.WriteLine($"Sending SMS to {PhoneNumber}: {Content}");
        }

    }
}
