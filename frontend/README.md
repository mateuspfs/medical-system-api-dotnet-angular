# Sistema Médico - Frontend Angular

Frontend refatorado do sistema médico utilizando Angular 17, Tailwind CSS e Axios para comunicação com a API.

## 🚀 Tecnologias

- **Angular 17**: Framework principal
- **Tailwind CSS**: Estilização moderna e responsiva
- **Axios**: Cliente HTTP para comunicação com a API
- **TypeScript**: Linguagem de programação

## 📁 Estrutura do Projeto

```
src/
├── app/
│   ├── core/                    # Camada core (serviços, guards, interceptors)
│   │   ├── config/              # Configurações (API, etc)
│   │   ├── guards/             # Guards de autenticação e autorização
│   │   ├── interceptors/       # Interceptors HTTP
│   │   ├── models/             # Modelos de dados
│   │   ├── services/            # Serviços de API
│   │   └── utils/               # Utilitários
│   ├── features/                # Módulos de funcionalidades
│   │   ├── admin/               # Módulo Admin
│   │   ├── doutor/              # Módulo Doutor
│   │   └── auth/                # Módulo de autenticação
│   └── shared/                  # Componentes compartilhados
│       └── components/          # Componentes reutilizáveis
└── styles.css                   # Estilos globais com Tailwind
```

## 🏗️ Arquitetura

### Camadas

1. **Core**: Serviços base, guards, interceptors e modelos
2. **Features**: Módulos de funcionalidades (Admin, Doutor, Auth)
3. **Shared**: Componentes compartilhados (Navbar, Modal, Pagination)

### Serviços de API

Todos os serviços de API utilizam Axios através do `ApiService` centralizado:

- `AuthService`: Autenticação e gerenciamento de usuário
- `AdminService`: CRUD de Admins
- `DoutorService`: CRUD de Doutores
- `PacienteService`: CRUD de Pacientes
- `TratamentoService`: CRUD de Tratamentos
- `EspecialidadeService`: Listagem de Especialidades

### Guards

- `AuthGuard`: Verifica se o usuário está autenticado
- `AdminGuard`: Verifica se o usuário é admin e valida token
- `DoutorGuard`: Verifica se o usuário é doutor e valida token

### Interceptors

- `AuthInterceptor`: Adiciona token de autenticação nas requisições
- `ErrorInterceptor`: Trata erros HTTP (401, 403) e redireciona para login

## 🔐 Autenticação

O sistema utiliza Google OAuth para autenticação. Após o login bem-sucedido:

- Admin: Redirecionado para `/admin`
- Doutor: Redirecionado para `/doutor`

O token de acesso é armazenado no `localStorage` e automaticamente incluído em todas as requisições via interceptor.

## 📦 Instalação

1. Instale as dependências:
```bash
npm install
```

2. Configure a URL da API em `src/app/core/config/api.config.ts`:
```typescript
export const API_CONFIG = {
  baseUrl: 'https://localhost:7225/api',
  googleClientId: 'seu-client-id'
};
```

3. Execute o projeto:
```bash
npm start
```

## 🎨 Estilização

O projeto utiliza Tailwind CSS com classes utilitárias customizadas definidas em `src/styles.css`:

- `.btn-primary`: Botão primário
- `.btn-secondary`: Botão secundário
- `.btn-danger`: Botão de perigo
- `.input-field`: Campo de entrada
- `.card`: Card container
- `.table-container`: Container de tabela

## 📝 Funcionalidades

### Admin
- Gerenciamento de Admins
- Gerenciamento de Doutores
- Gerenciamento de Pacientes
- Gerenciamento de Tratamentos
- Visualização de Especialidades

### Doutor
- Gerenciamento de Pacientes
- Gerenciamento de Tratamentos (apenas suas especialidades)

## 🔧 Desenvolvimento

### Adicionar novo serviço

1. Crie o modelo em `src/app/core/models/`
2. Crie o serviço em `src/app/core/services/` utilizando `ApiService`
3. Implemente os métodos necessários (get, post, put, delete)

### Adicionar novo componente

1. Crie o componente no módulo apropriado
2. Utilize componentes compartilhados quando possível
3. Aplique classes Tailwind para estilização

## 📄 Licença

Este projeto é privado e de uso interno.

