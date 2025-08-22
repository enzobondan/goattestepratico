# FinanceControl

Para rodar o projeto, basta executar o `docker-compose.yml` do projeto:

docker-compose up -d

Acesse:

* **Frontend:** [http://localhost:4200](http://localhost:4200/)
* **Backend:** [http://localhost:8080](http://localhost:8080/)
* **Swagger UI:** [http://localhost:8080/swagger-ui/index.html](http://localhost:8080/swagger-ui/index.html)

# O FinanceControl é um sistema de Contas a Pagar multi-tenant, pensado para empresas que desejam gerenciar suas despesas de forma descentralizada (cada empresa filha gerindo suas próprias contas).

## Multi-tenant: Cada Account (empresa mãe/user Owner) poderá cadastrar X empresas filhas (a definir do plano) (implementação parcial).

## Cadastro de fornecedores: Permite incluir fornecedores de produtos ou serviços.

## Fluxo de acesso: O usuário adquire o sistema e acessa via login, tendo acesso à plataforma e aos recursos de gerenciamento financeiro.

## Funcionalidades adicionais:

 - Leitura automática de arquivos XML de notas fiscais, preenchendo formulários e cadastrando fornecedores automaticamente.

 - Possibilidade de integração futura com sistemas de estoque, atualizando entradas e saídas de produtos com base nas notas fiscais.
