namespace ConsoleApp.DependencyInversion
{
    internal class MMS : IMessage

    {
        public string PhoneNumber { get; set; }
        public byte[] Content { get; set; }

        public void SendMessage()
        {
            SendMms();
        }

        public void SendMms()
        {
            // Logic to send MMS
            Console.WriteLine($"Sending MMS to {PhoneNumber}: {Content.Length} bytes");
        }
    }
}
