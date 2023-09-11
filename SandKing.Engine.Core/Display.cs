using SFML.Graphics;
using SFML.Window;

namespace SandKing.Engine.Core
{
    public class Display : RenderWindow
    {
        public Display(uint width, uint height, string title) : base(new VideoMode(width, height), title)
        {
            Closed += (sender, e) => Close();
            Resized += (sender, e) => OnResized(e);
            RenderTexture = new RenderTexture(width, height);
        }

        public RenderTexture RenderTexture { get; private set; }
        public Sprite Sprite => new(RenderTexture.Texture);

        public void Update()
        {
            DispatchEvents();
            Clear(Color.Black);
            RenderTexture.Display();
            Draw(Sprite);
            Display();
            RenderTexture.Clear(Color.Black);
        }

        public void Render(Drawable drawable)
        {
            RenderTexture.Draw(drawable);
        }

        private void OnResized(SizeEventArgs e)
        {
            SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            RenderTexture = new RenderTexture(e.Width, e.Height);
        }
    }
}
