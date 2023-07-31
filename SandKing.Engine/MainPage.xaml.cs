using Microsoft.Graphics.Canvas.UI.Xaml;
using SandKing.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SandKing.Engine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ISimulation _fallingSand = new FallingSand(3, 3);

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += (window, args) =>
            {
                if (args.VirtualKey == Windows.System.VirtualKey.Number0)
                    ((FallingSand)_fallingSand).CellToPlace = Elements.Empty;
                if (args.VirtualKey == Windows.System.VirtualKey.Number1)
                    ((FallingSand)_fallingSand).CellToPlace = Elements.Sand;
                if (args.VirtualKey == Windows.System.VirtualKey.Number2)
                    ((FallingSand)_fallingSand).CellToPlace = Elements.Water;
                if (args.VirtualKey == Windows.System.VirtualKey.Number3)
                    ((FallingSand)_fallingSand).CellToPlace = Elements.Stone;
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

        public void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            // Get the CanvasDrawingSession for rendering.
            using (var session = args.DrawingSession)
            {
                _fallingSand.Simulate(session);
                sender.Invalidate();
            }
        }
    }
}
