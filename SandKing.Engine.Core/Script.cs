namespace SandKing.Engine.Core
{
    public abstract class Script
    {
        public Script(bool enabled = true)
        {
            Enabled = enabled;
            ScriptManager.Add(this);
        }

        public bool Enabled { get; set; }

        public abstract void Run();
    }
}
