namespace ECommerce.Application.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public UserCreateFailedException() : base("Something went wrong when user created")
        {
        }
        public UserCreateFailedException(string? message) : base(message)
        {
        }
        public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
