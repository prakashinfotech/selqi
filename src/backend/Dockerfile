FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /repo

# Restore layer — copy only project files first for cache efficiency
COPY src/backend/Selqi.Api/Selqi.Api.csproj             src/backend/Selqi.Api/
COPY src/backend/Selqi.Application/Selqi.Application.csproj src/backend/Selqi.Application/
COPY src/backend/Selqi.Domain/Selqi.Domain.csproj       src/backend/Selqi.Domain/
COPY src/backend/Selqi.Infrastructure/Selqi.Infrastructure.csproj src/backend/Selqi.Infrastructure/
RUN dotnet restore src/backend/Selqi.Api/Selqi.Api.csproj

# Build & publish
COPY src/backend/ src/backend/
RUN dotnet publish src/backend/Selqi.Api/Selqi.Api.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Selqi.Api.dll"]
