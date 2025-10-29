FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Create build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/Backend.csproj", "."]
RUN dotnet restore "Backend.csproj"

# Copy everything else and build
COPY src/ .
WORKDIR "/src"
RUN dotnet build "Backend.csproj" -c Release -o /app/build

# Create publish stage
FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create final image from base and publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]