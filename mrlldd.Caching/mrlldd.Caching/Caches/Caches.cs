using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;

namespace mrlldd.Caching.Caches
{
    internal class Caches<T> : ICaches<T>
    {
        private readonly IEnumerable<ICache<T>> caches;
        //todo provide access to different interaction strategies with multiple caches
        // maybe priorities or something like that
        
        public Caches(IEnumerable<ICache<T>> caches) 
            => this.caches = caches;

        public Task<Result> SetAsync(T value, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Result Set(T value)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<T?>> GetAsync(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Result<T?> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> RefreshAsync(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Result Refresh()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> RemoveAsync(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Result Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}