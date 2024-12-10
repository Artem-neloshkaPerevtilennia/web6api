FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY web6api/*.csproj ./web6api/
RUN dotnet restore

COPY web6api/. ./web6api/
WORKDIR /source/web6api
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "web6api.dll"]