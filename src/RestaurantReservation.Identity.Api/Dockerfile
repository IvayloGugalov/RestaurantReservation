FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS https://*:443, http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/RestaurantReservation.Identity.Api/RestaurantReservation.Identity.Api.csproj", "src/RestaurantReservation.Identity.Api/"]
COPY ["src/RestaurantReservation.Core/RestaurantReservation.Core.csproj", "src/RestaurantReservation.Core/"]
COPY ["src/RestaurantReservation.Identity/RestaurantReservation.Identity.csproj", "src/RestaurantReservation.Identity/"]
RUN dotnet restore "src/RestaurantReservation.Identity.Api/RestaurantReservation.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/RestaurantReservation.Identity.Api"
RUN dotnet build "RestaurantReservation.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantReservation.Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantReservation.Identity.Api.dll", "--environment=docker"]
