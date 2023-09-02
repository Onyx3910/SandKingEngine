using Microsoft.Graphics.Canvas.UI.Xaml;
using SandKing.Simulation;
using SandKing.Simulation.Materials.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SandKing.Engine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ISimulation _fallingSand = new FallingSand(debug: false);

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += (window, args) =>
            {
                if (args.VirtualKey == Windows.System.VirtualKey.Number0)
                    ((FallingSand)_fallingSand).MaterialToPlace = MaterialType.None;
                if (args.VirtualKey == Windows.System.VirtualKey.Number1)
                    ((FallingSand)_fallingSand).MaterialToPlace = MaterialType.Sand;
                if (args.VirtualKey == Windows.System.VirtualKey.Number2)
                    ((FallingSand)_fallingSand).MaterialToPlace = MaterialType.Water;
                if (args.VirtualKey == Windows.System.VirtualKey.Number3)
                    ((FallingSand)_fallingSand).MaterialToPlace = MaterialType.Stone;
            };

            Window.Current.CoreWindow.KeyUp += (window, args) =>
            {
                if (args.VirtualKey == Windows.System.VirtualKey.Tab)
                    _fallingSand.ToggleDebug();
            };

            Window.Current.CoreWindow.PointerPressed += (window, args) =>
            {
                ((FallingSand)_fallingSand).MouseDown = true;
            };

            Window.Current.CoreWindow.PointerReleased += (window, args) =>
            {
                ((FallingSand)_fallingSand).MouseDown = false;
            };
        }

        private void Simulation_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            _fallingSand.Simulate();
            sender.Invalidate();
        }

        private void Chunk_0x0_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[0, 0].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_0x1_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[0, 1].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_0x2_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[0, 2].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_0x3_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[0, 3].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_1x0_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[1, 0].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_1x1_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[1, 1].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_1x2_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[1, 2].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_1x3_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[1, 3].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_2x0_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[2, 0].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_2x1_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[2, 1].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_2x2_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[2, 2].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_2x3_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[2, 3].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_3x0_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[3, 0].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_3x1_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[3, 1].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_3x2_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[3, 2].Draw(session);
            }
            sender.Invalidate();
        }

        private void Chunk_3x3_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                _fallingSand.Chunks[3, 3].Draw(session);
            }
            sender.Invalidate();
        }
    }
}
