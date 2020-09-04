FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/VidlyBackend.Api/VidlyBackend.csproj", "src/VidlyBackend.Api/"]
COPY ["src/VidlyBackend.DataManager/DataManager.csproj", "src/VidlyBackend.DataManager/"]
COPY ["src/VidlyBackend.Authentication/Authentication.csproj", "src/VidlyBackend.Authentication/"]
RUN dotnet restore "src/VidlyBackend.Api/VidlyBackend.csproj"
COPY . .
WORKDIR "/src/src/VidlyBackend.Api"
RUN dotnet build "VidlyBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VidlyBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VidlyBackend.dll"]