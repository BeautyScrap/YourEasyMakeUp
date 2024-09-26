namespace SubscriberAPI.Domain
{
    public class Result<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error{ get;}

        private Result(T value)
        {
            Value = value;
            IsSuccess = true;
            Error = Error.None;
        }

        private Result(Error error)
        {
            if (error == Error.None)
            {
                throw new ArgumentException("Invalid Exseption", nameof(error));
            }
            IsSuccess = false;
            Error = error;
        }
        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }
        public static Result<T> Failure(Error error)
        {
            return new Result<T>(error);
        }
    }
}
