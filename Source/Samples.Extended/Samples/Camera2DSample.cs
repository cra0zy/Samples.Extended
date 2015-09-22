using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Samples.Extended.Samples
{
    public class Camera2DSample : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;
        private Texture2D _backgroundSky;
        private Texture2D _backgroundClouds;
        private Texture2D[] _backgroundHills;

        public Camera2DSample()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _camera = new Camera2D(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            _bitmapFont = Content.Load<BitmapFont>("montserrat-32");
            _backgroundSky = Content.Load<Texture2D>("hills-sky");
            _backgroundClouds = Content.Load<Texture2D>("hills-clouds");

            _backgroundHills = new Texture2D[4];
            _backgroundHills[0] = Content.Load<Texture2D>("hills-1");
            _backgroundHills[1] = Content.Load<Texture2D>("hills-2");
            _backgroundHills[2] = Content.Load<Texture2D>("hills-3");
            _backgroundHills[3] = Content.Load<Texture2D>("hills-4");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        private float _cloudsOffset;
        private BitmapFont _bitmapFont;
        private const float _cloudsRepeatWidth = 800;
        private Vector2 _screenToWorldPosition;

        protected override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _cloudsOffset -= deltaTime * 5;

            if (_cloudsOffset < -_cloudsRepeatWidth)
                _cloudsOffset = _cloudsRepeatWidth;

            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // the camera properties of the camera can be conrolled to move, zoom and rotate
            const float movementSpeed = 200;
            const float rotationSpeed = 0.5f;
            const float zoomSpeed = 0.5f;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                _camera.Move(new Vector2(0, -movementSpeed) * deltaTime);

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                _camera.Move(new Vector2(-movementSpeed, 0) * deltaTime);

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                _camera.Move(new Vector2(0, movementSpeed) * deltaTime);

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                _camera.Move(new Vector2(movementSpeed, 0) * deltaTime);

            if (keyboardState.IsKeyDown(Keys.E))
                _camera.Rotation += rotationSpeed * deltaTime;

            if (keyboardState.IsKeyDown(Keys.Q))
                _camera.Rotation -= rotationSpeed * deltaTime;

            if (keyboardState.IsKeyDown(Keys.R))
                _camera.Zoom += zoomSpeed * deltaTime;

            if (keyboardState.IsKeyDown(Keys.F))
                _camera.Zoom -= zoomSpeed * deltaTime;

            _screenToWorldPosition = _camera.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // the camera produces a view matrix that can be applied to any sprite batch
            var transformMatrix = _camera.GetViewMatrix(Vector2.Zero);
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.Draw(_backgroundSky, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_backgroundClouds, new Vector2(-_cloudsOffset, 0), Color.White);
            _spriteBatch.Draw(_backgroundClouds, new Vector2(_cloudsOffset, 10), Color.White);
            _spriteBatch.End();

            for (var layerIndex = 0; layerIndex < 4; layerIndex++)
            {
                // different layers can have a parallax factor applied for a nice depth effect
                var parallaxFactor = Vector2.One * (0.25f * layerIndex);
                var viewMatrix = _camera.GetViewMatrix(parallaxFactor);
                _spriteBatch.Begin(transformMatrix: viewMatrix);
                
                for (var repeatIndex = -3; repeatIndex <= 3; repeatIndex++)
                {
                    var texture = _backgroundHills[layerIndex];
                    var position = new Vector2(repeatIndex * texture.Width, 0);
                    _spriteBatch.Draw(texture, position, Color.White);
                }

                _spriteBatch.End();
            }

            // not all sprite batches need to be affected by the camera
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_bitmapFont, "WASD: move\nRF: zoom in and out", new Vector2(5, 5), Color.DarkBlue);
            _spriteBatch.DrawString(_bitmapFont, string.Format("ScreenToWorld: {0:0}, {1:0}", _screenToWorldPosition.X, _screenToWorldPosition.Y), 
                new Vector2(5, 65), Color.DarkBlue);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}