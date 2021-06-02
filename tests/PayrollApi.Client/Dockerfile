FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["tests/PayrollApi.Client/PayrollApi.Client.csproj", "PayrollApi.Client/"]
RUN dotnet restore "tests/PayrollApi.Client/PayrollApi.Client.csproj"
COPY . .
WORKDIR "/src/PayrollApi.Client"
RUN dotnet build "PayrollApi.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PayrollApi.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PayrollApi.Client.dll"]
