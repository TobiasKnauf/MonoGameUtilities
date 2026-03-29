# ECS Overview

MonoGameUtilities adds a lightweight composition-based object model on top of MonoGame. MonoGame's `Game` class gives you `Update` and `Draw` loops, but no built-in concept of game objects, components, or scenes — this library provides that layer.

---

## Core Concepts

| Type | Role |
|------|------|
| `SceneObject` | The "game object" — a container that holds components and has a `Transform` |
| `Component` | A reusable behaviour (rendering, physics, collision) attached to a `SceneObject` |
| `Entity` | A `SceneObject` subclass pre-wired with `SpriteRenderer`, `PhysicsBody`, and `BoxCollider` |
| `Scene` | A list of `SceneObject`s; owns their collective update/draw loop |
| `SceneManager` | A singleton that holds the `CurrentScene` and delegates `Update`/`Draw` from `Game1` |

### Relationship diagram

```
Game1
 └─ SceneManager (singleton)
     └─ CurrentScene : Scene
         ├─ SceneObject  ←─ has Transform
         │   └─ Component[]
         ├─ Entity : SceneObject   ←─ pre-adds 3 components
         │   ├─ SpriteRenderer
         │   ├─ PhysicsBody
         │   └─ BoxCollider : Collider : Component
         └─ ...
```

---

## Object Lifecycle

### 1. Construction

When a `SceneObject` (or `Entity`) is constructed, it immediately calls:

```csharp
SceneManager.Instance.CurrentScene.SubscribeSceneObject(this);
```

This means **a scene must be loaded before you instantiate any object**. See [Getting Started](../getting-started.md).

`Entity`'s constructor also calls `AddComponent<T>()` for each of its three built-in components. Each component's `Init(SceneObject parent)` is called at that point, giving the component a back-reference to its owner.

### 2. Init

`Scene.Update()` checks `IsInitialized` on every object each frame. If an object hasn't been initialized yet, it calls `Init()` before `Update()`:

```csharp
if (!so.IsInitialized)
    so.Init();
so.Update(_time);
```

`Entity.Init()` uses this moment to size the `BoxCollider` from the texture:

```csharp
if (_spriteRenderer.Texture != null)
    _boxCollider.Size = new Vector2(
        _spriteRenderer.Texture.Width,
        _spriteRenderer.Texture.Height);
```

### 3. Update

Each frame, `SceneManager.Update()` → `Scene.Update()` → each `SceneObject.Update()` → each `Component.Update()`.

`Entity.Update()` also flips the sprite horizontally based on horizontal velocity:

```csharp
if (_physicsBody.Velocity.X < 0) _spriteRenderer.SpriteEffects = SpriteEffects.FlipHorizontally;
else if (_physicsBody.Velocity.X > 0) _spriteRenderer.SpriteEffects = SpriteEffects.None;
```

### 4. Draw

`SceneManager.Draw()` → `Scene.Draw()` → each `SceneObject.Draw()` → each `Component.Draw()`.

`SpriteRenderer.Draw()` calls `SpriteBatch.Draw()` using `Transform.Position` and `Transform.Scale`.

### 5. Destroy

`SceneObject.Destroy()` calls `Destroy()` on all components, then unsubscribes from the current scene:

```csharp
SceneManager.Instance.CurrentScene.UnsubscribeSceneObject(this);
```

### Full sequence

```
new MyEntity()
  └─ SceneObject() → SubscribeSceneObject
  └─ AddComponent<SpriteRenderer/PhysicsBody/BoxCollider>

Frame N (first frame after creation)
  └─ Scene.Update → Init() → IsInitialized = true
  └─ Scene.Update → Update()
  └─ Scene.Draw  → Draw()

myEntity.Destroy()
  └─ Component[].Destroy()
  └─ UnsubscribeSceneObject
```

---

## SceneObject vs Entity

Use `SceneObject` when you want to manage components yourself (e.g., a non-visual trigger zone, a camera controller, a UI element):

```csharp
public class CameraController : SceneObject
{
    private PhysicsBody _body;

    public CameraController() : base()
    {
        _body = AddComponent<PhysicsBody>();
    }
}
```

Use `Entity` when you need a visible, moving, collidable game object. It saves you three `AddComponent` calls and auto-sizes the collider.

---

## Scene Transitions

`SceneManager.LoadScene(scene)` unloads the current scene (calls `Destroy()` on all its objects) before activating the new one:

```csharp
// Transition from gameplay to a menu
SceneManager.Instance.LoadScene(new MainMenuScene());
```

---

## Known Limitations

| Area | Status |
|------|--------|
| **Collision detection** | `BoxCollider.Intersects(other)` detects overlap but does not resolve it — no push-back or response |
| **Transform.Translate()** | Implemented as `Position += direction * time` |
| **Collider base class** | Abstract empty class — always use `BoxCollider` concretely |
| **No transform hierarchy** | No parent-child relationships between `SceneObject`s |
| **No object pooling** | Components are instantiated via `Activator.CreateInstance` each time |
| **PhysicsBody** | No gravity, drag, friction, or mass — velocity is applied raw each frame |
| **Gamepad support** | Only `PlayerIndex.One`; no button input, only left thumbstick axis |
