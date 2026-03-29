# API Reference: Core

Namespace: `MonoGameUtilities.Core`

---

## SceneObject

**Namespace:** `MonoGameUtilities.Core.Scene`
**Assembly:** `MonoGameUtilities.Core`

The base class for all objects that live in a scene. Holds a `Transform` and a list of `Component`s. Automatically registers itself with the active scene on construction.

```csharp
public class SceneObject
```

### Constructor

```csharp
public SceneObject()
```

Registers the object with `SceneManager.Instance.CurrentScene`. A scene must be loaded before calling this.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Transform` | `Transform` | Position and scale in world space. Set via `Transform.Position` and `Transform.Scale`. |
| `IsInitialized` | `bool` | `true` after `Init()` has been called. Read-only. |

### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Init()` | `void` | Called once before the first `Update`. Sets `IsInitialized = true`. Override to run one-time setup. |
| `Update(GameTime)` | `void` | Called every frame. Delegates to all attached components. |
| `Draw(GameTime, SpriteBatch)` | `void` | Called every frame after `Update`. Delegates to all attached components. |
| `Destroy()` | `void` | Destroys all components and removes this object from the current scene. |
| `AddComponent<T>()` | `T` | Creates, initializes, and attaches a new component of type `T`. `T` must have a public parameterless constructor. |
| `GetComponent<T>()` | `T` | Returns the first attached component of type `T`, or `null` if none found. Throws if the component list is empty. |
| `TryGetComponent<T>(out T)` | `bool` | Safe variant of `GetComponent`. Returns `false` and sets the out param to `null` if not found. |

### Example

```csharp
public class Trigger : SceneObject
{
    private BoxCollider _collider;

    public Trigger() : base()
    {
        _collider = AddComponent<BoxCollider>();
        _collider.Size = new Vector2(32, 32);
    }

    public override void Update(GameTime time)
    {
        base.Update(time);
        // custom logic here
    }
}
```

---

## Component

**Namespace:** `MonoGameUtilities.Core.Components`

Abstract base class for all components. A component holds behaviour and is always owned by a `SceneObject`.

```csharp
public abstract class Component
```

### Methods

| Method | Description |
|--------|-------------|
| `virtual Init(SceneObject parent)` | Stores the owner reference (`_sceneObject`). Call `base.Init(parent)` when overriding. |
| `abstract Update(GameTime time)` | Called every frame by the owning `SceneObject`. |
| `abstract Draw(GameTime time, SpriteBatch sb)` | Called every frame after Update. |
| `abstract Destroy()` | Called when the owning `SceneObject` is destroyed. |

### Protected Members

| Member | Type | Description |
|--------|------|-------------|
| `_sceneObject` | `SceneObject` | The owning object. Available after `Init` is called. |

### Example: Custom Component

```csharp
public class HealthComponent : Component
{
    public int HP { get; private set; } = 100;

    public override void Init(SceneObject parent)
    {
        base.Init(parent);
        HP = 100;
    }

    public override void Update(GameTime time)
    {
        if (HP <= 0) _sceneObject.Destroy();
    }

    public override void Draw(GameTime time, SpriteBatch sb) { }
    public override void Destroy() { }

    public void TakeDamage(int amount) => HP -= amount;
}
```

---

## Entity

**Namespace:** `MonoGameUtilities.Core.Entities`

Abstract subclass of `SceneObject`. Pre-adds `SpriteRenderer`, `PhysicsBody`, and `BoxCollider` so you don't have to. The collider is automatically sized to the texture on `Init`.

```csharp
public abstract class Entity : SceneObject
```

### Protected Fields

| Field | Type | Description |
|-------|------|-------------|
| `_spriteRenderer` | `SpriteRenderer` | Controls texture, tint, and horizontal flip. |
| `_physicsBody` | `PhysicsBody` | Moves the transform by `Velocity` each frame. |
| `_boxCollider` | `BoxCollider` | Axis-aligned bounding box. Sized from texture in `Init`. |

### Behaviour

- **`Init()`** — sets `_boxCollider.Size` from `_spriteRenderer.Texture` dimensions (if a texture is assigned).
- **`Update()`** — flips the sprite horizontally when `_physicsBody.Velocity.X < 0`; resets to no-flip when `> 0`.

### Example

```csharp
public class Enemy : Entity
{
    public Enemy(Texture2D texture) : base()
    {
        _spriteRenderer.Texture = texture;
        Transform.Position = new Vector2(100, 100);
    }

    public override void Update(GameTime time)
    {
        base.Update(time); // handles flip + component updates

        // Move left at 2px per frame
        _physicsBody.Velocity = new Vector2(-2, 0);
    }
}
```

---

## Transform

**Namespace:** `MonoGameUtilities.Core.Types`

Holds an object's world-space position and scale.

```csharp
public class Transform
```

### Fields

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `Position` | `Vector2` | `Vector2.Zero` | World position in pixels. |
| `Scale` | `Vector2` | `Vector2.One` | Render scale (1,1 = original size). |

### Constructors

```csharp
Transform()                                  // Position = Zero, Scale = One
Transform(Vector2 position)                  // Scale = One
Transform(Vector2 position, Vector2 scale)
```

### Methods

| Method | Description |
|--------|-------------|
| `Translate(Vector2 direction, float time)` | Moves position by `direction * time`. Equivalent to `Position += direction * time`. |

---

## Scene

**Namespace:** `MonoGameUtilities.Core.Scene`

A container for `SceneObject`s. Drives their update/draw loops and manages subscription.

```csharp
public class Scene
```

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `SceneObjects` | `List<SceneObject>` | All objects currently in this scene. |

### Methods

| Method | Description |
|--------|-------------|
| `Load()` | Calls `Init()` on all subscribed objects. Called by `SceneManager.LoadScene()`. |
| `Unload()` | Calls `Destroy()` on all subscribed objects. Called before loading a new scene. |
| `Update(GameTime)` | Initializes any uninitialized objects, then calls `Update` on all. |
| `Draw(GameTime, SpriteBatch)` | Calls `Draw` on all objects. |
| `SubscribeSceneObject(SceneObject)` | Adds an object to the scene. No-op if already present or null. |
| `UnsubscribeSceneObject(SceneObject)` | Removes an object from the scene. No-op if not present or null. |

---

## SceneManager

**Namespace:** `MonoGameUtilities.Core.Scene`

Singleton. Holds the active scene and delegates `Update`/`Draw` from `Game1`.

```csharp
public class SceneManager
```

### Access

```csharp
SceneManager.Instance
```

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `CurrentScene` | `Scene` | The currently active scene. `null` until `LoadScene` is called. |

### Methods

| Method | Description |
|--------|-------------|
| `Init()` | No-op placeholder. Call it for clarity but it does nothing currently. |
| `LoadScene(Scene)` | Unloads `CurrentScene` (if any), sets the new scene, and calls `Load()` on it. |
| `Update(GameTime)` | Delegates to `CurrentScene.Update()`. No-op if no scene is loaded. |
| `Draw(GameTime, SpriteBatch)` | Delegates to `CurrentScene.Draw()`. No-op if no scene is loaded. |

---

## SpriteRenderer

**Namespace:** `MonoGameUtilities.Core.Components`

Renders a `Texture2D` at the owner's `Transform.Position` and `Transform.Scale`.

```csharp
public class SpriteRenderer : Component
```

### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Texture` | `Texture2D` | `null` | The texture to draw. If `null`, nothing is rendered. |
| `Color` | `Color` | `Color.White` | Tint colour applied to the texture. |
| `SpriteEffects` | `SpriteEffects` | `SpriteEffects.None` | Flip flags. Set automatically by `Entity.Update()` based on velocity. |

---

## PhysicsBody

**Namespace:** `MonoGameUtilities.Core.Components`

Moves the owner's `Transform.Position` by `Velocity` every frame. No gravity or friction.

```csharp
public class PhysicsBody : Component
```

### Members

| Member | Type | Description |
|--------|------|-------------|
| `Velocity` | `Vector2` (field) | Applied to `Transform.Position` each `Update`. Set to `Vector2.Zero` on `Init`. |
| `Speed` | `float` (property) | Read-only. Returns `Velocity.Length()` — the scalar magnitude. |

### Example

```csharp
// Move right at 3 pixels per frame
_physicsBody.Velocity = new Vector2(3, 0);

// Stop
_physicsBody.Velocity = Vector2.Zero;
```

---

## BoxCollider

**Namespace:** `MonoGameUtilities.Core.Components`

Axis-aligned bounding box (AABB) derived from the owner's position plus an optional offset.

```csharp
public class BoxCollider : Collider
```

### Fields

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `Offset` | `Vector2` | `Vector2.Zero` | Shifts the collider relative to `Transform.Position`. |
| `Size` | `Vector2` | `Vector2.Zero` | Width and height of the box. Set automatically by `Entity.Init()` from the texture. |

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Min` | `Vector2` | Top-left corner: `Position + Offset`. |
| `Max` | `Vector2` | Bottom-right corner: `Position + Offset + Size`. |

### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Intersects(BoxCollider other)` | `bool` | Returns `true` if this collider's AABB overlaps `other`. Returns `false` if `other` is `null`. |

```csharp
if (_boxCollider.Intersects(enemyCollider))
{
    // handle collision
}
```

---

## Collider

**Namespace:** `MonoGameUtilities.Core.Components`

Abstract intermediate class between `Component` and `BoxCollider`. It is an empty abstract class — it exists as a type constraint so you can `GetComponent<Collider>()` when you don't care which shape is attached. **Do not instantiate `Collider` directly**; always use `BoxCollider`.

```csharp
public abstract class Collider : Component
```
