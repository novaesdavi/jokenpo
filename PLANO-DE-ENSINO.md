# Plano de ensino: CLAUDE.md, subagentes e code review automático

Projeto: **jogoJokenpo** (.NET 10, console app, `jogoJokenpo/Program.cs`)

## Como usar este plano

1. Faça os passos **na ordem**. Cada passo tem um **✅ Critério de conclusão**.
2. Ao terminar um passo, me diga: **"concluí o passo X.Y"**.
3. Eu confiro os arquivos/comandos e marco `[x]` aqui. Se faltar algo, explico o que ajustar.

> Dica: escreva os arquivos você mesmo. Os modelos abaixo são pontos de partida, não algo para copiar sem ler.

---

## Progresso

| Módulo | Tema | Status |
|---|---|---|
| 0 | Conceitos | ✅ |
| 1 | CLAUDE.md | ✅ |
| 2 | Projeto de testes (pré-requisito) | ✅ |
| 3 | Subagente testador | 🟡 |
| 4 | Subagente de code review (time) | ⬜ |
| 5 | Code review automático após alterações | ⬜ |
| 6 | Validação final e entrega para o time | ⬜ |

---

## Módulo 0 — Conceitos (leitura, ~10 min)

| Peça | O que é | Onde fica | Vai pro git? |
|---|---|---|---|
| `CLAUDE.md` | Memória do projeto: instruções que o Claude lê **sempre** ao iniciar uma sessão nesta pasta | raiz do repo | Sim (compartilhado com o time) |
| `CLAUDE.local.md` | Igual ao anterior, mas pessoal | raiz do repo | Não (coloque no `.gitignore`) |
| Subagente | Um "especialista" com prompt próprio, ferramentas restritas e **contexto separado**. O Claude principal delega tarefas a ele | `.claude/agents/<nome>.md` (projeto) ou `~/.claude/agents/` (só você) | Sim, se estiver em `.claude/agents/` |
| Hook | Comando que o **harness** executa em eventos (ex.: quando o Claude termina de responder). Diferente do CLAUDE.md, não depende do modelo "lembrar" | `.claude/settings.json` | Sim |

Ideia-chave: **CLAUDE.md = pedido** (o modelo normalmente segue), **hook = garantia** (o sistema força).

- [x] **0.1** Ler a tabela acima e me explicar com suas palavras a diferença entre colocar uma regra no `CLAUDE.md` e num hook.
  - ✅ Critério: explicação correta no chat.

---

## Módulo 1 — CLAUDE.md

- [x] **1.1** Rode `/init` no Claude Code e leia o `CLAUDE.md` gerado.
  - ✅ Critério: `CLAUDE.md` existe na raiz.

- [x] **1.2** Edite o `CLAUDE.md` para conter **no mínimo** estas seções:
  - **Visão geral** — o que é o projeto, em 2–3 linhas.
  - **Comandos** — build, run e test:
    ```
    dotnet build jogoJokenpo.slnx
    dotnet run --project jogoJokenpo
    dotnet test jogoJokenpo.slnx
    ```
  - **Estrutura** — onde ficam regras do jogo (`RegrasJogo`), enums (`PecasJogo`, `OpcoesPlacar`) e o loop principal.
  - **Convenções** — nomes em português, `Nullable` habilitado, top-level statements etc.
  - **Fluxo de trabalho** — deixe um título vazio por enquanto; preencheremos no Módulo 5.
  - ✅ Critério: as 5 seções existem e os comandos estão corretos. Arquivo curto e objetivo (idealmente < 60 linhas — tudo nele consome contexto em toda sessão).

- [x] **1.3** Abra uma sessão nova do Claude e pergunte "como eu rodo os testes deste projeto?". Ele deve responder usando o que está no CLAUDE.md.
  - ✅ Critério: resposta coerente com o arquivo.

- [x] **1.4** Commit: `git add CLAUDE.md && git commit -m "docs: adiciona CLAUDE.md"`. Aproveite e adicione `.vs/` e `CLAUDE.local.md` ao `.gitignore`.
  - ✅ Critério: commit existe e `.vs/` não aparece mais no `git status`.

---

## Módulo 2 — Projeto de testes (pré-requisito para o testador)

Hoje o projeto não tem testes. O subagente testador precisa de um lugar para escrevê-los.

- [x] **2.1** Crie o projeto xUnit e ligue à solução:
  ```
  dotnet new xunit -n jogoJokenpo.Tests -o jogoJokenpo.Tests
  dotnet sln jogoJokenpo.slnx add jogoJokenpo.Tests/jogoJokenpo.Tests.csproj
  dotnet add jogoJokenpo.Tests reference jogoJokenpo/jogoJokenpo.csproj
  ```
  - ✅ Critério: `dotnet test jogoJokenpo.slnx` roda (o teste de exemplo passa).

- [x] **2.2** Escreva **um** teste à mão em `jogoJokenpo.Tests`, por ex.: `Pedra` vs `Tesoura` → `Vitoria`, chamando `RegrasJogo.ValidarJogoMaquinaVenceu`.
  - ✅ Critério: o teste aparece e passa no `dotnet test`.

---

## Módulo 3 — Subagente testador

- [x] **3.1** Crie `.claude/agents/testador.md` (crie o arquivo manualmente ou peça ao Claude: "crie um subagente testador que..."; o wizard `/agents` foi removido). Estrutura:

  ```markdown
  ---
  name: testador
  description: Escreve e executa testes xUnit para o jogoJokenpo. Use sempre que uma alteração de código for concluída ou quando o usuário pedir para testar.
  tools: Read, Grep, Glob, Edit, Write, Bash
  model: sonnet
  ---

  Você é o testador do projeto jogoJokenpo (.NET 10, xUnit).

  (escreva aqui, com suas palavras:)
  - O que fazer: rodar build, identificar código alterado, cobrir RegrasJogo com testes ...
  - Regras: NÃO alterar código de produção (jogoJokenpo/), só jogoJokenpo.Tests/ ...
  - Como reportar: lista de testes criados, resultado do dotnet test, falhas com causa provável ...
  ```

  Pontos para entender:
  - `description` é o que o Claude principal lê para **decidir quando** delegar. Seja específico.
  - `tools` restringe o que o agente pode fazer (princípio do menor privilégio).
  - `model` é opcional (`sonnet`, `opus`, `haiku` ou omitir para herdar).
  - ✅ Critério: arquivo com frontmatter válido, prompt com as 3 partes (o que fazer / regras / formato do relatório).

- [x] **3.2** Invoque: *"use o subagente testador para testar todas as combinações de RegrasJogo"*.
  - ✅ Critério: o agente cria testes para as **9 combinações** e reporta o resultado.
  - 🔍 Desafio: **o código atual tem bugs**. Um bom testador deve encontrar pelo menos 2 (um nas regras, outro na escolha da máquina). **Não corrija ainda** — vamos usá-los no Módulo 5.

- [x] **3.3** Me diga quais bugs o testador encontrou.
  - ✅ Critério: bugs listados corretamente.

---

## Módulo 4 — Subagente de code review (para o time)

- [x] **4.1** Crie `.claude/agents/code-reviewer.md`. Diferenças importantes em relação ao testador:
  - **Somente leitura**: `tools: Read, Grep, Glob, Bash` (Bash apenas para `git diff`/`git log`/`dotnet build` — deixe isso explícito no prompt).
  - Escopo: revisar **apenas o que mudou** (`git diff HEAD` + arquivos novos não rastreados).
  - Checklist do time (adapte): corretude, casos de borda, nomes em português consistentes, tratamento de entrada inválida, testes cobrindo a mudança, código morto/duplicado.
  - Formato de saída padronizado, por severidade:
    ```
    ## Resultado: APROVADO | APROVADO COM RESSALVAS | REPROVADO
    ### 🔴 Crítico   (arquivo:linha — problema — sugestão)
    ### 🟡 Atenção
    ### 🟢 Sugestões
    ```
  - ✅ Critério: frontmatter válido, ferramentas sem `Edit`/`Write`, checklist e formato de saída definidos.

- [x] **4.2** Faça uma alteração pequena qualquer em `Program.cs` (sem commitar) e invoque: *"use o code-reviewer para revisar minhas alterações"*.
  - ✅ Critério: relatório no formato definido, citando arquivo:linha.

- [x] **4.3** Commit de `.claude/agents/` para o time receber os agentes ao dar `git pull`.
  - ✅ Critério: `git ls-files .claude/agents` lista os dois arquivos.

---

## Módulo 5 — Code review automático ao concluir alterações

Requisito do projeto: **toda conclusão de alteração de código dispara o code review**. Vamos usar as duas camadas.

### Camada 1 — Regra no CLAUDE.md (pedido)

- [x] **5.1** Preencha a seção **Fluxo de trabalho** do `CLAUDE.md`, por exemplo:
  ```markdown
  ## Fluxo de trabalho (obrigatório)
  Ao concluir qualquer alteração em código (.cs/.csproj):
  1. Delegue ao subagente `testador` e garanta que `dotnet test` passa.
  2. Delegue ao subagente `code-reviewer` para revisar o diff.
  3. Corrija os itens 🔴 Crítico antes de dar a tarefa como concluída.
  4. Só então reporte ao usuário, incluindo o resultado do review.
  ```
  - ✅ Critério: seção preenchida com testador → reviewer → correção → relatório.

### Camada 2 — Hook `Stop` (garantia)

O evento `Stop` roda quando o Claude termina de responder. Se o hook devolver `"decision": "block"`, o Claude **não pode parar** e recebe o `reason` como instrução.

- [x] **5.2** Crie `.claude/hooks/review-gate.sh`:
  ```bash
  #!/usr/bin/env bash
  # Bloqueia o fim da resposta se há código alterado ainda não revisado.
  input=$(cat)

  # Evita loop infinito: se já estamos continuando por causa deste hook, libera.
  echo "$input" | grep -q '"stop_hook_active": *true' && exit 0

  cd "$CLAUDE_PROJECT_DIR" || exit 0
  diff=$(git diff HEAD -- '*.cs' '*.csproj'; git ls-files --others --exclude-standard -- '*.cs' '*.csproj')
  [ -z "$diff" ] && exit 0

  # Só bloqueia se este diff ainda não foi revisado.
  hash=$(printf '%s' "$diff" | sha1sum | cut -d' ' -f1)
  marker=".claude/.last-reviewed-diff"
  [ -f "$marker" ] && [ "$(cat "$marker")" = "$hash" ] && exit 0
  echo "$hash" > "$marker"

  echo '{"decision":"block","reason":"Há alterações de código não revisadas. Rode o subagente testador e depois o code-reviewer, corrija os itens críticos e só então finalize."}'
  ```
  E adicione `.claude/.last-reviewed-diff` ao `.gitignore`.
  - ✅ Critério: script existe e o marcador está ignorado pelo git.
  - 🧠 Entenda: por que precisamos de `stop_hook_active` **e** do hash? (me responda no chat)

- [x] **5.3** Registre o hook em `.claude/settings.json` (compartilhado com o time):
  ```json
  {
    "hooks": {
      "Stop": [
        {
          "hooks": [
            { "type": "command", "command": "bash \"$CLAUDE_PROJECT_DIR/.claude/hooks/review-gate.sh\"" }
          ]
        }
      ]
    }
  }
  ```
  Depois rode `/hooks` no Claude Code para confirmar que ele aparece.
  - ✅ Critério: `/hooks` mostra o hook `Stop`.

- [x] **5.4** Teste de ponta a ponta: peça *"corrija os bugs que o testador encontrou"*.
  - ✅ Critério: sem você pedir, o Claude roda testador → code-reviewer → corrige críticos → reporta. Todos os testes passam.

---

## Módulo 6 — Validação final e entrega para o time

- [x] **6.1** Commit de `CLAUDE.md`, `.claude/agents/`, `.claude/hooks/`, `.claude/settings.json`, `.gitignore` e das correções.
  - ✅ Critério: `git status` limpo.

- [x] **6.2** Atualize o `README.md` com uma seção "Desenvolvendo com Claude Code" explicando ao time: os agentes disponíveis, o fluxo obrigatório de review e como invocar o reviewer manualmente.
  - ✅ Critério: README atualizado.

- [ ] **6.3** Reflexão final (no chat): o que você mudaria no prompt do code-reviewer depois de vê-lo funcionando?
  - ✅ Critério: resposta no chat. 🎉

---

## Referências

- Memória/CLAUDE.md: https://docs.claude.com/en/docs/claude-code/memory
- Subagentes: https://docs.claude.com/en/docs/claude-code/sub-agents
- Hooks: https://docs.claude.com/en/docs/claude-code/hooks
