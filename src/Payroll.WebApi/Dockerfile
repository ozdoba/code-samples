FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["Payroll.WebApi/Payroll.WebApi.csproj", "Payroll.WebApi/"]
RUN dotnet restore "Payroll.WebApi/Payroll.WebApi.csproj"
COPY . .
WORKDIR "/src/Payroll.WebApi"
RUN dotnet build "Payroll.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payroll.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payroll.WebApi.dll"]
