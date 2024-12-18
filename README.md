# Teste Técnico: Desenvolvedor Full Stack (C# / React)

## Descrição

Este projeto é uma aplicação para gestão de produtos, desenvolvida como parte de um teste técnico para a vaga de Desenvolvedor Full Stack Pleno. A aplicação inclui uma API RESTful em C# e uma interface web em React.

---

## Tecnologias Utilizadas
### Backend
- C#
- .NET Core / .NET 6+
- ADO.NET: Para acesso ao banco de dados.
- MySQL: Banco de dados relacional utilizado.
- JWT: Para autenticação.
- Swashbuckle (Swagger): Para documentação da API.
- DotNetEnv: Para gerenciamento de variáveis de ambiente.
- BCrypt: Para hashing de senhas.

### Frontend
- React: Biblioteca JavaScript para construção da interface.
- TypeScript: Superset de JavaScript que adiciona tipagem estática.
- Axios: Cliente HTTP para realizar requisições à API.
- React Hook Form: Biblioteca para gerenciamento de formulários.
- Zod: Biblioteca para validações de inputs.
- React Router DOM: Para roteamento na aplicação.
- Bootstrap: Framework CSS para estilização.

---

## Funcionalidades

### Backend
1. CRUD de Produtos:
   - Criação, leitura, atualização e exclusão lógica de produtos.
2. Relacionamento entre Tabelas:
   - Produtos associados a categorias e fornecedores.
3. Autenticação:
   - Proteção dos endpoints da API com JWT.
4. Banco de Dados:
   - Utilização de MySQL como banco de dados relacional.

### Frontend
1. Listagem de Produtos:
   - Exibição dos produtos cadastrados em uma tabela responsiva.
2. Cadastro e Edição de Produtos:
   - Formulário para criar e editar produtos.
3. Validações:
   - Validação básica dos campos no frontend.
4. Exclusão de Produtos:
   - Possibilidade de excluir um produto da listagem.

---

## Como configurar e Executar o Projeto

### Configuração de Portas
O backend está configurado para executar na porta 5006, conforme especificado no arquivo launchSettings.json. A rota no frontend está direcionada para essa porta. Caso haja a necessidade de alterar a porta do backend, será necessário atualizar a configuração no frontend também.

Para isso, modifique o arquivo axios.tsx localizado na pasta lib do frontend, ajustando a URL base para a nova porta.

### Backend

Deixei configurado no arquivo launchSettings.json para o backend executar na porta 5006, com isso a rota no frontend está direcionada para essa porta, se por ventura houver a troca será necessário alterar no frontend também que fica localizado na pasta lib no arquivo axios.tsx

1. Clone o repositório

```bash
https://github.com/Thalesmau/teste-desenvolvedor-pleno.git
```

2. Navegue até a pasta backend
```bash
cd teste-desenvolvedor-pleno/backend
```

3. Renomeie o arquivo .env-example para .env
```bash
mv .env-example .env
```

4. Configure a váriavel de ambiente
```bash
DefaultConnection=sua-connection_string
```

5. Restaure os pacotes Nuget
```bash
dotnet restore
```

6. Inicie a Aplicação
```bash
dotnet run
```

---

### Frontend

1. Navegue até a pasta frontend
```bash
cd teste-desenvolvedor-pleno/frontend
```

2. Instale as dependências
```bash
npm install
```

3. Inicie a aplicação
```bash
npm run dev
```

5. Restaure os pacotes Nuget
```bash
dotnet restore
```

6. Inicie a Aplicação
```bash
dotnet run
```

---

### Decisões de Design
- Autenticação com JWT: Escolhi usar JWT para garantir a segurança dos endpoints da API. Isso permite que apenas usuários autenticados possam acessar e manipular os dados, aumentando a segurança da aplicação.
- Criação e População do Banco de Dados: Ao rodar o projeto pela primeira vez, o banco de dados é automaticamente criado e populado com dados iniciais nas tabelas categories e suppliers. Além disso, um usuário padrão é adicionado com as seguintes credenciais:
   - Username: Grupo Leonora
   - Senha: leonora
   - Também é possível adicionar novos usuários conforme necessário.
- Exclusão Lógica: Foi implementado a exclusão lógica para manter o histórico de produtos no banco de dados. Em vez de remover os registros, eles são marcados como inativos.
- React Hook Form: Utilizei o React Hook Form para o gerenciamento dos formulários no frontend. Ela biblioteca facilita a manipulação de inputs e validações, e torna o código mais limpo e eficiente.
- Bootstrap: Escolhi o Bootstrap para agilizar a estilização e garantir que a interface seja responsiva. Isso ajuda a criar uma experiência de usuário maisconsistente e adaptável.
- Zod: Para a validação de inputs no frontend, usei o Zod. Essa biblioteca garante que os dados inseridos pelos usuários estejam corretos e seguros antes de serem enviados para a API, evitando problemas comuns de validação.

