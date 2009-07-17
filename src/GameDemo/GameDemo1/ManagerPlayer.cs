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
using GameSharedObject.Components;
using System.Xml;
using GameSharedObject.DTO;
using GameSharedObject;


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ManagerPlayer : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private MainGame _game;

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

        private Boolean _mouseClickOnMap;//flag mouse // true nếu chuột nhấn và false nếu chuột ko nhấn(ko nhấn chứ ko phải được nhả)
        private Boolean _mouseLeftClickOnMenu;//flag mouse // true nếu chuột trái nhấn vào menu và false nếu chuột ko nhấn vào menu(ko nhấn chứ ko phải được nhả)
        private Boolean _mouseRightClickOnMenu;//flag mouse // true nếu chuột phải nhấn vào menu và false nếu chuột ko nhấn vào menu(ko nhấn chứ ko phải được nhả)
        private SpriteFont spriteFont; // spritefont obj// sprite để vẽ chữ
        private SpriteBatch spriteBatch;// spritebatch obj // sprite để vẽ hình        
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
        public ManagerPlayer(MainGame game)
            : base(game)
        {
            _game = game;
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

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (GlobalDTO.CURRENT_MODEGAME == "Playing")
            {
                // draw menu panel
                // vẽ các menu panel
                this.DrawMenuPanel();

                // vẽ thông tin vào các menu
                this.DrawInformationIntoMenu();
            }
            else
            {
                return;
            }

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
                        // vẽ thông tin của unit đó ra
                        spriteBatch.DrawString(this.spriteFont, "* NameLevel: " + tempUnit.Info.Name, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Health: " + tempUnit.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Power: " + ((UnitDTO)tempUnit.Info).InformationList["Power"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Dectection: " + ((UnitDTO)tempUnit.Info).InformationList["RadiusDetect"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 50), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Attack: " + ((UnitDTO)tempUnit.Info).InformationList["RadiusAttack"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 60), Color.White);
                    }
                    // if producer // nếu là 1 producer
                    else if (this._selectedSprites[this._selectedSprites.Count - 1] is ProducerUnit)
                    {
                        ProducerUnit tempUnit = ((ProducerUnit)this._selectedSprites[this._selectedSprites.Count - 1]);                        
                        // vẽ thông tin producer đó ra
                        spriteBatch.DrawString(this.spriteFont, "* NameLevel: " + tempUnit.Info.Name.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Health: " + tempUnit.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                        //spriteBatch.DrawString(this.spriteFont, "* Current Resource: ", new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                        //if (tempUnit.CurrentResourceExploiting != null)
                        //{
                        //    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage[tempUnit.CurrentResourceExploiting.Name]), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40 + 20), Color.White);
                        //    spriteBatch.DrawString(this.spriteFont, tempUnit.CurrentResourceExploiting.Name + " : " + tempUnit.CurrentResourceExploiting.Quantity, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 20, this._rectangleMenuBottom.Y + 40 + 20), Color.Gold);
                        //}
                    }
                }
                else if (this._selectedSprites[this._selectedSprites.Count - 1] is Structure) // if structure // nếu sprite là 1 structure
                {
                    Structure tempstructure = ((Structure)this._selectedSprites[this._selectedSprites.Count - 1]);                    
                    // vẽ thông tin structure đó ra
                    spriteBatch.DrawString(this.spriteFont, "* NameLevel: " + tempstructure.Info.Name, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 20), Color.White);
                    spriteBatch.DrawString(this.spriteFont, "* Health: " + tempstructure.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 30), Color.White);
                    if (!(this._selectedSprites[this._selectedSprites.Count - 1] is ResearchStructure))
                    {
                        spriteBatch.DrawString(this.spriteFont, "* Count Units: " + tempstructure.OwnerUnitList.Count.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                    }
                    if (tempstructure.PlayerContainer == this._playerIsUser)
                    {
                        //spriteBatch.DrawString(this.spriteFont, "* List Units: ", new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 60), Color.White);                        
                        // đồng thời, vẽ tập các unit mà structure này có thể tạo ra(các unit này bây giờ trở thành menu item)                    
                        if (tempstructure.CurrentIndex == tempstructure.Info.Action[tempstructure.CurrentStatus.Name].DirectionInfo[tempstructure.CurrentDirection.Name].Image.Count - 1) // it is finished building // nhưng structure chỉ có thể được mua unit khi nó đã hoàn tất việc xây dựng
                        {
                            this.CreateMenuItemForStructure(tempstructure); // tạo tập menu item là các unit mà structure có thể mua
                            this.DrawMenuItem();
                        }
                    }
                }

                // sau khi đã vẽ hết tất cả các thông tin liên quan đến sprite được chọn
                // --> cuối cùng, tính toán vị trí để vẽ sprite được chọn vào menu                
                Texture2D tempTexture = this._selectedSprites[this._selectedSprites.Count - 1].Info.Action[this._selectedSprites[this._selectedSprites.Count - 1].CurrentStatus.Name].DirectionInfo[this._selectedSprites[this._selectedSprites.Count - 1].CurrentDirection.Name].Image[this._selectedSprites[this._selectedSprites.Count - 1].CurrentIndex];                
                Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (1.5f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (1.5f)) / 2);                
                Rectangle tempRec = new Rectangle((int)positionImage.X + 10, this._rectangleMenuBottom.Y + (int)positionImage.Y, (int)(tempTexture.Width / (1.5f)), (int)(tempTexture.Height / (1.5f)));
                spriteBatch.Draw(tempTexture, tempRec, Color.White);
            }
            
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
                Sprite sprite = this._menuItemsBottom[i];
                // thiết lập màu sắc khác nếu item đó đang được user lựa chọn
                if (i == this._selectedIndexMenuItemBottom)
                {
                    sprite.Color = Color.LightPink;
                }
                else
                {
                    sprite.Color = Color.White;
                }                
                
                // nếu item muốn hiển thị là Unit hoặc Structure
                if (!(sprite is Technology))
                {
                    // vẽ hình đại diện của item
                    spriteBatch.Draw(sprite.Info.Icon, sprite.BoundRectangle, sprite.Color);
                    // nêu các đòi hỏi để mua item này                
                    spriteBatch.DrawString(spriteFont, sprite.Info.Name.Replace("_", " "), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width - 10, sprite.BoundRectangle.Y), Color.LightBlue);
                    if (sprite is Unit) // if is unit // giả như đây là 1 unit
                    {
                        int h = 16;
                        // viết các yêu cầu để mua unit này ra
                        int j = 0;
                        foreach (KeyValuePair<String, Resource> r in ((Unit)sprite).RequirementResources)
                        {
                            if (r.Value.Name == "Gold")
                            {
                                spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                            }
                            spriteBatch.DrawString(spriteFont, r.Value.Name + " : " + r.Value.Quantity, new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 20, sprite.BoundRectangle.Y + h * (j + 1)), Color.Goldenrod);
                            j++;
                        }
                        // viết số lượng unit này mà user đang đặt mua
                        j = 0;
                        for (j = 0; j < ((Structure)this._selectedSprites[0]).ListUnitsBuying.Count; j++)
                        {
                            if (((Structure)this._selectedSprites[0]).ListUnitsBuying[j][0].Info.Name == this._menuItemsBottom[i].Info.Name)
                            {
                                spriteBatch.DrawString(spriteFont, ((Structure)this._selectedSprites[0]).ListUnitsBuying[j].Count.ToString(), new Vector2(sprite.BoundRectangle.X + 16, this._menuItemsBottom[i].BoundRectangle.Y + 8), Color.Lime);
                            }
                        }
                    }
                    else if (sprite is Structure) // if is structure// giống như trên
                    {
                        int h = 16;
                        // draw required resource to buy this structure
                        for (int j = 0; j < ((Structure)sprite).RequirementResources.Count; j++)
                        {
                            if (((Structure)sprite).RequirementResources[j].Name == "Gold")
                            {
                                spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                            }
                            else if (((Structure)this._menuItemsBottom[i]).RequirementResources[j].Name == "Stone")
                            {
                                spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Stone"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                            }
                            spriteBatch.DrawString(spriteFont, ((Structure)(sprite)).RequirementResources[j].Name + " : " + ((Structure)(sprite)).RequirementResources[j].Quantity, new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 20, sprite.BoundRectangle.Y + h * (j + 1)), Color.YellowGreen);
                        }
                    }
                }
                else if(sprite is Technology) // item muốn hiển thị là Technology của công trình nghiên cứu
                {
                    // vẽ hình đại diện của item
                    spriteBatch.Draw(((Technology)sprite).TechInfo.Icon, sprite.BoundRectangle, sprite.Color);
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
            int i = 0;
            foreach (KeyValuePair<String,Resource> r in this._playerIsUser.Resources)
            {
                // vẽ các icon mô tả resource
                if (r.Value.Name == "Gold")
                {
                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(h * (i + 1), this._rectangleMenuTop.Y + 10), Color.White);
                }
                else if (r.Value.Name == "Stone")
                {
                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Stone"]), new Vector2(h * (i + 1), this._rectangleMenuTop.Y + 10), Color.White);
                }
                // viết thông tin resource
                spriteBatch.DrawString(spriteFont, r.Value.Name + " : " + r.Value.Quantity.ToString(), new Vector2(h * (i + 1) + 20, this._rectangleMenuTop.Y + 10), Color.LightBlue);
                i++;
            }
        }

        /// <summary>
        /// get all structures which player can create to add into menuitem
        /// tạo menu item mới từ tập các structure mà player này có thể xây dựng trong thời điểm hiện tại
        /// </summary>
        public void CreateMenuItemForUser()
        {            
            // Xóa list các menu item hiện tại
            this._menuItemsBottom.Clear();
            int i = 0;
            for (int j = 0; j < GlobalDTO.MANAGER_GAME.ListStructureOfGame.Count;j++ ) // với mỗi structure trong tập các structure 
            {
                //if (GlobalDTO.MANAGER_GAME.ListStructureOfGame[j].flag == true)//mà player này có thể xây dựng(true)
                {                   
                    Structure item = (Structure)GlobalDTO.MANAGER_GAME.ListStructureOfGame[j];                    
                    Texture2D tempTexture = item.Info.Icon;                    
                    item.PlayerContainer = this._playerIsUser;

                    // tính toán ra vị trí vẽ vào menu item                    
                    Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (2.0f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (2.0f)) / 2);                    
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
        public void CreateMenuItemForStructure(Structure structure)
        {
            if (!(structure is ResearchStructure))
            {
                int i = GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height;
                if (this._menuItemsBottom.Count > 0)
                {
                    return;
                }
                for (int j = 0; j < structure.ModelUnitList.Count; j++)// đối với mỗi cái tên unit có mà structure có thể mua
                {
                    //tạo ra 1 item là obj của Unit        
                    Unit item = structure.ModelUnitList[j];
                    Texture2D tempTexture = item.Info.Icon;
                    item.StructureContainer = (Structure)this._selectedSprites[0];// structure chứa nó là structure đang được chọn
                    item.PlayerContainer = this._playerIsUser;//  nó thuộc player là user vì user đang điều khiển trò chơi để mua nó

                    // tính toán vị trí để vẽ menu item(lúc này là unit) vào trong menu
                    Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (2.0f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (2.0f)) / 2);
                    Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y + 15, (int)(tempTexture.Width / (2.0f)), (int)(tempTexture.Height / (2.0f)));
                    item.BoundRectangle = tempRec;
                    this._menuItemsBottom.Add(item);
                    i += 60 + (int)(tempTexture.Width / (2.0f));
                }
            }
            else if (structure is ResearchStructure)
            {                
                int i = GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height;
                if (this._menuItemsBottom.Count > 0)
                {
                    return;
                }
                for (int j = 0; j < ((ResearchStructure)structure).ListTechnology.Count; j++)
                {                    
                    Technology item = ((ResearchStructure)structure).ListTechnology[j];
                    Texture2D tempTexture = item.TechInfo.Icon;                    
                 
                    Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (1.0f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (1.0f)) / 2);
                    Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y + 15, (int)(tempTexture.Width / (1.0f)), (int)(tempTexture.Height / (1.0f)));
                    item.BoundRectangle = tempRec;
                    this._menuItemsBottom.Add(item);
                    i += 60 + (int)(tempTexture.Width / (2.0f));
                }
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
                                if (this._selectedSprites[i] is Unit && ((Unit)this._selectedSprites[i]).PlayerContainer == this._playerIsUser)
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
                    this.CreateMenuItemForUser();
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
                            this.BuyStructure(mouse, this._menuItemsBottom[this._selectedIndexMenuItemBottom].Info.Name);
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
            this.SelectFromList(GlobalDTO.MANAGER_GAME.ListUnitOnMap);// ưu tiên việc select từ các unit
            // if not unit is selected
            if (this._selectedSprites.Count == 0) // nếu ko có unit nào được chọn
            {   // --> select từ các structure
                this.SelectFromList(GlobalDTO.MANAGER_GAME.ListStructureOnMap);
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
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListUnitOnMap.Count; i++)// với mỗi unit trong list các unit trên map
            {

                Sprite tempSprite = (GlobalDTO.MANAGER_GAME.ListUnitOnMap[i]);
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
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListStructureOnMap.Count; i++)
            {
                Sprite tempSprite = (GlobalDTO.MANAGER_GAME.ListStructureOnMap[i]);
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
                                AudioGame au = new AudioGame(this._game);
                                au.PlaySoundEffectGame("clickitemmenu", 0.3f, 0.0f);
                                au.Dispose();
                                this._selectedIndexMenuItemBottom = i;// get index of this item// sét lại index là chỉ số của item đó trong mãng các item
                                if (this._menuItemsBottom[i] is Unit)
                                {
                                    // thêm vào list các unit đang được mua                                    
                                    ((Structure)this._selectedSprites[0]).AddToListUnitBuying(((Unit)this._menuItemsBottom[i]).Clone() as Unit);                                    
                                }
                                // còn nếu là structure thì phải đợi xem người dùng có mua nó ko bằng cách nhả chuột vào trong map
                                this._mouseLeftClickOnMenu = true;
                                return;// thoát
                            }
                        }
                        // kiểm tra có click trên mini map ko
                        if (GlobalDTO.MANAGER_GAME.Minimap.RootPosition.X < mouse.X && mouse.X < GlobalDTO.MANAGER_GAME.Minimap.RootPosition.X + GlobalDTO.MANAGER_GAME.Minimap.Background.Width
                            && GlobalDTO.MANAGER_GAME.Minimap.RootPosition.Y < mouse.Y && mouse.Y < GlobalDTO.MANAGER_GAME.Minimap.RootPosition.Y + GlobalDTO.MANAGER_GAME.Minimap.Background.Height
                            )
                        {
                            float percent = GlobalDTO.MANAGER_GAME.Minimap.Background.Height * 1.0f / (GlobalDTO.CURRENT_CELL_SIZE.Height * GlobalDTO.MAP_SIZE_IN_CELL.Height * 1.0f);
                            GlobalDTO.CURRENT_COORDINATE = new Vector2(((mouse.X - GlobalDTO.MANAGER_GAME.Minimap.RootPosition.X) / percent) - (GlobalDTO.SCREEN_SIZE.Width >> 1), (mouse.Y - GlobalDTO.MANAGER_GAME.Minimap.RootPosition.Y) / percent - (GlobalDTO.SCREEN_SIZE.Height >> 1)); // tính ra vị trí trên tọa độ thực
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
                                    ((Structure)this._selectedSprites[0]).CancelBuyUnit((Unit)this._menuItemsBottom[i]);
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
        public void BuyStructure(MouseState mouse, string nameStructure)
        {
            // tính toán vị trí đặt structure dựa vào trỏ chuột
            Vector2 tempposition = new Vector2(mouse.X, mouse.Y) + GlobalDTO.CURRENT_COORDINATE;

            // tạo Structure từ tên của Structure tương ứng được chọn từ menu bottom
            string name = nameStructure;
            Sprite newstructure = ((Structure)this._game.StructureMgr[name]).Clone() as Sprite;                        
            this.LoadUnitListToStructure(// load tập unit của Structure
                        (Structure)newstructure,
                        ((Structure)newstructure).CurrentUpgradeInfo.Id);
            newstructure.Position = tempposition;
            this._playerIsUser.BuyStructure(newstructure);            
        }        

        public void LoadUnitListToStructure(Structure structure, int upgradeId)
        {
            //// get list units which can create
            //// lấy list tên các unit mà structure này có khả năng sinh ra
            structure.ModelUnitList = new List<Unit>();
            List<ItemInfo> uList = ((StructureDTO)structure.Info).UnitList;

            for (int i = 0; i < uList.Count; i++){
                Unit unit = (Unit)_game.UnitMgr[uList[i].Name];
                if (structure.CurrentUpgradeInfo.Id >= int.Parse(uList[i].Value)){
                    structure.ModelUnitList.Add(unit);
                }
            }
        }        
        #endregion
    }
}