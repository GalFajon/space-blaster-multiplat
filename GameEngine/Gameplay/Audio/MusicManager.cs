using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace GameEngine.Gameplay.Audio;

public class MusicManager
{
    public static Dictionary<string, Song> songs = null;
    public static string currentSong;
    public static void Initialize(Game _game)
    {
        songs = new Dictionary<string, Song>() {};
    }

    public static void AddSong(string name, Song song)
    {
        songs[name] = song;
    }

    public static float getVolume()
    {
        return MediaPlayer.Volume;
    }

    public static void setVolume(float v)
    {
        if (v >= 0f && v <= 1f) MediaPlayer.Volume = v;
    }

    public static Song getCurrentSong()
    {
        return songs[currentSong];
    }

    public static string getCurrentSongKey()
    {
        return currentSong;
    }

    public static void play(string song)
    {
        currentSong = song;
        MediaPlayer.Play(songs[song]);
    }

    public static void stop()
    {
        MediaPlayer.Stop();
    }
}