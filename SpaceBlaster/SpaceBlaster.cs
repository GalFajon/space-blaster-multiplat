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
using System.Diagnostics;

namespace SpaceBlaster
{
    public class SpaceBlasterGame : GameBase
    {
        public static Texture2D TextureAtlas = null;
        public static Texture2D Background = null;
        public static Texture2D BackgroundTitle = null;
        public static Texture2D BackgroundMenu = null;
        public static Texture2D Intro1 = null;
        public static Texture2D Intro2 = null;
        public static Texture2D Intro3 = null;
        public static Texture2D Intro4 = null;

        public int roomsCleared = 0;
        public static Settings Settings = new Settings();

        public SpaceBlasterGame() : base() {}

        protected override void Initialize()
        {
            base.Initialize();
            TextureAtlas = this.Content.Load<Texture2D>("atlas");

            Background = this.Content.Load<Texture2D>("background");
            BackgroundTitle = this.Content.Load<Texture2D>("background_title");
            BackgroundMenu = this.Content.Load<Texture2D>("background_menu");
            Intro1 = this.Content.Load<Texture2D>("intro_1");
            Intro2 = this.Content.Load<Texture2D>("intro_2");
            Intro3 = this.Content.Load<Texture2D>("intro_3");
            Intro4 = this.Content.Load<Texture2D>("intro_4");

            RoomLayouts.Content = this.Content;

            Settings.Load();

            UIRenderer.ButtonScale = 6;
            UIRenderer.LabelScale = 2;

            MusicManager.AddSong("title_screen", Content.Load<Song>("menu"));
            MusicManager.AddSong("gameplay", Content.Load<Song>("battle"));
            MusicManager.AddSong("lose", Content.Load<Song>("music_lose"));

            SoundEffectsManager.AddSFX("player_shoots", Content.Load<SoundEffect>("player_shoots"));
            SoundEffectsManager.AddSFX("enemy_spots_player", Content.Load<SoundEffect>("enemy_spots_player"));
            SoundEffectsManager.AddSFX("enemybee_fires", Content.Load<SoundEffect>("enemybee_fires"));
            SoundEffectsManager.AddSFX("health_collected", Content.Load<SoundEffect>("health_collected"));
            SoundEffectsManager.AddSFX("win_collected", Content.Load<SoundEffect>("win_collected"));
            SoundEffectsManager.AddSFX("explosion", Content.Load<SoundEffect>("explosion"));
            SoundEffectsManager.AddSFX("player_hit", Content.Load<SoundEffect>("player_hit"));
            SoundEffectsManager.AddSFX("enemy_hit", Content.Load<SoundEffect>("enemy_hit"));
            SoundEffectsManager.AddSFX("flamethrower", Content.Load<SoundEffect>("flamethrower"));
            SoundEffectsManager.AddSFX("door_open", Content.Load<SoundEffect>("door_open"));
            SoundEffectsManager.AddSFX("weapon_switch", Content.Load<SoundEffect>("weapon_switch"));

            MusicManager.setVolume(Settings.MusicVolume / 10f);
            SoundEffectsManager.setVolume(Settings.MusicVolume / 10f);
            
            this.IsMouseVisible = false;

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

        public void InitializeStoryScreen()
        {
            var level = new StoryScreen(this);
            this.CurrentScene = level;
        }

        public void InitializeCredits()
        {
            var credits = new CreditsScreen(this);
            this.CurrentScene = credits;
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