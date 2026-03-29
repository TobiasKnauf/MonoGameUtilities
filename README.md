# MonoGame Utilities

A lightweight utility library that adds structure and common tooling on top of the [MonoGame Framework](https://monogame.net/). It provides a composable ECS object model, pixel-perfect resolution scaling, input helpers, and debug tools — so you can skip the boilerplate and focus on building your game.

---

## Features

- **ECS Core** — `SceneObject`, `Component`, `Entity`, `Scene`, and `SceneManager` for composable game objects and scene transitions
- **Screen Management** — pixel-perfect 640×360 → 1920×1080 resolution scaling via `ScreenManager`
- **Input** — `InputHelper` for unified keyboard/gamepad axis input
- **Debug** — `Debug.ShowColliderOutlines` for visualizing `BoxCollider` bounds at runtime
- **Commons** — small utility extensions (e.g. `ICollection.IsNullOrEmpty()`)

---

## Requirements

- .NET 8
- MonoGame.Framework.DesktopGL 3.8.x

---

## Quick Start

```csharp
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

- [Getting Started](docs/getting-started.md)
- [ECS Overview](docs/ecs-overview.md)
- **API Reference**
  - [Core](docs/api/core.md)
  - [Graphics](docs/api/graphics.md)
  - [Input](docs/api/input.md)
  - [Debug](docs/api/debug.md)
  - [Commons](docs/api/commons.md)

---

## License

[MIT](https://choosealicense.com/licenses/mit/)
