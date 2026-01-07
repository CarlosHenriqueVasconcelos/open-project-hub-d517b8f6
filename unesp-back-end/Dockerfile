# Use uma imagem de base
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copie os arquivos de código-fonte para o contêiner
COPY . .

# Restaure as dependências e compile o aplicativo
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use uma imagem de base mais leve para a implantação
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Configure o ponto de entrada do aplicativo
ENTRYPOINT ["dotnet", "plataforma_gestao_ia_be.dll"]