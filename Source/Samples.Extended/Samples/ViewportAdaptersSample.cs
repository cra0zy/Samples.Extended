using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public ViewportAdaptersSample()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // the default viewport adapater is the simplest, it doesn't really do anything special
            // but is used by a Camera2D if no other adapter is specified.
            _defaultViewportAdapter = new DefaultViewportAdapter(GraphicsDevice);

            // the scaling viewport adapter stretches the output to fit in the viewport, ignoring the aspect ratio
            _scalingViewportAdapter = new ScalingViewportAdapter(GraphicsDevice, 800, 480);

            // the boxing viewport adapter uses letterboxing or pillarboxing to maintain aspect ratio
            // it's a little more complicated and needs to listen to the window client size changing event
            _boxingViewportAdapter = new BoxingViewportAdapter(GraphicsDevice, 800, 480);
            Window.ClientSizeChanged += (s, e) => _boxingViewportAdapter.OnClientSizeChanged();
            
            // typically you'll only ever want to use one viewport adapter for a game, but in this demo we'll be 
            // switching between them.
            _currentViewportAdapter = _boxingViewportAdapter;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _backgroundTexture = Content.Load<Texture2D>("bg_sharbi");

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.D))
                _currentViewportAdapter = _defaultViewportAdapter;

            if (keyboardState.IsKeyDown(Keys.S))
                _currentViewportAdapter = _scalingViewportAdapter;

            if (keyboardState.IsKeyDown(Keys.B))
                _currentViewportAdapter = _boxingViewportAdapter;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // when rendering sprites, you'll always work within the bounds of the virtual width and height
            // specified when setting up the viewport adapter. The default MonoGame window is 800x480.
            var destinationRectangle = new Rectangle(0, 0, 800, 480);

            _spriteBatch.Begin(transformMatrix: _currentViewportAdapter.GetScaleMatrix());
            _spriteBatch.Draw(_backgroundTexture, destinationRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
