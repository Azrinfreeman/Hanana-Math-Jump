# FunMath — Portrait + Tablet + Smartboard (Landscape) Support Plan

Status: **PLAN ONLY, nothing executed.** Based on ProjectSettings and a dump of `MainGame.unity` / `SplashScreen.unity` (canvas, scaler, anchors, cameras). Scene values below are measured, behaviour is predicted and must be confirmed in Game view / on device.

## 1. Current state (what blocks landscape today)

| Area | Finding | Effect |
|---|---|---|
| Orientation | `defaultScreenOrientation: 0` (Portrait), only `allowedAutorotateToPortrait: 1` | Android/iOS never rotate, so tablets and boards are stuck in portrait |
| Android aspect | `androidSupportedAspectRatio: 1` (custom), `androidMaxAspectRatio: 2.1` | Phones/tablets wider than 2.1:1 get black bars (many 20:9 phones are 2.2:1). Needs checking on a device |
| Safe area | `androidRenderOutsideSafeArea: 1`, no script uses `Screen.safeArea` | Notches/cutouts can cover corner HUD, worse in landscape (cutout on a side) |
| Desktop window | 1024x768 default, `resizableWindow: 0`, `fullscreenMode: 2` | Windows build (`Publish/FunMath.exe`) is already landscape, so it shows the portrait UI very small (see below) |
| Main canvas | One canvas, Screen Space-Camera (UICamera, distance 100), `Scale With Screen Size`, reference **1080x3900**, match **0.833** (mostly height) | Layout is designed for a very tall screen |
| UI layout | Corner HUD (TopLeft, TopRight, Bottom, version) anchored to corners; panels centered at fixed sizes: menu 1498x1321, store 1498x1960, scoreboard/credit 1498x2467, question 1797x1565, TimeToReach 1771x150; full-screen overlays stretch | Corner items adapt; centered panels keep a portrait shape |
| Splash | 1920x1080 reference, match width | Already landscape-friendly |
| Cameras | Main camera and all 3 FreeLook rigs FOV **64** (vertical); `UICamera` perspective | Vertical FOV is fixed, so landscape shows a much wider horizontal view |
| Scripts | No `Screen.*`, orientation, or aspect code at all; gameplay is aspect-independent | Game logic needs no change |

**Why landscape looks bad now (calculated):** at 1920x1080 the scaler scale is about 0.377, so a 1498-unit panel is about 565 px wide and 184-unit buttons are about 69 px. On a 4K board the same layout is just proportionally larger and equally lost in empty space.

## 2. Target behaviour
- **Portrait phones and tablets:** look exactly as today (no regression).
- **Landscape tablets, 16:9 and 16:10 boards, desktop windows:** UI scaled to the screen height; menus are centred cards; corner HUD sits in the screen corners; gameplay camera framed sensibly.
- Rotating the device or resizing the window re-lays out live, with no restart.
- Optional orientation lock (Auto / Portrait / Landscape) in Settings.

## 3. Design

### 3.1 `ResponsiveCanvasScaler` (new, on the Canvas next to `CanvasScaler`)
- Checks `Screen.width/height` each frame and re-applies only when the size changes (also catches window resize and rotation).
- **Portrait (aspect < 1):** keep today's reference 1080x3900, match 0.833.
- **Landscape (aspect >= 1):** use `Match = 1 (height)` with a landscape reference height. Starting value about **3300** so the tallest panel (scoreboard 2467 + margins) fits; the tuned value is picked from screenshots. Serialized fields so it can be adjusted in the Inspector without code changes.
- Because the canvas becomes wider than tall, the stretched `bg` and overlays fill the screen, corner HUD moves to the corners, and centred panels stay centred (pillar-boxed cards). This is the low-risk path.

### 3.2 `SafeAreaFitter` (new)
- Applied to a new full-stretch `SafeArea` container; the HUD groups (TopLeft, TopRight, Top, Bottom, version, Name) are re-parented into it with anchors unchanged. Backgrounds stay outside, so they still fill the whole screen.
- Updates when `Screen.safeArea` or orientation changes.

### 3.3 Camera
- `OrientationCamera` script sets the FreeLook rig lens FOV by orientation: portrait 64 (today); landscape a smaller vertical FOV, starting about **45-50**, so the horizontal view is about 80 degrees and not about 100. Tuned on screen.
- Verify the intro animation `startCameraAnimation` and `DisableCameraRotation` (heading bias) in landscape. They were authored in portrait.

### 3.4 Project settings
- `defaultScreenOrientation` -> Auto Rotation; allow Portrait, Landscape Left, Landscape Right (upside-down off); keep `useOSAutorotation: 1`.
- Android: set supported aspect ratio to Native (or raise max to at least 2.4) so no black bars; keep `androidRenderOutsideSafeArea: 1` once the safe area script exists.
- Standalone: `resizableWindow: 1` so desktop/board windows can be resized or maximised; default window changed from 1024x768 to 1280x720.
- Touch/board input: `Input.multiTouchEnabled = false` (avoids palms/second fingers pressing two answers); scale `EventSystem.pixelDragThreshold` with screen height so taps on 4K boards don't register as drags in the ranking/store lists.
- Optional: lower URP render scale on 4K boards through the existing Quality dropdown.

### 3.5 Optional orientation lock
- `OrientationSettings` + one dropdown in Settings (AUTO / PORTRAIT / LANDSCAPE), saved in PlayerPrefs, applied on start. Needs one new UI element in the scene (added through MCP). Skip if you do not need it.

## 4. Phases (each is small and testable)
1. **A – Settings:** orientation, Android aspect, resizable window, multi-touch. Check Splash and the menu rotate without errors. (Low risk.)
2. **B – Responsive scaler:** `ResponsiveCanvasScaler` on the MainGame canvas (and Splash canvas if needed); tune landscape reference height; verify portrait is unchanged pixel-for-pixel against a before screenshot.
3. **C – Safe area:** `SafeAreaFitter`, re-parent HUD groups in the scene via MCP, test with the Device Simulator cutout presets.
4. **D – Camera:** `OrientationCamera` FOV profile; check run, jump, trip, and intro animation in both orientations.
5. **E – Board/tablet polish:** drag threshold, touch-keyboard check for name entry on Windows touch boards, large-screen quality/render-scale option.
6. **F – Optional landscape layouts:** native layouts for the question panel (question text left, two answer buttons right or side by side) and the end screen, switched by orientation. Only if the centred portrait cards feel too small in play-testing.
7. **G – Verification and build:** Windows + Android builds; version bump.

## 5. Test matrix (Game view / Device Simulator, then real devices)
| Device | Resolution | Orientation |
|---|---|---|
| Phone 20:9 | 1080x2400 | Portrait, then landscape |
| Phone 16:9 | 1080x1920 | Portrait (baseline for "no change") |
| Tablet 4:3 | 2048x1536 / 1536x2048 | Both |
| Tablet 16:10 | 1920x1200 / 1200x1920 | Both |
| Smartboard / TV | 1920x1080, 3840x2160 | Landscape |
| Desktop window | 1280x720 resized freely | Both aspects |

Check on each: splash, name entry, main menu, level select, store, scoreboard, settings, question panel and timer, hearts/HUD in corners, end screen, camera framing while running, and rotating mid-question (the pause state must survive rotation).

## 6. Risks
- **Rotation mid-game:** layout changes while `Time.timeScale = 0` during a question; the scaler must not rely on `Time`. It uses screen size only.
- **Portrait regression:** mitigated by keeping the exact current scaler values for aspect below 1 and screenshot-comparing.
- **Tall panels in landscape** (scoreboard, store) are scaled down to fit the height, so they become narrow cards with side empty space. If that is unacceptable, Phase F (or scroll views) is needed.
- **Windows touch keyboard:** TMP input fields on Windows do not always open the touch keyboard on boards; may need an on-screen keyboard for name entry.
- Scene edits (re-parenting HUD, adding scripts) are done through Unity MCP or the editor, not by hand-editing YAML, and the scene must be saved from the editor.

## 7. Decisions (confirmed)
1. **Centred cards + corner HUD** is the first landscape version (Phases A-E). Native landscape layouts (Phase F) only if play-testing shows a need.
2. **Orientation: Auto only.** No lock dropdown (section 3.5 is dropped).
3. **Platform: Android only.** Windows/iOS items are out of scope: no `resizableWindow` / default-window change, and the Windows touch-keyboard check is dropped. Smartboards are covered as Android landscape devices.
4. **Remove the 2.1 max aspect limit** (set supported aspect ratio to Native).

Scope after these decisions: Phases A (Android orientation + aspect + multi-touch), B (scaler), C (safe area), D (camera FOV), E (drag threshold + 4K render-scale option), G (Android build + version bump). F stays optional.
