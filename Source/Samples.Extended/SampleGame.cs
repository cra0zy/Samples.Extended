using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Samples.Extended
{
    public class SampleGame
    {
        public Game1 MainGame;

        public SampleGame(Game1 game)
        {
            MainGame = game;
        }

        public void Exit()
        {
            MainGame.currentGame = null;
            MainGame.LoadDefaults();

            UnloadContent();
        }

        public void LoadStuff()
        {
            Initialize();
            MainGame.graphics.ApplyChanges();

            LoadContent();
        }

        public void UpdateStuff(GameTime gameTime)
        {
            Update(gameTime);
        }

        public void DrawStuff(GameTime gameTime)
        {
            Draw(gameTime);
        }

        protected virtual void Initialize() { }

        protected virtual void LoadContent() { }

        protected virtual void UnloadContent() { }

        protected virtual void Update(GameTime gameTime) { }

        protected virtual void Draw(GameTime gameTime) { }
    }
}

