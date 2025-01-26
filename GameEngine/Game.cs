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

        public GameBase()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            SoundEffectsManager.Initialize(this);
            MusicManager.Initialize(this);

            var input = new InputManager(this, graphics);

            Components.Add(new InputManager(this, graphics));
            Components.Add(new AIHandler(this));
            Components.Add(new PhysicsEngine(this, input));
            Components.Add(new CollisionHandler(this));
            
            this.Renderer = new GameRenderer(this);
            Components.Add(Renderer);

            RendererUI = new UIRenderer(this, input);
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
            base.Draw(gameTime);
        }

        public void Quit()
        {
            Exit();
        }
    }
}