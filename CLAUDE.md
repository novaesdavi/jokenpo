# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Visão geral

Jogo de Jokenpo (pedra, papel e tesoura) em console, contra a máquina. Projeto .NET 10 (`net10.0`), C# com `Nullable` e `ImplicitUsings` habilitados. A solução usa o formato novo `.slnx`.

## Comandos

```
dotnet build jogoJokenpo.slnx
dotnet run --project jogoJokenpo
dotnet test jogoJokenpo.slnx
```

## Estrutura

Todo o código está em `jogoJokenpo/Program.cs`, usando top-level statements:

- **Loop principal** (topo do arquivo): lê a jogada do usuário (`0`, `1`, `2` ou `q` para sair), sorteia a jogada da máquina e imprime o resultado.
- **`PecasJogo`** (enum `Pedra=0`, `Papel=1`, `Tesoura=2`): a entrada numérica do usuário é convertida direto para esse enum, então a ordem dos valores importa.
- **`OpcoesPlacar`** (`Vitoria`, `Derrota`, `Empate`): resultado do ponto de vista do **usuário**.
- **`RegrasJogo.CalcularResultado(usuario, maquina)`**: classe estática pública com a regra do jogo; retorna o resultado do ponto de vista do usuário e lança `ArgumentOutOfRangeException` para valores fora do enum. É o ponto testável do projeto; o loop principal não é testável diretamente.

## Convenções

- Identificadores e mensagens em português.

# Fluxo de trabalho

Ao concluir qualquer alteração em código (.cs/.csproj):
1. Delegue ao subagente `testador` e garanta que `dotnet test` passa.
2. Delegue ao subagente `code-reviewer` para revisar o diff.
3. Corrija os itens 🔴 Crítico antes de dar a tarefa como concluída.
4. Só então reporte ao usuário, incluindo o resultado do review.