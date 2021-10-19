Caching
======
Caching - flexible **[ASP.NET Core](https://github.com/dotnet/aspnetcore)**  library that provides caching utilities with dependency injection.

## Start
In Startup.cs
```csharp
public void ConfigureServices(IServiceCollection services)  
{  
	services.AddCaching(typeof(Startup).Assembly);  
}
```

Caching library provides services:
 - Caches;
 - Caching loaders;
Both of those services are inherited from [Caching](https://github.com/mrlldd/dotnet-caching/blob/main/mrlldd.Caching/mrlldd.Caching/Caching.cs) class that provides basic [Cache Store](https://github.com/mrlldd/dotnet-caching/blob/main/mrlldd.Caching/mrlldd.Caching.Abstractions/Stores/ICacheStore.cs) interacting methods (get, set, refresh, remove).
Caching has two generic parameters:
 - Type of entry value;
 - Caching flag - a flag that determines which store to use in current implementation. Current library version provides three flags:
  	* InMemory - used in cases when Microsoft IMemoryCache should be used;
	* InDistributed - used in cases when Microsoft IDistributedCache should be uses;
	* InVoid - used in cases when nothing should be used (usual case - unit tests);

## Cache Stores
Cache store - a wrapper service for interacting with different cache implementation (for example, [Microsoft Memory Cache Store](https://github.com/mrlldd/dotnet-caching/blob/main/mrlldd.Caching/mrlldd.Caching/Stores/Internal/MemoryCacheStore.cs) or [Microsoft Distributed Cache Store](https://github.com/mrlldd/dotnet-caching/blob/main/mrlldd.Caching/mrlldd.Caching/Stores/Internal/DistributedCacheStore.cs)). Which cache store to use in caching implementations to use is answered by usage of **caching flags - classes that are used only as generic parameters**.
You can also implement your caching flags & stores! :) In order to do that, you need: 
 1) Create a type inherited from [CachingFlag](https://github.com/mrlldd/dotnet-caching/blob/main/mrlldd.Caching/mrlldd.Caching.Abstractions/Flags/CachingFlag.cs) class.
```csharp
public sealed class InFile : CachingFlag  
{  
	private InFile()  
	{ }
}
```
 2) Implement a cache store for that flag.
```csharp
public class FileStore : ICacheStore<InFile>  
{
	/*...*/
}
```
3) Register that pair during services registration.
In Startup.cs
```csharp
public void ConfigureServices(IServiceCollection services)  
{  
	services.AddCaching(typeof(Startup).Assembly)  
		.UseCachingStore<InFile, FileStore>();  
}
```
4) Use it in caching implementations
```csharp
public class ImageCache : Cache<Image, InFile>  
{  
	protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.FromMinutes(5));  
	protected override string CacheKey => "singleton-image";  
}
```

### Store decoration
You can also decorate stores! :) To do so, you need:
1) Implement a decorated store wrapper and decorator:
```csharp
public class WrappedCacheStore<T>: ICacheStore<T> where T : CachingFlag 
{
	/*...*/
} 
```
```csharp
public class WrappingDecorator<TStoreFlag> : ICacheStoreDecorator<TStoreFlag> where TStoreFlag : CachingFlag  
{  
	public ICacheStore<TStoreFlag> Decorate(ICacheStore<TStoreFlag> cacheStore)   
		=> new WrappedCacheStore<TStoreFlag>(cacheStore);  

	public int Order => 2754;  
}
```
2) Register decorator during service registration.
In Startup.cs
```csharp
public void ConfigureServices(IServiceCollection services)  
{  
	services.AddCaching(typeof(Startup).Assembly)  
		.Decorators<InMemory>().Add<WrappingDecorator<InMemory>>()  
		.Decorators<InDistributed>().Add<WrappingDecorator<InDistributed>>();  
}
```
## Caches
Caches are usual services to store temporary data. 

Example of singleton entry cache:
```csharp
public class IntCache : Cache<int, InMemory>  
{  
	protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.FromSeconds(15));  
	protected override string CacheKey => "singleton-number";  
}
```
Example of contextful cache:
```csharp
public class ContextfulCache : Cache<UserContext, InDistributed>  
{  
	private readonly IUserContextProvider provider;  

	protected override CachingOptions Options 
		=> CachingOptions.Enabled(TimeSpan.FromMinutes(20));  

	protected override string CacheKey => "user-context";  

	protected override string CacheKeySuffix => provider.SessionId;  

	public ContextfulCache(IUserContextProvider provider)   
		=> this.provider = provider;  
}
```

Example of cache using service:
```csharp
public class IntCacheUsingService  
{  
	private readonly ICache<int, InMemory> intCache;  

	public IntCacheUsingService(ICache<int, InMemory> intCache)   
		=> this.intCache = intCache;  


	public int CalculateWithMemoization(int a, int b)  
	{
		var fromCache = intCache.Get();  
		if (fromCache.Successful)  
		{
			return fromCache;  
		} 

		var result = PerformExpensiveCalculation(a, b);  
		intCache.Set(result);  
		return result;  
	}
}
```
## Caching loaders
Caching loaders - services that are composed of caching store provided by base caching class and loaders - services used to load data that would be cached.

Example of loader:
```csharp
public class RemoteEntityLoader : ILoader<int, RemoteEntity>  
{  
	public async Task<RemoteEntity> LoadAsync(int args, CancellationToken token = default)  
	{ 
		await Task.Delay(5000, token);  
		return new RemoteEntity(args);  
	}
}
```
Example of caching loader:
```csharp
public class RemoteEntityCachingLoader : CachingLoader<int, RemoteEntity, InMemory>  
{  
	protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.FromMinutes(5));  
	protected override string CacheKey => nameof(RemoteEntity);  

	protected override string CacheKeySuffixFactory(int args)  
		=> args.ToString();  
}
```