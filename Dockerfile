# ============
# Build stage
# ============
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia dos projetos csproj
COPY StockControl.sln ./
COPY src/StockControl.Api/StockControl.Api.csproj src/StockControl.Api/
COPY src/StockControl.Application/StockControl.Application.csproj src/StockControl.Application/
COPY src/StockControl.Domain/StockControl.Domain.csproj src/StockControl.Domain/
COPY src/StockControl.Infrastructure/StockControl.Infrastructure.csproj src/StockControl.Infrastructure/

RUN dotnet restore StockControl.sln


COPY . .
RUN dotnet publish src/StockControl.Api/StockControl.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ============
# Runtime stage
# ============
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Usuário não-root
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build /app/publish .

# Porta padrão
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "StockControl.Api.dll"]
