namespace BL.Exceptions;

public class TypingNameMismatchException : Exception
{
    public TypingNameMismatchException()
    {
    }

    public TypingNameMismatchException(string message)
        : base(message)
    {
    }

    public TypingNameMismatchException(string message, Exception inner)
        : base(message, inner)
    {
    }
}