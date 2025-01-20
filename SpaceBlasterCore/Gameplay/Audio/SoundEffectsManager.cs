using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Scene.Components;

public class SoundEffectsManager
{
    public static Dictionary<string, SoundEffect> sfx = null;
    public static Queue<Tuple<SceneObject, SoundEffect>> SoundQueue = new Queue<Tuple<SceneObject, SoundEffect>>();
    public static float getVolume()
    {
        return SoundEffect.MasterVolume;
    }

    public static void setVolume(float v)
    {
        if (v > 0f && v <= 1f) SoundEffect.MasterVolume = v;
    }

    public static void Play(SceneObject request, string key)
    {
        if (sfx.ContainsKey(key))
        {
            SoundQueue.Enqueue(new Tuple<SceneObject, SoundEffect>(request, sfx[key]));
        }
    }

    public static void initialize(Game _game)
    {
        sfx = new Dictionary<string, SoundEffect>() {
            {"player_shoots",_game.Content.Load<SoundEffect>("player_shoots")},
            { "enemy_spots_player",_game.Content.Load<SoundEffect>("enemy_spots_player")},
            { "enemybee_fires",_game.Content.Load<SoundEffect>("enemybee_fires")},
            { "health_collected",_game.Content.Load<SoundEffect>("health_collected")},
            { "win_collected",_game.Content.Load<SoundEffect>("win_collected")},
            { "explosion", _game.Content.Load<SoundEffect>("explosion") }
        };
    }
}