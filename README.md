# Between RedKit
Lightweight zero-GC tween processor for Unity.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](./LICENSE)
[![Unity](https://img.shields.io/badge/Unity-2021.3%2B-black?logo=unity)]()

---

## Overview
**Between RedKit** is a compact and extensible tween engine for Unity, designed for **zero allocations at runtime** and **native UniTask integration**.  
Built around *value types, pools, and decoupled providers*.  
It’s not a replacement for DOTween — it’s a lightweight alternative for cases where you need determinism, control, and minimal overhead.


## Key Features
- **Zero GC:** Tweens are created and updated without allocations using a dense-remove pool and a free-slot stack.  
- **Version-safe handles:** `BetweenHandle` stores both `id` and `version` to prevent access to recycled slots.  
- **Flexible capacity management:** `ICapacityPolicy` controls how pools grow (`Doubling` or `Fixed`) for embedded or constrained environments.  
- **Deterministic tick:** `ITimeProvider` (`UnityTimeProvider`, `ManualTimeProvider`) allows testing and offline simulation with manual updates.  
- **Extensible easing provider:** `IEasingProvider` interface supports custom curves and easing definitions.  
- **MonoBehaviour-free:** Works as a pure service — easily integrates into DI containers, ECS, or custom game loops.  
- **Async / Await ready:** UniTask awaiters allow asynchronous tween completion without coroutines or GC pressure.  
- **Transform integration:** Built-in extensions for `MoveTo`, `RotateTo`, and `ScaleTo`.
- **Graphics integration:** Built-in extensions for `ColorTo` and `FadeTo`.
- **Lifecycle control:** `Dispose()`, `PauseAll()`, `Stop()`, `CullInvalidTargets()` for safe runtime cleanup.  
- **Minimal runtime footprint:** ~15 KB of code, no editor dependencies, single external dependency — UniTask.
- **Full easing pack:** all classical Penner functions (Sine, Quad, Cubic, Quart, Quint, Expo, Circ, Back, Elastic, Bounce).

Supported Unity versions: **2021.3 LTS and newer**

## Installation
**Unity Package Manager → Add package from Git URL:**
```
https://github.com/spikethehedgehogdev/BetweenRedKit.git
```

## Quick Start
```csharp
using BetweenRedKit.Core;
using BetweenRedKit.Easing;
using BetweenRedKit.Integrations.Cysharp.Integrations.CySharp;
using BetweenRedKit.Integrations.Unity;
using UnityEngine;

public class Example : MonoBehaviour
{
    private BetweenProcessor _between;

    void Awake() => _between = new BetweenProcessor(initialCapacity: 256);

    async void Start()
    {
        await transform
            .ScaleTo(_between, Vector3.one * 1.2f, 0.3f, EaseType.OutQuint)
            .AwaitCompletion();

        transform
            .MoveTo(_between, new Vector3(0, 2, 0), 0.25f, EaseType.OutQuad)
            .OnComplete(() => Debug.Log("Done"));
    }

    void Update() => _between.Tick(); // or through your own game loop / ticker
}
```

## Structure
```
Runtime/
 ├─ Core/          // BetweenProcessor, BetweenHandle, interfaces and time providers
 ├─ Easing/        // EaseType and easing provider
 └─ Integrations/
     ├─ Unity/     // Extensions
     └─ CySharp/   // UniTask awaiters: AwaitCompletion()
```

## Roadmap (pre-1.0)
- [ ] **Sequence API** — chaining tweens with `Append()` / `Join()` and async `Play()`
- [ ] **Loop / PingPong** — repeat, reverse, and auto-restart modes
- [ ] **Value Targets** — tweens for
- - [x]  `Color`, `CanvasGroup`
- - [ ]  `float`, `Material`, `RectTransform`
- [ ] **Groups / Tags API** — control by key (`PauseAllByKey`, `StopByKey`)
- [ ] **Between Debugger** — EditorWindow for inspecting active tweens, capacity, and GC allocations
- [ ] **Sequence Awaiters** — async await support for sequences
- [ ] **Burst/ECS адаптер** — compatibility with Job System and ECS workflows
- [ ] **Package Samples** — demo scenes and tests (`ManualTimeProvider`, visual examples)

## Changelog
See [CHANGELOG.md](./CHANGELOG.md)

---

**Ilias Abdullin** ([@spikethehedgehogdev](https://github.com/spikethehedgehogdev))
