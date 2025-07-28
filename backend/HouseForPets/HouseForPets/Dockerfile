FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HouseForPets/API.csproj", "HouseForPets/"]  # Копируем API.csproj
COPY ["../DataBaseContext/DataBaseContext.csproj", "DataBaseContext/"]
COPY ["../Service/Service.csproj", "Service/"]

RUN dotnet restore "HouseForPets/API.csproj"
RUN dotnet restore "DataBaseContext/DataBaseContext.csproj"
RUN dotnet restore "Service/Service.csproj"

COPY . .  # Копируем все остальные файлы проекта (включая .cs)
WORKDIR "/src/HouseForPets" # Correct workdir

RUN dotnet build "HouseForPets.csproj" -c Release -o /app/build  # Указываем, что собираем API

FROM build AS publish
RUN dotnet publish "HouseForPets.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HouseForPets.dll"]  # Укажите ваш API.dll, если отличается