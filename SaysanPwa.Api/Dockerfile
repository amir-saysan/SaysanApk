FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY dist /app
WORKDIR /app
EXPOSE 80/tcp
EXPOSE 443/tcp
ENTRYPOINT ["dotnet", "SaysanPwa.Api.dll"]