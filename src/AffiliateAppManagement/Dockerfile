#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .

COPY ["AffiliatePMS.WebAPI/_AffiliatePMS.WebAPI.csproj", "AffiliatePMS.WebAPI/"]
COPY ["AffiliatePMS.Domain/AffiliatePMS.Domain.csproj", "AffiliatePMS.Domain/"]
COPY ["AffiliatePMS.Infra/AffiliatePMS.Infra.csproj", "AffiliatePMS.Infra/"]
COPY ["AffiliatePMS.Application/AffiliatePMS.Application.csproj", "AffiliatePMS.Application/"]
RUN dotnet restore "./AffiliatePMS.WebAPI/./_AffiliatePMS.WebAPI.csproj"
 COPY . .
WORKDIR "/src/AffiliatePMS.WebAPI"
RUN dotnet build "./_AffiliatePMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./_AffiliatePMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "_AffiliatePMS.WebAPI.dll"]