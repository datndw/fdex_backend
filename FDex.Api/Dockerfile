FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FDex.Api/FDex.Api.csproj", "FDex.Api/"]
COPY ["FDex.Application/FDex.Application.csproj", "FDex.Application/"]
COPY ["FDex.Domain/FDex.Domain.csproj", "FDex.Domain/"]
COPY ["FDex.Identity/FDex.Identity.csproj", "FDex.Identity/"]
COPY ["FDex.Infrastructure/FDex.Infrastructure.csproj", "FDex.Infrastructure/"]
COPY ["FDex.Persistence/FDex.Persistence.csproj", "FDex.Persistence/"]
RUN dotnet restore "FDex.Api/FDex.Api.csproj"
COPY . .
WORKDIR "/src/FDex.Api"

#RUN dotnet ef migrations add Init --startup-project FDex.API --project FDex.Persistence --context FDexDbContext
RUN dotnet ef database update --startup-project FDex.API --project FDex.Persistence --context FDexDbContext
RUN dotnet build "FDex.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FDex.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FDex.Api.dll"]