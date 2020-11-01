namespace SoftwareArchitect.Service.Users.Auth
{
    public static class AuthConsts
    {
        public const string UserIdHeader = "X-UserId";

        public static class Schemas
        {
            public const string UserId = "UserId";
        }

        public static class Policies
        {
            public const string UserId = "user-id";
        }

        public static class Claims
        {
            public static class Types
            {
                public const string UserId = Policies.UserId;
            }
        }
    }
}