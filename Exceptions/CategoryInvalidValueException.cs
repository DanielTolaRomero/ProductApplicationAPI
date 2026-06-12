namespace WebApplicationPractica.Exceptions
{
    public class CategoryInvalidValueException : Exception
    {
        public CategoryInvalidValueException() { }
        public CategoryInvalidValueException(string message) : base(message) { }
    }
}
