FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SidecarApp/SidecarApp.csproj", "SidecarApp/"]
RUN dotnet restore "SidecarApp/SidecarApp.csproj"
COPY . .
WORKDIR "/src/SidecarApp"
RUN dotnet build "SidecarApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SidecarApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SidecarApp.dll"]
