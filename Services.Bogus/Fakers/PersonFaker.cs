namespace Services.Bogus.Fakers
{
    public class UserFaker : EntityFaker<Models.User>
    {
        public UserFaker(BogusConfig config) : base(config)
        {
            RuleFor(u => u.Username, f => f.Internet.UserName());
            RuleFor(u => u.Password, f => f.Internet.Password());
        }
    }
}
