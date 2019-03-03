using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MobileCreatureGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Camera camera;
        RenderTarget2D rt;
        Rectangle displayRectangle;

        public static Texture2D textureMap;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            textureMap = Content.Load<Texture2D>("grayscale");
            map = new Map();
            camera = new Camera();
            camera.Zoom = 1f;
            rt = new RenderTarget2D(GraphicsDevice, 256, 256);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 1024;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredDepthStencilFormat = DepthFormat.None;
            graphics.PreferredBackBufferFormat = SurfaceFormat.Bgra5551;
            graphics.ApplyChanges();
            displayRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                camera.Move(new Vector2(-2, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                camera.Move(new Vector2(2, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                camera.Move(new Vector2(0, -2));
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                camera.Move(new Vector2(0, 2));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            GraphicsDevice.SetRenderTarget(rt);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformation());
            map.Draw(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(rt, displayRectangle, Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
