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
using System.Xml;
using System.IO;
using System.Windows.Forms;
using GameDemo1.ButtonEvent;


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties
        private Map _map;// map for game
        private string _mapstr = "";
        List<string> _nameTerrains;// list name terrain

        private Texture2D _menupanel; // menu panel
        private Rectangle _recMenu;// rectangle of menu panel
        private List<Terrain> _menuItems;// menuitem on menu
        private int _selectedIndexMenuItem;// selected index
        private List<Sprite> _buttons;
        private string _filename = "a.xml";

        private CursorGame _cursor; // cursor game
        private Boolean _conflict;// conflict if create terrain on another terrain

        private SpriteFont spriteFont; // 
        private SpriteBatch spriteBatch;//


        public string Mapstr
        {
            get { return _mapstr; }
            set { _mapstr = value; }
        }
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }
        public List<string> NameTerrain
        {
            get { return _nameTerrains; }
            set { _nameTerrains = value; }
        }
        public Rectangle RecMenu
        {
            get { return _recMenu; }
            set { _recMenu = value; }
        }
        public List<Terrain> MenuItems
        {
            get { return _menuItems; }
            set { _menuItems = value; }
        }
        public CursorGame Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }
        public Texture2D Menupanel
        {
            get { return _menupanel; }
            set { _menupanel = value; }
        }
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
        #endregion


        #region basic method
        // -----------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------       basic Methods
        // -----------------------------------------------------------------------------------------------------------------------------------
        public Manager(Game game)
            : base(game)
        {
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            spriteFont = game.Content.Load<SpriteFont>(Config.PATH_TO_FONT);// get font

            // TODO: Construct any child components here
            this._cursor = new CursorGame(game, game.Content.Load<Texture2D>(Config.PATH_TO_CURSOR_IMAGE + Config.CURSOR)); // create cursor for game

            // create menu panel
            this._menupanel = game.Content.Load<Texture2D>(Config.PATH_TO_MENU_IMAGE + Config.MENU_PANEL);// create panel menu game
            this._menuItems = new List<Terrain>();
            this._recMenu = new Rectangle(0, Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height, Config.MENU_PANEL_SIZE.Width, Config.MENU_PANEL_SIZE.Height);
            this._selectedIndexMenuItem = -1;

            // create list terraint to build
            this._nameTerrains = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(Config.PATH_TO_TERRAIN_XML);
            foreach (FileInfo f in dir.GetFiles())
            {
                this._nameTerrains.Add(f.Name.Substring(0, f.Name.Length - 4));
            }
            this.CreateMenuItemIsTerrain();// create new menu item

            // Button
            this._buttons = new List<Sprite>();
            this.CreateButtons(game);

            //
            this._conflict = false;
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
        ///  Draw
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // draw menu panel
            spriteBatch.Draw(this._menupanel, this._recMenu, Color.White);

            // draw menu item
            this.DrawMenuItem();

            // draw button
            for (int i = 0; i < this._buttons.Count; i++)
            {
                spriteBatch.Draw(this._buttons[i].TextureSprites[0], this._buttons[i].Rec, this._buttons[i].Color);
            }  

            base.Draw(gameTime);
        }
        #endregion


        #region Function
        /// ----------------------------------------------------------------------------------------------------------------------
        /// -------------------------------------------- Function -----------------------------------------------------
        /// ----------------------------------------------------------------------------------------------------------------------

               
        /// <summary>
        /// Draw menu item on menu panel
        /// </summary>
        public void DrawMenuItem()
        {
            for (int i = 0; i < this._menuItems.Count; i++)
            {
                if (i == this._selectedIndexMenuItem)
                {
                    this._menuItems[i].Color = Color.Red;
                }
                else
                {
                    this._menuItems[i].Color = Color.White;
                }
                spriteBatch.Draw(this._menuItems[i].TextureSprites[0], this._menuItems[i].Rec, this._menuItems[i].Color);
            }
        }

        public void CreateButtons(Game game)
        {
            // save
            Sprite saveButton = new Sprite(game);
            saveButton.Position = new Vector2(game.Window.ClientBounds.Width - 128, game.Window.ClientBounds.Height - 125);
            saveButton.TextureSprites = new List<Texture2D>();
            saveButton.TextureSprites.Add(game.Content.Load<Texture2D>("Button\\Save"));
            saveButton.Rec = new Rectangle((int)saveButton.Position.X, (int)saveButton.Position.Y, 32, 32);
            this._buttons.Add(saveButton);

            // close
            Sprite closebutton = new Sprite(game);
            closebutton.Position = new Vector2(game.Window.ClientBounds.Width - 128, game.Window.ClientBounds.Height - 90);
            closebutton.TextureSprites = new List<Texture2D>();
            closebutton.TextureSprites.Add(game.Content.Load<Texture2D>("Button\\Close"));
            closebutton.Rec = new Rectangle((int)closebutton.Position.X, (int)closebutton.Position.Y, 32, 32);
            this._buttons.Add(closebutton);

            // close
            Sprite backbutton = new Sprite(game);
            backbutton.Position = new Vector2(game.Window.ClientBounds.Width - 128, game.Window.ClientBounds.Height - 55);
            backbutton.TextureSprites = new List<Texture2D>();
            backbutton.TextureSprites.Add(game.Content.Load<Texture2D>("Button\\Back"));
            backbutton.Rec = new Rectangle((int)backbutton.Position.X, (int)backbutton.Position.Y, 32, 32);
            this._buttons.Add(backbutton);
        }

        /// <summary>
        /// Create menu item is terrains
        /// </summary>
        public void CreateMenuItemIsTerrain()
        {
            this._menuItems.Clear();
            int i = 0;

            foreach (string terrain in this._nameTerrains)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Config.PATH_TO_TERRAIN_XML + terrain + ".xml");
                XmlNode node = doc.SelectNodes("//Image")[doc.SelectNodes("//Image").Count - 1];// get node image in xml file

                /// create menu item and draw it
                Terrain item = new Terrain(this.Game);
                // get texture of structure is item
                Texture2D tempTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                item.TextureSprites.Add(tempTexture);// get image
                item.Name = terrain;// get name
                // calculate position
                Vector2 positionImage = new Vector2((Config.MENU_PANEL_SIZE.Height - tempTexture.Width / (3.5f)) / 2, (Config.MENU_PANEL_SIZE.Height - tempTexture.Height / (3.5f)) / 2);
                // calculate rectangle for item
                Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._recMenu.Y + (int)positionImage.Y, (int)(tempTexture.Width / (3.5f)), (int)(tempTexture.Height / (3.5f)));
                item.Rec = tempRec;
                this._menuItems.Add(item);
                i += 10 + (int)(tempTexture.Width / (3.5f));
            }
        }

        /// <summary>
        /// Click to create a terrain if selected index greater than -1
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public void MousePressedOnMap(Game game)
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (0 < this._cursor.Position.Y && this._cursor.Position.Y < Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height)  // if click in map
                {

                    for (int i = 0; i < game.Components.Count; i++)
                    {
                        if (game.Components[i] is Terrain)
                        {
                            Terrain tempSprite = ((Terrain)game.Components[i]);
                            Rectangle tempRec = tempSprite.Rec;
                            if (tempRec.X < this.Cursor.Position.X && this.Cursor.Position.X < (tempRec.X + tempRec.Width * 7 / 8) && (tempRec.Y + tempRec.Height / 8) < this._cursor.Position.Y && this._cursor.Position.Y < (tempRec.Y + tempRec.Height))
                            {
                                this._conflict = true;
                                break;
                            }
                        }
                    }


                    if (this._selectedIndexMenuItem != -1)
                    {
                        if (this._conflict == false) // not conflict
                        {
                            Vector2 tempposition = new Vector2(this._cursor.Position.X, this.Cursor.Position.Y) + Config.CURRENT_COORDINATE;
                            Terrain tempstructure = new Terrain(this.Game, Config.PATH_TO_TERRAIN_XML + this._menuItems[this._selectedIndexMenuItem].Name + ".xml", tempposition);

                            // SET TO OCCUPIED TERRAIN
                            setOccupiedCellsToMatrix(tempstructure);
                            tempstructure.Position -= new Vector2(tempstructure.TextureSprites[0].Width / 4, tempstructure.TextureSprites[0].Height / 4);// change position to draw terrain

                            this.AddComponentIntoGame(game, (IGameComponent)tempstructure);
                        }
                        else
                        {
                            this._conflict = false;
                        }
                    }
                }
            }

            else if (mouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                this._selectedIndexMenuItem = -1;
            }
        }

        /// <summary>
        /// Add component into game
        /// </summary>
        /// <param name="game"></param>
        public void AddComponentIntoGame(Game game, IGameComponent component)
        {
            // get cursor and manager
            IGameComponent cursor = game.Components[game.Components.Count - 1];
            IGameComponent manager = game.Components[game.Components.Count - 2];
            //remove
            game.Components.Remove(cursor);
            game.Components.Remove(manager);

            // get all terrain with higher layer
            List<IGameComponent> templist = new List<IGameComponent>();
            for (int i = 0; i < game.Components.Count; i++)
            {
                if (game.Components[i] is Terrain)
                {
                    if (((Terrain)game.Components[i]).Position.Y > ((Terrain)component).Position.Y)
                    {
                        templist.Add(game.Components[i]);
                    }
                }
            }
            //remove
            for (int i = 0; i < templist.Count; i++)
            {
                game.Components.Remove(templist[i]);
            }

            game.Components.Add(component);

            // add again
            for (int i = 0; i < templist.Count; i++)
            {
                game.Components.Add(templist[i]);
            }
            game.Components.Add(manager);
            game.Components.Add(cursor);
        }

        /// <summary>
        /// Click item on menu, get selected index
        /// </summary>
        /// <param name="game"></param>
        public void MouseClickOnMenu(Game game)
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                // mouse click in menu
                if (Config.SCREEN_SIZE.Height > mouse.Y && mouse.Y > Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height)
                {
                    // click on button save
                    if (this._buttons[0].Position.X < mouse.X && mouse.X < (this._buttons[0].Position.X + this._buttons[0].Rec.Width) && this._buttons[0].Position.Y < mouse.Y && mouse.Y < (this._buttons[0].Position.Y + this._buttons[0].Rec.Height))
                    {
                        this._selectedIndexMenuItem = -1;
                        if (ButtonEvents.SaveButton(game,this._filename,this._mapstr))
                        {
                            MessageBox.Show("Successfully");
                        }
                        else
                        {
                            MessageBox.Show("Not Successfully");
                        }
                    }
                    // click on button close
                    else if (this._buttons[1].Position.X < mouse.X && mouse.X < (this._buttons[1].Position.X + this._buttons[1].Rec.Width) && this._buttons[1].Position.Y < mouse.Y && mouse.Y < (this._buttons[1].Position.Y + this._buttons[1].Rec.Height))
                    {
                        ButtonEvents.CloseButton();
                    }
                    // click on button back
                    else if (this._buttons[2].Position.X < mouse.X && mouse.X < (this._buttons[2].Position.X + this._buttons[2].Rec.Width) && this._buttons[2].Position.Y < mouse.Y && mouse.Y < (this._buttons[2].Position.Y + this._buttons[2].Rec.Height))
                    {
                        ButtonEvents.BackButton(game);
                    }

                    // click on item
                    else
                    {
                        for (int i = 0; i < this._menuItems.Count; i++)
                        {
                            Rectangle temp = ((Sprite)this._menuItems[i]).Rec;
                            // if click on 1 item
                            if (temp.X < mouse.X && mouse.X < (temp.X + temp.Width) && temp.Y < mouse.Y && mouse.Y < (temp.Y + temp.Height))
                            {
                                this._selectedIndexMenuItem = i;// get index of this item
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void setOccupiedCellsToMatrix(Terrain terrain)
        {
            Point ROOT_POS = new Point((Config.CURRENT_CELL_SIZE.Width >> 1) * (Config.MAP_SIZE_IN_CELL.Width - 1), 0);
            Transform trans = new RomhbusTransform(ROOT_POS, Config.CURRENT_CELL_SIZE.Width, Config.CURRENT_CELL_SIZE.Height);

            Point from = trans.PointToCell(new Point((int)terrain.Position.X, (int)terrain.Position.Y + (terrain.Rec.Width / 3)));
            Point to = trans.PointToCell(new Point((int)terrain.Position.X + terrain.Rec.Width, (int)terrain.Position.Y + terrain.Rec.Height));

            for (int i = from.X; i < to.X; i++){
                for (int j = from.Y; j < to.Y; j++){
                    Config.OccupiedMatrix[i, j] = 1;
                }
            }

        }

        #endregion
    }
}