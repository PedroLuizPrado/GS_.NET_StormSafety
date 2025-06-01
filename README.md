#  StormSafety – Sistema de Registro e Previsão de Ocorrências Climáticas 

> **Murilo Ferreira Ramos - RM553315**  
> **Pedro Luiz Prado - RM553874**  
> **William Kenzo Hayashi - RM552659**

---
## 🎯 Objetivo do Projeto
O StormSafety é uma API RESTful desenvolvida com .NET 8 que tem como objetivo principal permitir que usuários registrem ocorrências causadas por desastres naturais (como enchentes, quedas de árvore, apagões), e que essas informações sejam:

Registradas e persistidas em um banco Oracle,

Enviadas para uma fila RabbitMQ para tratamento assíncrono (eventualmente notificações ou análises),

Classificadas automaticamente por um modelo de Machine Learning treinado localmente com base na descrição textual,

Consultadas e monitoradas via endpoints documentados no Swagger.

## 🧠 Para que serve
População / Usuários podem registrar problemas que ocorrem durante tempestades (alagamentos, sem luz, árvore caída, etc).

Órgãos de Defesa Civil / Autoridades podem consumir essas ocorrências de forma assíncrona via RabbitMQ e tomar decisões.

Sistema de IA pode prever o tipo da ocorrência com base em sua descrição textual, facilitando a triagem.

O projeto integra funcionalidades como:
- **RabbitMQ** (mensageria assíncrona),
- **ML.NET** (machine learning para previsão de tipo de ocorrência),
- **Swagger + HATEOAS** (navegação por links REST),
- **Rate Limiting** (controle de requisições),
- **Oracle Database**.

## ✅ Controllers (Camada de Controle)
OcorrenciaController.cs
Responsável pelos endpoints de:

Criar ocorrência (POST /api/Ocorrencia)

Buscar todas ou uma ocorrência específica

Usar RabbitMQ para publicar ocorrências

Usar ML.NET para prever o tipo com base na descrição (POST /api/Ocorrencia/prever)

UsuarioController.cs
Gerencia os usuários do sistema:

Criar, atualizar, deletar e buscar usuários

Utiliza UsuarioCreateDTO para simplificar a entrada de dados

TipoOcorrenciaController.cs
Controla os tipos possíveis de ocorrências:

Ex: Alagamento, Falta de energia, Deslizamento etc

Cadastro via DTO TipoOcorrenciaCreateDTO

## 🧩 DTOs (Data Transfer Objects)
Evita expor o modelo completo e melhora a usabilidade da API.

UsuarioCreateDTO.cs → utilizado no POST /api/Usuario para inserir apenas nome, email e localização.

OcorrenciaCreateDTO.cs → utilizado no POST /api/Ocorrencia para simplificar os dados da ocorrência (sem enviar os objetos aninhados).

TipoOcorrenciaCreateDTO.cs → utilizado no POST /api/TipoOcorrencia.

## 🧠 Services (Camada de Serviço)
RabbitMQService.cs
Envia a ocorrência para uma fila RabbitMQ (fila_ocorrencias) após ela ser registrada na API. Serve para permitir o consumo assíncrono por outro sistema, como alertas ou dashboards.

MLModelService.cs
Contém o modelo de Machine Learning treinado com exemplos de descrições e tipos. Permite prever automaticamente o tipo de ocorrência com base apenas na descrição textual.

## 🗃️ Data (Banco de Dados)
AppDbContext.cs
Classe principal que gerencia a conexão com o banco Oracle e mapeia as tabelas:

StormDatabase_Usuario

StormDatabase_Ocorrencia

StormDatabase_TipoOcorrencia

## 🧪 StormSafety.Tests (Testes com xUnit)
Contém testes de integração e unidade para os controllers:

UsuarioControllerTests.cs → valida criação de usuários

OcorrenciaTests.cs → verifica comportamento da lógica de inserção de ocorrências

TipoOcorrenciaTests.cs → assegura que tipos de ocorrência são cadastrados corretamente

## 🖥️ StormSafety.Consumer (Console app)
Esse projeto console roda como consumidor do RabbitMQ.

Escuta mensagens da fila fila_ocorrencias

Exibe no terminal o conteúdo recebido

Futuramente pode ser usado para enviar alertas ou gravar logs


---

## 🧱 Estrutura da Solução

StormSafety.API/ # Projeto principal da API
├── Controllers/
│ ├── OcorrenciaController.cs
│ ├── UsuarioController.cs
│ └── TipoOcorrenciaController.cs
├── DTOs/
│ ├── UsuarioCreateDTO.cs
│ ├── OcorrenciaCreateDTO.cs
│ └── TipoOcorrenciaCreateDTO.cs
├── Models/
├── Services/
│ ├── RabbitMQService.cs
│ └── MLModelService.cs
├── Data/AppDbContext.cs
└── Program.cs

StormSafety.Tests/ # Projeto de testes xUnit
├── UsuarioControllerTests.cs
├── OcorrenciaTests.cs
└── TipoOcorrenciaTests.cs

StormSafety.Consumer/ # Console que consome mensagens do RabbitMQ
└── Program.cs
---

## 🚀 Como Rodar

### 1. 🔧 Pré-requisitos
- .NET 8 SDK
- Oracle Database (pode ser local ou em nuvem)
- RabbitMQ (pode rodar via Docker)
- Visual Studio 2022 (ou superior)

### 2. 💾 Configure o `appsettings.json`

```
json
"ConnectionStrings": {
  "OracleConnection": "Data Source=oracle.fiap.com.br:1521/orcl;User Id=SEU_USUARIO;Password=SUA_SENHA;"
}
```

3. ▶️ Rode a API
```
dotnet build
dotnet run --project StormSafety.API
```
## 📬 RabbitMQ
Envio:
O envio ocorre ao registrar uma nova ocorrência com POST /api/Ocorrencia.
O JSON da ocorrência é publicado na fila "fila_ocorrencias".

Consumo:
Execute o consumidor:

```
dotnet run --project StormSafety.Consumer
```

## 🤖 Machine Learning (ML.NET)
A API possui um endpoint inteligente:


```
POST /api/Ocorrencia/prever
{
  "descricao": "Ficamos sem energia após a tempestade"
}
```
Resposta:

json
``` 
{ "tipoPrevisto": "Falta de energia" }
Modelo embutido no MLModelService.cs, treinado com base em exemplos simples.
```

## ✅ Testes Automatizados
Executar testes com:

```
dotnet test
```
Inclui testes para criação de usuários e validação de comportamento dos controllers com base em DTOs.

## 📊 Tecnologias Utilizadas
.NET 8

Oracle.EntityFrameworkCore

RabbitMQ.Client

ML.NET

xUnit

Swagger

AspNetCoreRateLimit

## 💡 Futuras Melhorias
Persistência e visualização de logs de alertas

Treinamento contínuo do modelo ML com base nos dados reais

Front-end para consumo da API

Integração com serviços de geolocalização

