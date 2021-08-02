using System;

namespace mrlldd.Caching.Tests
{
    public class TestUnit
    {
        internal Guid InternalProperty { get; } = Guid.NewGuid(); 
        
        public Guid PublicProperty { get; } = Guid.NewGuid();

        private Guid PrivateProperty { get; } = Guid.NewGuid();

        protected Guid ProtectedProperty { get; } = Guid.NewGuid();
    }
}