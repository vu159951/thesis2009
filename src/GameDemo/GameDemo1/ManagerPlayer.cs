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
using GameDemo1.DTO;


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

        private Texture2D _menupanelTop; // menu panel // hình ảnh của menu bên trên
        private Rectangle _rectangleMenuTop;// rectangle of menu panel // rectangle của menu bên trên                

        private List<Sprite> _selectedSprites; // current selected sprites : Unit, Structure // list các sprite đang được chọn từ map : Unit, Structure, resource center
        private Rectangle _selectedRectangle;// rectangle to select sprite(with coodinate based on map) // rectangle được dùng để chọn nhiều spritte cũng lúc

        private List<List<Unit>> _listUnitsBuying = new List<List<Unit>>();// danh sách các unit đã chọn mua
        private Unit _processingBuyUnit = null;// unit đang được xử lý mua và chờ hoàn thành sau thời gian

        private Boolean _mouseClickOnMap;//flag mouse // true nếu chuột nhấn và false nếu chuột ko nhấn(ko nhấn chứ ko phải được nhả)
        private Boolean _mouseLeftClickOnMenu;//flag mouse // true nếu chuột trái nhấn vào menu và false nếu chuột ko nhấn vào menu(ko nhấn chứ ko phải được nhả)
        private Boolean _mouseRightClickOnMenu;//flag mouse // true nếu chuột phải nhấn vào menu và false nếu chuột ko nhấn vào menu(ko nhấn chứ ko phải được nhả)
        private SpriteFont spriteFont; // spritefont obj// sprite để vẽ chữ
        private SpriteBatch spriteBatch;// spritebatch obj // sprite để vẽ hình
        private int _delayTimeForProcessingBuyUnit = 1000;// thời gian trì hoãn cho xử lý mua unit
        private int _lastTickCountForProcessingBuyUnit = System.Environment.TickCount;// biến đếm timer cho thời gian trì hoãn

        public Player PlayerIsUser
        {
            get { return _playerIsUser; }
            set { _playerIsUser = value; }
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
            spriteFont = game.Content.Load<SpriteFont>(GlobalDTO.RES_FONT_PATH);// get font            
            /// TODO: Construct any child components here
            

            // create panel menu game
            // set thông tin cho menu phía dưới màn hình
            this._menupanelBottom = game.Content.Load<Texture2D>(GlobalDTO.PATH_TO_MENU_IMAGE + GlobalDTO.MENU_PANEL_BOTTOM); // hình menu
            this._menuItemsBottom = new List<Sprite>();//item // danh sách các item
            this._rectangleMenuBottom = new Rectangle(0, GlobalDTO.SCREEN_SIZE.Height - GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Width, GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height);//rectangle to draw // rectangle để vẽ
            this._selectedIndexMenuItemBottom = -1;// hiện tại selected index = -1 --> ko chọn cái nào cả

            // set thong tin cho menu phía trên màn hình
            this._menupanelTop = game.Content.Load<Texture2D>(GlobalDTO.PATH_TO_MENU_IMAGE + GlobalDTO.MENU_PANEL_TOP); // hình menu
            this._rectangleMenuTop = new Rectangle(0, 0, GlobalDTO.MENU_PANEL_TOP_SIZE.Width, GlobalDTO.MENU_PANEL_TOP_SIZE.Height);//rectangle to draw // rectangle để vẽ

            // create list selected Sprite
            this._selectedSprites = new List<Sprite>();// list các selected sprite đang được chọn trên map
            // false nghĩa là chuột ko trong trạng thái nhấn
            this._mouseClickOnMap = false;
            // false nghĩa là chuột ko trong trạng thái nhấn
            this._mouseLeftClickOnMenu = false; 
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
            // các thao tác chuột của user
            this.MousePressedOnMap();
            this.MousePressOnMenu();
            this.MouseUpOnMap();
            this.MouseUpOnMenu();

            // xử lý mua unit
            if (this._listUnitsBuying.Count > 0 && this._processingBuyUnit == null) // có unit đang được đợi mua và chưa có unit nào đang trong quá trình xử lý mua
            {
                this._processingBuyUnit = this._listUnitsBuying[0][0];// đẩy unit đầu tiên trong danh sách vào để xử lý
            }
            if (this._processingBuyUnit != null) // nếu có unit đang trong quá trình xử lý mua
            {
                // xử lý mua unit đó
                this.ProcessBuyUnitWithTime();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // draw menu panel
            // vẽ các menu panel
            this.DrawMenuPanel();

            // vẽ thông tin vào các menu
            this.DrawInformationIntoMenu();

            base.Draw(gameTime);
        }
        #endregion

        #region Function
        /// <summary>
        /// Vẽ các menu panel vào màn hình
        /// </summary>
        public void DrawMenuPanel()
        {
            spriteBatch.Draw(this._menupanelBottom, this._rectangleMenuBottom, Color.White);
            spriteBatch.Draw(this._menupanelTop, this._rectangleMenuTop, Color.Gray);
        }

        /// <summary>
        /// Vẽ các thông tin vào menu
        /// Menu trên chức thông tin player
        /// Menu dưới chứa
        ///     1. Sprite đang được select trên map nếu có
        ///     2. Các menu item để lựa chọn và mua
        /// </summary>
        public void DrawInformationIntoMenu()
        {
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
                        spriteBatch.DrawString(this.spriteFont, "* Name: " + tempUnit.Info.Name, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Health: " + tempUnit.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Power: " + ((UnitDTO)tempUnit.Info).InformationList["Power"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Dectection: " + ((UnitDTO)tempUnit.Info).InformationList["RadiusDetect"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 50), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Attack: " + ((UnitDTO)tempUnit.Info).InformationList["RadiusAttack"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 60), Color.White);
                    }
                    // if producer // nếu là 1 producer
                    else if (this._selectedSprites[this._selectedSprites.Count - 1] is ProducerUnit)
                    {
                        //ProducerUnit tempUnit = ((ProducerUnit)this._selectedSprites[this._selectedSprites.Count - 1]);
                        //// draw information about unit
                        //// vẽ thông tin producer đó ra
                        //spriteBatch.DrawString(this.spriteFont, "* Name: " + tempUnit.Name.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                        //spriteBatch.DrawString(this.spriteFont, "* Health: " + tempUnit.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                        //spriteBatch.DrawString(this.spriteFont, "* Current Resource: ", new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                        //if (tempUnit.CurrentResourceExploiting != null)
                        //{
                        //    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage[tempUnit.CurrentResourceExploiting.NameRerource]), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40 + 20), Color.White);
                        //    spriteBatch.DrawString(this.spriteFont, tempUnit.CurrentResourceExploiting.NameRerource + " : " + tempUnit.CurrentResourceExploiting.Quantity, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 20, this._rectangleMenuBottom.Y + 40 + 20), Color.Gold);
                        //}
                    }
                }
                else if (this._selectedSprites[this._selectedSprites.Count - 1] is Structure) // if structure // nếu sprite là 1 structure
                {
                    Structure tempstructure = ((Structure)this._selectedSprites[this._selectedSprites.Count - 1]);
                    // draw information about structure
                    // vẽ thông tin structure đó ra
                    spriteBatch.DrawString(this.spriteFont, "* Name: " + tempstructure.Info.Name, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Health: " + tempstructure.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Count Units: " + tempstructure.Units.Count.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Count Tectnology: " + tempstructure.Technologies.Count.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 50), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* List Units: ", new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 60), Color.White);
                    // create menu item for this structure
                    // đồng thời, vẽ tập các unit mà structure này có thể tạo ra(các unit này bây giờ trở thành menu item)
                    if (tempstructure.CurrentIndex == tempstructure.Info.Action[tempstructure.CurrentStatus.Name].DirectionInfo[tempstructure.CurrentDirection.Name].Image.Count - 1) // it is finished building // nhưng structure chỉ có thể được mua unit khi nó đã hoàn tất việc xây dựng
                    {
                        this.CreateMenuItemIsUnitForStructure(tempstructure); // tạo tập menu item là các unit mà structure có thể mua
                        this.DrawMenuItem();
                    }
                }

                // sau khi đã vẽ hết tất cả các thông tin liên quan đến sprite được chọn
                // --> cuối cùng, tính toán vị trí để vẽ sprite được chọn vào menu
                // get texture of selected sprite and draw
                Texture2D tempTexture = this._selectedSprites[this._selectedSprites.Count - 1].Info.Action[this._selectedSprites[this._selectedSprites.Count - 1].CurrentStatus.Name].DirectionInfo[this._selectedSprites[this._selectedSprites.Count - 1].CurrentDirection.Name].Image[this._selectedSprites[this._selectedSprites.Count - 1].CurrentIndex];
                // calculate to get position draw image onmenu
                Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (1.5f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (1.5f)) / 2);
                // calculate rectangle for draw texture of sprite(slected sprite) on menu
                Rectangle tempRec = new Rectangle((int)positionImage.X + 10, this._rectangleMenuBottom.Y + (int)positionImage.Y, (int)(tempTexture.Width / (1.5f)), (int)(tempTexture.Height / (1.5f)));
                spriteBatch.Draw(tempTexture, tempRec, Color.White);
            }

            /////   if selected sptites is nothing --> draw list structure that this player can build(now is player 1) 
            //  and also is item on menu
            // nếu ko có sprite nào trên map được người dùng chọn
            else if (this._selectedSprites.Count == 0)
            {
                this.DrawMenuItem(); // vẽ ra các structure mà player có thể mua được vào menu và biến chúng thành các item để player lựa chọn và mua
            }

            this.DrawInformationPlayer(); // viết thông tin player vào menu
        }

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
                spriteBatch.Draw(this._menuItemsBottom[i].Info.Icon, this._menuItemsBottom[i].BoundRectangle, this._menuItemsBottom[i].Color);
                // write required information to buy this
                // nêu các đòi hỏi để mua item này                
                spriteBatch.DrawString(spriteFont, this._menuItemsBottom[i].Info.Name.Replace("_"," "), new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width-10, this._menuItemsBottom[i].BoundRectangle.Y), Color.LightBlue);
                if (this._menuItemsBottom[i] is Unit) // if is unit // giả như đây là 1 unit
                {
                    //int h = 16;
                    //// draw required resource to buy this unit
                    //// viết các yêu cầu để mua unit này ra
                    //for (int j = 0; j < ((Unit)this._menuItemsBottom[i]).RequirementResources.Count; j++)
                    //{
                    //    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width, this._menuItemsBottom[i].BoundRectangle.Y + h * (j + 1)), Color.White);
                    //    spriteBatch.DrawString(spriteFont, ((Unit)(this._menuItemsBottom[i])).RequirementResources[j].NameRerource + " : " + ((Unit)(this._menuItemsBottom[i])).RequirementResources[j].Quantity, new Vector2(this._menuItemsBottom[i].BoundRectangle.X + this._menuItemsBottom[i].BoundRectangle.Width + 20, this._menuItemsBottom[i].BoundRectangle.Y + h * (j + 1)), Color.Goldenrod);
                    //}
                    //// viết số lượng unit này mà user đang đặt mua
                    //for (int j = 0; j < this._listUnitsBuying.Count; j++)
                    //{
                    //    if (this._listUnitsBuying[j][0].Name == this._menuItemsBottom[i].Name)
                    //    {
                    //        spriteBatch.DrawString(spriteFont, this._listUnitsBuying[j].Count.ToString(), new Vector2(this._menuItemsBottom[i].BoundRectangle.X + 16, this._menuItemsBottom[i].BoundRectangle.Y + 8), Color.Lime);
                    //    }
                    //}
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
            spriteBatch.DrawString(spriteFont, "* Resource ", new Vector2(50, this._rectangleMenuTop.Y + 10), Color.Gold);
            int h = 150;
            for (int i = 0; i < this._playerIsUser.Resources.Count; i++)
            {
                // vẽ các icon mô tả resource
                if (this._playerIsUser.Resources[i].NameRerource == "Gold")
                {
                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(h * (i + 1), this._rectangleMenuTop.Y + 10), Color.White);
                }
                else if (this._playerIsUser.Resources[i].NameRerource == "Stone")
                {
                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Stone"]), new Vector2(h * (i + 1), this._rectangleMenuTop.Y + 10), Color.White);
                }
                // viết thông tin resource
                spriteBatch.DrawString(spriteFont, this._playerIsUser.Resources[i].NameRerource + " : " + this._playerIsUser.Resources[i].Quantity.ToString(), new Vector2(h * (i + 1) + 20, this._rectangleMenuTop.Y + 10), Color.LightBlue);
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
            for (int j = 0; j < this._playerIsUser.ListStructureOfGameInPlayer.Count;j++ ) // với mỗi structure trong tập các structure 
            {
                if (this._playerIsUser.ListStructureOfGameInPlayer[j].flag == true)//mà player này có thể xây dựng(true)
                {
                    /// create menu item and draw it
                    // tạo ra menu item là structure
                    string structurename = this._playerIsUser.ListStructureOfGameInPlayer[j].structureOfGame.Name;
                    Structure item = (Structure)this._playerIsUser.ListStructureOfGameInPlayer[j].structureOfGame;
                    // lấy thông tin trong xml đặc tả
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load(GlobalDTO.SPEC_STRUCTURE_PATH + structurename + ".xml");
                    //item.PathSpecificationFile = GlobalDTO.SPEC_STRUCTURE_PATH + structurename + ".xml";
                    //XmlNode node = doc.SelectSingleNode("//RepresentativeImage");// get node image in xml file
                    // get texture of structure is item
                    //Texture2D tempTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                    Texture2D tempTexture = item.Info.Icon;
                    //item.TextureSprites.Add(tempTexture);// get image // lấy hình đại diện để vẽ vào menu
                    //item.GetInformationStructure();// lấy các thông tin liên quan
                    item.PlayerContainer = this._playerIsUser;

                    // tính toán ra vị trí vẽ vào menu item
                    // calculate position
                    Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (2.0f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (2.0f)) / 2);
                    // calculate rectangle for item
                    Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y, (int)(tempTexture.Width / (2.0f)), (int)(tempTexture.Height / (2.0f)));
                    item.BoundRectangle = tempRec;
                    this._menuItemsBottom.Add(item);
                    i += 100 + (int)(tempTexture.Width / (2.0f));
                }
            }
        }

        /// <summary>
        /// Create Menu Item for structure : Unit to buy
        /// tạo ra tập menu item mới là tất các unit mà structure(tham số input) có thể mua trong thời điểm hiện tại
        /// </summary>
        public void CreateMenuItemIsUnitForStructure(Structure structure)
        {
            int i = GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height;
            if (this._menuItemsBottom.Count > 0)
            {
                return;
            }
            foreach (ItemInfo unit in ((StructureDTO)structure.Info).UnitList)// đối với mỗi cái tên unit có mà structure có thể mua
            {
                /// create menu item and draw it
                // tạo ra 1 item là obj của Unit                
                Unit item = new Unit(this.Game);
                item.Info = new UnitDTO();
                item.Info = GlobalDTO.UNIT_DATA_READER.Load(GlobalDTO.SPEC_UNIT_PATH + unit.Name + ".xml");
                item.CurrentStatus = item.Info.Action[StatusList.IDLE.Name];
                item.CurrentDirection = item.Info.Action[item.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];
                //if (structure.Info.Name == "TownHall")
                //{
                //    item = new ProducerUnit(this.Game);
                //}

                // get texture of unit is item
                // lấy các thông tin cần thiết cho item
                //XmlDocument doc = new XmlDocument();
                //doc.Load(GlobalDTO.SPEC_UNIT_PATH + unit + ".xml");
                //item.PathSpecificationFile = GlobalDTO.SPEC_UNIT_PATH + unit + ".xml";
                //XmlNode node = doc.SelectSingleNode("//RepresentativeImage[1]"); // get representative image in xml file to draw as menu item
                //Texture2D tempTexture = Game.Content.Load<Texture2D>(node.Attributes[0].Value);
                Texture2D tempTexture = item.Info.Icon;
                //item.TextureSprites.Add(tempTexture);// lấy hình ảnh đại diện để thể hiện trên menu
                //item.GetInformationUnit();// lấy thông tin
                item.StructureContainer = (Structure)this._selectedSprites[0];// structure chứa nó là structure đang được chọn
                item.PlayerContainer = this._playerIsUser;//  nó thuộc player là user vì user đang điều khiển trò chơi để mua nó

                //calculate position
                // tính toán vị trí để vẽ menu item(lúc này là unit) vào trong menu
                Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (2.0f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (2.0f)) / 2);
                // calculate rectangle for item
                Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y + 15, (int)(tempTexture.Width / (2.0f)), (int)(tempTexture.Height / (2.0f)));
                item.BoundRectangle = tempRec;
                this._menuItemsBottom.Add(item);
                i += 60 + (int)(tempTexture.Width / (2.0f));
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
                if (0 < mouse.Y && mouse.Y < GlobalDTO.SCREEN_SIZE.Height - GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height) // và chuột đang nhấn trong vùng view port(không nhấn vào menu)
                {
                    if (this._mouseClickOnMap == false)// nếu ban đầu chuột ko nhấn(_mouseClick == false)
                    {
                        this._mouseClickOnMap = true; // chuyển thành cớ đánh dấu là chuột đã nhấn
                        this._selectedRectangle = new Rectangle(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X, mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y, 5, 5);// rectangle dùng để select multi sẽ được khởi tạo đủ nhỏ để chứa 1 sprite
                        /// Lưu ý : giả như khi đó user ko phải mún chọn sprite trên map chiến trường
                        // mà chỉ là click chuột để điều khiển sự di chuyển của quân --> ta có sự kiểm tra sau
                        if (this._selectedSprites.Count != 0)
                        {
                            for (int i = 0; i < this._selectedSprites.Count; i++)// với mỗi unit được chọn để di chuyển
                            {
                                if (this._selectedSprites[i] is Unit)
                                {
                                    // thiết lập vị trí end point dựa vào vị trí chuột click tính theo hệ tọa độ của toàn map
                                    ((Unit)this._selectedSprites[i]).EndPoint = new Point(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X, mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y);
                                    // tạo ra véc tơ di chuyển dựa vào điểm đầu và cuối 
                                    ((Unit)this._selectedSprites[i]).CreateMovingVector();
                                    //--> cuối cùng Unit này tự thực hiện đi với các hàm cài đặt trong nó
                                }
                            }
                        }
                    }
                } 
            }

            else if (mouse.RightButton == ButtonState.Pressed) // nếu chuột đang nhấn là chuột phải
            {
                // if click in map(viewport)
                if (0 < mouse.Y && mouse.Y < GlobalDTO.SCREEN_SIZE.Height - GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height) // và chuột đang nhấn trong vùng view port(không nhấn vào menu)
                {
                    // thiết lập menu về trang thái gốc : chứa các menu item là structure mà player này có thể xây dựng
                    this._selectedIndexMenuItemBottom = -1;
                    this.ClearSelectedSprites();
                    this.CreateMenuItemIsStructure();
                }
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
            if (this._mouseClickOnMap == true) // nếu ban đầu có nhận chuột
            {
                if (mouse.LeftButton == ButtonState.Released) // bây giờ chuột được nhả
                {
                    // if multi select
                    // nếu chuột nhả ra là sự kết thúc của 1 đợt multi select được thực hiện bằng selected Rectangle
                    if (Math.Abs(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X - this._selectedRectangle.X) > 10 || Math.Abs(mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y - this._selectedRectangle.Y) > 10)
                    {
                        // build selected rectangle
                        // khởi tạo lại selected Rectangle to hơn khi nhấn chuột để có thể bao trùm nhiều sprite hơn
                        this.CreateSelectedRectangle(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X, mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y);

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
                    // nếu ko có sprite nào được chọn và có item menu nào đó được chọn
                    if (this._selectedIndexMenuItemBottom != -1 && this._selectedSprites.Count == 0)
                    {
                        // mà nếu là item menu loại structure -> user muốn mua structure và thả vào map
                        if (this._menuItemsBottom[this._selectedIndexMenuItemBottom] is Structure && this._playerIsUser.CheckConditionToBuyStructure((Structure)this._menuItemsBottom[this._selectedIndexMenuItemBottom])) // kiểm tra  resource yêu cầu có thỏa
                        {
                            // mua structure và đặt vào vị trí trỏ chuột
                            this.BuyStructure(mouse);
                            this._selectedIndexMenuItemBottom = -1;
                        }
                    }
                    this._mouseClickOnMap = false;
                }
            }
        }

        /// <summary>
        /// Build selected rectangle with postion mouse click and position mouse up
        /// </summary>
        public void CreateSelectedRectangle(int endX, int endY)
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
                    tempRec.X += (int)GlobalDTO.CURRENT_COORDINATE.X;
                    tempRec.Y += (int)GlobalDTO.CURRENT_COORDINATE.Y;
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
                tempRec.X += (int)GlobalDTO.CURRENT_COORDINATE.X;
                tempRec.Y += (int)GlobalDTO.CURRENT_COORDINATE.Y;
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
                tempRec.X += (int)GlobalDTO.CURRENT_COORDINATE.X;
                tempRec.Y += (int)GlobalDTO.CURRENT_COORDINATE.Y;
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
        public void MousePressOnMenu()
        {
            MouseState mouse = Mouse.GetState();// lấy trạng thái chuột
            if (mouse.LeftButton == ButtonState.Pressed) // chuột trái đang nhấn
            {
                // mouse click in menu
                if (GlobalDTO.SCREEN_SIZE.Height > mouse.Y && mouse.Y > GlobalDTO.SCREEN_SIZE.Height - GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height) // chuột đang nhấn trong menu
                {
                    if (this._mouseLeftClickOnMenu == false)
                    {
                        this._selectedIndexMenuItemBottom = -1;// chuyển select index về thành -1
                        for (int i = 0; i < this._menuItemsBottom.Count; i++) // tìm kiếm trong tập các menu item
                        {
                            Rectangle temp = ((Sprite)this._menuItemsBottom[i]).BoundRectangle;
                            // if click on 1 item
                            // nếu item nào bị nhấn trúng
                            if (temp.X < mouse.X && mouse.X < (temp.X + temp.Width) && temp.Y < mouse.Y && mouse.Y < (temp.Y + temp.Height))
                            {
                                // play âm thanh nhấn vào menu item
                                AudioGame au = new AudioGame(this.Game);
                                au.PlaySoundEffectGame("clickitemmenu", 0.3f, 0.0f);
                                au.Dispose();
                                this._selectedIndexMenuItemBottom = i;// get index of this item// sét lại index là chỉ số của item đó trong mãng các item
                                if (this._menuItemsBottom[i] is Unit)
                                {
                                    // thêm vào list các unit đang được mua
                                    this.AddToListUnitBuying((Unit)this._menuItemsBottom[i]);
                                }
                                // còn nếu là structure thì phải đợi xem người dùng có mua nó ko bằng cách nhả chuột vào trong map
                                this._mouseLeftClickOnMenu = true;
                                return;// thoát
                            }
                        }
                        // kiểm tra có click trên mini map ko
                        if (ManagerGame._minimap.RootPosition.X < mouse.X && mouse.X < ManagerGame._minimap.RootPosition.X + ManagerGame._minimap.Background.Width
                            && ManagerGame._minimap.RootPosition.Y < mouse.Y && mouse.Y < ManagerGame._minimap.RootPosition.Y + ManagerGame._minimap.Background.Height
                            )
                        {
                            float percent = ManagerGame._minimap.Background.Height * 1.0f / (GlobalDTO.CURRENT_CELL_SIZE.Height * GlobalDTO.MAP_SIZE_IN_CELL.Height * 1.0f);
                            GlobalDTO.CURRENT_COORDINATE = new Vector2(((mouse.X - ManagerGame._minimap.RootPosition.X) / percent) - (GlobalDTO.SCREEN_SIZE.Width >> 1), (mouse.Y - ManagerGame._minimap.RootPosition.Y) / percent - (GlobalDTO.SCREEN_SIZE.Height >> 1)); // tính ra vị trí trên tọa độ thực
                            ManagerGame._map.CurrentRootCoordinate = GlobalDTO.CURRENT_COORDINATE;
                        }
                    }
                }
            }
            else if (mouse.RightButton == ButtonState.Pressed) // chuột phải đang nhấn
            {
                if (GlobalDTO.SCREEN_SIZE.Height > mouse.Y && mouse.Y > GlobalDTO.SCREEN_SIZE.Height - GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height) // chuột đang nhấn trong menu
                {
                    if (this._mouseRightClickOnMenu == false)
                    {                        
                        for (int i = 0; i < this._menuItemsBottom.Count; i++) // tìm kiếm trong tập các menu item
                        {
                            Rectangle temp = ((Sprite)this._menuItemsBottom[i]).BoundRectangle;
                            // if click on 1 item
                            // nếu item nào bị nhấn trúng
                            if (temp.X < mouse.X && mouse.X < (temp.X + temp.Width) && temp.Y < mouse.Y && mouse.Y < (temp.Y + temp.Height))
                            {
                                if (this._menuItemsBottom[i] is Unit)
                                {
                                    // hủy mua unit này
                                    this.CancelBuyUnit((Unit)this._menuItemsBottom[i]);
                                }
                                this._mouseRightClickOnMenu = true;
                                return;// thoát
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Nhả chuột trên menu
        /// </summary>
        public void MouseUpOnMenu()
        {
            MouseState mouse = Mouse.GetState();// lấy trạng thái chuột            
            if (this._mouseLeftClickOnMenu == true) // chuột đã nhấn bây giờ nhã
            {
                if (mouse.LeftButton == ButtonState.Released) // bây giờ chuột được nhả chuột trái
                {
                    this._mouseLeftClickOnMenu = false;// bật lại cờ
                    if (this._menuItemsBottom[0] is Unit) // chỉ khi nào menu item là unit thì mới bỏ selected index thôi
                    {
                        this._selectedIndexMenuItemBottom = -1;
                    }
                } 
            }
            if (this._mouseRightClickOnMenu == true) // chuột đã nhấn, bây giờ nhã
            {
                if (mouse.RightButton == ButtonState.Released) // bây giờ chuột được nhả chuột phải
                {
                    this._mouseRightClickOnMenu = false;                    
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

        /// <summary>
        /// Mua structure và đặt vào vị trí chuột
        /// </summary>
        /// <param name="mouse"></param>
        public void BuyStructure(MouseState mouse)
        {
            // tính toán vị trí đặt structure dựa vào trỏ chuột
            Vector2 tempposition = new Vector2(mouse.X, mouse.Y) + GlobalDTO.CURRENT_COORDINATE;
            Structure newstructure = new Structure(this.Game, GlobalDTO.SPEC_STRUCTURE_PATH + this._menuItemsBottom[this._selectedIndexMenuItemBottom].Name + ".xml", tempposition, this._playerIsUser.Code);
            // change position to draw structure which have just been built
            Texture2D img = newstructure.Info.Action[newstructure.CurrentStatus.Name].DirectionInfo[newstructure.CurrentDirection.Name].Image[0];
            newstructure.Position -= new Vector2(img.Width / 2, img.Height / 2);
            newstructure.PlayerContainer = this._playerIsUser; // xác định player sở hữu
            this._playerIsUser.Structures.Add(newstructure);// thêm vào tập các structure của player mua nó
            this.AddComponentIntoGame((IGameComponent)newstructure);// đưa vào game component để nó được vẽ
            this._playerIsUser.DecreaseResourceToBuyStructure((Structure)this._menuItemsBottom[this._selectedIndexMenuItemBottom]);
            ManagerGame._listStructureOnMap.Add(newstructure);// đưa vào list structure của manager game
            // play sound "start built"
            AudioGame au = new AudioGame(this.Game);
            au.PlaySoundEffectGame("startbuild", 0.15f, 0.0f);
            au.Dispose();
        }

        /// <summary>
        /// Add unit muốn mua vào danh sách đợi
        /// giàm tài nguyên của player
        /// </summary>
        /// <param name="unit"></param>
        public void AddToListUnitBuying(Unit unit)
        {
            if(this._listUnitsBuying.Count == 0) // chưa có unit nào đang mua
            {
                if (this._playerIsUser.CheckConditionToBuyUnit(unit)) // kiểm tra tài nguyên đủ ko
                {
                    List<Unit> list1 = new List<Unit>();
                    list1.Add(unit);
                    this._playerIsUser.DecreaseResourceToBuyUnit(unit); // giảm tài nguyên của player khi mua
                    this._listUnitsBuying.Add(list1);
                }
                return;
            }
            for (int i = 0; i < this._listUnitsBuying.Count; i++)// nếu trong list các unit đang mua ko rỗng ->> duyệt qua từng list unit con trong đó
            {
                if (this._listUnitsBuying[i][0].Name == unit.Name) // nếu unit mún mua trước đó đã được user mua vài con rồi --> add thêm vào list của unit cùng loại
                {
                    if (this._playerIsUser.CheckConditionToBuyUnit(unit))
                    {
                        this._listUnitsBuying[i].Add(unit);
                        this._playerIsUser.DecreaseResourceToBuyUnit(unit);
                    }
                    return;
                }
            }
            // nếu đây là loại unit hoàn toàn mới mà người dùng chưa mua --> thêm loại này vào
            if (this._playerIsUser.CheckConditionToBuyUnit(unit))
            {
                List<Unit> list = new List<Unit>();
                list.Add(unit);
                this._playerIsUser.DecreaseResourceToBuyUnit(unit);
                this._listUnitsBuying.Add(list);
            }
            return;

        }

        /// <summary>
        /// Hủy mua 1 unit nào đó
        /// </summary>
        /// <param name="unit"></param>
        public void CancelBuyUnit(Unit unit)
        {
            for (int i = 0; i < this._listUnitsBuying.Count; i++) // tìm trong list các unit đã chọn mua
            {
                if (this._listUnitsBuying[i][0].Name == unit.Name) // tìm đúng loại unit muốn hủy
                {
                    if (this._listUnitsBuying[i].Count == 1)// nếu hủy unit đang trong quá trình mua
                    { 
                        this._processingBuyUnit = null; // thì set unit đang xử lý mua về lại null
                    }
                    this._listUnitsBuying[i].RemoveAt(this._listUnitsBuying[i].Count - 1); // giảm số lượng mua xuống
                    // thu hồi tài nguyên cho player 
                    this._playerIsUser.RevokeResourceFromUnit(unit);
                    if (this._listUnitsBuying[i].Count == 0) // nếu đã hủy hết, ko mua con nào cả
                    {
                        this._listUnitsBuying.RemoveAt(i); // xóa luôn cả list các unit cùng loại
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Xử lý mua lính với thời gian quy định để mua lính
        /// bằng cách đếm ngược thời gian
        /// </summary>
        public void ProcessBuyUnitWithTime()
        {
            if ((System.Environment.TickCount - this._lastTickCountForProcessingBuyUnit) > this._delayTimeForProcessingBuyUnit)
            {
                this._lastTickCountForProcessingBuyUnit = System.Environment.TickCount;
                this._processingBuyUnit.TimeToBuyFinish--;
                if (this._processingBuyUnit.TimeToBuyFinish <= 0)
                {
                    // đã hết thời gian yêu cầu cho lính
                    this.BuyUnit(this._processingBuyUnit);//thực hiện mua unit
                    // xóa khỏi danh sách và chờ đợi 1 unit khác được xử lý mua;
                    this._listUnitsBuying[0].RemoveAt(0);
                    if (this._listUnitsBuying[0].Count == 0)
                    {
                        this._listUnitsBuying.RemoveAt(0);
                    }
                    this._processingBuyUnit = null;
                }
            }       
        }

        /// <summary>
        /// Xử lý khởi tạo và add unit vào các list liên quan
        /// </summary>
        /// <param name="unit"></param>
        public void BuyUnit(Unit unit)
        {
            if (unit is ProducerUnit)
            {
                ProducerUnit newunit = new ProducerUnit(this.Game, unit.PathSpecificationFile, new Vector2(2100, 2300), unit.PlayerContainer.Code); // khởi tạo
                newunit.PlayerContainer = unit.PlayerContainer;// xác định player mua no
                newunit.StructureContainer = unit.StructureContainer;// xác định structure đã sinh ra nó
                unit.StructureContainer.Units.Add(newunit);// add nó vào tập unit của structure sinh ra nó
                this.PlayerIsUser.Units.Add(newunit);// add vào tập unit của player mua nó
                ManagerGame._listUnitOnMap.Add(newunit);// add vào list của manager game mà quản lý
                this.AddComponentIntoGame((IGameComponent)newunit);// add vào component để vẽ ra
            }
            else
            {
                Unit newunit = new Unit(this.Game, unit.PathSpecificationFile, new Vector2(2100, 2300), unit.PlayerContainer.Code); // khởi tạo
                newunit.PlayerContainer = unit.PlayerContainer;// xác định player mua no
                newunit.StructureContainer = unit.StructureContainer;// xác định structure đã sinh ra nó
                unit.StructureContainer.Units.Add(newunit);// add nó vào tập unit của structure sinh ra nó
                this.PlayerIsUser.Units.Add(newunit);// add vào tập unit của player mua nó
                ManagerGame._listUnitOnMap.Add(newunit);// add vào list của manager game mà quản lý
                this.AddComponentIntoGame((IGameComponent)newunit);// add vào component để vẽ ra
            }
        }

        #endregion
    }
}