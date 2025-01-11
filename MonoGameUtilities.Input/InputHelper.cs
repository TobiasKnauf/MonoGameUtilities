using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MonoGameUtilities.Input
{
    public static class InputHelper
    {
        /// <summary>
        /// Returns a Vector2 based on the Movement Input given with no smoothing applied
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetRawMovementAxis()
        {
            Vector2 desiredVelocity;

            // Get axis movement
            if (Keyboard.GetState().GetPressedKeys().Length > 0)
                desiredVelocity = GetKeyboardAxis();
            else
                desiredVelocity = GetGamepadAxis();

            return desiredVelocity;
        }

        public static Vector2 GetKeyboardAxis()
        {
            Vector2 returnedVector = Vector2.Zero;
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) returnedVector.Y = -1;
            else if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down)) returnedVector.Y = 1;
            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right)) returnedVector.X = 1;
            else if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left)) returnedVector.X = -1;

            return returnedVector;
        }
        public static Vector2 GetGamepadAxis()
        {
            Vector2 returnedVector = Vector2.Zero;
            var state = GamePad.GetState(PlayerIndex.One);

            if (state.ThumbSticks.Left.X > 0) returnedVector.X = 1;
            else if (state.ThumbSticks.Left.X < 0) returnedVector.X = -1;
            if (state.ThumbSticks.Left.Y > 0) returnedVector.Y = -1;
            else if (state.ThumbSticks.Left.Y < 0) returnedVector.Y = 1;

            return returnedVector;
        }
    }
}
