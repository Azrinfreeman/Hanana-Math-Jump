# Documentation review and manual checks

Baseline: `1a7d237abddffaa829b361eaebf0a0652bec0553`.

## Scope

README and these validation notes only. Scripts, scenes, assets, packages, settings, gameplay, and saved data are unchanged.

Reviewed question/answer paths, automatic movement and obstacle triggers, timer behavior, health decrement, character purchase logic, local score storage, profile selection, scene navigation, and the existing score-reporting request. Checked documentation links, the enabled scene path, and patch whitespace.

Unity compilation, gameplay, serialized event wiring, and platform builds were not tested. No live backend requests were made. This is not a full-history security or asset-rights audit.

## Source observations

- GameController changes handling at question count 80. Its Update path can schedule `displayTimer` every frame while the later-flow condition holds. Check overlapping coroutines, prompt visibility, listener cleanup, and exactly-once scoring around this transition.
- PathController indexes the first obstacle and edge-trigger entries without checking whether the lists are empty. Several answer and trigger paths remove entries; test the end of the route and repeated contacts.
- HealthController indexes `Healths[healthCount - 1]` before decrementing, without a zero guard. Check repeated crash events, depleted health, and restart.
- TimeToAnswer counts unscaled time and resets to ten seconds in OnDisable; confirm initial serialized time and the timeout response. The reviewed star calculation leaves the exact ten-second boundary outside its reward ranges.
- Character unlocks and selection use shared PlayerPrefs keys while stars/rounds use the current player index. Check profile switching and purchase deductions across profiles.
- DetailPlayer parses only the final character of a profile key as its index. Test ten or more profiles, malformed keys, selection, and deletion.
- SoundSettings applies Log10 to slider values; verify that slider bounds and mute behavior handle zero safely.
- GameController and PathController rely on named objects, tags, fixed child indices, and singleton initialization. StartGame also parses the selected button name as the level number. Confirm hierarchy, numeric names, scene references, and startup order.
- Local PlayerPrefs state and the external score-reporting request are separate mechanisms. Server availability and error behavior remain unverified.

## Manual checklist

1. Import with Unity 2022.3.60f1; inspect compilation, package resolution, missing references, build settings, tags, and UI events.
2. Review or redirect backend reporting in a separately approved development setup before playing; use synthetic profile data.
3. Create/select profiles, start both arithmetic modes, and test all answer buttons, wrong answers, fast answers, timer expiry, pause, and restart.
4. Check waypoint movement, jump/crash triggers, health depletion, route exhaustion, and the transition before/at/after question 80 for duplicate prompts or rewards.
5. Verify saved stars/rounds after restart, character purchases, unlock scope across profiles, selection, and profile deletion; include multi-digit profile indices.
6. Test audio sliders at their minimum and maximum, animations, intended device controls, and a platform build. Review bundled asset permissions before distribution.

The source concerns above remain unfixed in this documentation-only change.
