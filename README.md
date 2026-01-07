
# Bem-vindo ao seu projeto Plataforma gestão MEI-U

## Informações do Projeto

**URL**: https://meiu.pg.utfpr.edu.br/

## Como editar este código?

Existem várias maneiras de editar sua aplicação.

O único requisito é ter Node.js & npm instalados - [instale com nvm](https://github.com/nvm-sh/nvm#installing-and-updating)

Siga estes passos:

```sh
# Passo 1: Clone o repositório usando a URL Git do projeto.
git clone <URL_DO_SEU_GIT>

# Passo 2: Navegue até o diretório do projeto.
cd <NOME_DO_SEU_PROJETO>

# Passo 3: Instale as dependências necessárias.
npm i

# Passo 4: Inicie o servidor de desenvolvimento com recarregamento automático e visualização instantânea.
npm run dev
```

**Editar um arquivo diretamente no GitHub**

- Navegue até o(s) arquivo(s) desejado(s).
- Clique no botão "Edit" (ícone de lápis) no canto superior direito da visualização do arquivo.
- Faça suas alterações e commit as mudanças.

**Usar GitHub Codespaces**

- Navegue até a página principal do seu repositório.
- Clique no botão "Code" (botão verde) próximo ao canto superior direito.
- Selecione a aba "Codespaces".
- Clique em "New codespace" para iniciar um novo ambiente Codespace.
- Edite os arquivos diretamente dentro do Codespace e faça commit e push de suas alterações quando terminar.

## Configuração do Firebase

Para configurar o Firebase em seu projeto:

1. Acesse o [Console do Firebase](https://console.firebase.google.com/)
2. Crie um novo projeto ou selecione um existente
3. No painel do projeto, clique em "Adicionar aplicativo" e selecione "Web"
4. Registre o aplicativo com um nome de sua escolha
5. Copie as configurações do Firebase fornecidas
6. No projeto, atualize o arquivo `src/lib/firebase.ts` com suas configurações

Exemplo de configuração do Firebase:
```typescript
const firebaseConfig = {
  apiKey: "sua-api-key",
  authDomain: "seu-projeto.firebaseapp.com",
  projectId: "seu-projeto",
  storageBucket: "seu-projeto.appspot.com",
  messagingSenderId: "seu-id",
  appId: "seu-app-id"
};
```

## Tecnologias Utilizadas

Este projeto é construído com:

- Vite
- TypeScript
- React
- shadcn-ui
- Tailwind CSS
- Firebase (Autenticação)
- jsPDF (Geração de PDFs)
- React Router DOM
- React Query

## Estrutura do Projeto

```
src/
├── components/      # Componentes reutilizáveis
├── pages/          # Páginas da aplicação
├── lib/            # Configurações e utilidades
└── config/         # Arquivos de configuração
```

## Funcionalidades Principais

- Autenticação com Google (domínio @alunos.utfpr.edu.br)
- Formulário multi-etapas para inscrição
- Upload de documentos
- Geração de PDF de confirmação
- Interface responsiva e intuitiva

## Requisitos do Sistema

- Node.js 18.x ou superior
- npm 8.x ou superior
- Navegador moderno (Chrome, Firefox, Safari, Edge)

## Comandos Disponíveis

```bash
# Instalar dependências
npm install

# Iniciar servidor de desenvolvimento
npm run dev

# Construir para produção
npm run build

# Visualizar build de produção
npm run preview
```

## Como fazer o Deploy

--escrever algo melhor

## Solução de Problemas

Se você encontrar problemas durante a configuração ou execução do projeto:

1. Verifique se todas as dependências foram instaladas corretamente
2. Confirme se as configurações do Firebase estão corretas
3. Certifique-se de que está usando as versões corretas do Node.js e npm
4. Verifique os logs do console para mensagens de erro específicas

## Suporte

Se precisar de ajuda:

- Abra uma issue no GitHub

## Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

