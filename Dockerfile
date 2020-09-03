FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /src/VidlyBackend.Web

COPY ./src/VidlyBackend.Web/*.csproj ./
RUN dotnet restore

COPY /src/VidlyBackend.Web ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /src/VidlyBackend.Web
COPY --from=build-env /src/VidlyBackend.Web/out .
ENTRYPOINT ["dotnet", "VidlyBackend.dll"]

# FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
# WORKDIR /src/VidlyBackend.Web/app

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./
# RUN dotnet publish -c Release -o out

# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
# WORKDIR /app
# COPY --from=build-env /app/out .
# ENTRYPOINT ["dotnet", "VidlyBackend.dll"]