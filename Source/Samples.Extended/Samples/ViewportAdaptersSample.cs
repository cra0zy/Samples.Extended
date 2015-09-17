using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;

namespace Samples.Extended.Samples
{
    public class ViewportAdaptersSample : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _backgroundTexture;
        private ViewportAdapter _currentViewportAdapter;
        private DefaultViewportAdapter _defaultViewportAdapter;
        private ScalingViewportAdapter _scalingViewportAdapter;
        private BoxingViewportAdapter _boxingViewportAdapter;
        private BitmapFont _bitmapFont;

        public ViewportAdaptersSample()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                // this sample sets the starting window to an unusual size to demonstrate the scaling behaviour
                PreferredBackBufferWidth = 900,
                PreferredBackBufferHeight = 700
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.Position = new Point(100, 100);
        }

        protected override void Initialize()
        {
            base.Initialize();

            // the default viewport adapater is the simplest, it doesn't do any scaling at all
            // but is used by a Camera2D if no other adapter is specified.
            // this is often useful if you have a game with a large map and you want the player to see 
            // more of the map on a bigger screen.
            _defaultViewportAdapter = new DefaultViewportAdapter(GraphicsDevice);

            // the scaling viewport adapter stretches the output to fit in the viewport, ignoring the aspect ratio
            // this works well if the aspect ratio doesn't change a lot between devices 
            // or you don't like the black bars of the boxing adapter
            _scalingViewportAdapter = new ScalingViewportAdapter(GraphicsDevice, 800, 480);

            // the boxing viewport adapter uses letterboxing or pillarboxing to maintain aspect ratio
            // it's a little more complicated and needs to listen to the window client size changing event
            _boxingViewportAdapter = new BoxingViewportAdapter(GraphicsDevice, 800, 480);

            Window.ClientSizeChanged += (s, e) => _currentViewportAdapter.OnClientSizeChanged(); 
            
            // typically you'll only ever want to use one viewport adapter for a game, but in this sample we'll be 
            // switching between them.
            _currentViewportAdapter = _boxingViewportAdapter;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _backgroundTexture = Content.Load<Texture2D>("vignette");
            _bitmapFont = Content.Load<BitmapFont>("montserrat-32");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var previousViewportAdapter = _currentViewportAdapter;

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.D))
                _currentViewportAdapter = _defaultViewportAdapter;

            if (keyboardState.IsKeyDown(Keys.S))
                _currentViewportAdapter = _scalingViewportAdapter;

            if (keyboardState.IsKeyDown(Keys.B))
                _currentViewportAdapter = _boxingViewportAdapter;

            // if we've changed the viewport adapter mid game we need to reset the viewport back to the window size
            // this wouldn't normally be required if you're only ever using one viewport adapter
            if (previousViewportAdapter != _currentViewportAdapter)
            {
                GraphicsDevice.Viewport = new Viewport(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
                _currentViewportAdapter.OnClientSizeChanged();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // when rendering sprites, you'll always work within the bounds of the virtual width and height
            // specified when setting up the viewport adapter. The default MonoGame window is 800x480.
            var destinationRectangle = new Rectangle(0, 0, 800, 480);

            _spriteBatch.Begin(transformMatrix: _currentViewportAdapter.GetScaleMatrix());
            _spriteBatch.Draw(_backgroundTexture, destinationRectangle, Color.White);

            _spriteBatch.DrawString(_bitmapFont, string.Format("Press D: {0}", typeof(DefaultViewportAdapter).Name), 
                new Vector2(5, 5), Color.White);

            _spriteBatch.DrawString(_bitmapFont, string.Format("Press S: {0}", typeof(ScalingViewportAdapter).Name), 
                new Vector2(5, 5 + _bitmapFont.LineHeight * 1), Color.White);

            _spriteBatch.DrawString(_bitmapFont, string.Format("Press B: {0}", typeof(BoxingViewportAdapter).Name), 
                new Vector2(5, 5 + _bitmapFont.LineHeight * 2), Color.White);

            _spriteBatch.DrawString(_bitmapFont, string.Format("Current: {0}", _currentViewportAdapter.GetType().Name), 
                new Vector2(5, 5 + _bitmapFont.LineHeight * 4), Color.Black);

            _spriteBatch.DrawString(_bitmapFont, @"Try resizing the window",
                    new Vector2(5, 5 + _bitmapFont.LineHeight * 6), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
