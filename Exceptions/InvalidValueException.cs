namespace WebApplicationPractica.Exceptions
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException() { }
        public InvalidValueException(string message) : base(message) { }
    }
}
