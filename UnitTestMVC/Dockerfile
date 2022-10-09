FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UnitTestMVC/UnitTestMVC.csproj", "UnitTestMVC/"]
RUN dotnet restore "UnitTestMVC/UnitTestMVC.csproj"
COPY . .
WORKDIR "/src/UnitTestMVC"
RUN dotnet build "UnitTestMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UnitTestMVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UnitTestMVC.dll"]
