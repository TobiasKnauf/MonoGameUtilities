using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameUtilities.Core.Scene;


namespace MonoGameUtilities.Core.Components
{
    public abstract class Component
    {
        protected SceneObject _sceneObject { get; private set; }

        public virtual void Init(SceneObject parent)
        {
            _sceneObject = parent;
        }
        public abstract void Update(GameTime time);
        public abstract void Draw(GameTime time, SpriteBatch sb);
        public abstract void Destroy();
    }
}
