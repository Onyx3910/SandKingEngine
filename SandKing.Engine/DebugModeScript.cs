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

        public int BrushSize { get; protected set; } = 2;
        public bool PlacingMaterials { get; protected set; }

        public override void Run()
        {
            if (PlacingMaterials)
            {
                PlaceMaterial();
            }
        }

        public void PlaceMaterial()
        {
            var mousePosition = Engine.Input.GetMousePosition();
            var gridPosition = new Vector2(mousePosition.X / Material.Size, mousePosition.Y / Material.Size);
            for(var x = gridPosition.X - BrushSize / 2; x < gridPosition.X + BrushSize; x++)
            {
                for (var y = gridPosition.Y - BrushSize / 2; y < gridPosition.Y + BrushSize; y++)
                {
                    Engine.Simulation.Add(new Sand(new Vector2(x, y)));
                }
            }
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
            if (Engine.Debug && e.Button == Mouse.Button.Left)
            {
                PlacingMaterials = true;
            }
        }

        protected void StopPlace(MouseButtonEventArgs e)
        {
            if (Engine.Debug && e.Button == Mouse.Button.Left)
            {
                PlacingMaterials = false;
            }
        }
    }
}
