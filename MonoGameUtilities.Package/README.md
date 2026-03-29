# MonoGame Utilities

An assortment of methods and engine features to enhance the MonoGame Framework.

MonoGameUtilities adds a lightweight ECS (Entity-Component-System) object model, resolution-scaling screen management, input helpers, and debug tools on top of MonoGame — giving you a structured foundation to build games without writing boilerplate from scratch.

---

## Features

- **ECS Core** — `SceneObject`, `Component`, `Entity`, `Scene`, and `SceneManager` for composable game objects and scene management
- **Screen Management** — pixel-perfect 640×360 → 1920×1080 resolution scaling via `ScreenManager`
- **Input** — `InputHelper` for raw keyboard/gamepad axis input
- **Debug** — `Debug.ShowColliderOutlines` for visualizing `BoxCollider` bounds
- **Commons** — `ICollection.IsNullOrEmpty()` extension

---

## Requirements

- .NET 8
- MonoGame.Framework.DesktopGL 3.8.x

---

## Quick Start

```csharp
// Game1.cs
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        ScreenManager.Graphics = _graphics;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        ScreenManager.Instance.Init();
        SceneManager.Instance.LoadScene(new MyScene());
    }

    protected override void Update(GameTime gameTime)
    {
        SceneManager.Instance.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        ScreenManager.Instance.ResetScreen();
        _spriteBatch.Begin();
        SceneManager.Instance.Draw(gameTime, _spriteBatch);
        _spriteBatch.End();
        ScreenManager.Instance.DrawRenderTarget(_spriteBatch);
        base.Draw(gameTime);
    }
}
```

---

## Documentation

- [Getting Started](../docs/getting-started.md)
- [ECS Overview](../docs/ecs-overview.md)
- **API Reference**
  - [Core — SceneObject, Entity, Component, Scene, SceneManager](../docs/api/core.md)
  - [Graphics — ScreenManager](../docs/api/graphics.md)
  - [Input — InputHelper](../docs/api/input.md)
  - [Debug — Debug](../docs/api/debug.md)
  - [Commons — Extensions](../docs/api/commons.md)
