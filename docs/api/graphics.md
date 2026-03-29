# API Reference: Graphics

Namespace: `MonoGameUtilities.Graphics`
Assembly: `MonoGameUtilities.Graphics`

---

## ScreenManager

Singleton that manages a pixel-art-friendly resolution scaling pipeline. Renders your game into a low-resolution `RenderTarget2D` (640×360), then scales it up to a high-resolution back buffer (1920×1080) using nearest-neighbour filtering.

```csharp
public class ScreenManager
```

### Access

```csharp
ScreenManager.Instance
```

### Prerequisites

Before calling `Init()`, assign your `GraphicsDeviceManager`:

```csharp
ScreenManager.Graphics = _graphics; // in Game1 constructor
```

---

### Constants

| Constant | Value | Description |
|----------|-------|-------------|
| `NATIVE_RESOLUTION_WIDTH` | `640` | Width of the internal render target in pixels. |
| `NATIVE_RESOLUTION_HEIGHT` | `360` | Height of the internal render target in pixels. |
| `TARGET_RESOLUTION_WIDTH` | `1920` | Width of the final back buffer. |
| `TARGET_RESOLUTION_HEIGHT` | `1080` | Height of the final back buffer. |

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Instance` | `ScreenManager` | Singleton accessor. Creates the instance on first access. |
| `Graphics` | `GraphicsDeviceManager` | Static. Must be assigned before `Init()`. |
| `GraphicsDevice` | `GraphicsDevice` | Static shortcut to `Graphics.GraphicsDevice`. |
| `ResolutionScale` | `int` | `TARGET_RESOLUTION_WIDTH / NATIVE_RESOLUTION_WIDTH` = `3`. |

---

### Methods

#### `Init()`

```csharp
public void Init()
```

Sets the back buffer to 1920×1080, applies the change, and creates the internal `RenderTarget2D` at 640×360. Call this in `Game1.LoadContent()`.

#### `ResetScreen()`

```csharp
public void ResetScreen()
```

Redirects rendering to the internal render target and clears it to a dark background (`#23242A`). Call this at the start of `Game1.Draw()`, before your `SpriteBatch.Begin`.

#### `DrawRenderTarget(SpriteBatch)`

```csharp
public void DrawRenderTarget(SpriteBatch sb)
```

Redirects rendering back to the screen (null render target), then draws the 640×360 render target scaled up to 1920×1080 using `SamplerState.PointClamp` (nearest-neighbour, no blurring). Call this at the end of `Game1.Draw()`.

#### `Update(GameTime)` / `Draw(GameTime, SpriteBatch)`

Both are currently empty. They exist as hooks for future use.

---

### Typical Draw Loop

```csharp
protected override void Draw(GameTime gameTime)
{
    // Step 1: Aim rendering at the 640×360 render target, clear it
    ScreenManager.Instance.ResetScreen();

    // Step 2: Draw everything into the render target at native resolution
    _spriteBatch.Begin();
    SceneManager.Instance.Draw(gameTime, _spriteBatch);
    _spriteBatch.End();

    // Step 3: Scale and blit to the 1920×1080 screen
    ScreenManager.Instance.DrawRenderTarget(_spriteBatch);

    base.Draw(gameTime);
}
```

> **Why PointClamp?** Pixel art looks blurry with bilinear filtering. `SamplerState.PointClamp` preserves sharp pixel edges when scaling up.

---

### Resolution Details

All game logic should use native-resolution coordinates (0–640 x, 0–360 y). The 3× scale happens entirely inside `DrawRenderTarget` and is transparent to your game code.

| Coordinate space | Range | Used for |
|-----------------|-------|----------|
| Native | 0–640 × 0–360 | Game logic, entity positions, UI |
| Screen | 0–1920 × 0–1080 | Final display only (handled automatically) |
