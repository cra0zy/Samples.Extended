using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;

namespace Samples.Extended.Samples
{
    public class TiledMapsSample : SampleGame
    {
        // ReSharper disable once NotAccessedField.Local
        private SpriteBatch _spriteBatch;
        private BitmapFont _bitmapFont;
        private TiledMap _tiledMap;
        private Camera2D _camera;
        
        public TiledMapsSample(Game1 game) : base (game)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _bitmapFont = Content.Load<BitmapFont>("montserrat-32");
            _tiledMap = Content.Load<TiledMap>("level01");

            var viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, 800, 480);
            _camera = new Camera2D(viewportAdapter)
            {
                Zoom = 0.5f,
                Position = new Vector2(_tiledMap.WidthInPixels / 4f, _tiledMap.HeightInPixels / 4f)
            };
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            const float cameraSpeed = 100f;
            const float zoomSpeed = 0.2f;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                _camera.Move(new Vector2(0, -cameraSpeed) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                _camera.Move(new Vector2(-cameraSpeed, 0) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                _camera.Move(new Vector2(0, cameraSpeed) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                _camera.Move(new Vector2(cameraSpeed, 0) * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.R))
                _camera.ZoomIn(zoomSpeed * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.F))
                _camera.ZoomOut(zoomSpeed * deltaSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // you can either draw the entire map in one go
            //_tiledMap.Draw(_camera);

            // or for more control, draw each layer separately
            foreach (var layer in _tiledMap.Layers)
            {
                layer.Draw(_camera);
            }

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_bitmapFont, "WASD/Arrows: move", new Vector2(5, 5), new Color(0.5f, 0.5f, 0.5f));
            _spriteBatch.DrawString(_bitmapFont, "RF: zoom", new Vector2(5, 5 + _bitmapFont.LineHeight), new Color(0.5f, 0.5f, 0.5f));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
