using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Graphics;
using GameSpecific;
using GameSpecific.LevelGenerator;
using GameEngine.Scene;
using GameEngine.Gameplay.Audio;
using GameEngine.Gameplay.Input;
using GameEngine.Gameplay.AI;
using GameEngine.Gameplay.Physics;
using GameEngine.Gameplay.Collision;
using GameEngine.Gameplay.Scene;
using System.Diagnostics;
using System;

namespace GameEngine
{
    public class GameBase : Game
    {
        public static int VirtualResolutionWidth = 1920;
        public static int VirtualResolutionHeight = 1080;

        public Scene.Scene CurrentScene = null;

        public GameRenderer Renderer = null;
        public UIRenderer RendererUI = null;

        private GraphicsDeviceManager graphics;
        private RenderTarget2D _renderTarget;
        private SpriteBatch _spriteBatch;
        public static Rectangle targetRect;

        public GameBase()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            IsFixedTimeStep = true;
        }

        private Rectangle CalculateTargetRect()
        {
            Point size = GraphicsDevice.Viewport.Bounds.Size;
            float scaleX = (float)size.X / (float)VirtualResolutionWidth;
            float scaleY = (float)size.Y / (float)VirtualResolutionHeight;
            float scale = Math.Min(scaleX,scaleY);

            Rectangle r = new Rectangle(
                (int)(size.X / 2 - VirtualResolutionWidth * scale /  2),
                (int)(size.Y / 2 - VirtualResolutionHeight * scale / 2),
                (int)((float)VirtualResolutionWidth * scale),
                (int)((float)VirtualResolutionHeight * scale)
            );

            return r;
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Window.AllowAltF4 = true;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.HardwareModeSwitch = false;
            graphics.IsFullScreen = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            _renderTarget = new RenderTarget2D(GraphicsDevice, VirtualResolutionWidth, VirtualResolutionHeight);
            targetRect = CalculateTargetRect();

            SoundEffectsManager.Initialize(this);
            MusicManager.Initialize(this);

            var input = new InputManager(this, graphics);

            Components.Add(new InputManager(this, graphics));
            Components.Add(new AIHandler(this));
            Components.Add(new CollisionHandler(this));
            Components.Add(new PhysicsEngine(this, input));
            
            this.Renderer = new GameRenderer(this, _spriteBatch);
            Components.Add(Renderer);

            RendererUI = new UIRenderer(this, input, _spriteBatch);
            Components.Add(RendererUI);

            Components.Add(new SceneHandler(this));
            Components.Add(new AudioHandler(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);

            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, targetRect, Color.White);
            _spriteBatch.End();
        }

        public void Quit()
        {
            Exit();
        }
    }
}