using Microsoft.Xna.Framework;
namespace GameSpecific;

using SpaceBlaster.Scene;

using GameSpecific.LevelGenerator;
using System.Collections;
using Scene.Components;
using System.Collections.Generic;
using GameSpecific.LevelGenerator.Rooms;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
using SpaceBlaster;

public class TitleScreen : Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;

    public TitleScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        _game = game;

        this.Camera = new Camera(0, 0, this, null);
        Camera.ScaleX = ((float)_game._graphics.PreferredBackBufferWidth / (float)SpaceBlasterGame.virtualResolutionWidth);
        Camera.ScaleY = ((float)_game._graphics.PreferredBackBufferHeight / (float)SpaceBlasterGame.virtualResolutionHeight);

        this._components.Add(new Label(SpaceBlasterGame.virtualResolutionWidth / 2 - 100, 50, "Space blaster", Color.White, this, null));
        this._components.Add(new Label(SpaceBlasterGame.virtualResolutionWidth / 2 - 100, 70, "High Score: " + SpaceBlasterGame.Settings.HighScore, Color.White, this, null));

        Button play = new Button(
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 3,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 16 * 2,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "Play", Color.White, this, null),
            this,
            null
        );

        this._components.Add(play);

        play.clickHandler = delegate ()
        {
            this._game.InitializeLoadoutScreen();
        };

        Button settings = new Button(
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 3,
            SpaceBlasterGame.virtualResolutionHeight / 2 + 16 * 2,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "Settings", Color.White, this, null),
            this,
            null
        );

        settings.clickHandler = delegate ()
        {
            this._game.InitializeSettings();
        };

        this._components.Add(settings);

        Button quit = new Button(
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 3,
            SpaceBlasterGame.virtualResolutionHeight / 2 + 16 * 6,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "Quit", Color.White, this, null),
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