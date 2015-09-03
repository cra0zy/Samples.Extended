using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.InputListeners;

namespace Samples.Extended.Samples
{
    public class InputListenersSample : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _backgroundTexture;
        private BitmapFont _bitmapFont;
        private string _text = string.Empty;

        public InputListenersSample()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            var inputManager = new InputListenerManager();
            var mouseListener = inputManager.AddListener<MouseListener>(new MouseListenerSettings());
            var keyboardListener = inputManager.AddListener<KeyboardListener>(new KeyboardListenerSettings());
            var touchListener = inputManager.AddListener<TouchListener>(new TouchListenerSettings());

            mouseListener.MouseClicked += MouseListenerOnMouseClicked;

            base.Initialize();
        }

        private void MouseListenerOnMouseClicked(object sender, MouseEventArgs mouseEventArgs)
        {
            _text = mouseEventArgs.ToString();
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
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.DrawString(_bitmapFont, _text, Vector2.Zero, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
