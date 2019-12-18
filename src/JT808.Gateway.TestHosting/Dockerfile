#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
EXPOSE 808/tcp
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["JT808.Gateway.TestHosting/JT808.Gateway.TestHosting.csproj", "JT808.Gateway.TestHosting/"]
COPY ["JT808.Gateway/JT808.Gateway.csproj", "JT808.Gateway/"]
COPY ["JT808.Gateway.Abstractions/JT808.Gateway.Abstractions.csproj", "JT808.Gateway.Abstractions/"]
RUN dotnet restore "JT808.Gateway.TestHosting/JT808.Gateway.TestHosting.csproj"
COPY . .
WORKDIR "/src/JT808.Gateway.TestHosting"
RUN dotnet build "JT808.Gateway.TestHosting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JT808.Gateway.TestHosting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JT808.Gateway.TestHosting.dll"]