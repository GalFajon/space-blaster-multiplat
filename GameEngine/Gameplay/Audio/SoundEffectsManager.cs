using System;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Gameplay.Audio;

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
        if (v >= 0f && v <= 1f) SoundEffect.MasterVolume = v;
    }

    public static void Play(SceneObject request, string key)
    {
        if (sfx.ContainsKey(key))
        {
            SoundQueue.Enqueue(new Tuple<SceneObject, SoundEffect>(request, sfx[key]));
        }
    }

    public static void Initialize(Game _game)
    {
        sfx = new Dictionary<string, SoundEffect>() {};
    }

    public static void  AddSFX(string key, SoundEffect effect)
    {
        sfx[key] = effect;
    }
}