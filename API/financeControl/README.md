# House Stock Backend

Backend para um sistema de organização doméstica que ajuda usuários a gerenciar itens, localizar objetos subutilizados e planejar tarefas domésticas. Futuramente será expandido para funcionalidades como todo-lists e listas de compras.

## ✨ Funcionalidades Atuais

- **Gestão de Itens Domésticos**
  - Cadastro de itens com nome, descrição, localização e categoria (tags)
  - Cadastro de containers (itens que armazenam itens) com as mesmas especificações de itens
- **Sistema de Busca Inteligente**
  - Busca hierarquizada (Place => Container => Container => Container => Item)
  - Busca por categoria (roupas, eletrônicos, alimentos, etc.)
- **Gestão de Inventário**
  - Controle de quantidades
  - Status de disponibilidade

## 🚀 Roadmap (Próximas Funcionalidades)

- [ ] Sistema de tarefas domésticas (Todo-lists)
- [ ] Listas de compras inteligentes
- [ ] Autenticação de usuários
- [ ] Módulo de estatísticas de uso
- [ ] Sistema de alertas para itens prestes a vencer
- [ ] API para integração com aplicativos móveis

## ⚙️ Tecnologias Utilizadas

- **Core**: .NET 9
- **Persistência**: Entity Framework Core + SQL Server
- **Autenticação**: JWT (futura implementação)
- **Containerização**: Docker + Docker Compose
- **CI/CD**: GitHub Actions (Docker Build & Push)

## 🐳 Docker Deployment

O projeto está totalmente containerizado e pode ser instalado e executado rapidamente usando Docker:

```bash
docker-compose pull
docker-compose up -d
```
