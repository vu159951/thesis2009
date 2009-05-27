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
using GameDemo1.Components;


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties
        private List<Player> _aliedPlayers;//list player is alied with it
        private List<Player> _antagonisticPlayers;//list player is antagonistic with it
        private int _code;// code of player
        private Color _color; // color
        private List<Sprite> _units; // list of units,are possessive units of this player
        private List<Sprite> _structures;// list of structures, are possessive structures of this player
        private List<Resource> _resources;// list of resources which this player have
        private List<string> _nameStructureCanCreate;// name of all structures which this player can create

        public List<string> NameStructureCanCreate
        {
            get { return _nameStructureCanCreate; }
            set { _nameStructureCanCreate = value; }
        }
        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
        public List<Sprite> Structures
        {
            get { return _structures; }
            set { _structures = value; }
        }
        public List<Sprite> Units
        {
            get { return _units; }
            set { _units = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public List<Player> AntagonisticPlayers
        {
            get { return _antagonisticPlayers; }
            set { _antagonisticPlayers = value; }
        }
        public List<Player> AliedPlayers
        {
            get { return _aliedPlayers; }
            set { _aliedPlayers = value; }
        }
        #endregion

        #region basic method
        // ----------------------------------------------------------------------------------------------
        //              Methods
        // ----------------------------------------------------------------------------------------------
        public Player(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this._aliedPlayers = new List<Player>();
            this._antagonisticPlayers = new List<Player>();
            this._code = 0;
            this._color = Color.White;
            this._structures = new List<Sprite>();
            this._units = new List<Sprite>();

            // resource
            this._resources = new List<Resource>();
            this._resources.Add(new Resource("Rock", 2000));
            this._resources.Add(new Resource("Gold", 150));

            this._nameStructureCanCreate = new List<string>();
            this._nameStructureCanCreate.Add("Military");
            this._nameStructureCanCreate.Add("Animal");
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

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        #region Function
        public Boolean CheckConditionToBuyStructure(Structure structure)
        {
            for (int i = 0; i < structure.RequirementResource.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (structure.RequirementResource[i].NameSource == this._resources[j].NameSource)
                    {
                        if (structure.RequirementResource[i].Quantity > this._resources[j].Quantity)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void DecreaseResourceToBuy(Structure structure)
        {
            for (int i = 0; i < structure.RequirementResource.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (structure.RequirementResource[i].NameSource == this._resources[j].NameSource)
                    {
                        this._resources[j].Quantity -= structure.RequirementResource[i].Quantity;
                    }
                }
            }
        }
        #endregion
    }
}