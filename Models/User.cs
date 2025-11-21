namespace Models
{
    public class User : Entity
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
