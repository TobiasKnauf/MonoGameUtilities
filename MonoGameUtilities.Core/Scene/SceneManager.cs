using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameUtilities.Core.Scene
{
    public class SceneManager
    {
        private static SceneManager instance;
        public static SceneManager Instance { get { instance ??= new SceneManager(); return instance; } }

        public Scene CurrentScene { get; private set; }

        public void Init() { }

        public void LoadScene(Scene scene)
        {
            if (CurrentScene != null)
                CurrentScene.Unload();

            CurrentScene = scene;
            CurrentScene.Load();
        }

        public void Draw(GameTime time, SpriteBatch sb)
        {
            if (CurrentScene == null) return;

            CurrentScene.Draw(time, sb);
        }
        public void Update(GameTime time)
        {
            if (CurrentScene == null) return;

            CurrentScene.Update(time);
        }
    }
}
