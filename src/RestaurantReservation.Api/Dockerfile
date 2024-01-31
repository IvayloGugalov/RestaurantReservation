#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /
COPY ["src/RestaurantReservation.Api/RestaurantReservation.Api.csproj", "RestaurantReservation.Api/"]
COPY ["src/RestaurantReservation.Core/RestaurantReservation.Core.csproj", "RestaurantReservation.Core/"]
COPY ["src/RestaurantReservation.Domain/RestaurantReservation.Domain.csproj", "RestaurantReservation.Domain/"]
COPY ["src/RestaurantReservation.Infrastructure.EF/RestaurantReservation.Infrastructure.EF.csproj", "RestaurantReservation.Infrastructure.EF/"]
RUN dotnet restore "RestaurantReservation.Api/RestaurantReservation.Api.csproj"
COPY . .
WORKDIR "/src/RestaurantReservation.Api"
RUN dotnet build "RestaurantReservation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantReservation.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantReservation.Api.dll"]
