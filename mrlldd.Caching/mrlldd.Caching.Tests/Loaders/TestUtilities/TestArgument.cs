using System;

namespace mrlldd.Caching.Tests.Loaders.TestUtilities
{
    public class TestArgument
    {
        public static TestArgument Create() => new()
        {
            Id = Guid.NewGuid()
        };
        public Guid Id { get; set; }
    }
}