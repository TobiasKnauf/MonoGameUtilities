using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameUtility.Graphics
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        public static ScreenManager Instance { get { instance ??= new ScreenManager(); return instance; } }

        public static GraphicsDeviceManager Graphics;
        public static GraphicsDevice GraphicsDevice => Graphics.GraphicsDevice;

        public const int NATIVE_RESOLUTION_WIDTH = 640;
        public const int NATIVE_RESOLUTION_HEIGHT = 360;
        public const int TARGET_RESOLUTION_WIDTH = 1920;
        public const int TARGET_RESOLUTION_HEIGHT = 1080;
        public int ResolutionScale => TARGET_RESOLUTION_WIDTH / NATIVE_RESOLUTION_WIDTH;

        private RenderTarget2D _rt;

        public void Init()
        {
            Graphics.PreferredBackBufferWidth = TARGET_RESOLUTION_WIDTH;
            Graphics.PreferredBackBufferHeight = TARGET_RESOLUTION_HEIGHT;
            Graphics.ApplyChanges();

            _rt = new RenderTarget2D(Graphics.GraphicsDevice, NATIVE_RESOLUTION_WIDTH, NATIVE_RESOLUTION_HEIGHT);
        }

        public void Update(GameTime _time)
        {
        }

        public void Draw(GameTime _time, SpriteBatch _sb)
        {

        }
        public void ResetScreen()
        {
            Graphics.GraphicsDevice.SetRenderTarget(_rt);
            Graphics.GraphicsDevice.Clear(new Color(35, 36, 41, 255));
        }
        public void DrawRenderTarget(SpriteBatch _sb)
        {
            Rectangle rect = new Rectangle(0, 0, NATIVE_RESOLUTION_WIDTH * ResolutionScale, NATIVE_RESOLUTION_HEIGHT * ResolutionScale);

            Graphics.GraphicsDevice.SetRenderTarget(null);
            _sb.Begin(samplerState: SamplerState.PointClamp);
            _sb.Draw(_rt, rect, Color.White);
            _sb.End();
        }
    }
}
