FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY EcommerceLifestyle.Api/EcommerceLifestyle.Api.csproj ./EcommerceLifestyle.Api/
COPY EcommerceLifestyle.BLL/EcommerceLifestyle.BLL.csproj ./EcommerceLifestyle.BLL/
COPY EcommerceLifestyle.DAL/EcommerceLifestyle.DAL.csproj ./EcommerceLifestyle.DAL/
RUN dotnet restore EcommerceLifestyle.Api/EcommerceLifestyle.Api.csproj

COPY EcommerceLifestyle.Api/ ./EcommerceLifestyle.Api/
COPY EcommerceLifestyle.BLL/ ./EcommerceLifestyle.BLL/
COPY EcommerceLifestyle.DAL/ ./EcommerceLifestyle.DAL/
RUN dotnet publish EcommerceLifestyle.Api/EcommerceLifestyle.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "EcommerceLifestyle.Api.dll"]
