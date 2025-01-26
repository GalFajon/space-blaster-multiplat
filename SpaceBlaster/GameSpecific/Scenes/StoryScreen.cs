using GameEngine.Gameplay.Audio;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using SpaceBlaster;
using Microsoft.Xna.Framework;
using GameSpecific;
using System;

//         this._components.Add(new IntroStoryBook(0,0,this));

public class StoryScreen : GameEngine.Scene.Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;

    public StoryScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        _game = game;

        var bg = new Background(0, 0, this);
        bg.AnimationPlayer.SetCurrentAnimation(0);
        _components.Add(bg);

        if (OperatingSystem.IsWindows())
        {
            Cursor c = new Cursor(0, 0, this, null);
            this.Spawn(c);
        }

        this.Camera = new Camera(0, 0, this, null);
        var storybook = new IntroStoryBook(SpaceBlasterGame.VirtualResolutionWidth / 2 - 64 * 5, SpaceBlasterGame.VirtualResolutionHeight / 2 - 64 * 5, this);
        this._components.Add(storybook);

        Button skip = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 24,
            new Label(32, 20, "Continue", Color.White, this, null),
            this,
            null
        );

        skip.clickHandler = delegate ()
        {
            storybook.Destroy();
            this._game.InitializeLoadoutScreen();
        };

        this._components.Add(skip);

        if (MusicManager.getCurrentSongKey() != "lose") MusicManager.play("lose");
    }
}