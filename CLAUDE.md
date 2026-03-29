# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build all projects
dotnet build MonoGameUtilities.sln

# Release build
dotnet build MonoGameUtilities.sln -c Release

# Pack NuGet package
dotnet pack MonoGameUtilities.Package/MonoGameUtilities.Package.csproj
```

There are no automated tests in this codebase.

## Architecture Overview

This is a **MonoGame utility library** split into 6 projects, all bundled into a single NuGet package (`MonoGameUtilities.Package`). All projects target **.NET 8** and depend on **MonoGame.Framework.DesktopGL 3.8.x**.

### Project Structure

| Project | Purpose |
|---|---|
| `MonoGameUtilities.Core` | ECS framework (Scene, SceneObject, Entity, Components) |
| `MonoGameUtilities.Graphics` | Resolution/screen management (`ScreenManager`) |
| `MonoGameUtilities.Input` | Keyboard + gamepad input (`InputHelper`) |
| `MonoGameUtilities.Debug` | Debug visualization (`Debug.ShowColliderOutlines`) |
| `MonoGameUtilities.Commons` | Extension methods (e.g., `ICollection.IsNullOrEmpty`) |
| `MonoGameUtilities.Package` | NuGet wrapper that references all the above |

### ECS System (Core)

The ECS hierarchy is: `SceneManager` → `Scene` → `SceneObject` → `Component`

- **`SceneObject`** is the base game object. It owns a `Transform` (position/scale) and a list of `Component` instances. Components are added via `AddComponent<T>()` and retrieved via `GetComponent<T>()` / `TryGetComponent<T>()`.
- **`Entity`** extends `SceneObject` and pre-wires three standard components: `SpriteRenderer`, `PhysicsBody`, and `BoxCollider`. On `Init()`, the collider is auto-sized to the texture, and `Update()` auto-flips the sprite based on velocity direction.
- **`Component`** is an abstract base. Subclasses implement `Init(SceneObject)`, `Update(GameTime)`, `Draw(GameTime, SpriteBatch)`, and `Destroy()`.
- **`SceneManager`** is a lazy-initialized singleton. Call `SceneManager.Instance.LoadScene(scene)` to transition scenes; it calls `Unload()` on the previous and `Load()` on the new.
- **`Scene`** holds a `List<SceneObject>`. Objects subscribe themselves to the current scene on construction.

**Object lifecycle:** Construction → `Init()` → `Update()` / `Draw()` loop → `Destroy()`

### Known Limitations (by design)

- `PhysicsBody` is velocity-only — no gravity, drag, or acceleration.
- `BoxCollider` detects AABB overlap (`Intersects`) but has **no collision response**.
- No transform hierarchy (parent/child SceneObjects).
- Input supports only one gamepad (`PlayerIndex.One`).

### Screen Management (Graphics)

`ScreenManager` implements a **pixel-perfect scaling pipeline**:
1. `Init()` sets the back buffer to 1920×1080 and creates an internal `RenderTarget2D` at 640×360.
2. Each frame: call `ResetScreen()` to redirect drawing to the 640×360 RT.
3. After all drawing: call `DrawRenderTarget()` to upscale the RT to 1920×1080 using `SamplerState.PointClamp`.

These native/target resolutions are hard-coded constants in `ScreenManager`.
