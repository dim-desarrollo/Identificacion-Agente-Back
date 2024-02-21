FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT="Production"

COPY published/ ./
RUN mkdir -p /app/imagenes/
ENTRYPOINT ["dotnet", "inspectores-api.dll"]


