# HP_Teste_Back

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)
![MongoDB](https://img.shields.io/badge/MongoDB-In--Memory-47A248?logo=mongodb)
![License](https://img.shields.io/badge/License-MIT-green)

## 📋 Descrição

API RESTful desenvolvida em .NET 8 para consulta de informações de CEP e previsão do tempo. O projeto demonstra boas práticas de arquitetura limpa, resiliência de APIs externas e cache de dados.

### Controllers Disponíveis

#### 🏠 CEP Controller (`/api/cep`)
Responsável por consultar e armazenar informações de endereços brasileiros através de CEPs.

**Funcionalidades:**
- Consulta de CEP com integração a múltiplas APIs públicas (BrasilAPI e ViaCEP)
- Sistema de fallback automático entre APIs
- Armazenamento persistente no MongoDB
- Cache em memória para consultas recentes
- Validação de formato de CEP
- Retry automático com Polly
- Retorna coordenadas geográficas (lat/lon) quando disponíveis

**Endpoints:**
- `POST /api/cep` - Registra um novo CEP
- `GET /api/cep/{zipCode}` - Consulta CEP específico

#### 🌤️ Weather Controller (`/api/weather`)
Fornece previsão do tempo baseada nos CEPs cadastrados no sistema.

**Funcionalidades:**
- Consulta de previsão do tempo para todos os CEPs salvos
- Integração com Open-Meteo (principal) e OpenWeatherMap (fallback)
- Geocoding automático quando coordenadas não estão disponíveis
- Suporte a previsões de 1 a 7 dias
- Cache de 10 minutos para otimizar performance
- Ordenação por data de cadastro (mais recentes primeiro)
- Sistema de fallback entre APIs

**Endpoints:**
- `GET /api/weather?days=3` - Retorna previsão do tempo (1-7 dias)

---

## 🚀 Tecnologias Utilizadas

### Core Technologies
- **[.NET 8.0](https://dotnet.microsoft.com/)** - Framework principal para desenvolvimento da API
- **[ASP.NET Core](https://docs.microsoft.com/aspnet/core)** - Framework web para criação de APIs RESTful
- **[C# 12](https://docs.microsoft.com/dotnet/csharp/)** - Linguagem de programação moderna e type-safe
- **[MongoDB](https://www.mongodb.com/)** - Banco de dados NoSQL para persistência de dados
- **[Docker](https://www.docker.com/)** - Containerização da aplicação para deploy consistente

### Principais Bibliotecas

#### Comunicação HTTP
- **[Refit 7.2.22](https://github.com/reactiveui/refit)** - Cliente HTTP type-safe que transforma interfaces em APIs REST através de atributos, eliminando código boilerplate

#### Resiliência e Retry
- **[Polly 8.5.0](https://github.com/App-vNext/Polly)** - Biblioteca de resiliência que implementa políticas de retry, circuit breaker e timeout para chamadas HTTP, garantindo estabilidade

#### Validação
- **[FluentValidation 11.8.0](https://fluentvalidation.net/)** - Framework declarativo para validação de objetos com regras fluentes e mensagens customizadas

#### Cache
- **[Microsoft.Extensions.Caching.Memory](https://docs.microsoft.com/dotnet/api/microsoft.extensions.caching.memory)** - Sistema de cache em memória nativo do .NET para armazenamento temporário de dados

#### Banco de Dados
- **[MongoDB.Driver 3.0.0](https://mongodb.github.io/mongo-csharp-driver/)** - Driver oficial do MongoDB para .NET com suporte assíncrono completo

#### Serialização
- **[System.Text.Json](https://docs.microsoft.com/dotnet/api/system.text.json)** - Serializador JSON de alta performance nativo do .NET 8

#### Testes
- **[xUnit 2.5.3](https://xunit.net/)** - Framework de testes com suporte a Theory/InlineData/ClassData
- **[Moq 4.20.72](https://github.com/moq/moq4)** - Criação de mocks para isolar dependências
- **[FluentAssertions 7.0.0](https://fluentassertions.com/)** - Asserções expressivas (Should/Be/Return pattern)
- **[Bogus 35.6.1](https://github.com/bchavez/Bogus)** - Gerador de dados fake para testes com localização pt_BR
- **[Coverlet](https://github.com/coverlet-coverage/coverlet)** - Ferramenta de cobertura de código

---

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design**:

```
HP_Teste_Back/
├── src/
│   ├── HP.Clima.API/           # Camada de apresentação (Controllers, Middleware)
│   ├── HP.Clima.Domain/        # Camada de domínio (Entities, DTOs, Interfaces)
│   ├── HP.Clima.Service/       # Camada de serviços (Business Logic, Handlers)
│   └── HP.Clima.Infra/         # Camada de infraestrutura (MongoDB, Repositories)
├── test/
│   └── unit/                   # Testes unitários
├── docker-compose.yml          # Configuração Docker Compose
├── docker.sh                   # Script helper de gerenciamento
└── README.md
```

### Padrões Implementados
- ✅ **Repository Pattern** - Abstração de acesso a dados
- ✅ **Handler Pattern** - Processamento modular de requisições
- ✅ **Retry Pattern** - Resiliência em chamadas externas
- ✅ **Circuit Breaker** - Proteção contra falhas em cascata
- ✅ **Dependency Injection** - Inversão de controle nativa do .NET
- ✅ **Problem Details (RFC 7807)** - Respostas de erro padronizadas

---

## 🐳 Como Executar o Projeto

### Pré-requisitos
- [Docker](https://www.docker.com/get-started) instalado
- [Docker Compose](https://docs.docker.com/compose/install/) instalado
- Porta **5109** disponível (API)
- Porta **27017** disponível (MongoDB)

### Opção 1: Usando o Script Helper (Recomendado)

#### 1️⃣ Clone o repositório
```bash
git clone https://github.com/Cristiliano/HP_Teste_Back.git
cd HP_Teste_Back
```

#### 2️⃣ Dê permissão de execução ao script
```bash
chmod +x docker.sh
```

#### 3️⃣ Execute o script
```bash
./docker.sh
```

#### 4️⃣ No menu interativo, siga este fluxo:

```
================================================
  HP Clima API - Docker Management
================================================

1) Build - Criar imagem Docker
2) Up - Iniciar todos os serviços
3) Down - Parar todos os serviços
4) Logs - Ver logs da aplicação
5) Restart - Reiniciar serviços
6) Clean - Limpar containers e volumes
7) Status - Ver status dos containers
8) Shell - Acessar shell do container da API
9) Test - Testar endpoints da API
0) Sair
```

**Primeira execução:**
1. Digite `1` → Criar imagem Docker (aguarde o build)
2. Digite `2` → Iniciar API + MongoDB
3. Digite `9` → Executar testes automatizados
4. Digite `4` → Ver logs em tempo real (Ctrl+C para sair)
5. Digite `0` → Sair do menu

### Opção 2: Usando Docker Compose Diretamente

```bash
# Build da imagem
docker-compose build

# Iniciar serviços
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar serviços
docker-compose down
```

---

## 🧪 Testando a API

### Health Check
```bash
curl http://localhost:5109/health
```

### Cadastrar um CEP
```bash
curl -X POST http://localhost:5109/api/cep \
  -H "Content-Type: application/json" \
  -d '{"zipCode": "01311000"}'
```

**Resposta esperada:**
```json
{
  "zipCode": "01311000",
  "street": "Avenida Paulista",
  "district": "Bela Vista",
  "city": "São Paulo",
  "state": "SP",
  "location": {
    "lat": -23.5575815,
    "lon": -46.6606235
  },
  "provider": "brasilapi"
}
```

### Consultar Previsão do Tempo
```bash
curl "http://localhost:5109/api/weather?days=3"
```

**Resposta esperada:**
```json
[
  {
    "sourceZipCodeId": -677638890,
    "location": {
      "lat": -23.5575815,
      "lon": -46.6606235,
      "city": "São Paulo",
      "state": "SP"
    },
    "current": {
      "temperatureC": 16.3,
      "humidity": 0.86,
      "apparentTemperatureC": 16.8,
      "observedAt": "2025-11-19T22:15:00"
    },
    "daily": [
      {
        "date": "2025-11-19",
        "tempMinC": 14.8,
        "tempMaxC": 25.2
      },
      {
        "date": "2025-11-20",
        "tempMinC": 14.5,
        "tempMaxC": 23.6
      },
      {
        "date": "2025-11-21",
        "tempMinC": 13.9,
        "tempMaxC": 29.6
      }
    ],
    "provider": "open-meteo"
  }
]
```

### Validações
```bash
# Tentativa com days inválido (deve retornar 400)
curl "http://localhost:5109/api/weather?days=10"

# Sem CEPs cadastrados (deve retornar 404)
curl "http://localhost:5109/api/weather?days=3"
```

---

## 📊 Monitoramento

### Ver Status dos Containers
```bash
docker-compose ps
```

### Ver Uso de Recursos
```bash
docker stats hp-clima-api hp-clima-mongodb
```

### Acessar Shell do Container
```bash
docker exec -it hp-clima-api /bin/sh
```

### Acessar MongoDB
```bash
docker exec -it hp-clima-mongodb mongosh

# Ou via connection string
mongodb://admin:admin123@localhost:27017
```

---

## 🔧 Configuração

### Variáveis de Ambiente (docker-compose.yml)

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - MongoDbSettings__ConnectionString=mongodb://admin:admin123@mongodb:27017
  - MongoDbSettings__DatabaseName=hp_clima
  - HttpClientOptions__OpenWeatherMap__ApiKey=sua-chave-aqui
```

### Ports Configurados
- **API**: `5109` (HTTP) / `5110` (HTTPS)
- **MongoDB**: `27017`

---

## 🧹 Limpeza e Manutenção

### Remover Containers
```bash
docker-compose down
```

### Remover Containers + Volumes (⚠️ apaga dados do MongoDB)
```bash
docker-compose down -v
```

### Limpar Sistema Docker Completo
```bash
docker system prune -a
```

---

## 📝 APIs Externas Utilizadas

### CEP
- **[BrasilAPI](https://brasilapi.com.br/)** (Principal) - API pública brasileira
- **[ViaCEP](https://viacep.com.br/)** (Fallback) - API consolidada de CEPs

### Weather
- **[Open-Meteo](https://open-meteo.com/)** (Principal) - API gratuita de meteorologia
- **[OpenWeatherMap](https://openweathermap.org/)** (Fallback) - API global de clima

### Geocoding
- **[Open-Meteo Geocoding](https://open-meteo.com/en/docs/geocoding-api)** - Conversão de cidade/estado para coordenadas

---

## 🛡️ Segurança

- ✅ Container executa com usuário não-root
- ✅ Imagem Alpine otimizada e reduzida
- ✅ Health checks configurados
- ✅ Secrets não commitados (API keys via env vars)
- ✅ Validação de entrada com FluentValidation
- ✅ Error handling com Problem Details

---

## 📈 Performance

- ✅ Cache em memória (10min TTL)
- ✅ Retry com backoff exponencial
- ✅ Circuit breaker para proteção
- ✅ MongoDB com índices otimizados
- ✅ Dockerfile multi-stage para imagem mínima
- ✅ Serialização JSON de alta performance

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👤 Autor

**Cristiliano Cardoso**

- GitHub: [@Cristiliano](https://github.com/Cristiliano)
- Projeto: [HP_Teste_Back](https://github.com/Cristiliano/HP_Teste_Back)

---

## 🤝 Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

---

**Desenvolvido com usando .NET 8 e Docker**