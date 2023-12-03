#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS build
WORKDIR /src
COPY ["MonolithApi/MonolithApi.csproj", "MonolithApi/"]
RUN dotnet restore "MonolithApi/MonolithApi.csproj"
COPY . .
WORKDIR "/src/MonolithApi"
RUN dotnet build "MonolithApi.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "MonolithApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

WORKDIR /app
ENTRYPOINT ["dotnet", "MonolithApi.dll"]
RUN dotnet ef database update -c InitialMigration -p MonolithApi.csproj -s MonolithApi.csproj -o /app/builds
