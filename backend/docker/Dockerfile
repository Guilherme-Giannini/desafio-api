FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY DesafioApi.sln . 
COPY src/DesafioApi.Api/DesafioApi.Api.csproj src/DesafioApi.Api/
COPY src/DesafioApi.Application/DesafioApi.Application.csproj src/DesafioApi.Application/
COPY src/DesafioApi.Infrastructure/DesafioApi.Infrastructure.csproj src/DesafioApi.Infrastructure/
COPY src/DesafioApi.Domain/DesafioApi.Domain.csproj src/DesafioApi.Domain/

RUN dotnet restore DesafioApi.sln

COPY src/. src/

RUN dotnet publish src/DesafioApi.Api/DesafioApi.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "DesafioApi.Api.dll"]