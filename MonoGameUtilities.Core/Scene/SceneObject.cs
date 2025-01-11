using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUtilities.Commons;
using MonoGameUtilities.Core.Components;
using MonoGameUtilities.Core.Types;

namespace MonoGameUtilities.Core.Scene
{
    public class SceneObject
    {
        public bool IsInitialized { get; private set; }

        public Transform Transform { get; private set; }

        protected List<Component> _components;

        public SceneObject()
        {
            SceneManager.Instance.CurrentScene.SubscribeSceneObject(this);

            Transform = new Transform();
            _components = new List<Component>();
        }

        public virtual void Init()
        {
            IsInitialized = true;
        }
        public virtual void Update(GameTime _time)
        {
            _components.ForEach(c => c.Update(_time));
        }
        public virtual void Draw(GameTime _time, SpriteBatch _sb)
        {
            _components.ForEach(c => c.Draw(_time, _sb));
        }
        public virtual void Destroy()
        {
            _components.ForEach(c => c.Destroy());

            SceneManager.Instance.CurrentScene.UnsubscribeSceneObject(this);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            // Create a new component with a reference to this sceneobject
            T? newComponent = Activator.CreateInstance(typeof(T)) as T;
            if (newComponent == null) throw new NullReferenceException("Component could not be added to the SceneObject");

            newComponent.Init(this);
            _components.Add(newComponent);
            return newComponent;
        }
        public T GetComponent<T>() where T : Component
        {
            if (_components.IsNullOrEmpty()) throw new NullReferenceException("Component list is either null or no components are attached");

            return _components.OfType<T>().FirstOrDefault();
        }
        public bool TryGetComponent<T>(out T _component) where T : Component
        {
            _component = GetComponent<T>();
            return _component != null;
        }
    }
}
