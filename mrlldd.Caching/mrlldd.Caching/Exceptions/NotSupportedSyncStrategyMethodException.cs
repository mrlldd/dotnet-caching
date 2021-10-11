namespace mrlldd.Caching.Exceptions
{
    public class NotSupportedSyncStrategyMethodException : CachingException
    {
        public NotSupportedSyncStrategyMethodException(string strategyName, string methodName) : base($"Sync method '{methodName}' is not supported in strategy '{strategyName}'")
        {
        }
    }
}