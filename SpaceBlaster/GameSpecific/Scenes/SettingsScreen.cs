using Microsoft.Xna.Framework;
namespace GameSpecific;

using SpaceBlaster;
using GameEngine.Scene.UI;
using GameEngine.Scene.Components;
using GameEngine.Gameplay.Audio;

public class SettingsScreen : GameEngine.Scene.Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;

    Label musicVolumeLabel = null;
    Label sfxVolumeLabel = null;
    Label cameraZoomLabel = null;

    public SettingsScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        _game = game;

        this.Camera = new Camera(0, 0, this, null);

        this._components.Add(new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 100, 50, "Settings", Color.White, this, null));

        // MUSIC VOLUME
        Button lowerVolume = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 13,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 10,
            new Label(32, 20, "-", Color.White, this, null),
            this,
            null
        );

        lowerVolume.clickHandler = delegate () { if (lowerVolume.clicked) this.setMusicVolume(MusicManager.getVolume() - 0.1f); };
        lowerVolume.Big = false;

        this._components.Add(lowerVolume);

        musicVolumeLabel = new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 10, "Music volume: " + MusicManager.getVolume(), Color.White, this, null);
        this._components.Add(musicVolumeLabel);

        Button higherVolume = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 + 16 * 13,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 10,
            new Label(32, 20, "+", Color.White, this, null),
            this,
            null
        );

        higherVolume.clickHandler = delegate () { if (higherVolume.clicked) this.setMusicVolume(MusicManager.getVolume() + 0.1f); };
        higherVolume.Big = false;

        this._components.Add(higherVolume);

        //SFX VOLUME
        Button lowerSfxVolume = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 13,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 2,
            new Label(32, 20, "-", Color.White, this, null),
            this,
            null
        );

        lowerSfxVolume.clickHandler = delegate () { if (lowerSfxVolume.clicked) this.setSFXVolume(SoundEffectsManager.getVolume() - 0.1f); };
        lowerSfxVolume.Big = false;

        this._components.Add(lowerSfxVolume);

        sfxVolumeLabel = new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5, SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 2, "SFX volume: " + SoundEffectsManager.getVolume(), Color.White, this, null);
        this._components.Add(sfxVolumeLabel);

        Button higherSfxVolume = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 + 16 * 13,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 2,
            new Label(32, 20, "+", Color.White, this, null),
            this,
            null
        );

        higherSfxVolume.Big = false;
        higherSfxVolume.clickHandler = delegate () { if (higherSfxVolume.clicked) this.setSFXVolume(SoundEffectsManager.getVolume() + 0.1f); };

        this._components.Add(higherSfxVolume);

        // CAMERA ZOOM
        Button lowerCameraZoom = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 13,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 8,
            new Label(32, 20, "-", Color.White, this, null),
            this,
            null
        );

        lowerCameraZoom.clickHandler = delegate () { if (lowerCameraZoom.clicked) this.setCameraZoom(SpaceBlasterGame.Settings.CameraZoom - 0.1f); };
        lowerCameraZoom.Big = false;

        this._components.Add(lowerCameraZoom);

        cameraZoomLabel = new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5, SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 8, "Camera zoom: " + float.Round(SpaceBlasterGame.Settings.CameraZoom * 10) / 10, Color.White, this, null);
        this._components.Add(cameraZoomLabel);

        Button higherCameraZoom = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 + 16 * 13,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 8,
            new Label(32, 20, "+", Color.White, this, null),
            this,
            null
        );

        higherCameraZoom.Big = false;
        higherCameraZoom.clickHandler = delegate () { if (higherCameraZoom.clicked) this.setCameraZoom(SpaceBlasterGame.Settings.CameraZoom + 0.1f); };

        this._components.Add(higherCameraZoom);

        // BACK TO TITLE
        Button backToTitle = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 16,
            new Label(32, 20, "Back", Color.White, this, null),
            this,
            null
        );

        backToTitle.clickHandler = delegate ()
        {
            this.ToTitle();
        };

        this._components.Add(backToTitle);
        MusicManager.play("title_screen");
    }

    public void setCameraZoom(float v)
    {
        if (v <= 1.5 && v >= 0.8)
        {
            SpaceBlasterGame.Settings.CameraZoom = v;
            cameraZoomLabel.Text = "Camera zoom: " + float.Round(SpaceBlasterGame.Settings.CameraZoom * 10) / 10;
        }
    }

    public void setMusicVolume(float v)
    {
        MusicManager.setVolume(float.Round(v * 10) / 10);
        musicVolumeLabel.Text = "Music volume: " + MusicManager.getVolume();

        SpaceBlasterGame.Settings.MusicVolume = (int)(float.Round(v * 10));
    }

    public void setSFXVolume(float v)
    {
        SoundEffectsManager.setVolume(float.Round(v * 10) / 10);
        sfxVolumeLabel.Text = "SFX volume: " + SoundEffectsManager.getVolume();
        SoundEffectsManager.Play(null, "health_collected");


        SpaceBlasterGame.Settings.SFXVolume = (int)(float.Round(v * 10));
    }

    public void ToTitle()
    {
        SpaceBlasterGame.Settings.Save();
        _game.InitializeTitleScreen();
    }
}