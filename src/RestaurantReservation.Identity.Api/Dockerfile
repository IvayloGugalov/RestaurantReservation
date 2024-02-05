FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RestaurantReservation.Identity.Api/RestaurantReservation.Identity.Api.csproj", "src/RestaurantReservation.Identity.Api/"]
COPY ["src/RestaurantReservation.Core/RestaurantReservation.Core.csproj", "src/RestaurantReservation.Core/"]
COPY ["src/RestaurantReservation.Identity/RestaurantReservation.Identity.csproj", "src/RestaurantReservation.Identity/"]
RUN dotnet restore "src/RestaurantReservation.Identity.Api/RestaurantReservation.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/RestaurantReservation.Identity.Api"
RUN dotnet build "RestaurantReservation.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RestaurantReservation.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantReservation.Identity.Api.dll"]
