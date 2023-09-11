using SandKing.Engine.Core;
using SandKing.FallingSand;
using SandKing.FallingSand.Materials;
using SFML.Window;
using System.Numerics;

namespace SandKing.Engine
{
    public class DebugModeScript : Script
    {
        public DebugModeScript() : base()
        {
            Engine.Input.SubscribeKeyPressed((sender, e) => ToggleDebug(e));
            Engine.Input.SubscribeMousePressed((sender, e) => StartPlace(e));
            Engine.Input.SubscribeMouseReleased((sender, e) => StopPlace(e));
        }

        public bool PlacingMaterials { get; protected set; }

        public override void Run()
        {
            if (PlacingMaterials)
            {
                PlaceMaterial();
            }
        }

        public static void PlaceMaterial()
        {
            var mousePosition = Engine.Input.GetMousePosition();
            Engine.Simulation.Add(new Sand(new Vector2(mousePosition.X / Material.Size, mousePosition.Y / Material.Size)));
        }

        protected virtual void ToggleDebug(KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.F1)
            {
                Engine.Debug = !Engine.Debug;
            }
        }

        protected void StartPlace(MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                PlacingMaterials = true;
            }
        }

        protected void StopPlace(MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                PlacingMaterials = false;
            }
        }
    }
}
