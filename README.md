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

### Backend

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

Ao rodar o projeto pela primeira vez, o banco de dados será criado e populado com alguns dados nas tabelas categories e suppliers. Além disso, um usuário será adicionado na tabela users com as seguintes credenciais:

- Username: Grupo Leonora
- Senha: leonora

Também é possível adicionar novos usuários.

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
