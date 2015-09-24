using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Samples.Extended
{
    public class SampleGame
    {
        public Game1 MainGame;

        public ContentManager Content { get { return MainGame.Content; } }
        public GraphicsDevice GraphicsDevice { get { return MainGame.GraphicsDevice; } }
        public GameWindow Window { get { return MainGame.Window; } }

        public bool IsMouseVisible
        {
            get { return MainGame.IsMouseVisible; }
            set { MainGame.IsMouseVisible = value; }
        }

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

        public void OnLoad()
        {
            Initialize();
            MainGame.graphics.ApplyChanges();

            LoadContent();
        }

        public void OnUpdate(GameTime gameTime)
        {
            Update(gameTime);
        }

        public void OnDraw(GameTime gameTime)
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

