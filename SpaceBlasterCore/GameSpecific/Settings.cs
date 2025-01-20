using System;
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

#if WINDOWS
    private string path = "./Content/";
#endif

#if ANDROID
    private string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"SpaceBlasterAndroid");
#endif 

    public bool PistolUnlocked { get; set; } = true;
    public bool SubMachineGunUnlocked { get; set; } = false;
    public bool ShotgunUnlocked { get; set; } = true;
    public bool MachineGunUnlocked { get; set; } = false;
    public void Save()
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        using (FileStream stream = File.Create(System.IO.Path.Combine(this.path, "settings.json"))) stream.Write(JsonSerializer.SerializeToUtf8Bytes(this));
    }

    public void Load()
    {
        if (File.Exists(System.IO.Path.Combine(path, "settings.json")))
        {
            Settings n = JsonSerializer.Deserialize<Settings>(File.ReadAllText(System.IO.Path.Combine(path, "./settings.json")));
            this.SFXVolume = n.SFXVolume;
            this.MusicVolume = n.MusicVolume;
            this.CameraZoom = n.CameraZoom;
            this.Currency = n.Currency;
            this.HighScore = n.HighScore;

            this.PistolUnlocked = n.PistolUnlocked;
            this.SubMachineGunUnlocked = n.SubMachineGunUnlocked;
            this.MachineGunUnlocked = n.MachineGunUnlocked;
            this.ShotgunUnlocked = n.ShotgunUnlocked;
        }
    }
};