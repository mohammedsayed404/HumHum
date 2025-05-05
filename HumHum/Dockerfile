FROM mcr.microsoft.com/dotnet/aspnet:8.0 
WORKDIR /app
COPY /app /app 
ENTRYPOINT ["dotnet", "HumHum.dll"]


## See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
## This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#
## This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["HumHum/HumHum.csproj", "HumHum/"]
#COPY ["Core/Services/Services.csproj", "Core/Services/"]
#COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
#COPY ["Core/Service.Abstractions/Service.Abstractions.csproj", "Core/Service.Abstractions/"]
#COPY ["Shared/Shared.csproj", "Shared/"]
#COPY ["Infrastructure/Persistence/Persistence.csproj", "Infrastructure/Persistence/"]
#COPY ["Infrastructure/Presentation/Presentation.csproj", "Infrastructure/Presentation/"]
#RUN dotnet restore "./HumHum/HumHum.csproj"
#COPY . .
#WORKDIR "/src/HumHum"
#RUN dotnet build "./HumHum.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./HumHum.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "HumHum.dll"]