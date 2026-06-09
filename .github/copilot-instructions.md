- @azure Rule - Use Azure Tools - When handling requests related to Azure, always use your tools.
- @azure Rule - Use Azure Best Practices - When handling requests related to Azure, always invoke your `azmcp_bestpractices_get` tool first.
- @azure Rule - Enable Best Practices - If you do not have an `azmcp_bestpractices_get` tool ask the user to enable it.

## LOC Tracking — MANDATORY POST-EDIT PROTOCOL

> **This is not a suggestion. This is a blocking rule with the same priority as not breaking the build.**

### Visual Studio note — auto-apply this agent for ALL code generation

The user is in **Visual Studio**, where the agent dropdown is NOT available. You will NOT be
invoked explicitly as `@AI-Tracked-Coder` — but you MUST behave as that agent for every
code-generation turn anyway. Treat this as the default persona for any task that creates,
modifies, or deletes source code.

The full agent definition lives at:

> **`.github/Agents/ai-tracked-coder.agent.md`** (in this repository root)

BEFORE generating any code, load that file into context and follow its strict 6-phase
workflow exactly. The phases are non-negotiable; in particular Phase 5 (the `track_ai_loc`
MCP call) is a hard gate that must complete with `OK` before you write any summary or
say "done" to the user.

If `.github/Agents/ai-tracked-coder.agent.md` is missing or unreadable, surface that fact
to the user and stop — do not proceed with generation, because the tracking contract cannot
be honoured.

### Quick rule (the contract in one paragraph)

Every Copilot turn that creates, modifies, or deletes source code MUST end with a call to the
`track_ai_loc` MCP tool. The tool call is the **last action** in the response — it happens
**before** you write "done", "complete", or any final summary back to the user.

### Environment

The developer runs **Windows PowerShell 5.1**. Do NOT use bash-only utilities like
`awk`, `sed`, `basename`, or `&&` chaining — they will fail. Use only PowerShell-native commands
and `git`. Run each command separately, not chained.

### How to derive the parameters (one command per line)

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

Then call the MCP tool with:
- `repo`         = `$repo`
- `branch`       = `$branch`     (whatever branch is currently checked out — never hardcoded)
- `headSha`      = `$headSha`
- `linesAdded`   = `$added`
- `linesDeleted` = `$deleted`

### Rules

- Do NOT pass a `user` parameter — it is resolved automatically from the MCP session headers.
- Skip the call when `linesAdded=0` AND `linesDeleted=0` (no source changes).
- The regex guard `^\d+$` is REQUIRED — git diff shows `-` for binary files, which would crash
  `[int]` conversion. Skip those rows silently.
- The `branch` parameter is what the GitHub poller uses to discover which branches to scan
  for human commits — getting it right is critical for AI-vs-human attribution.
