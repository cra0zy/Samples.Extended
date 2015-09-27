using Samples.Extended;

namespace Launcher
{
    static class Program
    {
        static void Main()
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
}

