namespace SubscriberAPI.Domain
{
    public record class Error(string Code, string Message)
    {
        public static readonly Error None = new(string.Empty, string.Empty);   
        
        public static Error NotFound(string message)
        {
            return new Error("Not Found", message);
        }
        public static Error Failure(string message)
        {
            return new Error("Failure", message);
        }
        public static Error ValidationError(string message)
        {
            return new Error("Validation Error", message);
        }
        public static Error SameSubscription(string message)
        {
            return new Error("Same Subscription", message);
        }
    }
}
