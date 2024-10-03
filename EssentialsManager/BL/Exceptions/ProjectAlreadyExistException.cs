namespace BL.Exceptions;

public class ProjectAlreadyExistException : Exception
{
    public ProjectAlreadyExistException()
    {
    }

    public ProjectAlreadyExistException(string message)
        : base(message)
    {
    }

    public ProjectAlreadyExistException(string message, Exception inner)
        : base(message, inner)
    {
    }
}