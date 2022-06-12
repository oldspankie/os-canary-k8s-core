FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY os-canary-k8s-core/*.csproj .
RUN dotnet restore

COPY os-canary-k8s-core/ .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "os-canary-k8s-core.dll"]