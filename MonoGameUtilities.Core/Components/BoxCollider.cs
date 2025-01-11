using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameUtilities.Core.Scene;

namespace MonoGameUtilities.Core.Components
{
    public class BoxCollider : Collider
    {
        public Vector2 Offset = Vector2.Zero;
        public Vector2 Size = Vector2.Zero;

        public Vector2 Min { get { return new Vector2(_sceneObject.Transform.Position.X + Offset.X, _sceneObject.Transform.Position.Y + Offset.Y); } }
        public Vector2 Max { get { return new Vector2(_sceneObject.Transform.Position.X + Offset.X + Size.X, _sceneObject.Transform.Position.Y + Offset.Y + Size.Y); } }

        public override void Init(SceneObject parent)
        {
            base.Init(parent);

            // Sceneobject that has this component should set this.Size to the size of e.g.: the Texture
        }
        public override void Update(GameTime time)
        {

        }
        public override void Draw(GameTime time, SpriteBatch sb)
        {

        }
        public override void Destroy()
        {

        }

        // public bool Intersects()
        // {
        //     foreach (var c in CollisionManager.Instance.Collider)
        //     {
        //         if (c == null) break;
        //         if (c == this) continue;

        //         if (Min.X < c.Max.X)
        //             if (Max.X > c.Min.X)
        //                 if (Min.Y < c.Max.Y)
        //                     if (Max.Y > c.Min.Y) return true;
        //     }

        //     return false;
        // }
    }
}
