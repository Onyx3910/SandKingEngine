using SandKing.Graphics;
using SFML.System;
using SFML.Window;

namespace SandKing.Engine
{
    public class Input
    {
        public Input(Display display)
        {
            Display = display;
        }

        protected Display Display { get; set; }

        public static bool IsKeyPressed(Keyboard.Key key)
        {
            return Keyboard.IsKeyPressed(key);
        }

        public static bool IsMouseButtonPressed(Mouse.Button button)
        {
            return Mouse.IsButtonPressed(button);
        }

        public Vector2i GetMousePosition()
        {
            return Mouse.GetPosition(Display);
        }

        public static Vector2i GetMousePosition(Window relativeTo)
        {
            return Mouse.GetPosition(relativeTo);
        }

        public void SetMousePosition(Vector2i position)
        {
            Mouse.SetPosition(position, Display);
        }

        public static void SetMousePosition(Vector2i position, Window relativeTo)
        {
            Mouse.SetPosition(position, relativeTo);
        }
    }
}
