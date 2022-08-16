#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy all the referenced projects
COPY ["ItAccept.Teste.Domain/ItAccept.Teste.Domain.csproj", "./ItAccept.Teste.Domain/ItAccept.Teste.Domain.csproj"]
COPY ["ItAccept.Teste.Infrastructure.Crosscutting.Automapper/ItAccept.Teste.Infrastructure.Crosscutting.Automapper.csproj", "./ItAccept.Teste.Infrastructure.Crosscutting.Automapper/ItAccept.Teste.Infrastructure.Crosscutting.Automapper.csproj"]
COPY ["ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth/ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.csproj", "./ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth/ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.csproj"]
COPY ["ItAccept.Teste.Infrastructure.Data/ItAccept.Teste.Infrastructure.Data.csproj", "./ItAccept.Teste.Infrastructure.Data/ItAccept.Teste.Infrastructure.Data.csproj"]
COPY ["ItAccept.Teste.Services/ItAccept.Teste.Services.csproj", "./ItAccept.Teste.Services/ItAccept.Teste.Services.csproj"]
COPY ["ItAccept.Teste.Infrastructure.Crosscutting.IoC/ItAccept.Teste.Infrastructure.Crosscutting.IoC.csproj", "./ItAccept.Teste.Infrastructure.Crosscutting.IoC/ItAccept.Teste.Infrastructure.Crosscutting.IoC.csproj"]
COPY ["ItAccept.Teste.Application/ItAccept.Teste.Application.csproj", "./ItAccept.Teste.Application/ItAccept.Teste.Application.csproj"]

# COPY source code 
COPY ["ItAccept.Teste.Domain/", "./ItAccept.Teste.Domain/"]
COPY ["ItAccept.Teste.Infrastructure.Crosscutting.Automapper/", "./ItAccept.Teste.Infrastructure.Crosscutting.Automapper/"]
COPY ["ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth/", "./ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth/"]
COPY ["ItAccept.Teste.Infrastructure.Data/", "./ItAccept.Teste.Infrastructure.Data/"]
COPY ["ItAccept.Teste.Services/", "./ItAccept.Teste.Services/"]
COPY ["ItAccept.Teste.Infrastructure.Crosscutting.IoC/", "./ItAccept.Teste.Infrastructure.Crosscutting.IoC/"]
COPY ["ItAccept.Teste.Application/", "./ItAccept.Teste.Application/"]

RUN dotnet build "./ItAccept.Teste.Application/ItAccept.Teste.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./ItAccept.Teste.Application/ItAccept.Teste.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ItAccept.Teste.Application.dll", "--environment=Production"]