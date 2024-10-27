#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0.403-alpine3.20 AS sdk
WORKDIR /src
COPY ["./src/", "./"]
WORKDIR /src/notifier
RUN dotnet restore "Shopzy.Notifier.csproj"
RUN dotnet build "Shopzy.Notifier.csproj" -c Release -o /app/build
RUN dotnet publish "Shopzy.Notifier.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0.10-alpine3.20 AS release
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shopzy.Notifier.dll"]