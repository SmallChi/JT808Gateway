FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY .output/docker/ .
EXPOSE 80 808
ENV LANG=en_US.UTF-8 TZ=Asia/Shanghai
ENTRYPOINT ["dotnet", "JT808.Gateway.SimpleServer.dll"]