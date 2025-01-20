using Microsoft.Xna.Framework;
namespace GameSpecific;

using SpaceBlaster.Scene;

using GameSpecific.LevelGenerator;
using System.Collections;
using Scene.Components;
using System.Collections.Generic;
using GameSpecific.LevelGenerator.Rooms;
using System.ComponentModel;
using SpaceBlaster;

public class Level : Scene
{
    private Label roomLabel = null;
    private Label currencyLabel = null;
    private Label weaponLabel = null;

#if WINDOWS
    private RetryLabel retryLabel = null;
#endif

    public Player _player = null;

#if ANDROID
    public Button switchWeaponButton = null;
#endif

    public SpaceBlaster.SpaceBlasterGame _game = null;

    public Level(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        _game = game;
        this.InitializeUI();
    }

    public void Win()
    {
        _game.roomsCleared += 1;

        if (_game.roomsCleared > SpaceBlasterGame.Settings.HighScore)
        {
            SpaceBlasterGame.Settings.HighScore = _game.roomsCleared;
            SpaceBlasterGame.Settings.Save();
        }

        this.GenerateGameplayLevel();
    }

    public void Lose()
    {
        _game.roomsCleared = 0;
        this._game.InitializeLoadoutScreen();
    }

    public void InitializeUI()
    {
#if WINDOWS
        retryLabel = new RetryLabel(SpaceBlasterGame.virtualResolutionWidth / 2, SpaceBlasterGame.virtualResolutionHeight / 2, Color.White, this, null);
        this.Spawn(retryLabel);
#endif
        roomLabel = new Label(20, 20, "Rooms cleared: " + _game.roomsCleared.ToString(), Color.White, this, null);
        this.Spawn(roomLabel);

        currencyLabel = new CurrencyLabel(20, 40, Color.White, this, null);
        this.Spawn(currencyLabel);

        weaponLabel = new WeaponLabel(20, 60, Color.White, this, null);
        this.Spawn(weaponLabel);

#if ANDROID
        switchWeaponButton = new Button(SpaceBlasterGame.virtualResolutionWidth - 16*3, SpaceBlasterGame.virtualResolutionHeight - 16*3, 16 * 3, 16 * 3, null, new Label(16, 10, "S", Color.White, this, null), this, null);
        switchWeaponButton.Big = false;
        switchWeaponButton.clickHandler = () =>
        {
            if (_player != null) _player.SwitchWeapon();
        };

        this.Spawn(switchWeaponButton);
#endif
    }

    public void GenerateGameplayLevel()
    {
        float oldPlayerHealth = 0;
        if (this._player != null) oldPlayerHealth = this._player.Health;

        Components = new List<SceneObject>() { };

        this.InitializeUI();
        _player = new Player(SpaceBlasterGame.Settings.primary, SpaceBlasterGame.Settings.secondary, 48 * 6, 48 * 6, this);
        if (oldPlayerHealth != 0) _player.Health = oldPlayerHealth;

        Camera = new Camera(-((float)SpaceBlasterGame.virtualResolutionWidth / (2 * SpaceBlasterGame.Settings.CameraZoom)), -((float)SpaceBlasterGame.virtualResolutionHeight / (SpaceBlasterGame.Settings.CameraZoom * 2)), this, _player);

        Camera.ScaleX = SpaceBlasterGame.Settings.CameraZoom * ((float)_game._graphics.PreferredBackBufferWidth / (float)SpaceBlasterGame.virtualResolutionWidth);
        Camera.ScaleY = SpaceBlasterGame.Settings.CameraZoom * ((float)_game._graphics.PreferredBackBufferHeight / (float)SpaceBlasterGame.virtualResolutionHeight);

        Components.AddRange(LevelGenerator.LevelGenerator.Generate(6, this));
        Components.Add(_player);

        MusicManager.play("gameplay");
    }

}