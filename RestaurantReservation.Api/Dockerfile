FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RestaurantReservation.Api/RestaurantReservation.Api.csproj", "RestaurantReservation.Api/"]
RUN dotnet restore "RestaurantReservation.Api/RestaurantReservation.Api.csproj"
COPY . .
WORKDIR "/src/RestaurantReservation.Api"
RUN dotnet build "RestaurantReservation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantReservation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantReservation.Api.dll"]
