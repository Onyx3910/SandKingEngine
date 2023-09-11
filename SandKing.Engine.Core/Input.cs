using SFML.System;
using SFML.Window;
using System;

namespace SandKing.Engine.Core
{
    public class Input
    {
        public Input(Display display)
        {
            Display = display;
        }

        protected Display Display { get; set; }

        public void SubscribeMouseReleased(EventHandler<MouseButtonEventArgs> handler)
        {
            Display.MouseButtonReleased += handler;
        }

        public void SubscribeMousePressed(EventHandler<MouseButtonEventArgs> handler)
        {
            Display.MouseButtonPressed += handler;
        }

        public void SubscribeMouseMoved(EventHandler<MouseMoveEventArgs> handler)
        {
            Display.MouseMoved += handler;
        }

        public void SubscribeKeyPressed(EventHandler<KeyEventArgs> handler)
        {
            Display.KeyPressed += handler;
        }

        public void SubscribeKeyReleased(EventHandler<KeyEventArgs> handler)
        {
            Display.KeyReleased += handler;
        }

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
