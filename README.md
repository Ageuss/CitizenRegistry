# CitizenRegistry

Sistema Full Stack para cadastro e gestão de cidadãos brasileiros. O projeto é composto por um backend em **C# (.NET)** com validação de regras de negócio (CPF), banco de dados **SQL Server** conteinerizado via Docker, e uma interface frontend em **Angular** moderna e desacoplada.

> [!IMPORTANT]
> **Observação sobre Segurança de Credenciais:** 
> Para fins práticos de avaliação deste desafio de vaga técnica, as credenciais e configurações de conexão com o banco de dados (como a string de conexão no `appsettings.json` e as senhas no `docker-compose.yml`) estão sendo **versionadas** para facilitar a clonagem e a execução imediata do projeto.
> 

---

## 🛠️ Pré-requisitos
Para executar a aplicação em seu ambiente local, você precisará das seguintes ferramentas instaladas:
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para subir o SQL Server)
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
* [Node.js](https://nodejs.org/) v18.0.0+ (gerenciador de pacotes npm incluso)

---

## 📁 Estrutura do Projeto
O repositório está dividido em dois projetos principais:
1. **[CitizenRegistry.API](file:///c:/Users/jegir/source/repos/Ageuss/CitizenRegistry/CitizenRegistry.API):** Backend RESTful em C# (.NET) responsável pelas regras de validação (FluentValidation) e conexão com banco de dados (EF Core).
2. **[CitizenRegistry.Web](file:///c:/Users/jegir/source/repos/Ageuss/CitizenRegistry/CitizenRegistry.Web):** Frontend SPA em Angular (v22) desacoplado com design responsivo escuro, validação reativa e máscara dinâmica de CPF.

---

## 🚀 Como Executar a Aplicação

Siga os passos abaixo na ordem indicada:

### Passo 1: Iniciar o Banco de Dados (Docker)
No diretório raiz do projeto, execute o comando abaixo para subir o container do SQL Server:
```bash
docker-compose up -d
```
*O banco estará pronto e rodando na porta padrão `1433`.*

### Passo 2: Aplicar Migrations e Executar o Backend (API .NET)
1. Navegue até a pasta da API:
   ```bash
   cd CitizenRegistry.API
   ```
2. Caso ainda não possua a ferramenta do EF Core, instale-a globalmente:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
3. Aplique as migrations no banco de dados para criar as tabelas:
   ```bash
   dotnet ef database update
   ```
4. Execute o projeto do backend:
   ```bash
   dotnet run
   ```
*A API será iniciada e ficará disponível em **`http://localhost:5052`** (HTTP) e **`https://localhost:7162`** (HTTPS).*
*A documentação Swagger estará ativa no ambiente de desenvolvimento em: **`http://localhost:5052/swagger`**.*

### Passo 3: Executar o Frontend (Angular)
1. Abra um novo terminal no diretório raiz e navegue até a pasta do frontend:
   ```bash
   cd CitizenRegistry.Web
   ```
2. Instale as dependências de pacotes (necessário apenas na primeira execução):
   ```bash
   npm install
   ```
3. Inicie o servidor de desenvolvimento do Angular:
   ```bash
   npm start
   ```
*O frontend compilará os arquivos e ficará disponível em **`http://localhost:4200`**.*

---

## 🔗 Endereços de Acesso (Localmente)
* **Interface da Aplicação (Frontend):** [http://localhost:4200/](http://localhost:4200/)
* **Documentação da API (Swagger):** [http://localhost:5052/swagger/index.html](http://localhost:5052/swagger/index.html)

---

## 🌟 Diferenciais e Boas Práticas Adotadas
* **Validação em Duas Camadas:** O CPF e Nome são validados em tempo real no frontend com máscara dinâmica para garantir excelente UX, e revalidados no backend com **FluentValidation** para blindar a integridade do banco de dados.
* **Paradigma Orientado a Objetos (POO):** Classes bem definidas, separação de conceitos através do padrão **Repository** no backend e uso do padrão **Service** no frontend.
* **Gerenciador de Pacotes:** Utilização nativa do `NuGet` (.NET) e `npm` (Angular).
