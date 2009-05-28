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
using GameDemo1.Components;


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ManagerGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties
        private Map _map;// map for game
        public static List<Sprite> _listUnitOnMap = new List<Sprite>();// list which manager unit on map
        public static List<Sprite> _listStructureOnMap = new List<Sprite>();// list which manager structure on map        
        private CursorGame _cursor; // cursor game
        private List<Player> _players; // players of game

        public CursorGame Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }
        public List<Sprite> ListStructureOnMap
        {
            get { return _listStructureOnMap; }
            set { _listStructureOnMap = value; }
        }
        public List<Sprite> ListUnitOnMap
        {
            get { return _listUnitOnMap; }
            set { _listUnitOnMap = value; }
        }
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
        public List<Player> Players
        {
            get { return _players; }
            set { _players = value; }
        }

        #endregion

        #region Basic method

        public ManagerGame(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

            // create cursor for game
            this._cursor = new CursorGame(game, game.Content.Load<Texture2D>(Config.PATH_TO_CURSOR_IMAGE + Config.CURSOR));
            // create list player
            this._players = new List<Player>();
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

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion

        #region Function
        /// <summary>
        /// Load battlefield from xml file
        /// </summary>
        /// <param name="battlefieldpath"></param>
        public void LoadBattleField(string battlefieldpath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(battlefieldpath);
            // load map
            this._map = new RhombusMap(this.Game, Config.PATH_TO_MAP + doc.SelectSingleNode("//Map[1]").Attributes[0].Value + ".txt", Config.START_COORDINATE);
            this.Game.Components.Add(this._map);
            // load terrain
            foreach (XmlNode nodeterrain in doc.SelectNodes("//Terrain"))
            {
                Terrain terrain = new Terrain(this.Game, Config.PATH_TO_TERRAIN_XML + nodeterrain.Attributes[0].Value + ".xml", new Vector2(float.Parse(nodeterrain.Attributes["X"].Value), float.Parse(nodeterrain.Attributes["Y"].Value)));
                this.Game.Components.Add(terrain);
            }
        }
        #endregion
    }
}