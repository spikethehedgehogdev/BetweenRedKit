# Changelog

All notable changes to this project will be documented in this file.  
The format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)  
and adheres to [Semantic Versioning](https://semver.org/).

---

## [0.9.0] – 2025-11-10
### Added
- Core systems: `BetweenProcessor`, `BetweenHandle`, `BetweenSlot`.  
- Version-safe handle system (`id` + `version`) preventing stale access.  
- `ICapacityPolicy` with two growth strategies: `Doubling` and `Fixed`.  
- `ITimeProvider` implementations:  
  - `UnityTimeProvider` — frame-based updates using `Time.deltaTime`;  
  - `ManualTimeProvider` — manual time control for tests and offline simulations.  
- `IEasingProvider` and built-in `DefaultEasingProvider` with common curves:  
  `Linear`, `In/Out/InOutQuad`, `In/Out/Quint`.  
- Transform targets:  
  `MoveTo`, `RotateTo`, `ScaleTo` with `EaseType` and `OnComplete` callbacks.  
- **UniTask integration:**  
  `AwaitCompletion()` and `AwaitCompletion(CancellationToken)` for async workflows.  
- Lifecycle management:  
  `Pause()`, `PauseAll()`, `Stop()`, `CullInvalidTargets()`, `Dispose()`.  
- Fully zero-GC update cycle (dense-remove, no dynamic lists).  
- MIT license, `package.json`, `README.md`, `CHANGELOG.md`.

### Changed
- Implemented safe validation of active slots in `Tick()` — skips inactive or dead tweens without calling `Apply()`.  
- Refined namespaces: `BetweenRedKit.Core`, `BetweenRedKit.Easing`, `BetweenRedKit.Integrations`.

### Planned
- Sequence API (`Append()`, `Join()`, `Play()`)  
- Loop / PingPong modes  
- Color / Float / Material targets  
- Between Debugger (EditorWindow)  
- Profiler hooks (`ActiveCount`, `AvgLifetime`)
