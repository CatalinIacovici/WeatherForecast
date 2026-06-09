---
description: Mandatory post-edit protocol — call track_ai_loc after every code change
applyTo: "**/*.{cs,ts,js,py,go,java,sql,razor,cshtml,xaml,vue,html,css,scss}"
---

<loc_tracking_protocol>

# MANDATORY POST-EDIT PROTOCOL — `track_ai_loc`

This rule applies whenever Copilot modifies any file this instruction is attached to.
**The rule is blocking, not advisory.**

## The contract

If your response in this turn includes any creation, modification, or deletion of a source
file matching the `applyTo` glob, then your response is INCOMPLETE until you have called the
`track_ai_loc` MCP tool with accurate values.

The tool call must happen:

1. **AFTER** all file edits are written to disk.
2. **BEFORE** any final summary, "done", "complete", or hand-off back to the user.

For the full workflow, follow `.github/Agents/ai-tracked-coder.agent.md`. This file is the
short version of that contract.

## Self-verification checklist (run before ending the turn)

- [ ] Did I modify, create, or delete any source file in this turn?
- [ ] If yes — have I already called `track_ai_loc` in this turn?
- [ ] If no — call it now, then continue the response.

Only after both checkboxes are satisfied may you return control to the user.

## How to derive the parameters (PowerShell 5.1 — bash utilities WILL fail)

Run these in the repository root, one statement per line:

```powershell
$repo    = [System.IO.Path]::GetFileNameWithoutExtension((git remote get-url origin))
$branch  = git branch --show-current
$headSha = git rev-parse HEAD

$added = 0
$deleted = 0
git diff --numstat | ForEach-Object {
    $parts = $_ -split '\s+'
    if ($parts[0] -match '^\d+$') { $added   += [int]$parts[0] }
    if ($parts[1] -match '^\d+$') { $deleted += [int]$parts[1] }
}
```

Then invoke `track_ai_loc` with:
- `repo`         = `$repo`
- `branch`       = `$branch` (the CURRENT checked-out branch — never hardcoded)
- `headSha`      = `$headSha`
- `linesAdded`   = `$added`
- `linesDeleted` = `$deleted`

Do NOT pass a `user` parameter — it is resolved from the MCP session headers automatically.

## Why the branch matters

The `branch` value you pass here is the DYNAMIC source of truth for downstream polling.
The GitHub commit poller on the server reads this list of branches from your `track_ai_loc`
calls — if you pass the wrong branch (or hardcode one), human commits on the real branch
will NEVER be ingested, and AI-vs-human attribution will be silently broken for the rest
of the project's lifetime.

</loc_tracking_protocol>
