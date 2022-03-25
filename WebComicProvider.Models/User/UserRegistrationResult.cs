namespace WebComicProvider.Models.User
{
    public record struct UserRegistrationResult(bool Success, string ErrorMessage)
    {
        public static implicit operator (bool, string)(UserRegistrationResult value)
        {
            return (value.Success, value.ErrorMessage);
        }

        public static implicit operator UserRegistrationResult((bool, string) value)
        {
            return new UserRegistrationResult(value.Item1, value.Item2);
        }
    }
}
