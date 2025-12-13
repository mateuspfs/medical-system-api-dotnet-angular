# Sistema Médico

Sistema completo de gerenciamento médico desenvolvido com ASP.NET Core Web API no backend e Angular 17 no frontend. O sistema permite o gerenciamento de pacientes, tratamentos, etapas de tratamento, doutores, especialidades e pagamentos, com autenticação via Google OAuth.

## 📋 Funcionalidades Gerais

### 🔐 Autenticação e Autorização
- Autenticação via Google OAuth
- Controle de acesso baseado em roles (Admin e Doutor)
- Guards de autenticação e autorização no frontend
- Interceptors HTTP para gerenciamento automático de tokens

### 👨‍💼 Módulo Administrador
- **Gerenciamento de Administradores**: CRUD completo de administradores do sistema
- **Gerenciamento de Doutores**: Cadastro, edição, listagem e exclusão de doutores
- **Gerenciamento de Pacientes**: Controle completo de pacientes (cadastro, edição, listagem)
- **Gerenciamento de Tratamentos**: Criação e gerenciamento de tratamentos médicos
- **Gerenciamento de Especialidades**: Visualização e gerenciamento de especialidades médicas
- **Gerenciamento de Etapas**: Controle de etapas dos tratamentos
- **Visualização de Tratamentos de Pacientes**: Acompanhamento de tratamentos atribuídos aos pacientes

### 👨‍⚕️ Módulo Doutor
- **Gerenciamento de Pacientes**: Visualização e gerenciamento de pacientes atribuídos
- **Gerenciamento de Tratamentos**: Visualização e gerenciamento de tratamentos relacionados às suas especialidades
- **Integração de Tratamentos**: Vinculação de tratamentos aos pacientes
- **Visualização de Etapas**: Acompanhamento das etapas dos tratamentos
- **Visualização de Pacientes por Etapa**: Filtro de pacientes por etapa de tratamento

### 📊 Funcionalidades Comuns
- **Sistema de Pagamentos**: Gerenciamento de pagamentos relacionados aos tratamentos
- **Auditoria**: Registro de ações e alterações no sistema
- **Upload de Arquivos**: Armazenamento de documentos relacionados aos tratamentos
- **Paginação**: Listagens paginadas para melhor performance
- **Filtros e Buscas**: Sistema de busca e filtros em todas as listagens
- **Swagger/OpenAPI**: Documentação automática da API

## 🛠️ Tecnologias Utilizadas

### Backend (API)
- **.NET 10.0**: Framework principal
- **ASP.NET Core Web API**: Framework para construção da API REST
- **Entity Framework Core 10.0**: ORM para acesso ao banco de dados
- **SQL Server**: Banco de dados relacional
- **AutoMapper 13.0.1**: Mapeamento de objetos
- **Google OAuth**: Autenticação via Google
- **Swashbuckle (Swagger)**: Documentação da API
- **Microsoft.EntityFrameworkCore.SqlServer**: Provider SQL Server para EF Core

### Frontend
- **Angular 17.3**: Framework frontend
- **TypeScript 5.2**: Linguagem de programação
- **Tailwind CSS 3.4**: Framework CSS utilitário
- **Axios 1.6.8**: Cliente HTTP para comunicação com a API
- **RxJS 7.8**: Biblioteca reativa para programação assíncrona

### Arquitetura
- **Repository Pattern**: Separação de responsabilidades no acesso a dados
- **Service Layer**: Lógica de negócio isolada
- **DTOs (Data Transfer Objects)**: Transferência de dados entre camadas
- **Dependency Injection**: Injeção de dependências nativa do .NET
- **Guards e Interceptors**: Proteção de rotas e interceptação de requisições HTTP

## 🚀 Como Rodar o Projeto

### Pré-requisitos
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (versão 18 ou superior)
- [SQL Server](https://www.microsoft.com/sql-server) ou SQL Server Express
- [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- Conta Google para configurar OAuth (opcional para desenvolvimento)

### Configuração do Backend (API)

1. **Navegue até a pasta da API:**
   ```bash
   cd api/SistemaMedico
   ```

2. **Configure a Connection String:**
   - Abra o arquivo `appsettings.json`
   - Atualize a `ConnectionStrings:DataBase` com suas credenciais do SQL Server:
   ```json
   "ConnectionStrings": {
     "DataBase": "Server=SEU_SERVIDOR;Database=db_medico;Trusted_Connection=True;Encrypt=False;"
   }
   ```

3. **Configure o Google OAuth (opcional):**
   - No arquivo `appsettings.json`, atualize as credenciais do Google OAuth:
   ```json
   "Authentication": {
     "Google": {
       "ClientId": "SEU_CLIENT_ID",
       "ClientSecret": "SEU_CLIENT_SECRET"
     }
   }
   ```

4. **Execute as Migrations:**
   ```bash
   dotnet ef database update
   ```
   Ou via Package Manager Console no Visual Studio:
   ```powershell
   Update-Database
   ```

5. **Configurar Seeders (Dados Iniciais):**
   - Abra o arquivo `Program.cs`
   - Descomente as linhas 29-34 para executar os seeders na primeira execução:
   ```csharp
   // Descomente para rodar as Seeders
   using (var serviceScope = services.BuildServiceProvider().CreateScope())
   {
       var dbContext = serviceScope.ServiceProvider.GetRequiredService<SistemaMedico.Data.SistemaMedicoDBContex>();
       SistemaMedico.Data.DbSeeder.Seed(dbContext);
   }
   ```

6. **Execute a API:**
   ```bash
   dotnet run
   ```
   Ou execute pelo Visual Studio (F5)

   A API estará disponível em:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:7225`
   - Swagger: `https://localhost:7225/swagger`

### Configuração do Frontend

1. **Navegue até a pasta do Frontend:**
   ```bash
   cd frontend
   ```

2. **Instale as dependências:**
   ```bash
   npm install
   ```

3. **Configure a URL da API:**
   - Abra o arquivo `src/app/core/config/api.config.ts` (se existir)
   - Ou verifique os serviços em `src/app/core/services/` para ajustar a URL base da API
   - A URL padrão deve apontar para: `https://localhost:7225/api`

4. **Execute o Frontend:**
   ```bash
   npm start
   ```
   Ou:
   ```bash
   ng serve --port 5500
   ```

   O frontend estará disponível em: `http://localhost:5500`

### Acessando o Sistema

1. Acesse `http://localhost:5500` no navegador
2. Faça login com sua conta Google (configurada no OAuth)
3. O sistema redirecionará automaticamente:
   - **Admin**: Para `/admin`
   - **Doutor**: Para `/doutor`

## 📁 Estrutura do Projeto

```
app-sistema-medico/
├── api/
│   └── SistemaMedico/
│       ├── Controllers/          # Controladores da API
│       ├── Services/             # Camada de serviços
│       ├── Repositories/         # Camada de repositórios
│       ├── Models/               # Modelos de dados
│       ├── DTOs/                 # Data Transfer Objects
│       ├── Data/                 # Contexto do banco e migrations
│       ├── Config/               # Configurações e DI
│       └── Program.cs            # Ponto de entrada da API
│
└── frontend/
    ├── src/
    │   ├── app/
    │   │   ├── core/             # Serviços, guards, interceptors
    │   │   ├── features/         # Módulos de funcionalidades
    │   │   │   ├── admin/        # Módulo Admin
    │   │   │   ├── doutor/       # Módulo Doutor
    │   │   │   └── auth/         # Módulo de autenticação
    │   │   └── shared/           # Componentes compartilhados
    │   └── assets/               # Arquivos estáticos
    ├── views/                    # Views HTML (legado)
    └── package.json
```

## 🔧 Comandos Úteis

### Backend
```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Executar a API
dotnet run

# Build do projeto
dotnet build
```

### Frontend
```bash
# Instalar dependências
npm install

# Executar em desenvolvimento
npm start

# Build para produção
npm run build

# Executar testes
npm test
```

## 📝 Notas Importantes

- ⚠️ **Primeira Execução**: Certifique-se de executar `Update-Database` antes de rodar a API pela primeira vez
- ⚠️ **Seeders**: Descomente as linhas do seeder no `Program.cs` apenas na primeira execução para popular dados iniciais
- ⚠️ **CORS**: A API está configurada para aceitar requisições de qualquer origem em desenvolvimento
- ⚠️ **Google OAuth**: Configure as credenciais do Google OAuth no `appsettings.json` para que a autenticação funcione
- ⚠️ **Connection String**: Ajuste a connection string do banco de dados conforme seu ambiente

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais e de estágio.
