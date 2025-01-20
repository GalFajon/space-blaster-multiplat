using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public class MusicManager
{
    public static Dictionary<string, Song> songs = null;
    public static string currentSong;
    public static void initialize(Game _game)
    {
        songs = new Dictionary<string, Song>() {
            {"title_screen",_game.Content.Load<Song>("menu")},
            {"gameplay",_game.Content.Load<Song>("battle")}
        };
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