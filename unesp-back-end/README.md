# Configuração

## Instalar:
- Docker
- .NET 7.0 (caso queira fazer modificações no fonte)

## Configuração do Banco de Dados MySQL:

1. **Baixe o contêiner Docker:**

   ```bash
   docker pull leandrolevvi/unesp-back-end-be_plataforma_gestao_ia:1.0
   ```
2. **Execute o contêiner Docker:**
   
   Estando na pasta raíz do projeto, onde se encontra o arquido docker-composer.yaml, executar os comandos a seguir para baixar, construir e executar as imagens necessárias:

   ```bash
   docker-compose up
   ```

   Aguarde até que o MySQL esteja pronto para ser utilizado. Isso pode demorar alguns minutos. A execução estará finalizada quando a última mensagem for semelhante a:

   ```bash
   dockercompose-mysql_plataforma_gestao_ia-1  | 2024-05-05T16:12:42.860642Z 0 [System] [MY-011323] [Server] X Plugin ready for connections. Bind-address: '::' port: 33060, socket: /var/run/mysqld/mysqlx.sock
   ```

   Nesta etapa, o projeto já está pronto para ser utilizado. Se nãoo pretender modificar o código, vá para a parte 6. Caso contrário, continue.

   Exclua a imagem referente à aplicação .NET com o comando:

   ```bash
   docker rm dockercompose-be_plataforma_gestao_ia-1
   ```
   >O nome "dockercompose-be_plataforma_gestao_ia-1" pode ser diferente em seu sistema. Verifique o nome do container com o comando docker ps.

4. **Build e execução do projeto:**

    Dê o build no projeto:
    ```bash
    dotnet build
    ```
    Rode o projeto (modo http padrão)
    ```bash
    dotnet run
    ```
5. **Configuração HTTPS:**
    Pode ser que para comunicar o backend ao frontend será necessário rodar o sistema com https ativo, para isso siga os passos a seguir:

    **Instale um certificado SSL autoassinado:**
    ```bash
    dotnet dev-certs https --trust
    ```

    **Execute o projeto com HTTPS:**

    ```bash
    dotnet run --launch-profile https
    ```

6. **Documentação (Swagger):**

    Com o sistema rodando, acesse http://localhost:5058/swagger/index.html para visualizar a documentação com todos os endpoints, conforme explicado no tópico funcionamento abaixo.

## Funcionamento:

1. **Registro de Usuário:**
    
    Envie um nome e e-mail para o endpoint http://localhost:5058/v1/accounts. Será retornado uma senha que pode ser usada para o login.

2. **Login:**
    Envie uma requisição de login para http://localhost:5058/v1/accounts/login com o e-mail e senha. Você receberá um token do tipo "Bearer Token" que deve ser enviado via Authorization para realizar outras operações.
    
>Todas as outras operações descritas na documentação dependerão desse Token, então garanta que ela esteja sempre sendo enviado, ou receberá como retorno o erro 401 (Unauthorized)