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
using GameDemo1.DTO;

namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public abstract class Sprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // =====================================================================================================
        //=============================================== Properties =======================================================
        // =====================================================================================================        
        private SpriteDTO _info;        
        private Vector2 _position;// position, pixel on map // vị trí của sprite trên map tính theo hệ tọa độ toàn map
        private DirectionInfo _currentDirection;// direction of unit// hướng của sprite
        private StatusInfo _currentStatus; // status of unit// trạng thái hiện thời        
        private int _currentIndex;// curent index of texture in set of textures // index hiện tại của hình ảnh trong tập hình, để hiện thị 1 phần của hành động
        private string _pathSpecificationFile;// path to file which specify about image of unit// đường dẫn đến file xml đặc tả
        private int _codeFaction;// code of faction of this Unit, = 0 is neutral// mã của sprite để biết nó thuộc phe nào, = 0 nếu là trung lập
        private Rectangle _boundRectangle; // rectangle of sprite(size sprite, with coodinate based on viewport) // rectangle bao ngoài của sprite, với tọa độ của thành phần position trong rectangle là dựa vào view viewport -> rec này dùng để xác định vị trí và kích thước hình phải vẽ trong viewport
        private string _name;// name sprite: name from xml // tên của sprite
        private Color _color;// color to draw // màu sắc
        private Boolean _selectedFlag = false;// flag to present that it selected // cờ chứng tỏ người dùng đang chọn nó
        private float _percentSize;// // phần trăm theo kích thước ảnh thật để vẽ vào viewport
        private Texture2D _selectedImage;// image which present that it selected // hình ảnh kèm theo -> sẽ được vẽ khi người dùng nhấn chọn sprite này
        protected Texture2D _healthImage;// hình biểu thị health của sprite(unit và structure mới được vẽ)

        public SpriteDTO Info
        {
            get { return _info; }
            set { _info = value; }
        }        
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
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public DirectionInfo CurrentDirection
        {
            get { return _currentDirection; }
            set { _currentDirection = value; }
        }
        public StatusInfo CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; }
        }

        protected KeyboardState keyState;
        protected MouseState mouseState;
        protected SpriteBatch spriteBatch;





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
            this._color = Color.White;
            this._currentIndex = 0;
            //this._currentRootCoordinate = Config.CURRENT_COORDINATE; // get current coordinate
            this._selectedFlag = false;
            this._selectedImage = game.Content.Load<Texture2D>(GlobalDTO.RES_SELECTION_PATH);
            _healthImage = this.Game.Content.Load<Texture2D>(GlobalDTO.PATH_TO_HEALTHIMAGE);
        }

        /// <summary>
        /// Get texture for current status and direction of unit
        /// </summary>
        //public void GetSetOfTexturesForSprite(string pathspecificationfile)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(pathspecificationfile);
        //    this._name = doc.DocumentElement.Name.Replace("_", " ");// get name // lấy tên sprite
        //    try
        //    {
        //        this._textureSprites.Clear();
        //        this._currentIndex = 0;
        //    }
        //    catch { }
        //    // load tập hình mô tả động tác hiện tại
        //    this._textureSprites = new List<Texture2D>();
        //    XmlNodeList list = doc.SelectNodes("//" + this._currentStatus.ToString() + "/" + this._currentDirection.ToString() + "/Image");
        //    foreach (XmlNode node in list)
        //    {
        //        Texture2D myTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
        //        this._textureSprites.Add(myTexture);// add to set of textures will draw for unit
        //    }
        //}

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
            //this.ScrollingMapByMouse();



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
            // sprite tự kiểm tra nó có đang nằm trong viewport ko
            Texture2D image = this._info.Action[this._currentStatus.Name].DirectionInfo[this._currentDirection.Name].Image[this._currentIndex];
            if ((this.Position.X + image.Width >= GlobalDTO.CURRENT_COORDINATE.X) 
                && (this.Position.Y + image.Height >= GlobalDTO.CURRENT_COORDINATE.Y))
            {
                if ((this.Position.X <= GlobalDTO.CURRENT_COORDINATE.X + Game.Window.ClientBounds.Width) && (this.Position.Y <= GlobalDTO.CURRENT_COORDINATE.Y + Game.Window.ClientBounds.Height))
                {
                    // nếu đang nằm trong viewport -> xác định rectagle  bao ngoài
                    this._boundRectangle = new Rectangle((int)(this.Position.X - GlobalDTO.CURRENT_COORDINATE.X), (int)(this.Position.Y - GlobalDTO.CURRENT_COORDINATE.Y), (int)(image.Width * this._percentSize), (int)(image.Height * this._percentSize));
                    if (this._selectedFlag) // nếu sprite đang bị user select
                    {
                        if (this is Unit) // sprite là unit
                        {                            
                            this.spriteBatch.Draw(this._healthImage, new Rectangle((int)(this._position.X - GlobalDTO.CURRENT_COORDINATE.X), (int)(this._position.Y - GlobalDTO.CURRENT_COORDINATE.Y), (int)((((Unit)this).CurrentHealth * 1.0f / ((Unit)this).MaxHealth) * 64), this._healthImage.Height), this._color); // vẽ máu
                        }
                        else if (this is Structure) // hoặc là structure
                        {
                            this.spriteBatch.Draw(this._healthImage, new Rectangle((int)(this._position.X - GlobalDTO.CURRENT_COORDINATE.X), (int)(this._position.Y - GlobalDTO.CURRENT_COORDINATE.Y), (int)((((Structure)this).CurrentHealth * 1.0f / ((Structure)this).MaxHealth) * 128), this._healthImage.Height), this._color);// vẽ máu
                        }
                        //this.spriteBatch.Draw(this._selectedImage, new Rectangle((int)(this._position.X - 10 - this._currentRootCoordinate.X), (int)(this._position.Y + this._boundRectangle.Width / 2 - this._currentRootCoordinate.Y), this._boundRectangle.Width + 20, this._boundRectangle.Height / 2), Color.White); // vẽ cái hình biểu thị nó đang được select
                    }
                    this.spriteBatch.Draw(image, this._boundRectangle, this._color); // vẽ sprite ra màn hình view port
                }
            }
            base.Draw(gameTime);
        }
    }
}