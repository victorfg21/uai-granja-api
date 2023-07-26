#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/web/UaiGranja.WebApp.MVC/UaiGranja.WebApp.MVC.csproj", "src/web/UaiGranja.WebApp.MVC/"]
RUN dotnet restore "src/web/UaiGranja.WebApp.MVC/UaiGranja.WebApp.MVC.csproj"
COPY . .
WORKDIR "/src/src/web/UaiGranja.WebApp.MVC"
RUN dotnet build "UaiGranja.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UaiGranja.WebApp.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UaiGranja.WebApp.MVC.dll"]