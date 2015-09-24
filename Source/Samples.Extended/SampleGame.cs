﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Samples.Extended
{
    public class SampleGame
    {
        public SampleGame(Game1 game)
        {
            _mainGame = game;
        }

        private readonly Game1 _mainGame;

        public ContentManager Content { get { return _mainGame.Content; } }
        public GraphicsDevice GraphicsDevice { get { return _mainGame.GraphicsDevice; } }
        public GameWindow Window { get { return _mainGame.Window; } }

        public bool IsMouseVisible
        {
            get { return _mainGame.IsMouseVisible; }
            set { _mainGame.IsMouseVisible = value; }
        }

        public void Exit()
        {
            _mainGame.Reset();
            UnloadContent();
        }

        public void OnLoad()
        {
            Initialize();
            _mainGame.GraphicsDeviceManager.ApplyChanges();

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

