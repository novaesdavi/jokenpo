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
