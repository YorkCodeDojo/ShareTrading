FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 55036
EXPOSE 44334

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ShareTradingAPI/ShareTradingAPI.csproj ShareTradingAPI/
RUN dotnet restore ShareTradingAPI/ShareTradingAPI.csproj
COPY . .
WORKDIR /src/ShareTradingAPI
RUN dotnet build ShareTradingAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ShareTradingAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ShareTradingAPI.dll"]
