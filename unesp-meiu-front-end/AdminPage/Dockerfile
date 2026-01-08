# Estágio de construção
FROM node:18 AS builder

WORKDIR /app

# Copiar e instalar dependências
COPY package*.json ./
RUN npm install --legacy-peer-deps --force

# Copiar todo o código-fonte
COPY . .

# Configurar variáveis de ambiente
ENV NODE_OPTIONS=--openssl-legacy-provider

# Construir ambas as aplicações
RUN npm run build:main && \
    npm run build:admin

# Estágio de produção
FROM nginx:alpine

# Copiar arquivos da aplicação principal
COPY --from=builder /app/dist /usr/share/nginx/html

# Copiar arquivos do AdminPage
COPY --from=builder /app/AdminPage/dist /usr/share/nginx/html/admin

# Copiar configuração do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Corrigir permissões
RUN chmod -R 755 /usr/share/nginx/html

# Expor a porta 80
EXPOSE 80

# Iniciar o Nginx
CMD ["nginx", "-g", "daemon off;"]