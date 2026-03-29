# Getting Started

This guide walks you through adding MonoGameUtilities to an existing MonoGame project and rendering your first entity on screen.

## Prerequisites

- An existing MonoGame desktop project targeting **.NET 8**
- MonoGame.Framework.DesktopGL 3.8.x

## Installation

Add the NuGet package to your game project:

```bash
dotnet add package MonoGameUtilities
```

Or search for `MonoGameUtilities` in the NuGet package manager in Visual Studio/Rider.

---

## Wiring Up the Managers

Both `ScreenManager` and `SceneManager` need to be initialized inside your `Game1` class.

### 1. Initialize ScreenManager

`ScreenManager` needs a reference to your `GraphicsDeviceManager` **before** `Init()` is called. Do this in your `Game1` constructor, then call `Init()` in `LoadContent`:

```csharp
using MonoGameUtilities.Graphics;
using MonoGameUtilities.Core.Scene;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        // Give ScreenManager a reference to your GraphicsDeviceManager
        ScreenManager.Graphics = _graphics;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Sets the window to 1920x1080 and creates the internal 640x360 render target
        ScreenManager.Instance.Init();

        // Load your first scene (must happen before creating any SceneObjects)
        SceneManager.Instance.LoadScene(new MyFirstScene());
    }
}
```

> **Important:** `SceneManager.Instance.LoadScene()` must be called before any `SceneObject` (or `Entity`) is instantiated. Each `SceneObject` registers itself with `CurrentScene` in its constructor, so there must be an active scene first.

### 2. Hook Up Update and Draw

```csharp
protected override void Update(GameTime gameTime)
{
    SceneManager.Instance.Update(gameTime);
    base.Update(gameTime);
}

protected override void Draw(GameTime gameTime)
{
    // 1. Point rendering at the 640x360 render target and clear it
    ScreenManager.Instance.ResetScreen();

    // 2. Draw your scene into the render target
    _spriteBatch.Begin();
    SceneManager.Instance.Draw(gameTime, _spriteBatch);
    _spriteBatch.End();

    // 3. Scale and blit the render target to the 1920x1080 back buffer
    ScreenManager.Instance.DrawRenderTarget(_spriteBatch);

    base.Draw(gameTime);
}
```

---

## Creating a Scene

Subclass `Scene` and populate it with objects inside `Load()`:

```csharp
using MonoGameUtilities.Core.Scene;

public class MyFirstScene : Scene
{
    // SceneObjects added here will auto-register via their constructors.
    // You can also override Load() to run setup logic after the scene is activated.
}
```

Scenes are bare containers — they don't need to override anything unless you want custom load/unload behaviour.

---

## Creating an Entity

`Entity` is abstract, so you always subclass it. It comes pre-wired with `SpriteRenderer`, `PhysicsBody`, and `BoxCollider`.

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUtilities.Core.Entities;

public class Player : Entity
{
    private ContentManager _content;

    public Player(ContentManager content) : base()
    {
        _content = content;

        // Position the player at the center of the native (640x360) canvas
        Transform.Position = new Vector2(320, 180);

        // Load and assign a texture to the built-in SpriteRenderer
        _spriteRenderer.Texture = content.Load<Texture2D>("player");
    }
}
```

> After `Init()` runs, `_boxCollider.Size` is automatically set to the texture's pixel dimensions.

### Spawning the Entity in a Scene

Because `SceneObject`'s constructor auto-subscribes to `CurrentScene`, simply instantiating `Player` is enough:

```csharp
public class MyFirstScene : Scene
{
    private ContentManager _content;

    public MyFirstScene(ContentManager content)
    {
        _content = content;
    }
}

// In Game1.LoadContent, after LoadScene:
SceneManager.Instance.LoadScene(new MyFirstScene(Content));
_ = new Player(Content); // auto-registers with the active scene
```

---

## Full Minimal Example

```
Game1
 └─ LoadContent
     ├─ ScreenManager.Graphics = _graphics
     ├─ ScreenManager.Instance.Init()
     ├─ SceneManager.Instance.LoadScene(new MyFirstScene())
     └─ new Player(Content)          ← auto-registered

Game1.Update  →  SceneManager.Instance.Update(gameTime)
Game1.Draw    →  ScreenManager.Instance.ResetScreen()
               →  _spriteBatch.Begin / SceneManager.Instance.Draw / _spriteBatch.End
               →  ScreenManager.Instance.DrawRenderTarget(_spriteBatch)
```

---

## Next Steps

- [ECS Overview](ecs-overview.md) — understand the component lifecycle and architecture
- [API Reference: Core](api/core.md) — full reference for `SceneObject`, `Entity`, `Component`, and more
- [API Reference: Graphics](api/graphics.md) — `ScreenManager` resolution pipeline
- [API Reference: Input](api/input.md) — `InputHelper` axis methods
