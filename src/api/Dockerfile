#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0.403-alpine3.20 AS build
WORKDIR /src
COPY ["./src/", "./"]
WORKDIR /src/api
RUN dotnet restore "Shopzy.Api.csproj"
RUN dotnet build "Shopzy.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Shopzy.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0.10-alpine3.20 AS publish
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Shopzy.Api.dll"]