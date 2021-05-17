FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Realtime/Realtime.csproj", "Realtime/"]
RUN dotnet restore "Realtime/Realtime.csproj"
COPY . .
WORKDIR "/src/Realtime"
RUN dotnet build "Realtime.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Realtime.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Realtime.dll"]
