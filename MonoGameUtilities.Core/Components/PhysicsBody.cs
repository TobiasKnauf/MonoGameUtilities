using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameUtilities.Core.Scene;

namespace MonoGameUtilities.Core.Components
{
    public class PhysicsBody : Component
    {
        public Vector2 Velocity;
        public float Speed { get { return Velocity.Length(); } }

        public override void Init(SceneObject parent)
        {
            base.Init(parent);

            Velocity = Vector2.Zero;
        }
        public override void Update(GameTime time)
        {
            _sceneObject.Transform.Position += Velocity;
        }
        public override void Draw(GameTime time, SpriteBatch sb)
        {

        }
        public override void Destroy()
        {

        }
    }
}
