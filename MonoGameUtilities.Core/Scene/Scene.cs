using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameUtilities.Core.Scene
{
    public class Scene
    {
        public List<SceneObject> SceneObjects { get; private set; }
        public Scene()
        {
            SceneObjects = new List<SceneObject>();
        }

        public void Load() => SceneObjects.ForEach(so => so.Init());
        public void Unload() => SceneObjects.ForEach(so => so.Destroy());

        public void Update(GameTime _time)
        {
            foreach (var so in SceneObjects)
            {
                if (so == null) throw new NullReferenceException("A SceneObject has not been found properly");

                if (!so.IsInitialized)
                    so.Init();

                so.Update(_time);
            }
        }
        public void Draw(GameTime _time, SpriteBatch _sb)
        {
            foreach (var so in SceneObjects)
            {
                if (so == null) throw new NullReferenceException("A SceneObject has not been found properly");

                so.Draw(_time, _sb);
            }
        }
        public void SubscribeSceneObject(SceneObject _so)
        {
            if (_so == null) return;

            if (SceneObjects.Contains(_so)) return;

            SceneObjects.Add(_so);
        }
        public void UnsubscribeSceneObject(SceneObject _so)
        {
            if (_so == null) return;

            if (!SceneObjects.Contains(_so)) return;

            SceneObjects.Remove(_so);
        }
    }
}
