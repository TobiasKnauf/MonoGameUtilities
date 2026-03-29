# API Reference: Commons

Namespace: `MonoGameUtilities.Commons`
Assembly: `MonoGameUtilities.Commons`

---

## Extensions

Static class providing extension methods for common collection checks.

```csharp
public static class Extensions
```

---

### `IsNullOrEmpty<T>(ICollection<T>)`

```csharp
public static bool IsNullOrEmpty<T>(this ICollection<T> list)
```

Returns `true` if the collection is `null` or contains no elements.

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `list` | `ICollection<T>` | The collection to check. |

**Returns:** `true` if `list == null` or `list.Count == 0`; otherwise `false`.

---

### When to Use vs LINQ

| Scenario | Recommended |
|----------|-------------|
| Null-safe empty check on a field/property that may be `null` | `IsNullOrEmpty()` |
| Collection is guaranteed non-null | `list.Count == 0` or `!list.Any()` |
| Checking an `IEnumerable<T>` (not `ICollection<T>`) | `!enumerable.Any()` |

`IsNullOrEmpty` uses `Count` (O(1) for `List<T>`) rather than `Any()` (which enumerates), so it's slightly more efficient for `List` and other `ICollection` types.

---

### Example

```csharp
using MonoGameUtilities.Commons;

if (_components.IsNullOrEmpty())
    throw new InvalidOperationException("No components attached.");
```
