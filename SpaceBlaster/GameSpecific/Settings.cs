using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using GameSpecific;

public class Settings
{
    public WeaponStats primary;
    public WeaponStats secondary;
    public int SFXVolume { get; set; } = 100;
    public int MusicVolume { get; set; } = 100;
    public float CameraZoom { get; set; } = 1;
    public int Currency { get; set; } = 0;
    public int HighScore { get; set; } = 0;
    public bool PistolUnlocked { get; set; } = true;
    public bool SubMachineGunUnlocked { get; set; } = false;
    public bool ShotgunUnlocked { get; set; } = true;
    public bool MachineGunUnlocked { get; set; } = false;

    public Settings(FileStream s) {}

    public Func<Stream> GetStream {  get; set; }
    public void Save()
    {
        //using (Stream stream = this.GetStream()) stream.Write(JsonSerializer.SerializeToUtf8Bytes(this));
    }

    public void Load()
    {
        if (this.GetStream != null)
        {
            /*var s = this.GetStream();
            var text = new StreamReader(s).ReadToEnd();

            Settings n = JsonSerializer.Deserialize<Settings>(text);
            this.SFXVolume = n.SFXVolume;
            this.MusicVolume = n.MusicVolume;
            this.CameraZoom = n.CameraZoom;
            this.Currency = n.Currency;
            this.HighScore = n.HighScore;

            this.PistolUnlocked = n.PistolUnlocked;
            this.SubMachineGunUnlocked = n.SubMachineGunUnlocked;
            this.MachineGunUnlocked = n.MachineGunUnlocked;
            this.ShotgunUnlocked = n.ShotgunUnlocked;
            s.Close();*/
        }
        //else throw new Exception("Delegate for getting settings file is not defined for this platform.");
    }
};