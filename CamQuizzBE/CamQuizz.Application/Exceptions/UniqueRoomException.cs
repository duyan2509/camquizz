namespace CamQuizz.Application.Exceptions;

public class UniqueRoomException : Exception
{
    public UniqueRoomException(string message)
        : base(message)
    {
    }
}