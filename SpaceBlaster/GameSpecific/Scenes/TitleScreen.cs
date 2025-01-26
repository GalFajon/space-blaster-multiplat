using Microsoft.Xna.Framework;
namespace GameSpecific;

using System;
using GameEngine.Gameplay.Audio;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using SpaceBlaster;

public class TitleScreen : GameEngine.Scene.Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;

    public TitleScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        _game = game;

        var bg = new Background(0, 0, this);
        bg.AnimationPlayer.SetCurrentAnimation(1);
        _components.Add(bg);

        if (OperatingSystem.IsWindows())
        {
            Cursor c = new Cursor(0, 0, this, null);
            this.Spawn(c);
        }

        this.Camera = new Camera(0, 0, this, null);
        this._components.Add(new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 100, 100, "High Score: " + SpaceBlasterGame.Settings.HighScore, Color.White, this, null));

        Button play = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 6,
            new Label(32, 20, "Play", Color.White, this, null),
            this,
            null
        );

        this._components.Add(play);

        play.clickHandler = delegate ()
        {
            this._game.InitializeStoryScreen();

            //this._game.InitializeLoadoutScreen();
        };

        Button settings = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 + 16 * 1,
            new Label(32, 20, "Settings", Color.White, this, null),
            this,
            null
        );

        settings.clickHandler = delegate ()
        {
            this._game.InitializeSettings();
        };

        this._components.Add(settings);

        Button credits = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 + 16 * 8,
            new Label(32, 20, "Credits", Color.White, this, null),
            this,
            null
        );

        credits.clickHandler = delegate ()
        {
            this._game.InitializeCredits();
        };

        this._components.Add(credits);

        Button quit = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 + 16 * 16,
            new Label(32, 20, "Quit", Color.White, this, null),
            this,
            null
        );

        quit.clickHandler = delegate ()
        {
            this._game.Quit();
        };

        this._components.Add(quit);

        if (OperatingSystem.IsWindows())
        {
            this._components.Add(new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 280, SpaceBlasterGame.VirtualResolutionHeight / 2 + 16 * 24, "HOW TO PLAY: [WASD] move,\n[click] shoot, [E] switch weapon", Color.White, this, null));
        }
        else if (OperatingSystem.IsAndroid())
        {
            this._components.Add(new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 280, SpaceBlasterGame.VirtualResolutionHeight / 2 + 16 * 24, "HOW TO PLAY: [left side of screen] move,\n[right side of screen] aim & shoot,\n[S button] switch weapon", Color.White, this, null));
        }

        if (MusicManager.getCurrentSongKey() != "title_screen") MusicManager.play("title_screen");
    }

    public void Start()
    {
        _game.InitializeLoadoutScreen();
    }

    public void ToSettings()
    {
        _game.InitializeSettings();
    }
}