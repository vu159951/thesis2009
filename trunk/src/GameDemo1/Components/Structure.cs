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
using System.Xml;

namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class Structure : Sprite
    {
        #region Properties
        private Boolean _flagAttacked; // flag
        private int _maxHealth; // max health of Structure
        private int _currentHealth;//  current health of this structure
        private List<Technology> _technologies; // technologies of Structure
        private List<Sprite> _Units; // all possessive units which it created
        private List<string> _nameUnitsCanCreate; // name of all unit which it can create
        private int _delayTimeToBuild;// delay time to build success
        private List<Resource> _requirementResource;// list resources which require to build this structure

        public List<Resource> RequirementResource
        {
            get { return _requirementResource; }
            set { _requirementResource = value; }
        }
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }
        public List<string> NaneUnitsCanCreate
        {
            get { return _nameUnitsCanCreate; }
            set { _nameUnitsCanCreate = value; }
        }
        public List<Technology> Technologies
        {
            get { return _technologies; }
            set { _technologies = value; }
        }
        public List<Sprite> Units
        {
            get { return _Units; }
            set { _Units = value; }
        }
        public Boolean FlagAttacked
        {
            get { return _flagAttacked; }
            set { _flagAttacked = value; }
        }

        protected int lastTickCountForChangeImage = System.Environment.TickCount;// counter delay time for change texture

        #endregion



        #region Basic method
        // ------------------------------------------------------------------------------------------------------
        //                          Methods
        // ------------------------------------------------------------------------------------------------------
        public Structure(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this._flagAttacked = false;
            this._maxHealth = 100;
            this._technologies = new List<Technology>();
            this._Units = new List<Sprite>();
        }

        public Structure(Game game, string pathspecificationfile, Vector2 position, int codeFaction)
            :base(game)
        {
            this.PercentSize = 1.0f;
            this._flagAttacked = false;
            this.CodeFaction = codeFaction;            
            this._technologies = new List<Technology>(); // set of techs
            this.Position = position;// for position
            this.PathSpecificationFile = pathspecificationfile;// get path to specification file
            this.GetSetOfTexturesForSprite(pathspecificationfile);// get texture

            // get name of all units which it can create, 
            // time to build finish, and set delay time to build
            // max health
            this.GetInformationStructure();
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
        /// Called when graphics resources need to be loaded.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: Add your load content code here

            base.LoadContent();
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Add your unload content code here

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            /// build this structure by change image
            if (this.CurrentIndex != -1 && this.CurrentIndex != this.TextureSprites.Count)
            {
                this.PerformBuild();
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here

            base.Draw(gameTime);
        }
        #endregion



        #region Function
        /// <summary>
        /// get name of all units that this structure can create
        /// </summary>
        public void GetInformationStructure()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.PathSpecificationFile);
            // get list units which can create
            this._Units = new List<Sprite>();// list units which this structure can create
            XmlNodeList list = doc.SelectNodes("//ListUnits/Unit");
            this._nameUnitsCanCreate = new List<string>();
            foreach (XmlNode node in list)
            {
                this._nameUnitsCanCreate.Add(node.Attributes[0].Value);// get name of unit which it can create
            }

            // get max health
            this._maxHealth = int.Parse(doc.SelectSingleNode("//MaxHealth").Attributes[0].Value);
            this._currentHealth = this._maxHealth;

            //calculate delay time to build this structure
            XmlNode timenode = doc.SelectSingleNode("//Time");// get time to build finish this structure
            this._delayTimeToBuild = (int.Parse(timenode.Attributes[0].Value) / this.TextureSprites.Count) * 1000; // get delay time to build this structure

            //get set of resource which require to build this resource
            this._requirementResource = new List<Resource>();// list resource which this structure require to build
            XmlNode requirementnode = doc.SelectSingleNode("//Requirements");
            foreach (XmlNode node in requirementnode.ChildNodes)
            {
                Resource resource = new Resource(node.Name, int.Parse(node.Attributes[0].Value));
                this._requirementResource.Add(resource);
            }
            this.Name = doc.DocumentElement.Name;
        }

        /// <summary>
        /// change current index of set of textures to change image for action
        /// </summary>
        public void PerformBuild()
        {
            if ((System.Environment.TickCount - this.lastTickCountForChangeImage) > this._delayTimeToBuild)
            {
                this.lastTickCountForChangeImage = System.Environment.TickCount;
                this.CurrentIndex++;
                if (this.CurrentIndex == this.TextureSprites.Count)
                {
                    this.CurrentIndex = this.TextureSprites.Count - 1;
                }
            }
        }
        #endregion
    }
}