using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceBlaster.Graphics;
using GameSpecific;
using GameSpecific.LevelGenerator;

namespace SpaceBlaster
{
    public class SpaceBlasterGame : Game
    {
        public static Texture2D TextureAtlas = null;

        public static int virtualResolutionWidth = 1920;
        public static int virtualResolutionHeight = 1080;
        public GraphicsDeviceManager _graphics;
        public Scene.Scene _currentLevel = null;
        public int roomsCleared = 0;
        public static Settings Settings = new Settings();
        public Vector2 screenBounds = new Vector2(0, 0);

        public SpaceBlasterGame()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            TextureAtlas = this.Content.Load<Texture2D>("atlas");

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.ApplyChanges();

            RoomLayouts.Content = this.Content;
            SoundEffectsManager.initialize(this);
            MusicManager.initialize(this);

            this.InitializeTitleScreen();

            var input = new InputManager(this, _graphics);

            Components.Add(new InputManager(this, _graphics));
            Components.Add(new AIHandler(this));
            Components.Add(new PhysicsEngine(this, input));
            Components.Add(new CollisionHandler(this));
            Components.Add(new GameRenderer(this));
            Components.Add(new UIRenderer(this, input));
            Components.Add(new SceneHandler(this));
            Components.Add(new AudioHandler(this));

            Settings.Load();

            MusicManager.setVolume(Settings.MusicVolume / 10f);
            SoundEffectsManager.setVolume(Settings.SFXVolume / 10f);

            base.Initialize();
        }

        public void InitializeTitleScreen()
        {
            var level = new TitleScreen(this);
            this._currentLevel = level;
        }

        public void InitializeLoadoutScreen()
        {
            var level = new LoadoutScreen(this);
            this._currentLevel = level;
        }


        public void InitializeSettings()
        {
            var level = new SettingsScreen(this);
            this._currentLevel = level;
        }

        public void InitializeLevel()
        {
            var level = new Level(this);
            this._currentLevel = level;
            level.GenerateGameplayLevel();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public Matrix getRendererTransformationMatrix()
        {
            Matrix t = Matrix.CreateTranslation(
                new Vector3(
                    -_currentLevel.Camera.Pos.X,
                    -_currentLevel.Camera.Pos.Y,
                    0
                )
            )
            * Matrix.CreateRotationZ(_currentLevel.Camera.Rot)
            * Matrix.CreateScale(new Vector3(_currentLevel.Camera.ScaleX, _currentLevel.Camera.ScaleY, 1.0f));

            return t;
        }

        public Matrix getUIRendererTransformationMatrix()
        {
            Matrix t = Matrix.CreateRotationZ(_currentLevel.Camera.Rot) * Matrix.CreateScale(new Vector3(_currentLevel.Camera.ScaleX, _currentLevel.Camera.ScaleY, 1.0f));
            return t;
        }


        public bool isOnScreen(Vector2 pos, float width = 0, float height = 0)
        {
            var e1 = Vector2.Transform(new Vector2(0, 0), Matrix.Invert(this.getRendererTransformationMatrix()));
            var e4 = new Vector2(_currentLevel.Camera.Pos.X + virtualResolutionWidth, _currentLevel.Camera.Pos.Y + virtualResolutionHeight);
            return
                pos.X + width > e1.X &&
                pos.X - width < e4.X &&
                pos.Y - height < e4.Y &&
                pos.Y + height > e1.Y;
        }

        protected override void Update(GameTime gameTime)
        {
            if (_currentLevel != null && _currentLevel.Camera != null)
                screenBounds = new Vector2(_currentLevel.Camera.Pos.X + virtualResolutionWidth / 2, _currentLevel.Camera.Pos.Y + virtualResolutionHeight / 2);
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