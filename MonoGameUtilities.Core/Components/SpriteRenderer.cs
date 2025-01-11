using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameUtilities.Core.Scene;

namespace MonoGameUtilities.Core.Components
{
    public class SpriteRenderer : Component
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public SpriteEffects SpriteEffects { get; set; }

        public override void Init(SceneObject parent)
        {
            base.Init(parent);

            Color = Color.White;
            SpriteEffects = SpriteEffects.None;
        }
        public override void Update(GameTime time) { }
        public override void Draw(GameTime time, SpriteBatch sb)
        {
            if (Texture == null) return;

            sb.Draw(
                Texture,
                _sceneObject.Transform.Position,
                null,
                Color,
                0f,
                Vector2.Zero,
                _sceneObject.Transform.Scale,
                SpriteEffects,
                0f);
        }
        public override void Destroy() { }
    }
}
