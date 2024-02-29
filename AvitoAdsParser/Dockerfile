FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AvitoAdsParser/AvitoAdsParser.csproj", "AvitoAdsParser/"]
RUN dotnet restore "AvitoAdsParser/AvitoAdsParser.csproj"
COPY . .
WORKDIR "/src/AvitoAdsParser"
RUN dotnet build "AvitoAdsParser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AvitoAdsParser.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AvitoAdsParser.dll"]
