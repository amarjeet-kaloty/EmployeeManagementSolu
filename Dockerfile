ARG DOTNET_VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build

WORKDIR /src
COPY EmployeeManagementSolu.sln .
COPY Presentation/*.csproj Presentation/
RUN dotnet restore Presentation/Presentation.csproj

COPY . .
WORKDIR /src/Presentation
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} as final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Presentation.dll"]
