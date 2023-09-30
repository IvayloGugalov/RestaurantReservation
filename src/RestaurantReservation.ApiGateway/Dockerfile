FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS https://*:443, http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/RestaurantReservation.ApiGateway/RestaurantReservation.ApiGateway.csproj", "src/RestaurantReservation.ApiGateway/"]
COPY ["src/RestaurantReservation.Core/RestaurantReservation.Core.csproj", "src/RestaurantReservation.Core/"]
RUN dotnet restore "src/RestaurantReservation.ApiGateway/RestaurantReservation.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/RestaurantReservation.ApiGateway"
RUN dotnet build "RestaurantReservation.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantReservation.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantReservation.ApiGateway.dll", "--environment=docker"]
