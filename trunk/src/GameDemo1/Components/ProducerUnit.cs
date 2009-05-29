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


namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ProducerUnit : Unit
    {
        #region Properties
        private List<Resource> _listResourceToGet;// list resources which this producer is bringing on itself
        private Resource _currentResourceExploit;// current resource which it is exploiting
        private int _maxExploit;// max number that this producer can exploit
        private int _speedExploit;// quatity this producer can exploit in 1/60 s(one time to perform update method)

        public int SpeedExploit
        {
            get { return _speedExploit; }
            set { _speedExploit = value; }
        }
        public int MaxExploit
        {
            get { return _maxExploit; }
            set { _maxExploit = value; }
        }
        public List<Resource> ListResourceToGet
        {
            get { return _listResourceToGet; }
            set { _listResourceToGet = value; }
        }

        private int _delayTimeGetResource = 100;// delay time to get resource(increase quatity resource which it is exploiting)
        private int _lastTickCountForGetResource = System.Environment.TickCount;// counter delay time for increase quatity resource
        #endregion

        #region Basic methods
        public ProducerUnit(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        public ProducerUnit(Game game, string pathspecificationfile, Vector2 position, int codeFaction)
            : base(game)
        {
            this.PercentSize = 0.5f;
            this.MovingVector = new Vector2(0, 0);// now is IDLE
            this.CodeFaction = codeFaction;// code to tell difference between this with another Npc or Structure
            this.Position = position;// for postion on map
            this.PathSpecificationFile = pathspecificationfile;//get file for specification
            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get textures
            this.GetInformationUnit();// get information about health, power, radius attack, radius detect
            this.WhomIHit = null;

            // list resource to get 
            this._listResourceToGet = new List<Resource>();
            this._listResourceToGet.Add(new Resource("Rock",0));
            this._listResourceToGet.Add(new Resource("Gold",0));

            // max exploiting
            this._maxExploit = 100;
            this._speedExploit = 1;
            this._currentResourceExploit = new Resource();
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
            if (this._currentResourceExploit != null)
            {
                this.PerformGetReource();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        #region Functions
        public void PerformGetReource()
        {
            if ((System.Environment.TickCount - this._lastTickCountForGetResource) > this._delayTimeGetResource)
            {
                this._lastTickCountForGetResource = System.Environment.TickCount;
                if (this._currentResourceExploit.NameSource == "Rock")
                {
                    this._listResourceToGet[0].Quantity += 1;
                    if (this._listResourceToGet[0].Quantity >= this._maxExploit)
                    {
                        this._currentResourceExploit = null;
                    }
                }
                else if (this._currentResourceExploit.NameSource == "Rock")
                {
                    this._listResourceToGet[1].Quantity += 1;
                    if (this._listResourceToGet[1].Quantity >= this._maxExploit)
                    {
                        this._currentResourceExploit = null;
                    }
                }
            }
        }
        #endregion
    }
}