#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/src/services/UaiGranja.Identidade.API/UaiGranja.Identidade.API.csproj", "src/src/services/UaiGranja.Identidade.API/"]
RUN dotnet restore "src/src/services/UaiGranja.Identidade.API/UaiGranja.Identidade.API.csproj"
COPY . .
WORKDIR "/src/src/src/services/UaiGranja.Identidade.API"
RUN dotnet build "UaiGranja.Identidade.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UaiGranja.Identidade.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UaiGranja.Identidade.API.dll"]