namespace ConsoleApp.DependencyInversion
{
    internal class Messanger
    {
        public IEnumerable<IMessage> Messages { get; set; }

        public Messanger(IEnumerable<IMessage> messages)
        {
            Messages = messages;
        }

        public void SendMessage(IEnumerable<IMessage> messages)
        {
            Messages = messages;
            SendMessage();
        } 

        public void SendMessage()
        {
            foreach (var message in Messages)
            {
                message.SendMessage();
            }
        }
    }
}
