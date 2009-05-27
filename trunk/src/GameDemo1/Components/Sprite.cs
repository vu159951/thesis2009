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
using System.Xml.XPath;

namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // =====================================================================================================
        //=============================================== Properties =======================================================
        // =====================================================================================================
        public enum Direction
        {
            S,
            ES,
            E,
            EN,
            N,
            WN,
            W,
            WS
        }
        public enum Status
        {
            ATTACK,
            IDLE,
            WALK,
            DEAD,
            HIT,
            FLY
        }

        private Vector2 _currentRootCoordinate;// current Root Coordiante of Coordinate
        private Vector2 _position;// position, pixel on map
        private Direction _currentDirection;// direction of unit
        private Status _currentStatus; // status of unit
        private List<Texture2D> _textureSprites; // set of textures for current status and direction
        private int _currentIndex;// curent index of texture in set of textures
        private string _pathSpecificationFile;// path to file which specify about image of unit
        private int _codeFaction;// code of faction of this Unit, = 0 is neutral
        private Rectangle _boundRectangle; // rectangle of sprite(size sprite, with coodinate based on viewport)
        private string _name;// name sprite: name from xml
        private Color _color;// color to draw
        private Texture2D _selectedImage;// image which present that it selected
        private Boolean _selectedFlag;// flag to present that it selected
        private float _percentSize;//

        public float PercentSize
        {
            get { return _percentSize; }
            set { _percentSize = value; }
        }
        public Boolean SelectedFlag
        {
            get { return _selectedFlag; }
            set { _selectedFlag = value; }
        }
        public Texture2D SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Rectangle BoundRectangle
        {
            get { return _boundRectangle; }
            set { _boundRectangle = value; }
        }
        public int CodeFaction
        {
            get { return _codeFaction; }
            set { _codeFaction = value; }
        }

        public string PathSpecificationFile
        {
            get { return _pathSpecificationFile; }
            set { _pathSpecificationFile = value; }
        }
        public List<Texture2D> TextureSprites
        {
            get { return _textureSprites; }
            set { _textureSprites = value; }
        }
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }
        public Vector2 CurrentRootCoordinate
        {
            get { return _currentRootCoordinate; }
            set { _currentRootCoordinate = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Direction CurrentDirection
        {
            get { return _currentDirection; }
            set { _currentDirection = value; }
        }
        public Status CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; }
        }

        protected KeyboardState keyState;
        protected MouseState mouseState;
        protected SpriteBatch spriteBatch;
        protected Texture2D healthImage;





        // =====================================================================================================
        // ============================================== basic Method ==============================================================
        // =====================================================================================================
        /// <summary>
        ///  contructor for unit
        /// </summary>
        /// <param name="game"></param>
        public Sprite(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            this._currentDirection = Direction.S;
            this._currentStatus = Status.IDLE;
            this._color = Color.White;
            this._textureSprites = new List<Texture2D>();
            this._currentIndex = 0;
            this._currentRootCoordinate = Config.CURRENT_COORDINATE; // get current coordinate
            this._selectedFlag = false;
            this._selectedImage = game.Content.Load<Texture2D>(Config.PATH_TO_SELECTEDIMAGE);
            healthImage = this.Game.Content.Load<Texture2D>(Config.PATH_TO_HEALTHIMAGE);
        }

        /// <summary>
        /// Get texture for current status and direction of unit
        /// </summary>
        public void GetSetOfTexturesForSprite(string pathspecificationfile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathspecificationfile);
            this._name = doc.DocumentElement.Name.Replace("_", " ");// get name 
            try
            {
                this._textureSprites.Clear();
                this._currentIndex = 0;
            }
            catch { }
            this._textureSprites = new List<Texture2D>();
            XmlNodeList list = doc.SelectNodes("//" + this._currentStatus.ToString() + "/" + this._currentDirection.ToString() + "/Image");
            foreach (XmlNode node in list)
            {
                Texture2D myTex = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                this._textureSprites.Add(myTex);// add to set of textures will draw for unit
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

            /// update direcion and status here .................................... change status or direction and GetSetOfTexturesForSprite() again

            /// scroll map by keyboard
            this.ScrollingMapByKeyBoard();
            /// scroll map by mouse
            this.ScrollingMapByMouse();



            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here

            // draw unit if it's in view area

            if ((this.Position.X + this._textureSprites[_currentIndex].Width >= this.CurrentRootCoordinate.X) && (this.Position.Y + this._textureSprites[_currentIndex].Height >= this.CurrentRootCoordinate.Y))
            {
                if ((this.Position.X <= this.CurrentRootCoordinate.X + Game.Window.ClientBounds.Width) && (this.Position.Y <= this.CurrentRootCoordinate.Y + Game.Window.ClientBounds.Height))
                {
                    this._boundRectangle = new Rectangle((int)(this.Position.X - this._currentRootCoordinate.X), (int)(this.Position.Y - this._currentRootCoordinate.Y), (int)(this._textureSprites[this._currentIndex].Width * this._percentSize), (int)(this._textureSprites[this._currentIndex].Height * this._percentSize));
                    if (this._selectedFlag)
                    {
                        if (this is Unit)
                        {
                            for (int i = 0; i < ((Unit)this).CurrentHealth / 4; i++)
                            {
                                this.spriteBatch.Draw(healthImage, new Rectangle((int)(this._position.X - this._currentRootCoordinate.X), (int)(this._position.Y - this._currentRootCoordinate.Y), healthImage.Width * i, healthImage.Height), this._color);
                            }
                        }
                        else if (this is Structure)
                        {
                            for (int i = 0; i < ((Structure)this).CurrentHealth / 4; i++)
                            {
                                this.spriteBatch.Draw(healthImage, new Rectangle((int)(this._position.X - this._currentRootCoordinate.X), (int)(this._position.Y - this._currentRootCoordinate.Y), healthImage.Width * i, healthImage.Height), this._color);
                            }
                        }
                        this.spriteBatch.Draw(this._selectedImage, new Rectangle((int)(this._position.X - 10 - this._currentRootCoordinate.X), (int)(this._position.Y + this._boundRectangle.Width / 2 - this._currentRootCoordinate.Y), this._boundRectangle.Width + 20, this._boundRectangle.Height / 2), Color.White);
                    }
                    this.spriteBatch.Draw(this._textureSprites[this._currentIndex], this._boundRectangle, this._color);
                }
            }
            base.Draw(gameTime);
        }




        //-----------------------------------------------------------------------------------------------------------------------------
        //                                              Function
        //-----------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll map with Key board
        /// </summary>
        public void ScrollingMapByKeyBoard()
        {
            this.keyState = Keyboard.GetState(); // get key
            if (keyState.IsKeyDown(Keys.Up))
            {
                this._currentRootCoordinate.Y -= Config.SPEED_SCROLL.Y;// scrool up
                if (this._currentRootCoordinate.Y < 0)// if can't scroll continuous, stand here
                {
                    this._currentRootCoordinate.Y = 0;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                this._currentRootCoordinate.Y += Config.SPEED_SCROLL.Y;// scrool down
                if (this._currentRootCoordinate.Y > (Config.MAP_SIZE_IN_CELL.Width * Config.CURRENT_CELL_SIZE.Height - Game.Window.ClientBounds.Height))
                {
                    this._currentRootCoordinate.Y = Config.MAP_SIZE_IN_CELL.Height * Config.CURRENT_CELL_SIZE.Height - Game.Window.ClientBounds.Height;
                }
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                this._currentRootCoordinate.X -= Config.SPEED_SCROLL.X; // scroll left
                if (this._currentRootCoordinate.X < 0)
                {
                    this._currentRootCoordinate.X = 0;
                }
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                this._currentRootCoordinate.X += Config.SPEED_SCROLL.X; // scroll right
                if (this._currentRootCoordinate.X > (Config.MAP_SIZE_IN_CELL.Width * Config.CURRENT_CELL_SIZE.Width - Game.Window.ClientBounds.Width))
                {
                    this._currentRootCoordinate.X = Config.MAP_SIZE_IN_CELL.Width * Config.CURRENT_CELL_SIZE.Width - Game.Window.ClientBounds.Width;
                }
            }
        }

        /// <summary>
        /// Scrolling map with mouse
        /// </summary>
        protected void ScrollingMapByMouse()
        {
            this.mouseState = Mouse.GetState(); // get mouse
            if (mouseState.X <= 0)
            {
                this._currentRootCoordinate.X -= Config.SPEED_SCROLL.X; // scroll left
                if (this._currentRootCoordinate.X < 0)
                {
                    this._currentRootCoordinate.X = 0;
                }
            }
            if (mouseState.Y <= 0)
            {
                this._currentRootCoordinate.Y -= Config.SPEED_SCROLL.Y;// scrool up
                if (this._currentRootCoordinate.Y < 0)// if can't scroll continuous, stand here
                {
                    this._currentRootCoordinate.Y = 0;
                }
            }
            if (mouseState.X >= Game.Window.ClientBounds.Width - Config.CURSOR_SIZE.Width)
            {
                this._currentRootCoordinate.X += Config.SPEED_SCROLL.X; // scroll right
                if (this._currentRootCoordinate.X > (Config.MAP_SIZE_IN_CELL.Width * Config.CURRENT_CELL_SIZE.Width - Game.Window.ClientBounds.Width))
                {
                    this._currentRootCoordinate.X = Config.MAP_SIZE_IN_CELL.Width * Config.CURRENT_CELL_SIZE.Width - Game.Window.ClientBounds.Width;
                }
            }
            if (mouseState.Y >= Game.Window.ClientBounds.Height - Config.CURSOR_SIZE.Height)
            {
                this._currentRootCoordinate.Y += Config.SPEED_SCROLL.Y;// scrool down
                if (this._currentRootCoordinate.Y > (Config.MAP_SIZE_IN_CELL.Height * Config.CURRENT_CELL_SIZE.Height - Game.Window.ClientBounds.Height))
                {
                    this._currentRootCoordinate.Y = Config.MAP_SIZE_IN_CELL.Height * Config.CURRENT_CELL_SIZE.Height - Game.Window.ClientBounds.Height;
                }
            }
            return;
        }
    }
}