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

	protected override string DefaultKeySuffix => provider.SessionId;  

	public ContextfulCache(IUserContextProvider provider)   
		=> this.provider = provider;  
}
```


Example of using of cache:
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

