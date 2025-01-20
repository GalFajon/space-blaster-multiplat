using System;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster.Scene;
using SpaceBlaster;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

public class AudioHandler : GameComponent
{

    private SpaceBlasterGame _game;

    public AudioHandler(SpaceBlasterGame game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        if (MediaPlayer.State == MediaState.Stopped)
            MediaPlayer.Play(MusicManager.getCurrentSong(), TimeSpan.Zero);

        while (SoundEffectsManager.SoundQueue.Count > 0)
        {
            var t = SoundEffectsManager.SoundQueue.Dequeue();

            if (t.Item1 is Position p) if (_game.isOnScreen(p.Pos, 0, 0)) t.Item2.Play();
                else if (t.Item1 == null) t.Item2.Play();
        }

        base.Update(gameTime);
    }
}