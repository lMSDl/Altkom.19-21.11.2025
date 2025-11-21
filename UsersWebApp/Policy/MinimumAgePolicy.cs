using Microsoft.AspNetCore.Authorization;

namespace UsersWebApp.Policy
{
    public class MinimumAgePolicy : IAuthorizationRequirement
    {
        public int Age { get;  }

        public MinimumAgePolicy(int age)
        {
            Age = age;
        }
    }
}
