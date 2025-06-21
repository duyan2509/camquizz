# Use the official .NET 9 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["camquizz-be.csproj", "./"]
RUN dotnet restore "camquizz-be.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src"

# Build the application
RUN dotnet build "camquizz-be.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "camquizz-be.csproj" -c Release -o /app/publish

# Build the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "camquizz-be.dll"] 