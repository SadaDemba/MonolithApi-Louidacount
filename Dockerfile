FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
EXPOSE 80
EXPOSE 443

# Copy csproj and restore as distinct layers
COPY ["MonolithApi/MonolithApi.csproj", "MonolithApi/"]
RUN dotnet restore "MonolithApi/MonolithApi.csproj"

# Install Entity Framework Core tools with the specified version
#RUN dotnet tool install --global dotnet-ef --version 7.0.13

# Copy everything else and build the application
COPY . .
WORKDIR "/src/MonolithApi"
#RUN dotnet build "MonolithApi.csproj" -c Release -o /app/build

FROM build AS publish
# Include the tools in the PATH
#ENV PATH="/root/.dotnet/tools:${PATH}"

# Apply EF Core migrations
#RUN dotnet ef database update -c AppDatabaseContext -p MonolithApi.csproj -s MonolithApi.csproj

# Publish the application
RUN dotnet publish "MonolithApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonolithApi.dll"]