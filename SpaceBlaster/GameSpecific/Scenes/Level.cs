using Microsoft.Xna.Framework;
namespace GameSpecific;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameEngine.Gameplay.Audio;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using SpaceBlaster;

public class Level : GameEngine.Scene.Scene
{
    private Label roomLabel = null;
    private Label currencyLabel = null;
    private Label weaponLabel = null;
    public Player _player = null;

    public SpaceBlaster.SpaceBlasterGame _game = null;

    public Level(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        _game = game;
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
        roomLabel = new Label(20, 20, "Rooms cleared: " + _game.roomsCleared.ToString(), Color.White, this, null);
        this.Spawn(roomLabel);

        currencyLabel = new CurrencyLabel(20, 60, Color.White, this, null);
        this.Spawn(currencyLabel);

        weaponLabel = new WeaponLabel(20, 100, Color.White, this, null);
        this.Spawn(weaponLabel);
    }

    public void GenerateGameplayLevel()
    {
        this.InitializeUI();

        float oldPlayerHealth = 0;
        if (this._player != null) oldPlayerHealth = this._player.Health;

        Components = new List<SceneObject>() { };

        _player = new Player(SpaceBlasterGame.Settings.primary, SpaceBlasterGame.Settings.secondary, 48 * 6, 48 * 6, this);
        if (oldPlayerHealth != 0) _player.Health = oldPlayerHealth;

        Camera = new Camera(-((float)SpaceBlasterGame.VirtualResolutionWidth / (2 * SpaceBlasterGame.Settings.CameraZoom)), -((float)SpaceBlasterGame.VirtualResolutionHeight / (SpaceBlasterGame.Settings.CameraZoom * 2)), this, _player);
        Camera.ScaleX = Camera.ScaleX * SpaceBlasterGame.Settings.CameraZoom;
        Camera.ScaleY = Camera.ScaleY * SpaceBlasterGame.Settings.CameraZoom;

        Components.AddRange(LevelGenerator.LevelGenerator.Generate(6, this));
        Components.Add(_player);

        MusicManager.play("gameplay");
    }

}