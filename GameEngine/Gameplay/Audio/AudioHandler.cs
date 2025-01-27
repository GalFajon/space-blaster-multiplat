using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using GameEngine.Scene.Components;

namespace GameEngine.Gameplay.Audio;

public class AudioHandler : GameComponent
{

    private GameBase _game;

    public AudioHandler(GameBase game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        if (MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(MusicManager.getCurrentSong(), TimeSpan.Zero);

        while (SoundEffectsManager.SoundQueue.Count > 0)
        {
            var t = SoundEffectsManager.SoundQueue.Dequeue();

            if (t.Item1 is Position p) if (_game.Renderer.IsOnScreen(p.Pos, 0, 0)) t.Item2.Play();
            else if (t.Item1 == null) t.Item2.Play();
        }

        base.Update(gameTime);
    }
}