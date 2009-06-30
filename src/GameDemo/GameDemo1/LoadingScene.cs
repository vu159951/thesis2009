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
using System.IO;
using GameDemo1.DTO;


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LoadingScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private List<Texture2D> _imageList;

        public List<Texture2D> ImageList
        {
            get { return _imageList; }
            set { _imageList = value; }
        }


        private SpriteBatch _spriteBatch;
        private int _delaytime = 100;
        private int _lastTick = System.Environment.TickCount;
        private int _indexImage = 0;

        public LoadingScene(Game game)
            : base(game)
        {
            this._spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            // TODO: Construct any child components here
            this._imageList = new List<Texture2D>();
            DirectoryInfo dir = new DirectoryInfo("Content//LoadingImage");
            FileInfo[] files = dir.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                string nameImage = files[i].Name;
                this._imageList.Add(GlobalDTO.GAME.Content.Load<Texture2D>(dir.FullName + "//" + nameImage.Substring(0, nameImage.LastIndexOf("."))));
            }
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
            this.ChangeIndexImage();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this._spriteBatch.Draw(this._imageList[this._indexImage], new Rectangle((Game.Window.ClientBounds.Width - this._imageList[this._indexImage].Width * 2) / 2, (Game.Window.ClientBounds.Height - this._imageList[this._indexImage].Height * 2) / 2, this._imageList[this._indexImage].Width * 2, this._imageList[this._indexImage].Height * 2), Color.WhiteSmoke);
            base.Draw(gameTime);
        }

        public void ChangeIndexImage()
        {
            if ((System.Environment.TickCount - this._lastTick) > this._delaytime)
            {
                this._lastTick = System.Environment.TickCount;
                this._indexImage++;
                if (this._indexImage == this._imageList.Count)
                {
                    this._indexImage = 0;
                }
            }
        }
    }
}