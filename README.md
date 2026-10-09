# Hanana Math Jump

A Unity educational game that combines timed arithmetic questions with a 3D running and obstacle-jumping course. The character follows a waypoint path automatically; players choose answers to influence obstacle encounters and earn stars and completed rounds.

## Project highlights

- Four-choice arithmetic questions: addition in level 1, addition and subtraction in level 2, with operand ranges that change as the question count increases.
- Obstacle-triggered jumps and collision-driven stumbling, health loss, and end-screen handling.
- An answer countdown and feedback animations/audio; the reviewed reward path uses remaining answer time to determine stars.
- Local player profiles and saved star/round totals using Unity PlayerPrefs.
- Character selection and unlock requirements based on stars and rounds, plus music and sound-effect settings.

These features are described from source inspection. Unity compilation, scene wiring, gameplay, and device builds have not been verified in this documentation review. See [validation notes](docs/VALIDATION.md) for source observations and a manual test checklist.

## Open the project

1. Clone the repository and install **Unity 2022.3.60f1** through Unity Hub, matching [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt).
2. Add the repository root as a Unity project and allow package resolution and asset import to finish.
3. Open [MainGame](Assets/Scenes/MainGame.unity), the single enabled scene in [build settings](ProjectSettings/EditorBuildSettings.asset).
4. Before entering Play mode, review the backend integration below and use test profiles in an appropriate development environment. Exercise profile creation, level selection, and the four answer buttons.

The [package manifest](Packages/manifest.json) includes Universal Render Pipeline 14.0.12, Cinemachine 2.10.3, ProBuilder 5.2.3, TextMesh Pro 3.0.7, and Unity UI 1.0.0.

## Code guide

| Area | Entry point |
| --- | --- |
| Questions, answers, rewards, and game start | [GameController](Assets/GameController.cs) |
| Automatic movement, jump/crash triggers, and character animation | [PathController](Assets/PathController.cs) |
| Answer countdown | [TimeToAnswer](Assets/TimeToAnswer.cs) |
| Saved star and round totals | [CollectionController](Assets/CollectionController.cs) |
| Character purchases and selection | [CharacterSelectionController](Assets/CharacterSelectionController.cs) |
| Profile setup and score reporting | [GameStartupController](Assets/GameStartupController.cs) |

## Data and backend integration

PlayerPrefs holds local profile state and per-player star/round totals. Character unlock flags and the selected character use shared keys, so this is not a completely isolated per-profile inventory.

GameStartupController contains a score-reporting POST to the existing Hanana service at `app-hanana.com/funmath/insertmark.php`, sending player name, player ID, total stars, and total rounds. Server code and deployment instructions are not included here. The service was not contacted during this review; availability, authorization, and server behavior are unverified. Review this path before using real player data or demonstrating the project.

## Assets and reuse

Bundled characters, audio, fonts, packages, and other assets remain unchanged. Their presence does not establish redistribution rights; review the relevant asset licenses before distributing a build or reusing assets. No new license is assigned by this documentation update.
