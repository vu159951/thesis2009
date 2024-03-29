using System;
using System.Collections;
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
using GameSharedObject.Components;
using GameSharedObject.DTO;


namespace GameSharedObject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ManagerGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties
        protected String _pathToMap;// đường dẫn tới file bản đồ        
        protected List<Sprite> _listUnitOnMap = new List<Sprite>();// list which manager unit on map // tập các unit có trên map
        protected List<Sprite> _listStructureOnMap = new List<Sprite>();// list which manager structure on map        // tập các structure có trên map
        protected List<Sprite> _listResourceCenterOnmap = new List<Sprite>();// list which resource center on map        // tập các resource center có trên map
        protected List<Sprite> _listUnitOnViewport = new List<Sprite>();
        protected List<Sprite> _listStructureOnViewport = new List<Sprite>();
        protected List<Structure> _listStructureOfGame = new List<Structure>();        
        public static Hashtable _iconsImage = new Hashtable();// tập các icon của game
        private CursorGame _cursor; // cursor game// con trỏ chuột của game
        public static Map _map;// map for game // map nền của game
        private List<Player> _players; // players of game // tập các player của game
        public MiniMap _minimap; // thành phần mini map trên bản đồ
        public List<SpriteDTO> _dtoList;

        public String PathToMap
        {
            get { return _pathToMap; }
            set { _pathToMap = value; }
        }
        public List<Structure> ListStructureOfGame
        {
            get { return _listStructureOfGame; }
            set { _listStructureOfGame = value; }
        }
        public MiniMap Minimap
        {
            get { return _minimap; }
            set { _minimap = value; }
        }
        public Hashtable IconsImage
        {
            get { return ManagerGame._iconsImage; }
            set { ManagerGame._iconsImage = value; }
        }
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
        public List<Sprite> ListResourceCenterOnmap
        {
            get { return _listResourceCenterOnmap; }
            set { _listResourceCenterOnmap = value; }
        }
        public List<Sprite> ListUnitOnViewport
        {
            get { return _listUnitOnViewport; }
            set { _listUnitOnViewport = value; }
        }
        public List<Sprite> ListStructureOnViewport
        {
            get { return _listStructureOnViewport; }
            set { _listStructureOnViewport = value; }
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
            GlobalDTO.GAME = this.Game;

            // create cursor for game
            this._cursor = new CursorGame(game);
            // create list
            // khởi tạo list player
            this._players = new List<Player>();
            // khởi tạo tập các icon
            this.IconsImage.Add("Gold", this.Game.Content.Load<Texture2D>("Sprites\\ResourceCenter\\GoldResourceCenter\\Icon"));
            this.IconsImage.Add("Stone", this.Game.Content.Load<Texture2D>("Sprites\\ResourceCenter\\StoneResourceCenter\\Icon"));
            this.IconsImage.Add("Health", this.Game.Content.Load<Texture2D>("Images\\Icon\\Health"));
            this.IconsImage.Add("Sword", this.Game.Content.Load<Texture2D>("Images\\Icon\\Sword"));
            this.IconsImage.Add("Eye", this.Game.Content.Load<Texture2D>("Images\\Icon\\Eye"));
            this.IconsImage.Add("Clock", this.Game.Content.Load<Texture2D>("Images\\Icon\\Clock"));
            this.IconsImage.Add("Pixel", this.Game.Content.Load<Texture2D>("Images\\Misc\\Pixel"));
            // mini map cho game
            this._minimap = new MiniMap(game, new Point(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Width - 250, GlobalDTO.SCREEN_SIZE.Height - GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 40));
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
        /// Load map và các terrain trên battle field lên từ file xml đặc tả
        /// </summary>
        /// <param name="battlefieldpath"></param>
        public void LoadBattleField(string battlefieldpath)
        {
            this._pathToMap = battlefieldpath;
            XmlDocument doc = new XmlDocument();
            doc.Load(battlefieldpath);
            // load map
            ManagerGame._map = new RhombusMap(this.Game, GlobalDTO.SPEC_MAP_PATH + doc.SelectSingleNode("//Map[1]").Attributes[0].Value + ".txt", GlobalDTO.START_COORDINATE);
            this.Game.Components.Add(ManagerGame._map);

            this.Map.OccupiedMatrix = new int[GlobalDTO.MAP_SIZE_IN_CELL.Width, GlobalDTO.MAP_SIZE_IN_CELL.Height];

            // load terrain
            foreach (XmlNode nodeterrain in doc.SelectNodes("//Terrain")){
                Terrain terrain = new Terrain(this.Game, GlobalDTO.SPEC_TERRAIN_PATH + nodeterrain.Attributes[0].Value + ".xml", new Vector2(float.Parse(nodeterrain.Attributes["X"].Value), float.Parse(nodeterrain.Attributes["Y"].Value)));

                //if (nodeterrain.Attributes[0].Value.ToLower().Contains("Rock_1") ||
                //    nodeterrain.Attributes[0].Value.ToLower().Contains("OakTree_1") ||
                //    nodeterrain.Attributes[0].Value.ToLower().Contains("CavePillar_1"))
                //    GlobalFunction.SetOccupiedCellsToMatrix(terrain);
                this.Game.Components.Add(terrain);
            }

            // load occupied matrix
            this.LoadOccupiedMatrix(battlefieldpath.Replace(".xml", ".oms"));
        }

        public void LoadOccupiedMatrix(String filePath)
        {
            this.Map.OccupiedMatrix = GameSharedObject.Data.MatrixMgr.Read(filePath).Data;
        }

        /// <summary>
        /// Add component into game
        /// Add một component mới vào game thì phải đảm báo nó luôn được vẽ sau con trỏ chuột và menu
        /// </summary>
        /// <param name="game"></param>
        public void AddComponentIntoGame(IGameComponent component)
        {
            IGameComponent cursor = this.Game.Components[this.Game.Components.Count - 1];
            IGameComponent minimap = this.Game.Components[this.Game.Components.Count - 2];
            IGameComponent managergame = this.Game.Components[this.Game.Components.Count - 3];
            IGameComponent managerplayer = this.Game.Components[this.Game.Components.Count - 4];

            // remove cursor and managerplayer
            // tạm thời lấy con trỏ chuột và menu ra tập các game component
            this.Game.Components.Remove(cursor);
            this.Game.Components.Remove(minimap);
            this.Game.Components.Remove(managergame);
            this.Game.Components.Remove(managerplayer);
            // add component
            // add component muốn add vào
            this.Game.Components.Add(component);
            // add cursor and managerplayer again
            // add con trỏ chuột vào menu vào lại
            this.Game.Components.Add(managerplayer);
            this.Game.Components.Add(managergame);
            this.Game.Components.Add(minimap);
            this.Game.Components.Add(cursor);
            // add to listunit and list structure on map
            // add component vào list quản lý game
            if (component is Structure)
            {
                GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add((Structure)component);
            }
            else if (component is Unit)
            {
                GlobalDTO.MANAGER_GAME.ListUnitOnMap.Add((Unit)component);
            }
        }
        #endregion
    }
}