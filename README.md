# jokenpo

Jogo de Jokenpo (pedra, papel e tesoura) em console, contra a máquina. .NET 10.

```
dotnet build jogoJokenpo.slnx
dotnet run --project jogoJokenpo
dotnet test jogoJokenpo.slnx
```

## Desenvolvendo com Claude Code

O repositório já vem configurado para o [Claude Code](https://claude.com/claude-code). Depois de um `git pull`, os agentes e o hook abaixo ficam disponíveis automaticamente ao abrir o Claude Code na raiz do projeto.

### Agentes disponíveis

Ficam em `.claude/agents/`:

| Agente | O que faz | Pode editar? |
|---|---|---|
| `testador` | Roda o build, escreve testes xUnit em `jogoJokenpo.Tests/` para o código alterado e executa `dotnet test`. Deriva o resultado esperado das regras oficiais do jokenpo, então um teste falhando indica bug no código. | Só `jogoJokenpo.Tests/`, nunca código de produção |
| `code-reviewer` | Revisa **apenas o que mudou** (`git diff HEAD` + arquivos novos) com o checklist do time e boas práticas de console .NET. Devolve um relatório com severidades 🔴 Crítico, 🟡 Atenção e 🟢 Sugestões, citando `arquivo:linha`. | Não, é somente leitura |

### Fluxo obrigatório de review

Toda alteração em código (`.cs`/`.csproj`) passa por:

1. **testador**: `dotnet test` precisa passar.
2. **code-reviewer**: revisa o diff.
3. **Correção** de todos os itens 🔴 Crítico.
4. **Relatório** ao desenvolvedor, incluindo o resultado do review.

Duas camadas garantem isso:

- **`CLAUDE.md`** (seção *Fluxo de trabalho*): instrui o Claude a seguir os passos acima.
- **Hook `Stop`** (`.claude/settings.json` → `.claude/hooks/review-gate.sh`): quando o Claude tenta encerrar a resposta com código alterado ainda não revisado, o hook bloqueia e manda rodar o fluxo. Ele guarda o hash do diff revisado em `.claude/.last-reviewed-diff` (ignorado pelo git), então só bloqueia de novo quando o código mudar.

O hook roda com `bash`. No Windows, use o Git Bash (vem com o Git for Windows).

### Invocando o reviewer manualmente

No Claude Code, peça em linguagem natural:

```
use o code-reviewer para revisar minhas alterações
```

O mesmo vale para os testes:

```
use o testador para testar minhas alterações
```

Para listar ou editar os agentes, use `/agents`. Para conferir o hook registrado, use `/hooks`.
