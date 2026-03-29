using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameUtilities.Core.Components;
using MonoGameUtilities.Graphics;

namespace MonoGameUtilities.Debug
{
    public static class Debug
    {
        public static void ShowColliderOutlines(BoxCollider _col, SpriteBatch _sb)
        {
            var Min = _col.Min;
            var Max = _col.Max;
            var Size = _col.Size;

            // Draw the debug outline of the box collider
            Rectangle r = new Rectangle((int)Min.X, (int)Min.Y, (int)Size.X, (int)Size.Y);

            Texture2D pixel = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            // Draw top line
            _sb.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, 1), Color.Red);
            // Draw bottom line
            _sb.Draw(pixel, new Rectangle(r.Left, r.Bottom, r.Width, 1), Color.Red);
            // Draw left line
            _sb.Draw(pixel, new Rectangle(r.Left, r.Top, 1, r.Height), Color.Red);
            // Draw right line
            _sb.Draw(pixel, new Rectangle(r.Right, r.Top, 1, r.Height), Color.Red);
        }
    }
}
