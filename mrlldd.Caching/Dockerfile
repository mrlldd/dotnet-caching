FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "mrlldd.Caching.Benchmarks/mrlldd.Caching.Benchmarks.csproj"
RUN dotnet build "mrlldd.Caching.Benchmarks/mrlldd.Caching.Benchmarks.csproj" -c Release -o /src/build

RUN dotnet publish "mrlldd.Caching.Benchmarks/mrlldd.Caching.Benchmarks.csproj" -c Release -o /src/publish
WORKDIR /src/publish
ENTRYPOINT ["dotnet", "mrlldd.Caching.Benchmarks.dll"]
