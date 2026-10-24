# FunMath — UI / Layout Polish Plan

Status: **PLAN ONLY, nothing executed.** Based on a dump of `MainGame.unity` plus **26 Game-view screenshots** (13 screens x portrait 1080x2400 and landscape 1920x1080) in `output/ui-shots/`, captured by the editor tool `UiScreenshotRunner` (menu: Tools > FunMath > Capture UI Screenshots; or create `output/ui-shots/RUN` containing `1080x2400;1920x1080` while the editor is open). Phase 0 is therefore done. Note: in the captures the title "FUN MATH JUMP" and the main HUD show behind every panel because the tool only toggles panels; in real play flow the title is hidden, so those overlaps are **not** counted as bugs.

Design rules applied (from `AGENTS.md` section 7): let layout and hierarchy explain things, remove redundant text, keep short action labels, give feedback on actions, keep accessibility and touch size.

## 1. What the scene data shows

### Typography
| Finding | Evidence |
|---|---|
| No fixed type scale | 78 TMP texts, auto-size **off** on all; ~15 distinct sizes (54.13, 56.8, 60.76, 70, 75.66, 87.9, 92.8, 93.92, 114.76, 150.6, 177.7 ...), many fractional |
| Three fonts in use | 40 texts on one font asset, 23 on another, **15 on a third** (project fonts are only Arbutus and Wellfleet, so the third is probably TMP's default Liberation Sans) |
| Leftover placeholder text in the scene | "New Text" x2, "ASFASFASASFSFA SFAS" x2, "Option A", "Button", "Timmy", "X", "11", "1000000", "4 second", "1 + 1" (some inactive, some may show) |

### Buttons and touch targets
- Menu buttons (Play/Store/Score) are all 1187 wide but **195 / 209 / 218 / 245 high**: uneven stack.
- Close/exit buttons are three sizes (144.5, 144.6, 184). The 144-unit ones are about 31 dp on a typical phone, **below the 48 dp touch guideline**; the credit scrollbar is 11.7 units wide (about 7 px).
- All 51 Buttons use plain colour-tint; only some have a click animator. No click sound anywhere in the code.
- The credits button shows a bare letter "C" instead of an icon.

### Panels
- Panel sizes differ with no reason: Store 1498x1960, Scoreboard/Credit 1498x2467, Player select 1498x1558, Question 1797x1565. Scrollbar widths differ too (66, 48, 12).
- Question panel has **4 answer slots in two rows but only 2 are ever used**; the unused row still takes layout space, pushing the timer ring down. Timer ring (310) is large relative to the answer buttons (412x327).
- Several screens just toggle `SetActive`; only Player select, Question, Prompt and Lock have animation. Panel open/close feels inconsistent.
- Quality dropdown ("GAME QUALITY") sits in the top-right HUD group, not in Settings. `version` label is a 200x50 box in the bottom-right corner.
- Object names are placeholders ("PlayButton (2)" used for answer buttons, "Text (TMP) (1)", "Image (1)"). Several scripts depend on **child index** (`GetChild(3).GetChild(1)...`), so renaming or reordering is unsafe without migrating those first.

### Feedback gaps (behaviour, not just look)
- Answering gives no visible result before the panel disappears: no correct/wrong highlight, no "+10" star popup (stars are awarded invisibly).
- Timer ring has one colour for all 10 seconds; no urgency at the end.
- Losing a heart just hides the heart (`SetActive(false)`).
- End screen shows "YOU OUT OF LIVES" and buttons, but **not how the run went** (stars, rounds, questions answered).
- Scoreboard: blank while loading, blank on network failure, always starts at the top even if you are rank 40; own row is highlighted (purple) but not scrolled to.
- Name entry: empty name is silently ignored, no length limit, no trimming; the success message "*Valid Name" is the only message.
- Buying a character: only a red flash on the missing requirement (added earlier); no confirm or purchase success feedback besides the prompt.
- `GameController.Update` rewrites "LEVEL n" **every frame** (string allocation + TMP rebuild).

### Colour
Palette is consistent in spirit (white, purple 0.53/0.18/0.96, cyan 0/0.85/1, blue 0.22/0.71/0.96, red 1/0.32/0.32) but scripts use raw `Color.green/red/yellow`, so feedback colours differ from the art.

### 1b. What the screenshots confirm or add (real issues only)
| Screen | Finding |
|---|---|
| All | Fonts mixed: titles and buttons use the chunky Wellfleet/Arbutus look, but HUD numbers, dropdown, settings labels, player name pill, scoreboard/credit text, "QUIT GAME", "RESTART", "PLAYER NAME" use plain sans (Liberation). Looks like two different games |
| All | "GAME QUALITY" dropdown is a default white Unity control, clashing with the art; **stays in the HUD** (your decision) so it only gets restyled |
| All | Bottom-left/right icons are inconsistent: eye and avatar are illustrated, **"C" is a grey letter button**, gear is dark red |
| HUD | Counter plates are flat dark rectangles; numbers in sans; fine otherwise (hearts and coins read well) |
| Menu | Looks good (green/blue/purple/red stack), but button heights differ slightly and "SCOREBOARD" text crowds its trophy icon; "EXIT" has no icon unlike the others |
| Settings | Sliders are the **default red-fill Unity slider with no visible handle** (hard to drag), labels EFFECT/MUSIC overlap the bottom of the bars; two near-duplicate objects exist (`Settings`, `Settings (1)`) |
| Level select | Clear and attractive; buttons sit close to screen edges in portrait |
| Store | Good wood-frame look. SELECT in its disabled state is a washed-out teal with low contrast; "0 Coins / 0 Trophy" text is small, tiny in landscape; name "Amar" small |
| Scoreboard | Rows are flat green rectangles with **no rounded corners**, run **under the scrollbar** (right end hidden), rank "10." is clipped on the left; **missing-glyph boxes** in names ("Rina(□•ᴗ•□)□"); panel top overlaps the trophy counter; in landscape BACK touches the name pill. Order looks non-descending (1997, 2009, 2065 at ranks 8-10), likely server-side |
| Players | Large empty area; flat green row; header icons "-" and "+" are cryptic (reset vs add) |
| Credit | Dense default-font text, almost invisible 5 px scrollbar, panel bottom covers the eye icon |
| Name entry | "PLAYER NAME" header text small and low contrast (white on light blue); placeholder low contrast grey on grey |
| Question | In portrait the answer buttons are **flush with the screen edges** (panel is wider than the canvas); four slots show in the capture but only two are used; timer tile "09" small and low in the layout; in landscape the buttons are small (about 130x100 px at 1080p) |
| Time remaining | "TIME REMAINING" tab and bar fine, but text "4 second" is sans overlay on the bar |
| End screen | Single RESTART button in sans font, no stats (title overlap is a capture artefact) |
| Unlock prompt | Yellow card with black Wellfleet text reads well; no visible close affordance (auto-dismiss) |
| Landscape in general | Portrait-shaped cards (store, scoreboard, credit) use about a quarter of the width, so text is small on tablets/boards |

## 2. Proposed changes (grouped, with priority)

### P1 — Consistency and touch (low risk, high visible value)
1. **Type scale:** define 5 sizes (Display 150, Title 96, Heading 72, Body 56, Caption 40) and map every TMP text to the nearest; fixes the fractional sizes. All text on the two project fonts (replace the third font). Done with an editor tool that lists the change before applying.
2. **Remove placeholder text** and dead objects after confirming they are unused (references checked first).
3. **Menu stack:** same height for Play/Store/Score, even spacing, one layout group.
4. **Touch targets:** every interactive element at least about 56 dp (about 190 canvas units): enlarge exit/back/icon buttons or add transparent padding to the hit area; widen scrollbars to about 48 units.
5. **Panel shell:** one size/margin/header/close-button position for Store, Scoreboard, Credit, Player select (scrollable content inside). Same scrollbar style.
6. **Question panel:** drop the unused second answer row and its spacing; answer buttons side by side, larger; timer ring moved to the top of the panel or the HUD so it is not pushed down; question text biggest element.
7. **Credit button:** icon instead of "C"; version label larger and to the lower edge.
8. **Quality dropdown** stays in the HUD but is restyled to match the art (themed box, Wellfleet font, same corner radius as buttons).

### P2 — Feedback and "feel" (small scripts)
9. **Answer feedback:** correct button turns green and pops, wrong turns red and shakes (about 0.3 s, unscaled time because the game is paused), then the panel closes. The right answer is shown on a wrong tap.
10. **Star popup:** "+N" floats up from the question toward the star counter; counter does a short scale pulse.
11. **Timer ring:** green, then yellow at 5 s, red at 3 s, with a gentle pulse in the last 3 s.
12. **Hearts:** scale-pop and fade when lost instead of vanishing.
13. **Panel transitions:** one reusable `UiPanel` (CanvasGroup fade + 0.95 to 1 scale, about 0.2 s, unscaled) used by Settings/Store/Scoreboard/Credit/Player panels; button press = scale 0.95.
14. **Click sound:** one global UI click via `Sfx`, attached to all Buttons at scene load (no per-button wiring). Optional vibration on wrong answer.
15. **Colour tokens:** a small `UiColors` class (primary purple, cyan, correct green, wrong red, star yellow) used by scripts instead of raw `Color.green/red/yellow`.

### P3 — Screen content
16. **End screen:** run summary (stars, rounds, questions, best streak) with Restart as the primary button and Quit secondary. Needs a small `RunStats` counter in `GameController`.
17. **Scoreboard:** loading state, error + Retry, auto-scroll to the player's row, medal icons for top 3.
18. **Name entry:** trim, 2-16 characters, only letters/digits/space, Confirm disabled until valid, short inline error ("2-16 characters").
19. **HUD:** set "LEVEL n" once instead of every frame; stat counters use TMP fixed-width digits so they do not jitter.
20. **Level select:** icons (+ and +/-) on Level 1/2 instead of extra text.

Additions from the screenshots (all within the current look):
- **A1 Fonts:** all remaining Liberation Sans text to the two project fonts; add a TMP **fallback font** (or filter names) so missing glyphs show no boxes in scoreboard names.
- **A2 Settings sliders:** themed slider with a visible round handle and larger hit area; labels above the bars, not overlapping.
- **A3 Scoreboard rows:** rounded row sprite, inside padding that clears the scrollbar, rank column wide enough for 3 digits, own-row highlight, top-3 medal icons.
- **A4 Icon set:** replace the "C" letter with an icon; give EXIT an icon; label-free but self-explanatory Players header icons (reset and add) or small captions.
- **A5 Question panel (portrait):** keep side margins so buttons never touch the screen edge; answer buttons larger and centred as one row; timer tile larger.
- **A6 Landscape:** allow wider cards (store, scoreboard, credit) in landscape, for example 1.5x width with two-column credit text, so content is legible on tablets and boards (this is the optional Phase F from the orientation plan, now justified by the screenshots).
- **A7 Disabled button state:** SELECT/BUY disabled state with readable contrast.

### P4 — Structure (enables later changes, optional)
21. Replace child-index lookups (`GetChild(3).GetChild(1)...`) with serialized references in `GameController`, `CharacterSelectionController`, `DetailPlayer`, and others, then rename objects meaningfully. Large but mechanical; do last and one script at a time.

## 3. How it will be applied
- No hand-edited scene YAML. Scene changes are made by **editor menu tools** (like the earlier UI-text fixer) that preview then apply, or through Unity MCP once it is connected; you then review in the editor and save the scene.
- Runtime behaviour (feedback, transitions, click sound, colour tokens, run stats) is plain C# that attaches itself at startup, like `ResponsiveCanvasScaler`, so it works in both orientations.
- Layout changes are checked in portrait (1080x2400) and landscape (1920x1080) and on a tablet (2048x1536).

## 4. Phases
0. **Screenshots: done** (26 images in `output/ui-shots/`). Items A1-A7 above come from them. Re-run the capture tool after each phase to compare before/after.
1. **Foundation:** `UiColors`, `UiPanel`, global button click, HUD level-text fix, placeholder cleanup (items 2, 14, 15, 19).
2. **Type + buttons + panel shell** (items 1, 3, 4, 5, 7, 8) through the editor tool; review.
3. **Question panel + feedback** (items 6, 9, 10, 11, 12); play-test.
4. **Screens:** end screen, scoreboard, name entry, level select (items 16-18, 20).
5. **Structure (optional):** item 21.
6. **Verification:** both orientations, EditMode tests still pass, Android build.

## 5. Risks
- **Layout is script-coupled** (child indices): reordering/reparenting children of the canvas breaks `GameController` and others. Plan only resizes and restyles in place until Phase 5.
- **Animator conflicts:** some objects already have controllers; new tweens must not fight them (check each before adding).
- **Paused game:** the question runs at `timeScale = 0`, so all question-time animation must use unscaled time.
- **Unseen visuals:** without screenshots some items may already look fine; Phase 0 prevents needless changes.

## 6. Decisions (confirmed)
1. **Keep the current look** (art, colours, fonts); polish only.
2. **Include the feedback items** (answer colours, star popup, urgency timer, hearts, panel transitions, click sound).
3. **End-screen run summary and scoreboard loading/retry/auto-scroll: yes.**
4. **Name rules:** 2-16 characters, letters/digits/space: OK.
5. **Quality dropdown stays in the HUD** (restyle only, item 8).
6. **Screenshots:** done by the capture tool; re-run for before/after comparison.

Ready to implement; suggested order is Phase 1 (foundation), then 2, 3, 4 as in section 4.
