#Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#COPY csproj files from all layers in clean architecture
COPY ["RecipeWorld/RecipeWorld/RecipeWorld.csproj", "RecipeWorld/RecipeWorld/"]
COPY ["RecipeWorld/RecipeWorld.Client/RecipeWorld.Client.csproj", "RecipeWorld/RecipeWorld.Client/"]
COPY ["RecipeWorld.Shared/RecipeWorld.Shared.csproj", "RecipeWorld.Shared/"]


#Restore all dependencies
RUN dotnet restore "./RecipeWorld/RecipeWorld/RecipeWorld.csproj"

# Copy the rest of the source code
COPY . .

# Set working directory to API layer
WORKDIR "/src/RecipeWorld/RecipeWorld"

# Build the solution
RUN dotnet build "RecipeWorld.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RecipeWorld.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipeWorld.dll"]