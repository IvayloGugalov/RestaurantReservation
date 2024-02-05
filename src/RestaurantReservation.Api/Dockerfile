FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RestaurantReservation.Api/RestaurantReservation.Api.csproj", "src/RestaurantReservation.Api/"]
COPY ["src/RestaurantReservation.Core/RestaurantReservation.Core.csproj", "src/RestaurantReservation.Core/"]
COPY ["src/RestaurantReservation.Domain/RestaurantReservation.Domain.csproj", "src/RestaurantReservation.Domain/"]
COPY ["src/RestaurantReservation.Infrastructure.Mongo/RestaurantReservation.Infrastructure.Mongo.csproj", "src/RestaurantReservation.Infrastructure.Mongo/"]
RUN dotnet restore "src/RestaurantReservation.Api/RestaurantReservation.Api.csproj"
COPY . .
WORKDIR "/src/src/RestaurantReservation.Api"
RUN dotnet build "RestaurantReservation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RestaurantReservation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantReservation.Api.dll"]
