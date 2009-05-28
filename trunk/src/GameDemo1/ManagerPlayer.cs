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


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ManagerPlayer : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties
        
        /// <summary>
        ///  gia su minh la player 1
        /// </summary>
        private Player _playerIsUser;
        private Texture2D _menupanel; // menu panel
        private Rectangle _rectangleMenu;// rectangle of menu panel
        private List<Sprite> _menuItems;// menuitem on menu
        private int _selectedIndexMenuItem;// selected index
        private List<Sprite> _selectedSprites; // current selected sprites : Unit, Structure
        private Rectangle _selectedRectangle;// rectangle to select sprite(with coodinate based on map)
        private Boolean _mouseClick;//flag mouse
        private SpriteFont spriteFont; // spritefont obj
        private SpriteBatch spriteBatch;// spritebatch obj

        public Player PlayerIsUser
        {
            get { return _playerIsUser; }
            set { _playerIsUser = value; }
        }
        public Rectangle SelectedRectangle
        {
            get { return _selectedRectangle; }
            set { _selectedRectangle = value; }
        }
        public Rectangle RectangleMenu
        {
            get { return _rectangleMenu; }
            set { _rectangleMenu = value; }
        }
        public List<Sprite> MenuItems
        {
            get { return _menuItems; }
            set { _menuItems = value; }
        }
        public Boolean MouseClick
        {
            get { return _mouseClick; }
            set { _mouseClick = value; }
        }
        public List<Sprite> SelectedSprites
        {
            get { return _selectedSprites; }
            set { _selectedSprites = value; }
        }
        public Texture2D Menupanel
        {
            get { return _menupanel; }
            set { _menupanel = value; }
        }
        #endregion

        #region Base Methods
        // -----------------------------------------------------------------------------------------------------------------------------------
        // ---------------------------------------       basic Methods
        // -----------------------------------------------------------------------------------------------------------------------------------
        public ManagerPlayer(Game game)
            : base(game)
        {
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            spriteFont = game.Content.Load<SpriteFont>(Config.PATH_TO_FONT);// get font

            /// TODO: Construct any child components here
            

            // create panel menu game
            this._menupanel = game.Content.Load<Texture2D>(Config.PATH_TO_MENU_IMAGE + Config.MENU_PANEL);
            this._menuItems = new List<Sprite>();//item
            this._rectangleMenu = new Rectangle(0, Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height, Config.MENU_PANEL_SIZE.Width, Config.MENU_PANEL_SIZE.Height);//rectangle to draw
            this._selectedIndexMenuItem = -1;

            // create list selected Sprite
            this._selectedSprites = new List<Sprite>();
            //flag mouse click false
            this._mouseClick = false;
            // selected rectangle to select unit or structure
            this._selectedRectangle = new Rectangle(0, 0, 0, 0);
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
            // draw menu panel
            spriteBatch.Draw(this._menupanel, this._rectangleMenu , Color.White);

            // draw selected sprite
            if (this._selectedSprites.Count > 0)
            {
                this._menuItems.Clear();// clear all menu items
                    // if unit
                if (this._selectedSprites[this._selectedSprites.Count - 1] is Unit)
                {
                    Unit tempUnit = ((Unit)this._selectedSprites[this._selectedSprites.Count - 1]);
                    // draw information about unit
                    spriteBatch.DrawString(this.spriteFont, "* Name: " + tempUnit.Name.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 20), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Health: " + tempUnit.CurrentHealth.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 30), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Power: " + tempUnit.Power.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 40), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Radius Dectection: " + tempUnit.RadiusDetect.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 50), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Radius Attack: " + tempUnit.RadiusAttack.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 60), Color.White);
                }
                    // if structure
                else if (this._selectedSprites[this._selectedSprites.Count - 1] is Structure)
                {
                    Structure tempstructure = ((Structure)this._selectedSprites[this._selectedSprites.Count - 1]);
                    // draw information about structure
                    spriteBatch.DrawString(this.spriteFont, "* Name: " + tempstructure.Name.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 20), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Health: " + tempstructure.CurrentHealth.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 30), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Count Units: " + tempstructure.Units.Count.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 40), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Count Tectnology: " + tempstructure.Technologies.Count.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 50), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* List Units: ", new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenu.Y + 60), Color.White);
                    // create menu item for this structure
                    if (tempstructure.CurrentIndex == tempstructure.TextureSprites.Count - 1) // it is finished building
                    {
                        this.CreateMenuItemIsUnitForStructure(tempstructure);
                        this.DrawMenuItem();
                    }
                }
                // get texture of selected sprite and draw
                Texture2D tempTexture = this._selectedSprites[this._selectedSprites.Count - 1].TextureSprites[this._selectedSprites[this._selectedSprites.Count - 1].CurrentIndex];
                // calculate to get position draw image onmenu
                Vector2 positionImage = new Vector2((Config.MENU_PANEL_SIZE.Height - tempTexture.Width/(1.5f))/2, (Config.MENU_PANEL_SIZE.Height - tempTexture.Height/(1.5f))/2);
                // calculate rectangle for draw texture of sprite(slected sprite) on menu
                Rectangle tempRec = new Rectangle((int)positionImage.X + 10, this._rectangleMenu.Y + (int)positionImage.Y, (int)(tempTexture.Width/(1.5f)), (int)(tempTexture.Height/(1.5f)));
                spriteBatch.Draw(tempTexture, tempRec, Color.White);
            }

            /////   if selected sptites is nothing --> draw list structure that this player can build(now is player 1) 
                                                   //  and also is item on menu
            else if(this._selectedSprites.Count == 0)
            {
                this.DrawMenuItem();
            }

            base.Draw(gameTime);
        }
        #endregion

        #region Function
        /// <summary>
        /// Draw menu item on menu panel
        /// </summary>
        public void DrawMenuItem()
        {
            for (int i = 0; i < this._menuItems.Count; i++)
            {
                // select color for selected item
                if (i == this._selectedIndexMenuItem)
                {
                    this._menuItems[i].Color = Color.LightPink;
                }
                else
                {
                    this._menuItems[i].Color = Color.White;
                }
                // draw represent picture
                spriteBatch.Draw(this._menuItems[i].TextureSprites[0], this._menuItems[i].BoundRectangle, this._menuItems[i].Color);

                // write required information to buy this
                spriteBatch.DrawString(spriteFont, "*Require*", new Vector2(this._menuItems[i].BoundRectangle.X + this._menuItems[i].BoundRectangle.Width + 10, this._menuItems[i].BoundRectangle.Y), Color.White);
                if (this._menuItems[i] is Unit) // if is unit
                {
                    int h = 12;
                    // draw required resource to buy this unit
                    for (int j = 0; j < ((Unit)this._menuItems[i]).RequirementResource.Count; j++)
                    {
                        spriteBatch.DrawString(spriteFont, "-" + ((Unit)(this._menuItems[i])).RequirementResource[j].NameSource + " : " + ((Unit)(this._menuItems[i])).RequirementResource[j].Quantity, new Vector2(this._menuItems[i].BoundRectangle.X + this._menuItems[i].BoundRectangle.Width + 15, this._menuItems[i].BoundRectangle.Y + h * (j+1)), Color.YellowGreen);
                    }
                }
                else if (this._menuItems[i] is Structure) // if is structure
                {
                    int h = 12;
                    // draw required resource to buy this structure
                    for (int j = 0; j < ((Structure)this._menuItems[i]).RequirementResource.Count; j++)
                    {
                        spriteBatch.DrawString(spriteFont, "-" + ((Structure)(this._menuItems[i])).RequirementResource[j].NameSource + " : " + ((Structure)(this._menuItems[i])).RequirementResource[j].Quantity, new Vector2(this._menuItems[i].BoundRectangle.X + this._menuItems[i].BoundRectangle.Width + 15, this._menuItems[i].BoundRectangle.Y + h * (j+1)), Color.YellowGreen);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Draw information of player(player 1)
        /// </summary>
        public void DrawInformationPlayer()
        {
 
        }

        /// <summary>
        /// get all structures which player can create to add into menuitem
        /// </summary>
        public void CreateMenuItemIsStructure()
        {
            // clear all menu item is unit of structure;
            this._menuItems.Clear();
            Player tempplayer = this._playerIsUser;
            int i = 0;

            foreach (string structure in tempplayer.NameStructureCanCreate)
            {
                /// create menu item and draw it
                Structure item = new Structure(this.Game);
                XmlDocument doc = new XmlDocument();
                doc.Load(Config.PATH_TO_STRUCTURE_XML + structure + ".xml");
                item.PathSpecificationFile = Config.PATH_TO_STRUCTURE_XML + structure + ".xml";
                XmlNode node = doc.SelectSingleNode("//RepresentativeImage");// get node image in xml file
                // get texture of structure is item
                Texture2D tempTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                item.TextureSprites.Add(tempTexture);// get image
                item.GetInformationStructure();
                // calculate position
                Vector2 positionImage = new Vector2((Config.MENU_PANEL_SIZE.Height - tempTexture.Width / (2.0f)) / 2, (Config.MENU_PANEL_SIZE.Height - tempTexture.Height / (2.0f)) / 2);
                // calculate rectangle for item
                Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenu.Y + (int)positionImage.Y, (int)(tempTexture.Width / (2.0f)), (int)(tempTexture.Height / (2.0f)));
                item.BoundRectangle = tempRec;
                this._menuItems.Add(item);
                i += 100 + (int)(tempTexture.Width/(2.0f));
            }
        }

        /// <summary>
        /// Create Menu Item for structure : Unit to buy
        /// </summary>
        public void CreateMenuItemIsUnitForStructure(Structure structure)
        {
            int i = Config.MENU_PANEL_SIZE.Height;
            foreach (string unit in structure.NaneUnitsCanCreate)
            {
                /// create menu item and draw it
                Unit item = new Unit(this.Game);
                // get texture of unit is item
                XmlDocument doc = new XmlDocument();
                doc.Load(Config.PATH_TO_UNIT_XML + unit + ".xml");
                item.PathSpecificationFile = Config.PATH_TO_UNIT_XML + unit + ".xml";
                XmlNode node = doc.SelectSingleNode("//RepresentativeImage[1]"); // get representative image in xml file to draw as menu item

                Texture2D tempTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                item.TextureSprites.Add(tempTexture);// get image
                item.GetInformationUnit();
                //calculate position
                Vector2 positionImage = new Vector2((Config.MENU_PANEL_SIZE.Height - tempTexture.Width / (3.0f)) / 2, (Config.MENU_PANEL_SIZE.Height - tempTexture.Height / (3.0f)) / 2);
                // calculate rectangle for item
                Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenu.Y + (int)positionImage.Y + 15, (int)(tempTexture.Width / (3.0f)), (int)(tempTexture.Height / (3.0f)));
                item.BoundRectangle = tempRec;
                this._menuItems.Add(item);
                i += 100 + (int)(tempTexture.Width / (3.0f));
            }
        }

        /// <summary>
        /// Click to choose a Unit or a Structure
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public void MousePressedOnMap()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                // if click in map(viewport)
                if (0 < mouse.Y && mouse.Y < Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height)
                {
                    if (this._mouseClick == false)
                    {
                        this._mouseClick = true;
                        this._selectedRectangle = new Rectangle(mouse.X + (int)Config.CURRENT_COORDINATE.X, mouse.Y + (int)Config.CURRENT_COORDINATE.Y, 5, 5);
                        if (this._selectedSprites.Count != 0)
                        {
                            for (int i = 0; i < this._selectedSprites.Count; i++)
                            {
                                if (this._selectedSprites[i] is Unit)
                                {
                                    ((Unit)this._selectedSprites[i]).EndPoint = new Point(mouse.X + (int)Config.CURRENT_COORDINATE.X, mouse.Y + (int)Config.CURRENT_COORDINATE.Y);
                                    ((Unit)this._selectedSprites[i]).CreateMovingVector();
                                }
                            }
                        }
                    }
                } 
            }

            else if (mouse.RightButton == ButtonState.Pressed)
            {
                this._selectedIndexMenuItem = -1;
                this.ClearSelectedSprites();
                this.CreateMenuItemIsStructure();
            }
        }

        /// <summary>
        /// Mouse up
        /// </summary>
        /// <param name="game"></param>
        public void MouseUpOnMap()
        {
            MouseState mouse = Mouse.GetState();
            if (this._mouseClick == true)
            {
                if (mouse.LeftButton == ButtonState.Released)
                {
                    // if multi select

                    if (Math.Abs(mouse.X + (int)Config.CURRENT_COORDINATE.X - this._selectedRectangle.X) > 10 || Math.Abs(mouse.Y + (int)Config.CURRENT_COORDINATE.Y - this._selectedRectangle.Y) > 10)
                    {
                        // build selected rectangle
                        this.BuildSelectedRectangle(mouse.X + (int)Config.CURRENT_COORDINATE.X, mouse.Y + (int)Config.CURRENT_COORDINATE.Y);

                        // select sprite based on selected rectangle
                        this.GetSelectedSpriteWithSelectedRectangle();
                    }
                    else // if single select
                    {
                        this.GetSelectedSpriteWithPointClick();
                    }



                    // if don't select  -> build Structure
                    if (this._selectedIndexMenuItem != -1 && this._selectedSprites.Count == 0)
                    {
                        if (this._menuItems[this._selectedIndexMenuItem] is Structure && this._playerIsUser.CheckConditionToBuyStructure((Structure)this._menuItems[this._selectedIndexMenuItem]))
                        {
                            Vector2 tempposition = new Vector2(mouse.X, mouse.Y) + Config.CURRENT_COORDINATE;
                            Structure tempstructure = new Structure(this.Game, Config.PATH_TO_STRUCTURE_XML + this._menuItems[this._selectedIndexMenuItem].Name + ".xml", tempposition, this._playerIsUser.Code);
                            tempstructure.Position -= new Vector2(tempstructure.TextureSprites[0].Width / 2, tempstructure.TextureSprites[0].Height / 2);// change position to draw structure which have just been built
                            this._playerIsUser.Structures.Add(tempstructure);
                            this.AddComponentIntoGame((IGameComponent)tempstructure);
                            this._playerIsUser.DecreaseResourceToBuy((Structure)this._menuItems[this._selectedIndexMenuItem]);
                        }
                    }
                    else
                    {
                        
                    }
                    this._mouseClick = false;
                }
            }
        }

        /// <summary>
        /// Build selected rectangle with postion mouse click and position mouse up
        /// </summary>
        public void BuildSelectedRectangle(int endX, int endY)
        {
            this._selectedRectangle.Width = Math.Abs(this._selectedRectangle.X - endX) + 10;
            this._selectedRectangle.Height = Math.Abs(this._selectedRectangle.Y - endY) + 10;
            if (this._selectedRectangle.X > endX)
            {
                this._selectedRectangle.X = endX;
            }
            if (this._selectedRectangle.Y > endY)
            {
                this._selectedRectangle.Y = endY;
            }
        }

        /// <summary>
        /// Loop on list to choose unit or structure and add on selected sprite
        /// </summary>
        /// <param name="list"></param>
        public void SelectFromList(List<Sprite> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                {
                    Sprite tempSprite = list[i];
                    Rectangle tempRec = tempSprite.BoundRectangle;
                    tempRec.X += (int)Config.CURRENT_COORDINATE.X;
                    tempRec.Y += (int)Config.CURRENT_COORDINATE.Y;
                    if (tempRec.Contains(this._selectedRectangle)
                        || this._selectedRectangle.Contains(tempRec)
                        || tempRec.Intersects(this._selectedRectangle)
                        || this._selectedRectangle.Intersects(tempRec)
                        )
                    {
                        this._selectedSprites.Add(tempSprite);// if a sprite is selected
                        tempSprite.SelectedFlag = true;
                    }
                    else
                    {
                        tempSprite.SelectedFlag = false;
                    }
                }
            }
        }

        /// <summary>
        /// Get all sprite in selected rectangle
        /// </summary>
        /// <param name="game"></param>
        public void GetSelectedSpriteWithSelectedRectangle()
        {
            this.ClearSelectedSprites();
            // for on unit first
            this.SelectFromList(ManagerGame._listUnitOnMap);
            // if not unit is selected
            if (this._selectedSprites.Count == 0)
            {
                this.SelectFromList(ManagerGame._listStructureOnMap);
            }
        }

        /// <summary>
        /// Click to select single sprite
        /// </summary>
        /// <param name="game"></param>
        public void GetSelectedSpriteWithPointClick()
        {
            // click to select a unit
            for (int i = 0; i < ManagerGame._listUnitOnMap.Count; i++)
            {

                Sprite tempSprite = (ManagerGame._listUnitOnMap[i]);
                Rectangle tempRec = tempSprite.BoundRectangle;
                tempRec.X += (int)Config.CURRENT_COORDINATE.X;
                tempRec.Y += (int)Config.CURRENT_COORDINATE.Y;
                tempRec.X += tempRec.Width / 4;
                tempRec.Y += tempRec.Height / 4;
                tempRec.Height /= 2;
                tempRec.Width /= 2;
                if (tempRec.Contains(this._selectedRectangle)
                    || this._selectedRectangle.Contains(tempRec)
                    || tempRec.Intersects(this._selectedRectangle)
                    || this._selectedRectangle.Intersects(tempRec)
                    )
                {
                    this.ClearSelectedSprites();
                    this._selectedSprites.Add(tempSprite);// if a sprite is selected
                    tempSprite.SelectedFlag = true;
                    return;
                }
            }
            // or a structure
            for (int i = 0; i < ManagerGame._listStructureOnMap.Count; i++)
            {
                Sprite tempSprite = (ManagerGame._listStructureOnMap[i]);
                Rectangle tempRec = tempSprite.BoundRectangle;
                tempRec.X += (int)Config.CURRENT_COORDINATE.X;
                tempRec.Y += (int)Config.CURRENT_COORDINATE.Y;
                tempRec.X += tempRec.Width / 4;
                tempRec.Y += tempRec.Height / 4;
                tempRec.Height /= 2;
                tempRec.Width /= 2;
                if (tempRec.Contains(this._selectedRectangle)
                    || this._selectedRectangle.Contains(tempRec)
                    || tempRec.Intersects(this._selectedRectangle)
                    || this._selectedRectangle.Intersects(tempRec)
                    )
                {
                    this.ClearSelectedSprites();
                    this._selectedSprites.Add(tempSprite);// if a sprite is selected
                    tempSprite.SelectedFlag = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Click item on menu, get selected index
        /// </summary>
        /// <param name="game"></param>
        public void MouseClickOnMenu()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                // mouse click in menu
                if (Config.SCREEN_SIZE.Height > mouse.Y && mouse.Y > Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height)
                {
                    this._selectedIndexMenuItem = -1;
                    for (int i = 0; i < this._menuItems.Count; i++)
                    {
                        Rectangle temp = ((Sprite)this._menuItems[i]).BoundRectangle;
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

        /// <summary>
        /// Add component into game
        /// </summary>
        /// <param name="game"></param>
        public void AddComponentIntoGame(IGameComponent component)
        {
            IGameComponent cursor = this.Game.Components[this.Game.Components.Count - 1];
            IGameComponent managerplayer = this.Game.Components[this.Game.Components.Count - 2];
            // remove cursor and managerplayer
            this.Game.Components.Remove(cursor);
            this.Game.Components.Remove(managerplayer);
            // add component
            this.Game.Components.Add(component);
            // add cursor and managerplayer again
            this.Game.Components.Add(managerplayer);
            this.Game.Components.Add(cursor);
            // add to listunit and list structure on map
            if (component is Structure)
            {
                ManagerGame._listStructureOnMap.Add((Structure)component);
            }
            else if (component is Unit)
            {
                ManagerGame._listUnitOnMap.Add((Unit)component);
            }
        }

        /// <summary>
        /// Clear all seleted sprites in list
        /// </summary>
        public void ClearSelectedSprites()
        {
            for (int i = 0; i < this._selectedSprites.Count; i++)
            {
                this._selectedSprites[i].SelectedFlag = false;
            }
            this._selectedSprites.Clear();
        }

        #endregion
    }
}