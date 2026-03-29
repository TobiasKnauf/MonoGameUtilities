# API Reference: Debug

Namespace: `MonoGameUtilities.Debug`
Assembly: `MonoGameUtilities.Debug`

---

## Debug

Provides visual debugging helpers. All methods are static.

```csharp
public static class Debug
```

---

### Methods

#### `ShowColliderOutlines(BoxCollider, SpriteBatch)`

```csharp
public static void ShowColliderOutlines(BoxCollider col, SpriteBatch sb)
```

Draws a 1-pixel red rectangle outline around a `BoxCollider`'s bounds. Useful for verifying collider size and position during development.

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `col` | `BoxCollider` | The collider to visualize. Uses `col.Min`, `col.Max`, and `col.Size`. |
| `sb` | `SpriteBatch` | An active `SpriteBatch` (must be inside a `Begin`/`End` block). |

**Visual output:** Four 1-pixel-wide red lines forming a rectangle at the collider's world-space bounds.

> **Performance note:** This method creates a new `Texture2D(1,1)` on every call. Use it only during development/debugging, not in production builds.

---

### Example

```csharp
protected override void Draw(GameTime gameTime)
{
    ScreenManager.Instance.ResetScreen();

    _spriteBatch.Begin();
    SceneManager.Instance.Draw(gameTime, _spriteBatch);

    // Draw collider outlines for all entities
    foreach (var so in SceneManager.Instance.CurrentScene.SceneObjects)
    {
        if (so.TryGetComponent<BoxCollider>(out var col))
            Debug.ShowColliderOutlines(col, _spriteBatch);
    }

    _spriteBatch.End();

    ScreenManager.Instance.DrawRenderTarget(_spriteBatch);
    base.Draw(gameTime);
}
```

---

### Dependencies

`Debug` depends on `ScreenManager.GraphicsDevice` to create the 1×1 pixel texture. `ScreenManager.Init()` must have been called before using this class.
