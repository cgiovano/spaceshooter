using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter.GameUtils
{
    class Sounds
    {
        public static readonly SoundEffect Explosion;
        public static readonly SoundEffect PlayerShot;
        public static readonly SoundEffect EnemyShot;
        public static readonly SoundEffect PlayerHurt;
        public static readonly SoundEffect OneUp;

        public static readonly Song Soundtrack;

        public static int CurrentSoundTrackIndex { get; private set; } = 0;

        public Sounds(ContentManager content)
        {
            typeof(Sounds).GetField("Explosion").SetValue(Explosion, content.Load<SoundEffect>("Assets\\Sounds\\Boom"));
            typeof(Sounds).GetField("PlayerShot").SetValue(PlayerShot, content.Load<SoundEffect>("Assets\\Sounds\\Zap1"));
            typeof(Sounds).GetField("EnemyShot").SetValue(EnemyShot, content.Load<SoundEffect>("Assets\\Sounds\\Zap2"));
            typeof(Sounds).GetField("PlayerHurt").SetValue(PlayerHurt, content.Load<SoundEffect>("Assets\\Sounds\\Hurt1"));

            typeof(Sounds).GetField("Soundtrack").SetValue(Soundtrack, content.Load<Song>("Assets\\Sounds\\SoundTrack\\01"));
            MediaPlayer.IsRepeating = true;
        }

        /*
        public void SoundEffectVolume(int volume) => SoundEffect.MasterVolume = (float)volume / 100f;

        public void SoundTrackVolume(int volume) => MediaPlayer.Volume = (float)volume / 100f;*/

        public static void StopSoundTrack() => MediaPlayer.Stop();

        internal static void PlaySoundTrack()
        {
            MediaPlayer.Play(Soundtrack);
        }
    }
}
