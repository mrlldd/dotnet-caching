namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents missing support of synchronous method in strategy.
    /// </summary>
    public class NotSupportedSyncStrategyMethodException : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="strategyName">The name of strategy.</param>
        /// <param name="methodName">The name of method.</param>
        public NotSupportedSyncStrategyMethodException(string strategyName, string methodName) : base(
            $"Sync method '{methodName}' is not supported in strategy '{strategyName}'")
        {
            StrategyName = strategyName;
            MethodName = methodName;
        }

        /// <summary>
        ///     The name of strategy.
        /// </summary>
        public string StrategyName { get; }

        /// <summary>
        ///     The name of method.
        /// </summary>
        public string MethodName { get; }
    }
}