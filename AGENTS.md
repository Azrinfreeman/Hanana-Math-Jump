# AGENTS.md — Efficient AI Development Rules

Project: Math-Jump-Hanana ("FunMath"), Unity 2022.3.62f3, URP 14, TextMeshPro, uGUI. Math runner game.
Scenes: `Assets/Scenes/SplashScreen.unity`, `MainGame.unity`. Scripts are flat in `Assets/*.cs`. Builds go to `Publish/`.

**Priority:** Correctness → Safety → Task Completion → Maintainability → Efficiency → Token Conservation → Concise Communication. Token conservation never overrides correctness, safety, or essential verification.

## 1. Core Principles
- Complete the user's task with minimal unnecessary tokens, tool calls, and analysis.
- Prefer simple, effective solutions. Keep code quality, reliability, security, readability.
- Never omit critical warnings, assumptions, errors, or details merely to save output.

## 2. Token Efficiency
- Be concise; don't restate the request, repeat established info, or explain obvious concepts.
- No unnecessary intros, conclusions, or status reports. Don't output whole files when a brief change summary suffices.
- Read only relevant sections of large files; prefer targeted searches over reading directories.
- Reuse existing context and tool output; don't re-read unchanged files or regenerate results.
- Keep notes/handoffs compact but sufficient.

## 3. Task Execution
Before starting: understand the objective; check whether the functionality already exists; pick the simplest reliable approach; reuse existing code, utilities, dependencies, patterns. Plan only if the task is complex.

During:
- Batch independent operations; keep dependent ones sequential.
- Targeted edits over rewriting files; no unnecessary project-wide refactors or unrelated changes.
- Use existing tooling; automate only genuinely repetitive work.
- Decide independently when requirements are clear; ask only when missing info affects correctness or safety.
- Fix root causes, not temporary patches.

## 4. Avoid Repetitive Work
- Don't repeat completed work, identical searches/commands with still-valid results, or redundant research.
- Check existing implementation and docs before writing new ones. Don't install dependencies already available.
- After a failure, analyze before retrying; never retry the same failed solution unchanged. Stop and report after two identical failures.
- Record important discoveries in longer tasks to avoid re-investigating.

## 5. Tool Usage
- Use the most direct tool, only when it contributes. Find files with focused search (Grep/Glob) first, then read the needed line range.
- Check existing errors/logs before new investigations.
- Don't run expensive builds or full test suites unnecessarily: focused tests first, broader when required. Never skip essential testing/security checks to save tokens.
- Prefer summarized output over large dumps. Don't spawn subagents for lookups a single search can do.

## 6. Communication
- Answer/result first; short, clear, technically accurate; minimal jargon, filler, and compliments.
- Briefly explain important decisions; mention risks, tradeoffs, breaking changes, limitations.
- Step-by-step only when it helps the user act; deeper explanations on request.

## 7. UI/UX Design and Microcopy
- Clean, intuitive, polished, functional. Layout, hierarchy, spacing, and interaction should explain themselves; every element needs a clear purpose.
- Omit helper text, subtitles, captions, tooltips, badges, and status text that duplicate context or only fill space. Short, action-oriented labels.
- Keep labels/instructions/feedback needed for accessibility, safety, or understanding.
- Use spacing, alignment, grouping, typography, contrast; avoid redundant containers and clutter; stay consistent; support different screen sizes.
- Common actions easy to find; avoid extra clicks and confirmations for reversible low-risk actions; give success/failure feedback; errors concise, specific, actionable; progressive disclosure for advanced items.
- Preserve accessible names, keyboard navigation, screen-reader support.

Before adding any label, description, component, or decoration ask: Is it info users need? Is it already obvious from context? Would removing it hurt usability/accessibility? Can clearer layout do the same? If it adds no value, omit it.

## 8. Coding Standards
- Clean, maintainable, efficient; follow existing architecture and conventions; match neighbouring style.
- Reuse components/functions; avoid duplicated logic, overengineering, new dependencies.
- Handle relevant errors and edge cases; preserve existing functionality; no unrelated changes.
- Comments meaningful only; none for self-evident code.

## 9. Debugging
1. Inspect error messages and context. 2. Identify likely root cause. 3. Verify with focused investigation. 4. Apply the smallest reliable fix. 5. Test the affected functionality. 6. Expand only if needed.
- No random trial-and-error; don't re-suggest failed fixes; don't touch unrelated systems.
- Distinguish confirmed causes from hypotheses; don't claim resolved without verification.

## 10. Planning
- Simple tasks: execute directly. Complex tasks: short actionable plan, logical steps, dependencies identified; reuse and update only on material change; continue autonomously when next steps are clear.

## 11. Multiple Agents
- One agent unless delegation is requested or required. No duplicate assignments; delegate by expertise; clear responsibilities.
- Concise handoffs: done, pending, decisions, blockers. Share findings instead of repeating research; avoid whole-project reads per agent; no conflicting edits to the same files.

## 12. Context Management
- Keep summaries to decisions, constraints, progress, unresolved issues. Prefer existing project notes over rediscovery.
- Preserve critical details and next steps when context is limited. Don't assume prior-session info; don't claim to have saved what wasn't.

## 13. Testing and Verification
- Smallest appropriate scope; widen for multi-system or critical changes. Don't rerun unchanged tests.
- Never claim tests passed unless executed; state when verification couldn't be completed; don't hide failures.

## 14. Never
- Rewrite working code without justification; generate unnecessary boilerplate/docs; introduce redundant dependencies; make unverified claims; use many agents/tools where one suffices; drop safety/correctness checks to save tokens.

## 15. Completion Format
Brief, omit empty sections:
**Completed:** what was done. **Verification:** checks actually run. **Notes:** limitations/risks/remaining actions, only if relevant.

## 16. Unity Rules

Scope and discovery:
- Follow the latest request. Planning-only = Markdown plus read-only inspection; implement only after approval (approval persists).
- State target feature/scenes, likely files, and acceptance checks before editing.
- Start with files already identified; otherwise consult `Docs/AgentWorkflow/ProjectMap.md` (relevant row only) if it exists. Other docs are optional references.
- Search progressively: exact file → feature files → owning folder → direct dependencies; expand only after a miss, conflicting evidence, or a shared dependency, and say why.
- Prefer `rg -n -C 3` on explicit files and `rg --files <folder> -g '<pattern>'` (PowerShell: wildcard filenames in `-g`). Don't read whole large scripts when one method suffices.
- Don't scan repo root, all Assets, all scenes, or all docs by default. Always exclude `Library/`, `Temp/`, `Logs/`, `obj/`, `Publish/`, `output/`, `.git`, `*.meta`, generated files, and heavy asset folders (`Assets/TextMesh Pro`, `UI`, `Abandoned World`, `Models`, `Music`, `SFX`, `fonts`) unless directly relevant.
- Re-read files only after relevant edits, branch changes, or staleness. Reconfirm live editor state; never reuse instance IDs blindly.

Unity MCP and output:
- Use the `unity-mcp-skill` once per conversation. Check editor readiness before editor-dependent actions and after reloads; read custom tools once per connection or capability change. Doc-only work needs no Unity connection.
- Files for source searches; MCP for live scene/component questions, starting with exact scene/object paths and required fields (include inactive children when relevant).
- Never open `.unity`, `.prefab`, `.asset`, `.mat`, `.anim` YAML directly; use MCP. Only if MCP is unavailable, use targeted `Grep -n` plus small `Read` ranges.
- Default first queries: 20 hierarchy rows, 5 console entries, 20–30 search matches (a starting target, not a correctness cap). Keep errors, truncation, cursors; page further when coverage needs it.
- One tool representation per result. No duplicate dumps, full scene YAML, full tool catalogs, base64, or entire repo status. Store genuinely needed large evidence under `output/<task>` and report summary/path.
- Batch independent reads; keep mutations, compilation, scene transitions, and dependent ops sequential. Check every batch result. No repeated polling, refreshes, or captures of unchanged state.
- Read `Docs/AgentWorkflow/UnityMcpWorkflow.md` (helpers, capture, recovery) and `Docs/AgentWorkflow/UIDesign.md` (visual changes) only if they exist and are needed.

Preserve and verify:
- Inspect runtime writers, serialized overrides, animations, and shared dependencies before changing. Preserve dirty scenes, unrelated edits, event targets, and existing behaviour.
- Don't replace shared fonts/materials globally for a local change. Never write PlayerPrefs, reset progress, or unlock content merely to test appearance.
- Make a coherent edit batch. Refresh only if imports require it; wait for compilation and check console errors via MCP before using new types (don't build the player unless asked). Doc edits need only readback/link checks, no Play mode.
- Verify by risk: UI → rendered review and relevant interactions; runtime → focused behaviour checks; wiring → targets and signatures.
- Capture only a ready view; check capture status and file existence before reading an image. Avoid the `EditorApplication.Step` recursion path.
- Restore transient state. Don't save an unrelated dirty scene. Don't change global permissions, Git trust, models, packages, or credentials for efficiency.
- Report changed files, outcome, checks, and real limitations. Never claim exact token savings without measured data.

Unity/C# conventions:
- Cache component refs in `Awake`/`Start`; no `Find*`/`GetComponent` in `Update`. Avoid per-frame allocations (string concat, LINQ, `new`). Use `TMP_Text` for UI text.
- Prefer inspector-assigned references; pool spawned objects (see `SpawnPrefab`, `ObstaclesController`) instead of repeated Instantiate/Destroy.
- Persisted data uses `PlayerPrefs` (see `PlayerPrefsEditor`, `PlayerResetController`); grep existing keys before adding one.
- Keep `.meta` files with assets; don't delete/rename assets outside the Editor/MCP (breaks GUIDs).
- Don't rename serialized fields on existing components; if unavoidable use `[FormerlySerializedAs]`.

## 17. Git
- Never commit/push unless asked. The working tree has many unrelated pending changes (scenes, packages, `Publish/`); stage only files you touched. Don't touch `Publish/` or `ProjectSettings/` unless the task requires it.

**Final instruction:** Be resourceful. Minimize unnecessary work and output, preserve essential context, and deliver reliable results with the least reasonable effort.
