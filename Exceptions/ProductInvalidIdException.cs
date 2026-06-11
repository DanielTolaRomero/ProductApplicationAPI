namespace WebApplicationPractica.Exceptions
{
    public class ProductInvalidIdException : Exception
    {
        public ProductInvalidIdException() { }

        public ProductInvalidIdException(string message) : base(message) { }
    }
}
