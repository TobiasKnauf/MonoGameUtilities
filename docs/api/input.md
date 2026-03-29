# API Reference: Input

Namespace: `MonoGameUtilities.Input`
Assembly: `MonoGameUtilities.Input`

---

## InputHelper

Static utility class for reading movement input from keyboard or gamepad. Returns a normalised direction vector with no smoothing or dead-zone processing.

```csharp
public static class InputHelper
```

---

### Methods

#### `GetRawMovementAxis()`

```csharp
public static Vector2 GetRawMovementAxis()
```

Returns a direction `Vector2` based on current movement input. Prefers keyboard: if **any** keyboard key is currently held, keyboard input is used; otherwise gamepad input is used.

**Returns:** A `Vector2` where each axis is `-1`, `0`, or `1`. Not normalized — diagonal input can return `(-1,-1)` through `(1,1)`.

```csharp
// In an Entity's Update:
public override void Update(GameTime time)
{
    base.Update(time);

    Vector2 input = InputHelper.GetRawMovementAxis();
    _physicsBody.Velocity = input * 3f; // 3 pixels per frame
}
```

---

#### `GetKeyboardAxis()`

```csharp
public static Vector2 GetKeyboardAxis()
```

Reads WASD and arrow keys directly.

| Input | Effect |
|-------|--------|
| W / Up | `Y = -1` (screen Y is top-to-bottom) |
| S / Down | `Y = 1` |
| D / Right | `X = 1` |
| A / Left | `X = -1` |

Vertical and horizontal axes are independent; holding both W and D gives `(1, -1)`.

---

#### `GetGamepadAxis()`

```csharp
public static Vector2 GetGamepadAxis()
```

Reads the left thumbstick of `PlayerIndex.One`. Thumbstick values are thresholded to integer steps (`-1`, `0`, or `1`) — no analogue sensitivity.

| Thumbstick | X result | Y result |
|------------|----------|----------|
| Left (`< 0`) | `-1` | — |
| Right (`> 0`) | `1` | — |
| Up (`Y > 0`) | — | `-1` (inverted: gamepad Y-up → screen Y-down) |
| Down (`Y < 0`) | — | `1` |

> **Gamepad Y-axis inversion:** MonoGame reports thumbstick Y as positive-up, but screen coordinates are positive-down. `GetGamepadAxis()` flips this so the result matches `GetKeyboardAxis()`.

---

### Limitations

- No dead-zone filtering on the gamepad (any non-zero value maps to ±1).
- No analogue values — always returns integer steps.
- Only `PlayerIndex.One` is supported for gamepad input.
- No support for button input (A/B/X/Y, triggers, bumpers, etc.).
- Keyboard has priority over gamepad: if any key is pressed, gamepad is ignored entirely.
