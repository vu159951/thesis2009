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
        private Player _playerIsUser;// player đang được quản lý bởi người chơi(user cũng là player này) hiện tại trong demo user là player 1. Nói cách khác, thuộc tính này cho biết User đang điều khiển player nào

        private Texture2D _menupanelBottom; // menu panel // hình ảnh của menu bên dưới
        private Rectangle _rectangleMenuBottom;// rectangle of menu panel // rectangle của menu bên dưới
        private List<Sprite> _menuItemsBottom;// menuitem on menu // tập các menu items của menu bên dưới
        private int _selectedIndexMenuItemBottom;// selected index // index của item được chọn hiện tại

        private List<Sprite> _selectedSprites; // current selected sprites : Unit, Structure // list các sprite đang được chọn từ map : Unit, Structure, resource center
        private Rectangle _selectedRectangle;// rectangle to select sprite(with coodinate based on map) // rectangle được dùng để chọn nhiều spritte cũng lúc

        private Boolean _mouseClick;//flag mouse // true nếu chuột nhấn và false nếu chuột ko nhấn(ko nhấn chứ ko phải được nhả)
        private SpriteFont spriteFont; // spritefont obj// sprite để vẽ chữ
        private SpriteBatch spriteBatch;// spritebatch obj // sprite để vẽ hình

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
        public Rectangle RectangleMenuBottom
        {
            get { return _rectangleMenuBottom; }
            set { _rectangleMenuBottom = value; }
        }
        public List<Sprite> MenuItemsBottom
        {
            get { return _menuItemsBottom; }
            set { _menuItemsBottom = value; }
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
            get { return _menupanelBottom; }
            set { _menupanelBottom = value; }
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
            // set thông tin cho menu phía dưới màn hình
            this._menupanelBottom = game.Content.Load<Texture2D>(Config.PATH_TO_MENU_IMAGE + Config.MENU_PANEL); // hình menu
            this._menuItemsBottom = new List<Sprite>();//item // danh sách các item
            this._rectangleMenuBottom = new Rectangle(0, Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height, Config.MENU_PANEL_SIZE.Width, Config.MENU_PANEL_SIZE.Height);//rectangle to draw // rectangle để vẽ
            this._selectedIndexMenuItemBottom = -1;// hiện tại selected index = -1 --> ko chọn cái nào cả

            // create list selected Sprite
            this._selectedSprites = new List<Sprite>();// list các selected sprite đang được chọn trên map
            //flag mouse click false
            this._mouseClick = false; // false nghĩa là chuột ko trong trạng thái nhấn
            // selected rectangle to select unit or structure
            this._selectedRectangle = new Rectangle(0, 0, 0, 0);// hình chữ nhật dùng cho multi select chưa được tạo
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
            spriteBatch.Draw(this._menupanelBottom, this._rectangleMenuBottom , Color.White);

            // draw selected sprite
            // nếu có ít nhất 1 sprite trên map được chọn --> vẽ sprite đó kèm với thông tin của nó ra menu
            if (this._selectedSprites.Count > 0)
            {
                this._menuItemsBottom.Clear();// clear all menu items // xóa tất cả các menu item hiện tại
                // vẽ các thông tin về sprite đươc chọn từ map ( chỉ vẽ sprite cuối cùng trong tập các sprite được chọn )
                if (this._selectedSprites[this._selectedSprites.Count - 1] is Unit)// if unit // nếu sprite là 1 unit
                {
                        // if not producer // nếu là unit thường
                    if (!(this._selectedSprites[this._selectedSprites.Count - 1] is ProducerUnit))
                    {
                        Unit tempUnit = ((Unit)this._selectedSprites[this._selectedSprites.Count - 1]);
                        // draw information about unit
                        // vẽ thông tin của unit đó ra
                        spriteBatch.DrawString(this.spriteFont, "* Name: " + tempUnit.Name.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Health: " + tempUnit.CurrentHealth.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Power: " + tempUnit.Power.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Dectection: " + tempUnit.RadiusDetect.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 50), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Attack: " + tempUnit.RadiusAttack.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 60), Color.White);
                    }
                        // if producer // nếu là 1 producer
                    else if (this._selectedSprites[this._selectedSprites.Count - 1] is ProducerUnit)
                    {
                        ProducerUnit tempUnit = ((ProducerUnit)this._selectedSprites[this._selectedSprites.Count - 1]);
                        // draw information about unit
                        // vẽ thông tin producer đó ra
                        spriteBatch.DrawString(this.spriteFont, "* Name: " + tempUnit.Name.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Health: " + tempUnit.CurrentHealth.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Current Resource: ", new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                        if (tempUnit.CurrentResourceExploiting != null)
                        {
                            spriteBatch.DrawString(this.spriteFont, tempUnit.CurrentResourceExploiting.NameRerource + " : " + tempUnit.CurrentResourceExploiting.Quantity, new Vector2(Config.MENU_PANEL_SIZE.Height + 10, this._rectangleMenuBottom.Y + 40 + 10), Color.Gold);
                        }
                    }
                }
                else if (this._selectedSprites[this._selectedSprites.Count - 1] is Structure) // if structure // nếu sprite là 1 structure
                {
                    Structure tempstructure = ((Structure)this._selectedSprites[this._selectedSprites.Count - 1]);
                    // draw information about structure
                    // vẽ thông tin structure đó ra
                    spriteBatch.DrawString(this.spriteFont, "* Name: " + tempstructure.Name.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Health: " + tempstructure.CurrentHealth.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Count Units: " + tempstructure.Units.Count.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Count Tectnology: " + tempstructure.Technologies.Count.ToString(), new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 50), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* List Units: ", new Vector2(Config.MENU_PANEL_SIZE.Height, this._rectangleMenuBottom.Y + 60), Color.White);
                    // create menu item for this structure
                    // đồng thời, vẽ tập các unit mà structure này có thể tạo ra(các unit này bây giờ trở thành menu item)
                    if (tempstructure.CurrentIndex == tempstructure.TextureSprites.Count - 1) // it is finished building // nhưng structure chỉ có thể được mua unit khi nó đã hoàn tất việc xây dựng
                    {
                        this.CreateMenuItemIsUnitForStructure(tempstructure); // tạo tập menu item là các unit mà structure có thể mua
                        this.DrawMenuItem();
                    }
                }

                // sau khi đã vẽ hết tất cả các thông tin liên quan đến sprite được chọn
                // --> cuối cùng, tính toán vị trí để vẽ sprite được chọn vào menu
                // get texture of selected sprite and draw
                Texture2D tempTexture = this._selectedSprites[this._selectedSprites.Count - 1].TextureSprites[this._selectedSprites[this._selectedSprites.Count - 1].CurrentIndex];
                // calculate to get position draw image onmenu
                Vector2 positionImage = new Vector2((Config.MENU_PANEL_SIZE.Height - tempTexture.Width/(1.5f))/2, (Config.MENU_PANEL_SIZE.Height - tempTexture.Height/(1.5f))/2);
                // calculate rectangle for draw texture of sprite(slected sprite) on menu
                Rectangle tempRec = new Rectangle((int)positionImage.X + 10, this._rectangleMenuBottom.Y + (int)positionImage.Y, (int)(tempTexture.Width/(1.5f)), (int)(tempTexture.Height/(1.5f)));
                spriteBatch.Draw(tempTexture, tempRec, Color.White);
            }

            /////   if selected sptites is nothing --> draw list structure that this player can build(now is player 1) 
                                                   //  and also is item on menu
            // nếu ko có sprite nào trên map được người dùng chọn
            else if(this._selectedSprites.Count == 0)
            { 
                this.DrawMenuItem(); // vẽ ra các structure mà player có thể mua được vào menu và biến chúng thành các item để player lựa chọn và mua
            }

            this.DrawInformationPlayer(); // viết thông tin player vào menu

            base.Draw(gameTime);
        }
        #endregion

        #region Function
        /// <summary>
        /// Draw menu item on menu panel
        /// Vẽ các item vào trrong menu bên dưới
        /// </summary>
        public void DrawMenuItem()
        {
            for (int i = 0; i < this._menuItemsBottom.Count; i++)// với mỗi item
            {
                // select color for selected item
                // thiết lập màu sắc khác nếu item đó đang được user lựa chọn
                if (i == this._selectedIndexMenuItemBottom)
                {
                    this._menuItemsBottom[i].Color = Color.LightPink;
                }
                else
                {
                    this._menuItemsBottom[i].Color = Color.White;
                }
                // draw represent picture
                // vẽ hình đại diện của item
                spriteBatch.Draw(this._menuItemsBottom[i].TextureSprites[0], this._menuItemsBottom[i].BoundRectangle, this._menuItemsBottom[i].Color);

                // write required information to buy this
                // nêu các đòi hỏi để mua item này
                spriteBatch.DrawString(spriteFont, "*Require*", new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width + 10, this._menuItemsBottom[i].BoundRectangle.Y), Color.White);
                if (this._menuItemsBottom[i] is Unit) // if is unit // giả như đây là 1 unit
                {
                    int h = 16;
                    // draw required resource to buy this unit
                    // viết các yêu cầu để mua unit này ra
                    for (int j = 0; j < ((Unit)this._menuItemsBottom[i]).RequirementResource.Count; j++)
                    {
                        spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width, this._menuItemsBottom[i].BoundRectangle.Y + h * (j + 1)), Color.White);
                        spriteBatch.DrawString(spriteFont, ((Unit)(this._menuItemsBottom[i])).RequirementResource[j].NameRerource + " : " + ((Unit)(this._menuItemsBottom[i])).RequirementResource[j].Quantity, new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width + 20, this._menuItemsBottom[i].BoundRectangle.Y + h * (j+1)), Color.Goldenrod);
                    }
                }
                else if (this._menuItemsBottom[i] is Structure) // if is structure// giống như trên
                {
                    int h = 16;
                    // draw required resource to buy this structure
                    for (int j = 0; j < ((Structure)this._menuItemsBottom[i]).RequirementResource.Count; j++)
                    {
                        if (((Structure)this._menuItemsBottom[i]).RequirementResource[j].NameRerource == "Gold")
                        {
                            spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width, this._menuItemsBottom[i].BoundRectangle.Y + h * (j + 1)), Color.White);
                        }
                        else if (((Structure)this._menuItemsBottom[i]).RequirementResource[j].NameRerource == "Stone")
                        {
                            spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Stone"]), new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width, this._menuItemsBottom[i].BoundRectangle.Y + h * (j + 1)), Color.White);
                        }
                        spriteBatch.DrawString(spriteFont, ((Structure)(this._menuItemsBottom[i])).RequirementResource[j].NameRerource + " : " + ((Structure)(this._menuItemsBottom[i])).RequirementResource[j].Quantity, new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width + 20, this._menuItemsBottom[i].BoundRectangle.Y + h * (j+1)), Color.YellowGreen);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Draw information of player(player 1)
        /// Viết thông tin về player là user(player1) vào menu
        /// </summary>
        public void DrawInformationPlayer()
        {
            // viết thông tin về các resource mà player đang có
            spriteBatch.DrawString(spriteFont, "* Inforamtion player *", new Vector2(this._rectangleMenuBottom.Width - 200, this._rectangleMenuBottom.Y + 30), Color.Gold);
            spriteBatch.DrawString(spriteFont, "* Resource", new Vector2(this._rectangleMenuBottom.Width - 220, this._rectangleMenuBottom.Y + 40), Color.LightYellow);
            int h = 10;
            for (int i = 0; i < this._playerIsUser.Resources.Count; i++)
            {
                spriteBatch.DrawString(spriteFont, "- " + this._playerIsUser.Resources[i].NameRerource + " : " + this._playerIsUser.Resources[i].Quantity.ToString(), new Vector2(this._rectangleMenuBottom.Width - 210, this._rectangleMenuBottom.Y + 40 + h * (i + 1)), Color.White);
            }
        }

        /// <summary>
        /// get all structures which player can create to add into menuitem
        /// tạo menu item mới từ tập các structure mà player này có thể xây dựng trong thời điểm hiện tại
        /// </summary>
        public void CreateMenuItemIsStructure()
        {
            // clear all menu item is unit of structure;
            // Xóa list các menu item hiện tại
            this._menuItemsBottom.Clear();
            int i = 0;
            foreach (string structure in this._playerIsUser.NameStructureCanCreate) // với mỗi structure trong tập các structure mà player này có thể xây dựng
            {
                /// create menu item and draw it
                // tạo ra menu item là structure
                Structure item = new Structure(this.Game);
                // lấy thông tin trong xml đặc tả
                XmlDocument doc = new XmlDocument();
                doc.Load(Config.PATH_TO_STRUCTURE_XML + structure + ".xml");
                item.PathSpecificationFile = Config.PATH_TO_STRUCTURE_XML + structure + ".xml";
                XmlNode node = doc.SelectSingleNode("//RepresentativeImage");// get node image in xml file
                // get texture of structure is item
                Texture2D tempTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                item.TextureSprites.Add(tempTexture);// get image // lấy hình đại diện để vẽ vào menu
                item.GetInformationStructure();// lấy các thông tin liên quan

                // tính toán ra vị trí vẽ vào menu item
                // calculate position
                Vector2 positionImage = new Vector2((Config.MENU_PANEL_SIZE.Height - tempTexture.Width / (2.0f)) / 2, (Config.MENU_PANEL_SIZE.Height - tempTexture.Height / (2.0f)) / 2);
                // calculate rectangle for item
                Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y, (int)(tempTexture.Width / (2.0f)), (int)(tempTexture.Height / (2.0f)));
                item.BoundRectangle = tempRec;
                this._menuItemsBottom.Add(item);
                i += 100 + (int)(tempTexture.Width/(2.0f));
            }
        }

        /// <summary>
        /// Create Menu Item for structure : Unit to buy
        /// tạo ra tập menu item mới là tất các unit mà structure(tham số input) có thể mua trong thời điểm hiện tại
        /// </summary>
        public void CreateMenuItemIsUnitForStructure(Structure structure)
        {
            int i = Config.MENU_PANEL_SIZE.Height;
            foreach (string unit in structure.NaneUnitsCanCreate)// đối với mỗi cái tên unit có mà structure có thể mua
            {
                /// create menu item and draw it
                // tạo ra 1 item là obj của Unit
                Unit item = new Unit(this.Game);

                // get texture of unit is item
                // lấy các thông tin cần thiết cho item
                XmlDocument doc = new XmlDocument();
                doc.Load(Config.PATH_TO_UNIT_XML + unit + ".xml");
                item.PathSpecificationFile = Config.PATH_TO_UNIT_XML + unit + ".xml";
                XmlNode node = doc.SelectSingleNode("//RepresentativeImage[1]"); // get representative image in xml file to draw as menu item
                Texture2D tempTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                item.TextureSprites.Add(tempTexture);// lấy hình ảnh đại diện để thể hiện trên menu
                item.GetInformationUnit();// lấy thông tin

                //calculate position
                // tính toán vị trí để vẽ menu item(lúc này là unit) vào trong menu
                Vector2 positionImage = new Vector2((Config.MENU_PANEL_SIZE.Height - tempTexture.Width / (3.0f)) / 2, (Config.MENU_PANEL_SIZE.Height - tempTexture.Height / (3.0f)) / 2);
                // calculate rectangle for item
                Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y + 15, (int)(tempTexture.Width / (3.0f)), (int)(tempTexture.Height / (3.0f)));
                item.BoundRectangle = tempRec;
                this._menuItemsBottom.Add(item);
                i += 100 + (int)(tempTexture.Width / (3.0f));
            }
        }

        /// <summary>
        /// Click to choose a Unit or a Structure
        /// Xử lý khi chuột được nhấn trên map chiến trường
        /// Các sự kiện có thể phát sinh
        ///     1. User chọn 1 sprite nào đó
        ///     2. User muốn di chuyển unit nào đó
        ///     3. User nhấn chuột để bắt đầu vẽ selectedRec cho 1 sự lựa chọn multi
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public void MousePressedOnMap()
        {
            MouseState mouse = Mouse.GetState(); // lấy trạng thái chuột
            if (mouse.LeftButton == ButtonState.Pressed) // nếu chuột đang nhấn là chuột trái
            {
                // if click in map(viewport)
                if (0 < mouse.Y && mouse.Y < Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height) // và chuột đang nhấn trong vùng view port(không nhấn vào menu)
                {
                    if (this._mouseClick == false)// nếu ban đầu chuột ko nhấn(_mouseClick == false)
                    {
                        this._mouseClick = true; // chuyển thành cớ đánh dấu là chuột đã nhấn
                        this._selectedRectangle = new Rectangle(mouse.X + (int)Config.CURRENT_COORDINATE.X, mouse.Y + (int)Config.CURRENT_COORDINATE.Y, 5, 5);// rectangle dùng để select multi sẽ được khởi tạo đủ nhỏ để chứa 1 sprite
                        /// Lưu ý : giả như khi đó user ko phải mún chọn sprite trên map chiến trường
                        // mà chỉ là click chuột để điều khiển sự di chuyển của quân --> ta có sự kiểm tra sau
                        if (this._selectedSprites.Count != 0)
                        {
                            for (int i = 0; i < this._selectedSprites.Count; i++)// với mỗi unit được chọn để di chuyển
                            {
                                if (this._selectedSprites[i] is Unit)
                                {
                                    // thiết lập vị trí end point dựa vào vị trí chuột click tính theo hệ tọa độ của toàn map
                                    ((Unit)this._selectedSprites[i]).EndPoint = new Point(mouse.X + (int)Config.CURRENT_COORDINATE.X, mouse.Y + (int)Config.CURRENT_COORDINATE.Y);
                                    // tạo ra véc tơ di chuyển dựa vào điểm đầu và cuối --> cuối cùng Unit này tự thực hiện đi với các hàm cài đặt trong nó
                                    ((Unit)this._selectedSprites[i]).CreateMovingVector();
                                }
                            }
                        }
                    }
                } 
            }

            else if (mouse.RightButton == ButtonState.Pressed) // nếu chuột đang nhấn là chuột phải
            {
                // thiết lập menu về trang thái gốc : chứa các menu item là structure mà player này có thể xây dựng
                this._selectedIndexMenuItemBottom = -1;
                this.ClearSelectedSprites();
                this.CreateMenuItemIsStructure();
            }
        }

        /// <summary>
        /// Mouse up
        /// Chuột được nhã trên map chiến trường
        /// </summary>
        /// <param name="game"></param>
        public void MouseUpOnMap()
        {
            MouseState mouse = Mouse.GetState();// lấy trạng thái chuột
            if (this._mouseClick == true) // nếu ban đầu có nhận chuột
            {
                if (mouse.LeftButton == ButtonState.Released) // bây giờ chuột được nhả
                {
                    // if multi select
                    // nếu chuột nhả ra là sự kết thúc của 1 đợt multi select được thực hiện bằng selected Rectangle
                    if (Math.Abs(mouse.X + (int)Config.CURRENT_COORDINATE.X - this._selectedRectangle.X) > 10 || Math.Abs(mouse.Y + (int)Config.CURRENT_COORDINATE.Y - this._selectedRectangle.Y) > 10)
                    {
                        // build selected rectangle
                        // khởi tạo lại selected Rectangle to hơn khi nhấn chuột để có thể bao trùm nhiều sprite hơn
                        this.BuildSelectedRectangle(mouse.X + (int)Config.CURRENT_COORDINATE.X, mouse.Y + (int)Config.CURRENT_COORDINATE.Y);

                        // select sprite based on selected rectangle
                        // lấy tập các sprite bị bao trùm bởi selected rectangle
                        this.GetSelectedSpriteWithSelectedRectangle();
                    }
                    else // if single select // còn nếu chuột nhả ra mà ko phải là việc chọn multi
                    {
                        // thì với 1 selected rectangle nhỏ hơn được khởi tạo khi nhấn chuột, 1 sprite sẽ được select
                        this.GetSelectedSpriteWithPointClick();
                    }



                    // if don't select  -> build Structure
                    if (this._selectedIndexMenuItemBottom != -1 && this._selectedSprites.Count == 0)
                    {
                        if (this._menuItemsBottom[this._selectedIndexMenuItemBottom] is Structure && this._playerIsUser.CheckConditionToBuyStructure((Structure)this._menuItemsBottom[this._selectedIndexMenuItemBottom]))
                        {
                            Vector2 tempposition = new Vector2(mouse.X, mouse.Y) + Config.CURRENT_COORDINATE;
                            Structure tempstructure = new Structure(this.Game, Config.PATH_TO_STRUCTURE_XML + this._menuItemsBottom[this._selectedIndexMenuItemBottom].Name + ".xml", tempposition, this._playerIsUser.Code);
                            // change position to draw structure which have just been built
                            tempstructure.Position -= new Vector2(tempstructure.TextureSprites[0].Width / 2, tempstructure.TextureSprites[0].Height / 2);
                            this._playerIsUser.Structures.Add(tempstructure);
                            this.AddComponentIntoGame((IGameComponent)tempstructure);
                            this._playerIsUser.DecreaseResourceToBuy((Structure)this._menuItemsBottom[this._selectedIndexMenuItemBottom]);
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
            // duyệt list cần duyệt
            for (int i = 0; i < list.Count; i++)
            {
                {
                    Sprite tempSprite = list[i];// lấy ra sprite từ list
                    Rectangle tempRec = tempSprite.BoundRectangle;
                    tempRec.X += (int)Config.CURRENT_COORDINATE.X;
                    tempRec.Y += (int)Config.CURRENT_COORDINATE.Y;
                    if (tempRec.Contains(this._selectedRectangle)// kiểm tra sprite này có nằm trong vùng của selected rectangle ko
                        || this._selectedRectangle.Contains(tempRec)
                        || tempRec.Intersects(this._selectedRectangle)
                        || this._selectedRectangle.Intersects(tempRec)
                        )
                    {
                        // nếu true
                        // --> add nó vào _selectedSprites
                        this._selectedSprites.Add(tempSprite);// if a sprite is selected
                        tempSprite.SelectedFlag = true;// bật cờ được chọn cho nó
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
        /// Lấy ra tất cả các sprite bị bao trùm bởi selected Rectangle để làm thành list các selected sprite
        /// </summary>
        /// <param name="game"></param>
        public void GetSelectedSpriteWithSelectedRectangle()
        {
            this.ClearSelectedSprites();// xóa tập các selected sprite hiện thời
            // for on unit first
            this.SelectFromList(ManagerGame._listUnitOnMap);// ưu tiên việc select từ các unit
            // if not unit is selected
            if (this._selectedSprites.Count == 0) // nếu ko có unit nào được chọn
            {   // --> select từ các structure
                this.SelectFromList(ManagerGame._listStructureOnMap);
            }
        }

        /// <summary>
        /// Click to select single sprite
        /// Lấy ra sprite bị click chọn bỏ vào list các selected sprite
        /// </summary>
        /// <param name="game"></param>
        public void GetSelectedSpriteWithPointClick()
        {
            // click to select a unit
            // ưu tiên cho các unit
            for (int i = 0; i < ManagerGame._listUnitOnMap.Count; i++)// với mỗi unit trong list các unit trên map
            {

                Sprite tempSprite = (ManagerGame._listUnitOnMap[i]);
                Rectangle tempRec = tempSprite.BoundRectangle;
                tempRec.X += (int)Config.CURRENT_COORDINATE.X;
                tempRec.Y += (int)Config.CURRENT_COORDINATE.Y;
                tempRec.X += tempRec.Width / 4;
                tempRec.Y += tempRec.Height / 4;
                tempRec.Height /= 2;
                tempRec.Width /= 2;
                if (tempRec.Contains(this._selectedRectangle) // kiểm tra unit có bị chọn ko
                    || this._selectedRectangle.Contains(tempRec)
                    || tempRec.Intersects(this._selectedRectangle)
                    || this._selectedRectangle.Intersects(tempRec)
                    )
                {
                    // nếu bị chọn, xóa đi list các selected sprite hiện  tại và add nó vào
                    this.ClearSelectedSprites();
                    this._selectedSprites.Add(tempSprite);// if a sprite is selected
                    tempSprite.SelectedFlag = true; // bật cờ được chọn cho nó thành true
                    return;// thoát hàm
                }
            }
            // or a structure
            // thực hiện tương tự cho các structure
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
        /// Click chuột trên menu
        /// </summary>
        /// <param name="game"></param>
        public void MouseClickOnMenu()
        {
            MouseState mouse = Mouse.GetState();// lấy trạng thái chuột
            if (mouse.LeftButton == ButtonState.Pressed) // chuột đang nhấn
            {
                // mouse click in menu
                if (Config.SCREEN_SIZE.Height > mouse.Y && mouse.Y > Config.SCREEN_SIZE.Height - Config.MENU_PANEL_SIZE.Height) // chuột đang nhấn trong menu
                {
                    this._selectedIndexMenuItemBottom = -1;// chuyển select index về thành -1
                    for (int i = 0; i < this._menuItemsBottom.Count; i++) // tìm kiếm trong tập các menu item
                    {
                        Rectangle temp = ((Sprite)this._menuItemsBottom[i]).BoundRectangle;
                        // if click on 1 item
                        // nếu item nào bị nhấn trúng
                        if (temp.X < mouse.X && mouse.X < (temp.X + temp.Width) && temp.Y < mouse.Y && mouse.Y < (temp.Y + temp.Height))
                        {
                            this._selectedIndexMenuItemBottom = i;// get index of this item// sét lại index là chỉ số của item đó trong mãng các item
                            return;// thoát
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add component into game
        /// Add một component mới vào game thì phải đảm báo nó luôn được vẽ sau con trỏ chuột và menu
        /// </summary>
        /// <param name="game"></param>
        public void AddComponentIntoGame(IGameComponent component)
        {
            IGameComponent cursor = this.Game.Components[this.Game.Components.Count - 1];
            IGameComponent managerplayer = this.Game.Components[this.Game.Components.Count - 2];
            // remove cursor and managerplayer
            // tạm thời lấy con trỏ chuột và menu ra tập các game component
            this.Game.Components.Remove(cursor);
            this.Game.Components.Remove(managerplayer);
            // add component
            // add component muốn add vào
            this.Game.Components.Add(component);
            // add cursor and managerplayer again
            // add con trỏ chuột vào menu vào lại
            this.Game.Components.Add(managerplayer);
            this.Game.Components.Add(cursor);
            // add to listunit and list structure on map
            // add component vào list quản lý game
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
        /// Xóa tất cả các sprite trong danh sách các sprite được select trên  map
        /// </summary>
        public void ClearSelectedSprites()
        {
            for (int i = 0; i < this._selectedSprites.Count; i++)
            {
                this._selectedSprites[i].SelectedFlag = false; // chuyển cờ thành false
            }
            this._selectedSprites.Clear(); // clear danh sách
        }

        #endregion
    }
}