namespace FoodSharing.Application.Exceptions;

internal class OrderException : Exception
{

    public OrderException(string message) : base(message)
    {
    }
}