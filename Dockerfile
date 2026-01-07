FROM node:18 AS builder

WORKDIR /app

# Instale dependências de build críticas
RUN apt-get update && apt-get install -y python3 make g++

# Copie e instale as dependências
COPY package*.json ./
RUN npm install --legacy-peer-deps --force

# Copie o código-fonte
COPY . .

# Configure variáveis de ambiente
ENV NODE_OPTIONS=--openssl-legacy-provider

# Execute os builds separadamente
RUN npm run build:main
RUN npm run build:admin

FROM nginx:alpine

# Copie os arquivos da aplicação principal
COPY --from=builder /app/dist /usr/share/nginx/html

# Copie os arquivos do AdminPage
COPY --from=builder /app/AdminPage/dist /usr/share/nginx/html/admin

# Copie a configuração do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Corrija as permissões
RUN chmod -R 755 /usr/share/nginx/html

# Exponha a porta 80
EXPOSE 80

# Inicie o Nginx
CMD ["nginx", "-g", "daemon off;"]