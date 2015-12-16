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
        public Game1()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            LoadDefaults();
        }

        private SpriteBatch _spriteBatch;
        private BitmapFont _bitmapFont;
        private SampleMetadata[] _samples;
        private int _selectedIndex;
        private KeyboardState _previousKeyboardState;

        public SampleGame CurrentSample { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public void Reset()
        {
            CurrentSample = null;
            LoadDefaults();
        }

        private void LoadDefaults()
        {
            Window.Title = "MonoGame.Extended Samples";
            //Window.Icon = new System.Drawing.Icon("Icon.ico");
            Window.AllowUserResizing = false;
            IsMouseVisible = true;

            GraphicsDeviceManager.PreferredBackBufferWidth = 800;
            GraphicsDeviceManager.PreferredBackBufferHeight = 480;
            GraphicsDeviceManager.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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

            _selectedIndex = 0;
        }

        protected override void Update(GameTime gameTime)
        {
            if (CurrentSample != null)
            {
                CurrentSample.OnUpdate(gameTime);
                return;
            }

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down))
            {
                _selectedIndex++;

                if (_selectedIndex == _samples.Length)
                    _selectedIndex--;
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
            {
                _selectedIndex--;

                if (_selectedIndex < 0)
                    _selectedIndex++;
            }
            else if (keyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                var sample = _samples[_selectedIndex].CreateSampleFunction();
                sample.OnLoad();
                CurrentSample = sample;
            }

            _previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (CurrentSample != null)
            {
                CurrentSample.OnDraw(gameTime);
                return;
            }

            GraphicsDevice.Clear(Color.DarkRed);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_bitmapFont, "Samples:", new Vector2(50, 50), new Color(Color.Black, 0.5f), wrapWidth: 750);

            for (var i = 0; i < _samples.Length; i++)
                _spriteBatch.DrawString(_bitmapFont, _samples[i].Name, new Vector2(50, 80 + i * _bitmapFont.LineHeight + 15), (i == _selectedIndex) ? Color.White * 1f : new Color(Color.Black, 0.5f), wrapWidth: 750);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class SampleMetadata
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

