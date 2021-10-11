using System.Threading;
using System.Threading.Tasks;

namespace mrlldd.Caching.Loaders
{
    public interface ILoader<TArgs, TResult>
    {
        Task<TResult> LoadAsync(TArgs args, CancellationToken token = default);
    }
}