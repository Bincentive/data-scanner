FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim
WORKDIR /app
EXPOSE 80
COPY scanner/api-scanner/bin/Release/netcoreapp3.0/linux-x64/publish. .
ENTRYPOINT ["dotnet", "api-scanner.dll"]
