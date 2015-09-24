using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.BitmapFonts;

using Samples.Extended.Samples;

namespace Samples.Extended
{
    public class Game1 : Game
    {
        public SampleGame currentGame;
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BitmapFont _bitmapFont;
        SampleMetadata[] _samples;
        int _selected;
        KeyboardState _prevKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            LoadDefaults();
        }

        public void LoadDefaults()
        {
            Window.Title = "MonoGame.Extended Samples";
            Window.Icon = new System.Drawing.Icon("Icon.ico");
            Window.AllowUserResizing = false;
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _bitmapFont = Content.Load<BitmapFont>("montserrat-32");

            _samples = new []
                {
                    new SampleMetadata("Bitmap Fonts", () => new BitmapFontsSample(this)),
                    new SampleMetadata("Sprites", () => new SpritesSample(this)),
                    new SampleMetadata("Input Listeners", () => new InputListenersSample(this)),
                    new SampleMetadata("Camera2D", () => new Camera2DSample(this)),
                    new SampleMetadata("Viewport Adapters", () => new ViewportAdaptersSample(this)),
                    new SampleMetadata("Tiled Maps", () => new TiledMapsSample(this))
                }
                .OrderBy(i => i.Name)
                .ToArray();

            _selected = 0;
        }

        protected override void Update(GameTime gameTime)
        {
            if (currentGame != null)
            {
                currentGame.OnUpdate(gameTime);
                return;
            }

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Down) && !_prevKeyboardState.IsKeyDown(Keys.Down))
            {
                _selected++;
                if (_selected == _samples.Length)
                    _selected--;
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && !_prevKeyboardState.IsKeyDown(Keys.Up))
            {
                _selected--;
                if (_selected < 0)
                    _selected++;
            }
            else if (keyboardState.IsKeyDown(Keys.Enter) && !_prevKeyboardState.IsKeyDown(Keys.Enter))
            {
                var tmpGame = _samples[_selected].CreateSampleFunction();
                tmpGame.OnLoad();

                currentGame = tmpGame;
            }

            _prevKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (currentGame != null)
            {
                currentGame.OnDraw(gameTime);
                return;
            }

            GraphicsDevice.Clear(Color.DarkRed);

            spriteBatch.Begin();

            BitmapFontExtensions.DrawString(spriteBatch, _bitmapFont, "Samples:", new Vector2(50, 50), new Color(Color.Black, 0.5f), wrapWidth: 750);
            for (int i = 0; i < _samples.Length; i++)
                spriteBatch.DrawString(_bitmapFont, _samples[i].Name, new Vector2(50, 80 + i * _bitmapFont.LineHeight + 15), (i == _selected) ? Color.White * 1f : new Color(Color.Black, 0.5f), wrapWidth: 750);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    class SampleMetadata
    {
        public SampleMetadata(string name, Func<SampleGame> createSampleFunction)
        {
            Name = name;
            CreateSampleFunction = createSampleFunction;
        }

        public string Name { get; private set; }
        public Func<SampleGame> CreateSampleFunction { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

