FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore Identity/Edc.Identity.WebApi/Edc.Identity.WebApi.csproj
RUN dotnet build Identity/Edc.Identity.WebApi/Edc.Identity.WebApi.csproj -c Release -o /app/build
RUN dotnet publish Identity/Edc.Identity.WebApi/Edc.Identity.WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
#EXPOSE 443

ENV JwtSettings:Issuer="https://my.ussuer.com"
ENV JwtSettings:Audience="https://my.audience.com"
ENV JwtSettings:Key="ThisIsMyJwtSecurityKeyThisIsMyJwtSecurityKeyThisIsMyJwtSecurityKeyThisIsMyJwtSecurityKey"
ENV MongoHosts="mongodb://localhost:27017"
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Edc.Identity.WebApi.dll"]