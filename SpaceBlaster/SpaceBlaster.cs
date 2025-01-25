using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Graphics;
using GameSpecific;
using GameSpecific.LevelGenerator;
using GameEngine;
using GameEngine.Gameplay.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Xml.Linq;

namespace SpaceBlaster
{
    public class SpaceBlasterGame : GameBase
    {
        public static Texture2D TextureAtlas = null;
        public int roomsCleared = 0;
        public static Settings Settings = new Settings();

        public SpaceBlasterGame() : base() {}

        protected override void Initialize()
        {
            base.Initialize();
            TextureAtlas = this.Content.Load<Texture2D>("atlas");

            RoomLayouts.Content = this.Content;

            Settings.Load();

            UIRenderer.ButtonScale = 6;
            UIRenderer.LabelScale = 2;

            MusicManager.AddSong("title_screen", Content.Load<Song>("menu"));
            MusicManager.AddSong("gameplay", Content.Load<Song>("battle"));

            SoundEffectsManager.AddSFX("player_shoots", Content.Load<SoundEffect>("player_shoots"));
            SoundEffectsManager.AddSFX("enemy_spots_player", Content.Load<SoundEffect>("enemy_spots_player"));
            SoundEffectsManager.AddSFX("enemybee_fires", Content.Load<SoundEffect>("enemybee_fires"));
            SoundEffectsManager.AddSFX("health_collected", Content.Load<SoundEffect>("health_collected"));
            SoundEffectsManager.AddSFX("win_collected", Content.Load<SoundEffect>("win_collected"));
            SoundEffectsManager.AddSFX("explosion", Content.Load<SoundEffect>("explosion"));

            MusicManager.setVolume(Settings.MusicVolume / 10f);
            SoundEffectsManager.setVolume(Settings.MusicVolume / 10f);

            this.InitializeTitleScreen();
        }

        public void InitializeTitleScreen()
        {
            var level = new TitleScreen(this);
            this.CurrentScene = level;
        }

        public void InitializeLoadoutScreen()
        {
            var level = new LoadoutScreen(this);
            this.CurrentScene = level;
        }


        public void InitializeSettings()
        {
            var level = new SettingsScreen(this);
            this.CurrentScene = level;
        }

        public void InitializeLevel()
        {
            var level = new Level(this);
            this.CurrentScene = level;
            level.GenerateGameplayLevel();
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
    }
}