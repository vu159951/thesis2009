using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace GameSharedObject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AudioGame : Microsoft.Xna.Framework.GameComponent
    {
        //AudioEngine _audioEngine;
        //SoundBank _soundBank;
        //WaveBank _waveBank;
        SoundEffect _soundeffect;
        Song _song;        

        public AudioGame(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            //this._audioEngine.Update();

            base.Update(gameTime);
        }

        /// <summary>
        ///  Play âm thanh với volume và vị trí loa phát
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="volumn"></param>
        /// <param name="pan"></param>
        public void PlaySoundEffectGame(string soundName, float volumn,float pan)
        {
            //this._soundBank.PlayCue(soundName);
            ContentManager contentManager = new ContentManager(this.Game.Services, @"Content\Sound\");
            this._soundeffect = contentManager.Load<SoundEffect>(soundName);
            this._soundeffect.Play(volumn, -0.1f, pan, false);            
        }

        public void PlayBackSound(string songname)
        {
            ContentManager contentManager = new ContentManager(this.Game.Services, @"Content\Sound\");
            this._song = contentManager.Load<Song>(songname);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(this._song);            
        }

        public void StopPlayingBackSound()
        {
            MediaPlayer.Stop();
        }
    }
}