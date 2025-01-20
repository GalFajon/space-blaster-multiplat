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
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.Json;
using System;
using System.Text.Unicode;
using System.Text;
using SpaceBlaster;

public class LoadoutScreen : Scene
{
    private SpaceBlaster.SpaceBlasterGame _game = null;
    Label currencyLabel = null;
    private static WeaponStats[] buyableWeapons = new WeaponStats[] { PremadeWeapons.Pistol, PremadeWeapons.SubMachineGun };
    private static WeaponStats[] buyableWeaponsSecondary = new WeaponStats[] { PremadeWeapons.Shotgun, PremadeWeapons.MachineGun };
    private int selectedWeaponSecondary = 0;
    private int selectedWeapon = 0;
    Label currentWeaponLabel = null;
    Label currentSecondaryWeaponLabel = null;
    Button buyEquipButton = null;
    Button buyEquipSecondaryButton = null;

    public LoadoutScreen(SpaceBlaster.SpaceBlasterGame game) : base(game)
    {
        _game = game;

        this.Camera = new Camera(0, 0, this, null);
        Camera.ScaleX = ((float)_game._graphics.PreferredBackBufferWidth / (float)SpaceBlasterGame.virtualResolutionWidth);
        Camera.ScaleY = ((float)_game._graphics.PreferredBackBufferHeight / (float)SpaceBlasterGame.virtualResolutionHeight);

        this._components.Add(new Label(SpaceBlasterGame.virtualResolutionWidth / 2 - 100, 50, "Loadout", Color.White, this, null));

        this.currencyLabel = new Label(SpaceBlasterGame.virtualResolutionWidth / 2 - 100, 100, "", Color.White, this, null);
        this._components.Add(this.currencyLabel);

        SpaceBlasterGame.Settings.primary = PremadeWeapons.Pistol;
        SpaceBlasterGame.Settings.secondary = PremadeWeapons.Shotgun;

        // buy / unlock buttons, equip primary and equip secondary  buttons, next weapon / previous weapon
        this.currentWeaponLabel = new Label(SpaceBlasterGame.virtualResolutionWidth / 2 - 100, SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 9, "", Color.White, this, null);
        this._components.Add(currentWeaponLabel);

        Button nextWeapon = new Button(
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 + 16 * 5,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 9,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, ">", Color.White, this, null),
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
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 10,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 9,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "<", Color.White, this, null),
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
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 10,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 5,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "Buy", Color.White, this, null),
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

        // secondary weapon button
        this.currentSecondaryWeaponLabel = new Label(SpaceBlasterGame.virtualResolutionWidth / 2 - 100, SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 17, "", Color.White, this, null);
        this._components.Add(currentSecondaryWeaponLabel);

        Button nextSecondaryWeapon = new Button(
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 + 16 * 5,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 17,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, ">", Color.White, this, null),
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
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 10,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 17,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "<", Color.White, this, null),
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
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 10,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 - 16 * 13,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "Buy", Color.White, this, null),
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

        // PLAY
        Button play = new Button(
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 3,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 + 16 * 2,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "Play", Color.White, this, null),
            this,
            null
        );

        play.clickHandler = delegate ()
        {
            SpaceBlasterGame.Settings.Save();
            this.Start();
        };

        this._components.Add(play);

        // BACK TO TITLE
        Button backToTitle = new Button(
            SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 3,
            SpaceBlasterGame.virtualResolutionHeight / 2 - 8 * 3 + 16 * 5,
            32 * 3,
            16 * 3,
            null,
            new Label(16, 10, "Back", Color.White, this, null),
            this,
            null
        );

        backToTitle.clickHandler = delegate ()
        {
            SpaceBlasterGame.Settings.Save();
            this.ToTitle();
        };

        this._components.Add(backToTitle);
        MusicManager.play("title_screen");

        this.UpdateCurrencyLabel();
        this.updateCurrentWeaponLabel();
        this.updateCurrentSecondaryWeaponLabel();
    }

    public void updateCurrentSecondaryWeaponLabel()
    {
        if (!buyableWeaponsSecondary[this.selectedWeaponSecondary].Unlocked)
        {
            this.buyEquipSecondaryButton.label.Text = "Buy";
            this.currentSecondaryWeaponLabel.Text = buyableWeaponsSecondary[this.selectedWeaponSecondary].DisplayName + " (" + buyableWeapons[this.selectedWeaponSecondary].Price + ")";
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