using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameUtilities.Core.Scene;
using MonoGameUtilities.Core.Components;

namespace MonoGameUtilities.Core.Entities
{
    public abstract class Entity : SceneObject
    {
        protected SpriteRenderer _spriteRenderer;
        protected PhysicsBody _physicsBody;
        protected BoxCollider _boxCollider;

        public Entity() : base()
        {
            // Add components
            _spriteRenderer = AddComponent<SpriteRenderer>();
            _physicsBody = AddComponent<PhysicsBody>();
            _boxCollider = AddComponent<BoxCollider>();
        }

        public override void Init()
        {
            base.Init();

            if (_spriteRenderer.Texture != null)
                _boxCollider.Size = new Vector2(_spriteRenderer.Texture.Width, _spriteRenderer.Texture.Height);
        }

        public override void Update(GameTime _time)
        {
            base.Update(_time);

            if (_physicsBody.Velocity.X < 0) _spriteRenderer.SpriteEffects = SpriteEffects.FlipHorizontally;
            else if (_physicsBody.Velocity.X > 0) _spriteRenderer.SpriteEffects = SpriteEffects.None;
        }
        public override void Draw(GameTime _time, SpriteBatch _sb)
        {
            base.Draw(_time, _sb);
        }
        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
