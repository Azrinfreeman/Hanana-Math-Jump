# FunMath (Math-Jump-Hanana) — Bug / Polish Implementation Plan

Status: **PLAN ONLY, nothing executed.** Based on a static read of all gameplay/UI/server scripts (not yet play-tested; items marked *(verify)* need a quick in-editor confirmation before fixing).

## 1. Game summary
Endless runner for kids. The player auto-runs along generated land tiles (`SpawnPrefab` spawns the next tile on trigger; `InitPath` registers each tile's waypoints/obstacles). When the player gets within 2 units of the next obstacle, `GameController.ShowQuestion` pauses (`timeScale = 0`) and shows an addition (level 1) or addition/subtraction (level 2) question with 2 answers and a 10 s countdown (`TimeToAnswer`).
- Correct: jump collider on, stars by speed (10 / seconds / 3), +1 round.
- Wrong: crash collider on, trip animation, lose a heart (`HealthController`); 0 hearts shows the end screen.
- Difficulty steps every 20 questions (also swaps tile colour set: green → Blue → Yellow → Pink → Cyan via `enterLevel[]`). After 80 questions `ShowQuestion80` runs without pausing.
- Meta: PlayerPrefs profiles (up to 20), character shop (stars + rounds), ranking and score upload to `app-hanana.com/funmath/*.php`, settings (audio mixer, quality).

## 2. Findings

### P0 — likely to break builds or gameplay
| # | Where | Problem | Fix |
|---|---|---|---|
| 1 | `DisableCameraRotation.cs:4` | `using UnityEditor.Rendering;` in a runtime script. Fails player builds on a clean compile (the IDE auto-added it). | Delete the line. |
| 2 | `SpawnPrefab.OnTriggerEnter` (all 5 colour branches) | If `player.LandType` / `eulerAngles.y` match no branch (e.g. y ≥ 180, `Right` land with y < 80, or the first tile of a new colour set) the code logs "The problem is here" and **spawns nothing, so the track ends**. `while` re-roll loops can also **spin forever** if `prefabList` has no valid tile (main-thread freeze). | Replace the 1,600 lines with one data-driven spawner: choose the allowed tile list by (colour set, current land type, facing), spawn from that list, never loop on random re-rolls. Always spawn a straight fallback. |
| 3 | `GameController.ShowQuestion80` L1187 | Level 2 `+` wrong-answer button wired to `buttonFunction()` (the paused-mode handler) instead of `buttonFunction80()`. It runs `HideQuestion`, so it double-removes the obstacle, resets `timeScale`, bumps `questionCount`. | Fixed automatically by consolidating (see #8); otherwise change to `buttonFunction80`. |
| 4 | `PathController.startJump` / `GameController.collectedRound80` | After question 80, rounds are added with `CollectionController.rounds++` (a non-persisted counter) instead of `addRounds(1)`. Displayed and uploaded rounds come from `roundsTotal`/PlayerPrefs, so **late-game rounds and stars never count**. *(verify; matches the recent "fix bug bila menang" commits)* | Use `addRounds(1)` (and stars if intended) in the ≥80 path. |
| 5 | `GameStartupController.InsertScoreToDB` callers | Score upload runs **only at app start and on name submit**, never after a run. The leaderboard updates only on the next launch. | Call `ApplyScoreAgain()` on game over and when returning to the menu (debounced). |
| 6 | `GameStartupController.SaveNewPlayerName` + `SetAsCurrentPlayer` | When a new player fills a freed slot `a`, `CurrentPlayerNo_` is set to `PlayerTotal - 1`, not `a`. New profile can read/write **another player's stars and rounds**. | `SaveNewPlayerName` returns the slot index; `SetAsCurrentPlayer(slot)` uses it. |
| 7 | `DetailPlayer.TukarPlayer`, `PlayerDeleteController` | Slot number parsed with `Substring(Length - 1)` → `Player_10`..`Player_19` resolve to 0..9. Wrong profile selected or deleted. Delete also crashes (`userCollection[0]`) if no profiles remain. | `int.Parse(key.Substring("Player_".Length))`; guard empty list. |
| 8 | `GameController` (1,459 lines) | `ShowQuestion` / `ShowQuestion80` are ~500-line near-duplicates. Source of #3 and of the distractor-range typos. | Extract `GenerateQuestion()` (pure, unit-testable) + `BindAnswerButtons(onCorrect, onWrong)`. Keep both entry points as thin wrappers. |

### P1 — glitches and robustness
| # | Where | Problem | Fix |
|---|---|---|---|
| 9 | `PathController.Update` L152, L161 | `ObstaclesList[0]` / `TriggerJumpList[0]` read every frame with no guard → `ArgumentOutOfRange` spam if the list is empty (tile not registered yet, or after `RemoveAt`). | Guard with `Count > 0`; skip distance calc otherwise. |
| 10 | `HealthController.getHurt` | `Healths[healthCount-1]` with `healthCount == 0` throws if a second crash trigger fires while dying. | Early-return when `healthCount <= 0`. |
| 11 | `PathController.startTripping` | Can re-enter (several crash colliders / OnTriggerEnter twice) → double heart loss. Also teleports the player +0.4 on the axis (visible pop). | Ignore while `isTripping`; replace the teleport with a short tween or remove it. |
| 12 | `GameController.Start` vs `PathController.Start` | Reads `PathController.instance.TransformAnimList.Count` which is filled in `PathController.Start`. Order is not guaranteed → `MaxCharacters = 0` and wrong unlock init. *(verify)* | Fill the list in `Awake`, or move init to a `PathController` callback. |
| 13 | `GameController.Start` unlock init | Resets all unlock flags whenever `Character_No_0 == 0` (e.g. after any PlayerPrefs edit). Unlocks are global while stars are per-profile. | Use a dedicated `UnlockInitialized` flag. Decide: per-profile unlocks (design question, see §4). |
| 14 | `CharacterSelectionController.characterBuying` | Removes the BELI listener first; on "not enough coins" it never restores it → button dead until the user swipes away and back. Only `Debug.Log` feedback. | Re-add the listener / call `ReassignImageandName()`; show a short "need X coins / Y trophies" shake or toast. |
| 15 | `PlayerDeleteController` / `PlayerResetController` `PlayerDetails` | `btnConfirm.onClick.AddListener` on every `OnEnable` without removing → N opens = N delete/reset calls. Failed delete gives no feedback. | `RemoveAllListeners()` first; show error text on failure. |
| 16 | Network calls (`fetchDelete`, `insertmark`, `fetchRanking`) | Player name concatenated unescaped into URL. Delete sends name only (no id). No timeout. Ranking JSON parse not guarded (null `ranking` crashes). `ConnectionError`/`ProtocolError` only, `DataProcessingError` ignored. | `UnityWebRequest.EscapeURL`, send id, set `www.timeout = 10`, `try/catch` JSON, handle `Result != Success`. |
| 17 | `InitPath.AddNewPath/AddObstacle` | Loop bound uses `GameObject.Find("Paths")` (first active one in scene) but reads from **this** tile; `Find` runs every iteration. Wrong counts if tiles differ, plus a hitch per spawn. | Use `transform.Find("Paths")` once and cache. |
| 18 | `TimeToAnswer` | No timeout handling: at 0 the question stays forever (player can think without limit; stars just stop). | Decide rule (treat as wrong answer? or no stars). If wrong: call the wrong handler at 0. |
| 19 | `TimeRemainingController.Update` | Divides by `moveSpeed` (0 until first frame sets 3) → NaN/∞ on slider for a frame. | Guard `moveSpeed > 0`. |
| 20 | `SoundSettings` | `Log10(0)` = -∞ if slider min is 0; volumes not saved between sessions. | Clamp to `0.0001f`; persist in PlayerPrefs and apply on Start. `QualitySetttingScript` choice also isn't persisted. |
| 21 | `PathController` turning | `transform.forward = towards` each frame snaps rotation on corners. | `Quaternion.RotateTowards`/`Slerp` with a turn rate. |

### P2 — difficulty/balance and content
| # | Where | Problem |
|---|---|---|
| 22 | Level 2 subtraction distractors (`rnd.Next(0,6)`, `(6,11)`, …) | Distractor range is about half of the answer range, so large answers are always the correct one. Kids can answer by size without maths. Make distractors `answer ± 1..3` (clamped ≥ 0) so choices are plausible. |
| 23 | Level 1 `(11,16)`/`(10,16)` and `rnd.Next(10,21)` re-roll | Inconsistent ranges (copy/paste). Falls out of the table-driven rewrite. |
| 24 | Tiers `> 80` same as `60–80`; `enterLevel` switches mid-question | Tile colour change timing depends on when a question is generated. Move to a clean `difficultyTier = questionCount / 20` property. |
| 25 | Wrong answers can repeat the same question pair back-to-back | Avoid identical consecutive question. |
| 26 | Malay/English mix in UI ("TUKAR", "PILIH", "BELI", "DELETE", "Trophy"/"Coins") | Pick one language or add a small localization table. |

### P3 — performance / cleanup
- `CollectionController.Update` calls `PlayerPrefs.GetInt` ×4 and sets two TMP texts **every frame**. Cache values, update only in `addStars/addRounds`.
- `TimeToAnswer.Update` allocates a string and walks 3 `GetChild` calls every frame; `TimeRemainingController` and `ButtonHandling` also do per-frame `GetComponent`. Cache refs, update the text only when the integer second changes.
- `GameObject.Find("collected" / "notification" / "fellSfx" / "awwSfx")` inside gameplay coroutines. Cache `AudioSource` refs once (an `AudioManager`).
- Tiles are `Instantiate`d and only `SetActive(false)` after 16 s (never destroyed; `Points`/`ObstaclesList` also grow unbounded) → memory growth on long runs. Destroy or pool tiles and trim consumed points/obstacles.
- `LandsController.destroyPrevLand` uses scaled `WaitForSeconds(16)`: pausing on questions delays cleanup and the removal index can drift from `countland`. Remove tiles by distance/passed-index instead.
- Huge `Debug.Log` volume in hot paths (each spawn/question). Wrap in `[Conditional("UNITY_EDITOR")]` helper or delete.
- Dead/empty code: empty `Update()`/`Start()` everywhere, commented blocks, `delayStartCode`, `HideQuestion80`, `AddRound`, `objectTrigger` unused? *(verify usage in scenes before deleting)*.
- `PlayerPrefsEditor/` + `SimpleEncryption`: scores/stars are plain PlayerPrefs sent to the server by the client, so they are trivially editable. Optional: server-side sanity limits; not a client fix.
- Tutorial leftovers are already removed in git; `Publish/` holds binaries tracked in the working tree (consider `.gitignore`).

## 3. Proposed implementation phases
Each phase = small, independently testable commit; compile-check via Unity MCP console between steps.

1. **Phase A – Safety fixes (P0 quick wins, ≈ 1 h)**: #1, #9, #10, #11 (re-entry guard), #14, #15, #19, #4, #5. Low risk, no behaviour redesign.
2. **Phase B – Profile data (#6, #7, #13, #16)**: slot index return value, safe slot parsing, empty-list guard, escaped/timeout network helper (`ServerApi` static class, one place for URLs), unlock flag.
3. **Phase C – Question system refactor (#3, #8, #18, #22–25)**: new `QuestionGenerator` (pure C#, tier table → min/max, distractor logic) + thin `GameController` wiring. Add EditMode tests: answers within range, distractor ≠ answer, subtraction ≥ 0, no infinite loops over 10k samples.
4. **Phase D – Tile spawner rewrite (#2, #17, #24, P3 tile cleanup)**: data-driven `SpawnPrefab` with a `(colour, from, facing) → allowed prefabs` table built from prefab names at startup, with straight fallback; tile cleanup by distance. Highest risk, so **play-test 5+ minutes per level/colour** and keep old file until verified.
5. **Phase E – Polish & perf (P3, #20, #21, #26)**: cache refs/audio, per-frame work removal, volume/quality persistence, smooth turning, shop feedback toast, log cleanup.
6. **Phase F – Verification + build**: Windows and Android build (confirm #1 resolved), run 20-question and 80+ question sessions on both levels, check end screen, ranking, profile add/delete/switch with ≥ 11 profiles, bump `bundleVersion`/`AndroidBundleVersionCode`.

## 4. Decisions (confirmed)
1. **Timer hits 0** = wrong answer: lose one heart and play the trip/fall (same path as `incorrectAnswer`). Hook into `TimeToAnswer` at 0 with a one-shot event so it fires once, only while a question is showing.
2. **Character unlocks stay per device.** Only fix the reset condition (#13) with a dedicated `UnlockInitialized` flag; no per-profile migration.
3. **UI language: English only.** Replace Malay strings (TUKAR→SWITCH, PILIH→SELECT, BELI→BUY, "Tak cukup duit" log → in-UI "Need X coins / Y trophies", "*failed..." etc.) via a single `UiText` constants class so wording stays consistent. Scene/prefab text found through MCP is updated in Phase E.
4. **Stars after question 80:** award stars on each correct answer using the same speed-based formula as before 80 (and persist rounds with `addRounds`, see #4). Wrong answers award nothing.
5. **Delete and player id:** the id is already stored on the server (`insertmark.php` receives `Playerid`; ranking returns `id_ingame`). Only `fetchDelete.php` currently gets the name alone. Client will also send `id=CurrentPlayerid_` (escaped). Harmless if the server ignores it; the parameter name is unconfirmed, so check `fetchDelete.php` when convenient.

## 5. Out of scope / risks
- Scene/prefab edits (colliders, tags, UI hierarchy indices like `GetChild(3).GetChild(1)…`) are only touched via Unity MCP; the hard-coded child indices are fragile and are replaced by serialized references in Phase C/E.
- Working tree has unrelated uncommitted scene/package changes; each phase should stage only its own files.
