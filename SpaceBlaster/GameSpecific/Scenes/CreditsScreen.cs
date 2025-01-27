using Microsoft.Xna.Framework;
namespace GameSpecific;

using SpaceBlaster;
using GameEngine.Scene.UI;
using GameEngine.Scene.Components;
using GameEngine.Gameplay.Audio;
using System;

public class CreditsScreen : GameEngine.Scene.Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;

    public CreditsScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        if (OperatingSystem.IsWindows())
        {
            Cursor c = new Cursor(0, 0, this, null);
            this.Spawn(c);
        }

        //SpaceBlasterGame.Settings.Save();
        _game = game;

        var bg = new Background(0, 0, this);
        bg.AnimationPlayer.SetCurrentAnimation(2);
        _components.Add(bg);

        this.Camera = new Camera(0, 0, this, null);

        this._components.Add(new Label(SpaceBlasterGame.VirtualResolutionWidth / 4 + 100, 50, "Credits:\n- Sound effects:\nShapeform Audio Free Sound Effects by Shapeforms:\nhttps://shapeforms.itch.io/shapeforms-audio-free-sfx\n- Music:\nSpace Music Pack by Goose Ninja:\nhttps://gooseninja.itch.io/space-music-pack\n- Graphics - Tiles, player, enemies, etc.:\nRobot Warfare Asset Pack by MattWalkden:\nhttps://mattwalkden.itch.io/free-robot-warfare-pack\n- Graphics - UI buttons:\nPixel UI buttons by TotusLotus:\nhttps://totuslotus.itch.io/pixel-ui-buttons\n\n- Graphics - Backgrounds:\nSpace background generator by Deep-Fold:\nhttps://deep-fold.itch.io/space-background-generator\n- Font:\nArial (UI)\nRobtronika Font by GGBotNet:\nhttps://ggbot.itch.io/robtronika-font", Color.White, this, null));

        // BACK TO TITLE
        Button backToTitle = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 20,
            new Label(32, 20, "Back", Color.White, this, null),
            this,
            null
        );

        backToTitle.clickHandler = delegate ()
        {
            this.ToTitle();
        };

        this._components.Add(backToTitle);

        if (MusicManager.getCurrentSongKey() != "title_screen") MusicManager.play("title_screen");
    }

    public void ToTitle()
    {
        SpaceBlasterGame.Settings.Save();
        _game.InitializeTitleScreen();
    }
}