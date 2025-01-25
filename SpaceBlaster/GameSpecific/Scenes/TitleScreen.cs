using Microsoft.Xna.Framework;
namespace GameSpecific;

using GameEngine.Gameplay.Audio;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using SpaceBlaster;

public class TitleScreen : GameEngine.Scene.Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;

    public TitleScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        SpaceBlasterGame.Settings.Save();
        _game = game;

        this.Camera = new Camera(0, 0, this, null);

        this._components.Add(new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 100, 50, "Space blaster", Color.White, this, null));
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
            this._game.InitializeLoadoutScreen();
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

        Button quit = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 + 16 * 8,
            new Label(32, 20, "Quit", Color.White, this, null),
            this,
            null
        );

        quit.clickHandler = delegate ()
        {
            this._game.Quit();
        };

        this._components.Add(quit);

        MusicManager.play("title_screen");
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