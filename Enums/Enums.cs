namespace Jarvis_Brain.Code
{
    public enum StateEnum
    {
        Received, 
        Sent, 
        Error
    }

    public enum ErrorType
    {
        ConnectionFailure,
        ConnectionInterupted,
        InconsistentDataShape,
        Unknown
    }
}
