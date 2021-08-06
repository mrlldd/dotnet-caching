using System;

namespace mrlldd.Caching.Tests.TestUtilities
{
    public class TestUnit
    {
        public static TestUnit Create() => new()
        {
            PublicProperty = Guid.NewGuid(),
            PrivateProperty = Guid.NewGuid(),
            ProtectedProperty = Guid.NewGuid()
        };

        public Guid PublicProperty { get; set; }

        private Guid PrivateProperty { get; set; }

        protected Guid ProtectedProperty { get; set; }
    }
}