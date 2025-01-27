using Microsoft.Xna.Framework;
namespace GameSpecific;

using SpaceBlaster;
using GameEngine.Scene.UI;
using GameEngine.Scene.Components;
using GameEngine.Gameplay.Audio;
using System;

public class LoadoutScreen : GameEngine.Scene.Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;
    Label currencyLabel = null;
    private static WeaponStats[] buyableWeapons = new WeaponStats[] { PremadeWeapons.Pistol, PremadeWeapons.SubMachineGun };
    private static WeaponStats[] buyableWeaponsSecondary = new WeaponStats[] { PremadeWeapons.Shotgun, PremadeWeapons.MachineGun, PremadeWeapons.FlameThrower };
    private int selectedWeaponSecondary = 0;
    private int selectedWeapon = 0;
    Label currentWeaponLabel = null;
    Label currentSecondaryWeaponLabel = null;
    Button buyEquipButton = null;
    Button buyEquipSecondaryButton = null;

    public LoadoutScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        var bg = new Background(0, 0, this);
        bg.AnimationPlayer.SetCurrentAnimation(2);
        _components.Add(bg);

        if (OperatingSystem.IsWindows())
        {
            Cursor c = new Cursor(0, 0, this, null);
            _components.Add(c);
        }

        _game = game;

        this.Camera = new Camera(0, 0, this, null);

        this._components.Add(new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 100, 50, "Loadout", Color.White, this, null));

        this.currencyLabel = new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 100, 100, "", Color.White, this, null);
        this._components.Add(this.currencyLabel);

        SpaceBlasterGame.Settings.primary = PremadeWeapons.Pistol;
        SpaceBlasterGame.Settings.secondary = PremadeWeapons.Shotgun;

        this.currentWeaponLabel = new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 150, SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 20, "", Color.White, this, null);
        this._components.Add(currentWeaponLabel);

        Button nextWeapon = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 + 16 * 10,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 20,
            new Label(32, 20, ">", Color.White, this, null),
            this,
            null
        );

        nextWeapon.Big = false;
        nextWeapon.clickHandler = delegate ()
        {
            if (this.selectedWeapon + 1 < buyableWeapons.Length) this.selectedWeapon += 1;
            this.updateCurrentWeaponLabel();
        };

        this._components.Add(nextWeapon);

        Button prevWeapon = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 15,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 20,
            new Label(32, 20, "<", Color.White, this, null),
            this,
            null
        );

        prevWeapon.Big = false;
        prevWeapon.clickHandler = delegate ()
        {
            if (this.selectedWeapon - 1 >= 0) this.selectedWeapon -= 1;
            this.updateCurrentWeaponLabel();
        };

        this._components.Add(prevWeapon);

        this.buyEquipButton = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 15,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 - 16 * 12,
            new Label(32, 20, "Buy", Color.White, this, null),
            this,
            null
        );

        buyEquipButton.clickHandler = delegate ()
        {
            if (!buyableWeapons[this.selectedWeapon].Unlocked && SpaceBlasterGame.Settings.Currency >= buyableWeapons[this.selectedWeapon].Price)
            {
                SpaceBlasterGame.Settings.Currency -= buyableWeapons[this.selectedWeapon].Price;

                buyableWeapons[this.selectedWeapon].Unlocked = true;
                if (buyableWeapons[this.selectedWeapon].DisplayName == PremadeWeapons.SubMachineGun.DisplayName) SpaceBlasterGame.Settings.SubMachineGunUnlocked = true;

                this.UpdateCurrencyLabel();
                this.updateCurrentWeaponLabel();

                SpaceBlasterGame.Settings.Save();
                SpaceBlasterGame.Settings.primary = buyableWeapons[this.selectedWeapon];
            }
            else if (buyableWeapons[this.selectedWeapon].Unlocked)
            {
                SpaceBlasterGame.Settings.primary = buyableWeapons[this.selectedWeapon];
            }
        };

        this._components.Add(buyEquipButton);

        this.currentSecondaryWeaponLabel = new Label(SpaceBlasterGame.VirtualResolutionWidth / 2 - 150, SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 - 16 * 5, "", Color.White, this, null);
        this._components.Add(currentSecondaryWeaponLabel);

        Button nextSecondaryWeapon = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 + 16 * 10,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 - 16 * 5,
            new Label(32, 20, ">", Color.White, this, null),
            this,
            null
        );

        nextSecondaryWeapon.Big = false;
        nextSecondaryWeapon.clickHandler = delegate ()
        {
            if (this.selectedWeaponSecondary + 1 < buyableWeaponsSecondary.Length) this.selectedWeaponSecondary += 1;
            this.updateCurrentSecondaryWeaponLabel();
        };

        this._components.Add(nextSecondaryWeapon);

        Button prevSecondaryWeapon = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 15,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 - 16 * 5,
            new Label(32, 20, "<", Color.White, this, null),
            this,
            null
        );

        prevSecondaryWeapon.Big = false;
        prevSecondaryWeapon.clickHandler = delegate ()
        {
            if (this.selectedWeaponSecondary - 1 >= 0) this.selectedWeaponSecondary -= 1;
            this.updateCurrentSecondaryWeaponLabel();
        };

        this._components.Add(prevSecondaryWeapon);

        this.buyEquipSecondaryButton = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 15,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 2,
            new Label(32, 20, "Buy", Color.White, this, null),
            this,
            null
        );

        buyEquipSecondaryButton.clickHandler = delegate ()
        {
            if (!buyableWeaponsSecondary[this.selectedWeaponSecondary].Unlocked && SpaceBlasterGame.Settings.Currency >= buyableWeaponsSecondary[this.selectedWeaponSecondary].Price)
            {
                SpaceBlasterGame.Settings.Currency -= buyableWeaponsSecondary[this.selectedWeaponSecondary].Price;

                buyableWeaponsSecondary[this.selectedWeaponSecondary].Unlocked = true;
                if (buyableWeaponsSecondary[this.selectedWeaponSecondary].DisplayName == PremadeWeapons.MachineGun.DisplayName) SpaceBlasterGame.Settings.MachineGunUnlocked = true;
                if (buyableWeaponsSecondary[this.selectedWeaponSecondary].DisplayName == PremadeWeapons.FlameThrower.DisplayName) SpaceBlasterGame.Settings.FlameThrowerUnlocked = true;

                this.UpdateCurrencyLabel();
                this.updateCurrentSecondaryWeaponLabel();

                SpaceBlasterGame.Settings.Save();
                SpaceBlasterGame.Settings.secondary = buyableWeaponsSecondary[this.selectedWeaponSecondary];
            }
            else if (buyableWeaponsSecondary[this.selectedWeaponSecondary].Unlocked)
            {
                SpaceBlasterGame.Settings.secondary = buyableWeaponsSecondary[this.selectedWeaponSecondary];
            }
        };

        this._components.Add(buyEquipSecondaryButton);

        Button play = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 9,
            new Label(32, 20, "Play", Color.White, this, null),
            this,
            null
        );

        play.clickHandler = delegate ()
        {
            SpaceBlasterGame.Settings.Save();
            this.Start();
        };

        this._components.Add(play);

        Button backToTitle = new Button(
            SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 5,
            SpaceBlasterGame.VirtualResolutionHeight / 2 - 8 * 3 + 16 * 16,
            new Label(32, 20, "Back", Color.White, this, null),
            this,
            null
        );

        backToTitle.clickHandler = delegate ()
        {
            SpaceBlasterGame.Settings.Save();
            this.ToTitle();
        };

        this._components.Add(backToTitle);

        this.UpdateCurrencyLabel();
        this.updateCurrentWeaponLabel();
        this.updateCurrentSecondaryWeaponLabel();

        if (MusicManager.getCurrentSongKey() != "title_screen") MusicManager.play("title_screen");
    }

    public void updateCurrentSecondaryWeaponLabel()
    {
        if (!buyableWeaponsSecondary[this.selectedWeaponSecondary].Unlocked)
        {
            this.buyEquipSecondaryButton.label.Text = "Buy";
            this.currentSecondaryWeaponLabel.Text = buyableWeaponsSecondary[this.selectedWeaponSecondary].DisplayName + " (" + buyableWeaponsSecondary[this.selectedWeaponSecondary].Price + ")";
        }
        else
        {
            this.buyEquipSecondaryButton.label.Text = "Equip";
            this.currentSecondaryWeaponLabel.Text = buyableWeaponsSecondary[this.selectedWeaponSecondary].DisplayName;
        }
    }

    public void updateCurrentWeaponLabel()
    {
        if (!buyableWeapons[this.selectedWeapon].Unlocked)
        {
            this.buyEquipButton.label.Text = "Buy";
            this.currentWeaponLabel.Text = buyableWeapons[this.selectedWeapon].DisplayName + " (" + buyableWeapons[this.selectedWeapon].Price + ")";
        }
        else
        {
            this.buyEquipButton.label.Text = "Equip";
            this.currentWeaponLabel.Text = buyableWeapons[this.selectedWeapon].DisplayName;
        }
    }

    public void UpdateCurrencyLabel()
    {
        this.currencyLabel.Text = "Currency: " + SpaceBlasterGame.Settings.Currency;
    }

    public void Start()
    {
        _game.InitializeLevel();
    }

    public void ToTitle()
    {
        SpaceBlasterGame.Settings.Save();
        _game.InitializeTitleScreen();
    }
}