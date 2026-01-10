# MiniSense API - Desafio Técnico Backend (.NET 9)

Este projeto consiste em uma API RESTful para gestão de dispositivos IoT e sensoreamento remoto, desenvolvida como parte do processo seletivo para a vaga de Desenvolvedor Backend.

O sistema permite o gerenciamento de usuários, dispositivos sensores (gateways), streams de dados (temperatura, umidade, etc.) e a ingestão de medições em tempo real.

## 📋 Entregável Nível 2
[cite_start]Este projeto atende aos requisitos do **Nível 2** do desafio, incluindo:
- ✅ Implementação da API em **.NET 9.0**.
- ✅ Containerização completa com **Docker**.
- ✅ Documentação de API interativa com **Scalar** (moderno substituto ao Swagger).
- ✅ Explicação detalhada da modelagem de domínio.

---

## 🚀 Como Rodar o Projeto

A maneira mais simples e recomendada de executar a aplicação é utilizando o **Docker Compose**, que orquestra a API e o Banco de Dados PostgreSQL automaticamente.

### Pré-requisitos
- [Docker](https://www.docker.com/) e Docker Compose instalados.

### Passo a Passo

1. Clone este repositório:
   ```bash
   git clone <seu-repositorio-url>
   cd MiniSense
   
2. Execute o build e suba os containers:
    ```bash
    docker-compose up --build

3. Aguarde alguns segundos. A aplicação estará disponível quando o terminal exibir que o container minisense-api está rodando.

Nota: As migrações do banco de dados (Criação de tabelas e Seed de Unidades de Medida) são aplicadas automaticamente na inicialização da API.

📖 Documentação da API
A documentação interativa dos endpoints, incluindo contratos JSON, tipos de dados e ferramenta de teste ("Try it out"), está disponível via Scalar.

Após rodar o projeto, acesse em seu navegador:

👉 http://localhost:5000/scalar/v1

Funcionalidades Disponíveis

Conforme solicitado no desafio, a API oferece os seguintes recursos:

Unidades de Medida: Consulta de unidades padronizadas (Celsius, Lux, etc.).

Dispositivos:

Cadastro de novos dispositivos sensores.

Listagem de dispositivos por usuário.

Consulta detalhada de um dispositivo (incluindo as 5 medições mais recentes de cada stream).

Streams de Dados:

Registro de novas streams (ex: Temperatura) vinculadas a um dispositivo.

Consulta de histórico completo de uma stream.

Medições (Sensor Data):

Publicação de novas leituras de sensores.

🏗️ Arquitetura e Modelagem do Domínio
O projeto foi construído seguindo os princípios da Clean Architecture e Domain-Driven Design (DDD) para garantir desacoplamento, testabilidade e facilidade de manutenção.

1. Estrutura de Camadas

A solução está dividida em 4 camadas principais:

MiniSense.Domain (Core):

Contém as Entidades, Enums, Constantes e Interfaces de Repositório.

Decisão de Design: Não possui dependências de bibliotecas externas ou frameworks (como EF Core). Representa a "verdade" do negócio.

MiniSense.Application:

Contém os Serviços (MiniSenseService), DTOs (Records) e Interfaces de Serviço.

Orquestra o fluxo de dados e converte Entidades de Domínio para DTOs de resposta.

MiniSense.Infrastructure:

Implementa o acesso a dados com Entity Framework Core (PostgreSQL).

Contém os Repositórios concretos e o padrão Unit of Work.

Configurações de banco (Mapeamento Fluent API) ficam aqui para não poluir o Domínio.

MiniSense.API:

Camada de entrada (Controllers REST).

Responsável apenas por receber requisições HTTP, validar o formato e chamar a Camada de Aplicação.

2. Modelagem Rica (Rich Domain Model)

Em vez de utilizar classes anêmicas (apenas get; set; públicos), o domínio foi modelado para garantir integridade:

Encapsulamento: As propriedades das entidades (ex: SensorDevice, DataStream) possuem private set.

Auto-Validação: Os construtores validam as regras de negócio imediatamente. Por exemplo, é impossível instanciar um DataStream com uma unidade de medida inválida ou sem nome.

Aggregate Roots: O SensorDevice atua como uma raiz de agregação. A adição de uma DataStream passa por validação dentro do objeto SensorDevice (ex: verificar duplicidade de Label) antes de ser persistida.

3. Padrões Técnicos Utilizados

.NET 9 Features: Uso de Primary Constructors e Records para código mais limpo e imutabilidade nos DTOs.

Repository Pattern & Unit of Work: Abstração da camada de persistência para facilitar testes e garantir transações atômicas (ACID) ao salvar dados complexos.

CQRS (Simplificado): Separação lógica entre comandos de leitura (Queries otimizadas com AsNoTracking) e escrita.

Scalar: Adotado como ferramenta de documentação moderna, alinhado às recomendações recentes do ecossistema .NET.

🛠️ Tecnologias
.NET 9.0 (C# 13)

Entity Framework Core 9

PostgreSQL (Banco de Dados)

Docker & Docker Compose

Scalar (OpenAPI UI)