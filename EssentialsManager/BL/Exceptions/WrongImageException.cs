namespace BL.Exceptions;

public class WrongImageException : Exception
{
    public WrongImageException()
    {
    }

    public WrongImageException(string message)
        : base(message)
    {
    }

    public WrongImageException(string message, Exception inner)
        : base(message, inner)
    {
    }
}