FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build_env
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS release
COPY --from=build_env /app/out .

ENTRYPOINT ["dotnet","APICarsGQL.dll"]