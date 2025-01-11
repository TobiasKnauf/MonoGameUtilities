using Microsoft.Xna.Framework;

namespace MonoGameUtilities.Core.Types
{
    public class Transform
    {
        public Vector2 Position;
        public Vector2 Scale;

        public Transform()
        {
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
        }
        public Transform(Vector2 _position)
        {
            this.Position = _position;
            this.Scale = Vector2.One;
        }
        public Transform(Vector2 _position, Vector2 _scale)
        {
            this.Position = _position;
            this.Scale = _scale;
        }

        public void Translate(Vector2 _direction, float _time) { }
    }
}
