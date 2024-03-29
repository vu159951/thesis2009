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
using GameSharedObject.Factory;


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

        private int lastTickCount;
        private int pointerIndex;
        private Point currentPointer;
        private bool isPointerDrawn;
        private Texture2D[] pointers;

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

            // load pointer images
            pointers = new Texture2D[3];
            pointers[0] = game.Content.Load<Texture2D>("Images\\Pointer\\Pointer0");
            pointers[1] = game.Content.Load<Texture2D>("Images\\Pointer\\Pointer1");
            pointers[2] = game.Content.Load<Texture2D>("Images\\Pointer\\Pointer2");
            pointerIndex = 0;
            isPointerDrawn = false;
            currentPointer = new Point(0, 0);
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
            if (!this.Game.IsActive) return;

            // TODO: Add your update code here
            // các thao tác chuột của user
            MouseState mouse = Mouse.GetState(); // lấy trạng thái chuột

            this.MousePointerOnMap(mouse);
            this.MousePressedOnMap(mouse);
            this.MousePressOnMenu(mouse);
            this.MouseUpOnMap(mouse);
            this.MouseUpOnMenu(mouse);

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
                // vẽ các menu panel
                this.DrawMenuPanel();

                // vẽ thông tin vào các menu
                this.DrawInformationIntoMenu();
            }
            else
            {
                return;
            }

            /// vẽ selected rectangle
            MouseState mouse = Mouse.GetState();
            if (0 < mouse.Y && mouse.Y < GlobalDTO.SCREEN_SIZE.Height - GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height)  // chuột nhấn trên map
            {
                if (this._selectedSprites.Count == 0)
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        if (this._selectedRectangle.X != 0 && this._selectedRectangle.Y != 0)
                        {
                            this.DrawLine(new Vector2(this._selectedRectangle.X - GlobalDTO.CURRENT_COORDINATE.X, this._selectedRectangle.Y - GlobalDTO.CURRENT_COORDINATE.Y), new Vector2(mouse.X, this._selectedRectangle.Y - GlobalDTO.CURRENT_COORDINATE.Y));
                            this.DrawLine(new Vector2(mouse.X, this._selectedRectangle.Y - GlobalDTO.CURRENT_COORDINATE.Y), new Vector2(mouse.X, mouse.Y));
                            this.DrawLine(new Vector2(this._selectedRectangle.X - GlobalDTO.CURRENT_COORDINATE.X, this._selectedRectangle.Y - GlobalDTO.CURRENT_COORDINATE.Y), new Vector2(this._selectedRectangle.X - GlobalDTO.CURRENT_COORDINATE.X, mouse.Y));
                            this.DrawLine(new Vector2(this._selectedRectangle.X - GlobalDTO.CURRENT_COORDINATE.X, mouse.Y), new Vector2(mouse.X, mouse.Y));
                        }
                    }
                }
            }

            this.DrawPointerOnMap();

            base.Draw(gameTime);
        }
        #endregion

        #region Function

        /// <summary>
        /// vẽ đường thẳng
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void DrawLine(Vector2 start, Vector2 end)
        {
            int distance = (int)Vector2.Distance(start, end);


            float alpha = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            {
                spriteBatch.Draw((Texture2D)GlobalDTO.MANAGER_GAME.IconsImage["Pixel"], new Rectangle((int)start.X, (int)start.Y, distance, 1),
                    null, Color.White, alpha, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }

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
                        spriteBatch.DrawString(this.spriteFont, tempUnit.Info.Name.Replace("_", " "), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 35), Color.White);

                        spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Health"]), new Rectangle(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 50, 15, 15), Color.White);
                        spriteBatch.DrawString(this.spriteFont, tempUnit.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 25, this._rectangleMenuBottom.Y + 50), Color.White);

                        spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Sword"]), new Rectangle(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 65, 15, 15), Color.White);
                        spriteBatch.DrawString(this.spriteFont, ((UnitDTO)tempUnit.Info).InformationList["Power"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 25, this._rectangleMenuBottom.Y + 65), Color.White);

                        spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Eye"]), new Rectangle(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 80, 15, 15), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Dectection: " + ((UnitDTO)tempUnit.Info).InformationList["RadiusDetect"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 25, this._rectangleMenuBottom.Y + 80), Color.White);
                        spriteBatch.DrawString(this.spriteFont, "* Radius Attack: " + ((UnitDTO)tempUnit.Info).InformationList["RadiusAttack"].Value, new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 25, this._rectangleMenuBottom.Y + 95), Color.White);
                    }
                    // if producer // nếu là 1 producer
                    else if (this._selectedSprites[this._selectedSprites.Count - 1] is ProducerUnit)
                    {
                        ProducerUnit tempUnit = ((ProducerUnit)this._selectedSprites[this._selectedSprites.Count - 1]);
                        // vẽ thông tin producer đó ra
                        spriteBatch.DrawString(this.spriteFont, tempUnit.Info.Name.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 35), Color.White);

                        spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Health"]), new Rectangle(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 50, 15, 15), Color.White);
                        spriteBatch.DrawString(this.spriteFont, tempUnit.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 25, this._rectangleMenuBottom.Y + 50), Color.White);

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
                    spriteBatch.DrawString(this.spriteFont, "* Name: " + tempstructure.Info.Name.Replace("_", " ").Substring(0, tempstructure.Info.Name.Replace("_", " ").IndexOf("0")), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 35), Color.White);

                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Health"]), new Rectangle(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 50, 15, 15), Color.White);
                    spriteBatch.DrawString(this.spriteFont, tempstructure.CurrentHealth.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height + 25, this._rectangleMenuBottom.Y + 50), Color.White);

                    //if (!(this._selectedSprites[this._selectedSprites.Count - 1] is ResearchStructure))
                    //{
                    //    spriteBatch.DrawString(this.spriteFont, "Count Units: " + tempstructure.OwnerUnitList.Count.ToString(), new Vector2(GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height, this._rectangleMenuBottom.Y + 40), Color.White);
                    //}
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
            try
            {
                for (int i = 0; i < this._menuItemsBottom.Count; i++)// với mỗi item
                {
                    Sprite sprite = this._menuItemsBottom[i];

                    // nếu item muốn hiển thị là Unit hoặc Structure
                    if (!(sprite is Technology))
                    {
                        // thiết lập màu sắc khác nếu item đó đang được user lựa chọn
                        if (i == this._selectedIndexMenuItemBottom)
                        {
                            sprite.Color = Color.LightPink;
                        }
                        else
                        {
                            sprite.Color = Color.White;
                        }
                        // vẽ hình đại diện của item
                        if (sprite is Structure)
                        {
                            if (this._playerIsUser.CheckConditionToBuyStructure((Structure)sprite) == true)
                            {
                                spriteBatch.Draw(sprite.Info.Icon, sprite.BoundRectangle, sprite.Color);
                            }
                            else
                            {
                                spriteBatch.Draw(sprite.Info.Icon, sprite.BoundRectangle, Color.Gray);
                            }
                        }
                        else if (sprite is Unit)
                        {
                            if (((Structure)this._selectedSprites[0]).CheckConditionToBuyUnit((Unit)sprite) == true)
                            {
                                spriteBatch.Draw(sprite.Info.Icon, sprite.BoundRectangle, sprite.Color);
                            }
                            else
                            {
                                spriteBatch.Draw(sprite.Info.Icon, sprite.BoundRectangle, Color.Gray);
                            }
                        }
                        // nêu các đòi hỏi để mua item này                
                        string name = sprite.Info.Name;
                        if (sprite is Unit) // if is unit // giả như đây là 1 unit
                        {
                            spriteBatch.DrawString(spriteFont, name.Replace("_", " "), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width - 10, sprite.BoundRectangle.Y), Color.LightBlue);
                            int h = 16;
                            // viết các yêu cầu để mua unit này ra
                            int j = 0;
                            foreach (KeyValuePair<String, Resource> r in ((Unit)sprite).RequirementResources)
                            {
                                if (r.Value.Name == "Gold")
                                {
                                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                                }
                                else if (r.Value.Name == "Time")
                                {
                                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Clock"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                                }
                                spriteBatch.DrawString(spriteFont, r.Value.Quantity.ToString(), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 20, sprite.BoundRectangle.Y + h * (j + 1)), Color.Goldenrod);
                                j++;
                            }
                            // viết số lượng unit này mà user đang đặt mua
                            j = 0;
                            for (j = 0; j < ((Structure)this._selectedSprites[0]).ListUnitsBuying.Count; j++)
                            {
                                if (((Structure)this._selectedSprites[0]).ListUnitsBuying[j][0].Info.Name == this._menuItemsBottom[i].Info.Name)
                                {
                                    spriteBatch.DrawString(spriteFont, ((Structure)this._selectedSprites[0]).ListUnitsBuying[j].Count.ToString(), new Vector2(sprite.BoundRectangle.X + 16, this._menuItemsBottom[i].BoundRectangle.Y + 8), Color.Lime);
                                    double percent = (((Structure)this._selectedSprites[0]).ListUnitsBuying[j][0].TimeToBuyFinish * 1.0f) / (((Unit)sprite).TimeToBuyFinish * 1.0f) * 100.0f;
                                    percent = 100.0f - percent;
                                    spriteBatch.DrawString(spriteFont, Math.Round(percent, 0).ToString() + "%", new Vector2(sprite.BoundRectangle.X + 10, sprite.BoundRectangle.Y + sprite.BoundRectangle.Height - 20), Color.LightBlue, 0.0f, Vector2.Zero, 1.15f, SpriteEffects.None, 1.0f);
                                }
                            }
                        }
                        else if (sprite is Structure) // if is structure// giống như trên
                        {
                            spriteBatch.DrawString(spriteFont, name.Replace("_", " ").Substring(0, name.IndexOf("0")), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width - 10, sprite.BoundRectangle.Y), Color.LightBlue);
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
                                else if (((Structure)this._menuItemsBottom[i]).RequirementResources[j].Name == "Time")
                                {
                                    spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Clock"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                                }
                                spriteBatch.DrawString(spriteFont, ((Structure)(sprite)).RequirementResources[j].Quantity.ToString(), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 20, sprite.BoundRectangle.Y + h * (j + 1)), Color.YellowGreen);
                            }
                        }
                    }
                    else if (sprite is Technology) // item muốn hiển thị là Technology của công trình nghiên cứu
                    {
                        // vẽ hình đại diện của item
                        if (((ResearchStructure)this._selectedSprites[0]).CheckConditionToReSearch((Technology)sprite) == true)
                        {
                            spriteBatch.Draw(((Technology)sprite).TechInfo.Icon, sprite.BoundRectangle, sprite.Color);
                        }
                        else
                        {
                            spriteBatch.Draw(((Technology)sprite).TechInfo.Icon, sprite.BoundRectangle, Color.Gray);
                        }
                        spriteBatch.DrawString(spriteFont, ((Technology)sprite).TechInfo.Name.Replace("_", " "), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 20, sprite.BoundRectangle.Y), Color.LightBlue);
                        // viết các yêu cầu để nghiên cứu technology
                        int h = 16;
                        int j = 0;
                        foreach (KeyValuePair<String, ItemInfo> r in ((Technology)sprite).TechInfo.Upgrade.Requirements)
                        {
                            if (r.Value.Type == "Technology")
                            { continue; }
                            if (r.Value.Name == "Gold")
                            {
                                spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Gold"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 10, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                            }
                            else if (r.Value.Name == "Stone")
                            {
                                spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Stone"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 10, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                            }
                            else if (r.Value.Name == "Time")
                            {
                                spriteBatch.Draw(((Texture2D)ManagerGame._iconsImage["Clock"]), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 10, sprite.BoundRectangle.Y + h * (j + 1)), Color.White);
                            }

                            spriteBatch.DrawString(spriteFont, r.Value.Value.ToString(), new Vector2(sprite.BoundRectangle.X + sprite.BoundRectangle.Width + 40, sprite.BoundRectangle.Y + h * (j + 1)), Color.Goldenrod);
                            j++;
                        }

                        // viết thông số đang xử lý nghiên cứu theo %
                        if (((ResearchStructure)this._selectedSprites[0]).CurrentTechResearch != null)
                        {
                            if (((Technology)sprite).TechInfo.Name == ((ResearchStructure)this._selectedSprites[0]).CurrentTechResearch.TechInfo.Name)
                            {
                                double percent = (int.Parse(((ResearchStructure)this._selectedSprites[0]).CurrentTechResearch.TechInfo.Upgrade.Requirements["Time"].Value) * 1.0f) / (int.Parse(((Technology)sprite).TechInfo.Upgrade.Requirements["Time"].Value) * 1.0f) * 100.0f;
                                percent = 100.0f - percent;
                                spriteBatch.DrawString(spriteFont, Math.Round(percent, 0).ToString() + "%", new Vector2(sprite.BoundRectangle.X + 10, sprite.BoundRectangle.Y + sprite.BoundRectangle.Height + 10), Color.SeaGreen, 0.0f, Vector2.Zero, 1.15f, SpriteEffects.None, 1.0f);
                            }
                        }
                    }
                }
            }
            catch
            { }
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
            foreach (KeyValuePair<String, Resource> r in this._playerIsUser.Resources)
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
            foreach (KeyValuePair<String, Sprite> s in this._playerIsUser.ModelStructureList) // với mỗi structure trong tập các structure 
            {
                //if (GlobalDTO.MANAGER_GAME.ListStructureOfGame[j].Info.Name.Contains(this._playerIsUser.Code.ToString()))//mà player này có thể xây dựng(true)
                {
                    Structure item = (Structure)s.Value;
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
                    Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (1.5f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (1.5f)) / 2);
                    Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y + 15, (int)(tempTexture.Width / (1.5f)), (int)(tempTexture.Height / (1.5f)));
                    item.BoundRectangle = tempRec;
                    this._menuItemsBottom.Add(item);
                    i += 40 + (int)(tempTexture.Width / (1.5f));
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

                    Vector2 positionImage = new Vector2((GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Width / (1.5f)) / 2, (GlobalDTO.MENU_PANEL_BOTTOM_SIZE.Height - tempTexture.Height / (1.5f)) / 2);
                    Rectangle tempRec = new Rectangle((int)positionImage.X + i, this._rectangleMenuBottom.Y + (int)positionImage.Y, (int)(tempTexture.Width / (1.5f)), (int)(tempTexture.Height / (1.5f)));
                    item.BoundRectangle = tempRec;
                    this._menuItemsBottom.Add(item);
                    i += 120 + (int)(tempTexture.Width / (1.5f));
                }
            }
        }

        public Boolean CheckClickOnSprite(MouseState mouse)
        {
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListUnitOnMap.Count; i++)// với mỗi unit trong list các unit trên map
            {

                if (((Unit)GlobalDTO.MANAGER_GAME.ListUnitOnMap[i]).PlayerContainer != this._playerIsUser)
                {
                    continue;
                }
                Sprite tempSprite = (GlobalDTO.MANAGER_GAME.ListUnitOnMap[i]);                
                Rectangle tempRec = tempSprite.BoundRectangle;
                tempRec.X += (int)GlobalDTO.CURRENT_COORDINATE.X;
                tempRec.Y += (int)GlobalDTO.CURRENT_COORDINATE.Y;
                tempRec.X += tempRec.Width / 4;
                tempRec.Y += tempRec.Height / 4;
                tempRec.Height /= 2;
                tempRec.Width /= 2;
                if (tempRec.Contains(new Point(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X, mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y)) // kiểm tra unit có bị chọn ko                    
                    )
                {
                    this.ClearSelectedSprites();
                    this._selectedSprites.Add(tempSprite);// if a sprite is selected
                    tempSprite.SelectedFlag = true; // bật cờ được chọn cho nó thành true
                    return true;
                }
            }
            // thực hiện tương tự cho các structure
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListStructureOnMap.Count; i++)
            {
                if (((Structure)GlobalDTO.MANAGER_GAME.ListStructureOnMap[i]).PlayerContainer != this._playerIsUser)
                {
                    continue;
                }
                Sprite tempSprite = (GlobalDTO.MANAGER_GAME.ListStructureOnMap[i]);
                Rectangle tempRec = tempSprite.BoundRectangle;
                tempRec.X += (int)GlobalDTO.CURRENT_COORDINATE.X;
                tempRec.Y += (int)GlobalDTO.CURRENT_COORDINATE.Y;
                tempRec.X += tempRec.Width / 4;
                tempRec.Y += tempRec.Height / 4;
                tempRec.Height /= 2;
                tempRec.Width /= 2;
                if (tempRec.Contains(new Point(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X,mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y)
                    ))
                {
                    this.ClearSelectedSprites();
                    this._selectedSprites.Add(tempSprite);// if a sprite is selected
                    tempSprite.SelectedFlag = true; // bật cờ được chọn cho nó thành true
                    return true;
                }
            }
            return false;
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
        public void MousePressedOnMap(MouseState mouse)
        {
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
                            if (this.CheckClickOnSprite(mouse) == true)
                            {                                
                                return;
                            }
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
        public void MouseUpOnMap(MouseState mouse)
        {
            if (this._mouseClickOnMap == true) // nếu ban đầu có nhận chuột
            {
                if (mouse.LeftButton == ButtonState.Released) // bây giờ chuột được nhả
                {
                    if (this._selectedSprites.Count == 0)
                    {
                        // nếu chuột nhả ra là sự kết thúc của 1 đợt multi select được thực hiện bằng selected Rectangle
                        if (Math.Abs(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X - this._selectedRectangle.X) > 10 || Math.Abs(mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y - this._selectedRectangle.Y) > 10)
                        {
                            // khởi tạo lại selected Rectangle to hơn khi nhấn chuột để có thể bao trùm nhiều sprite hơn
                            this.CreateSelectedRectangle(mouse.X + (int)GlobalDTO.CURRENT_COORDINATE.X, mouse.Y + (int)GlobalDTO.CURRENT_COORDINATE.Y);

                            // lấy tập các sprite bị bao trùm bởi selected rectangle
                            this.GetSelectedSpriteWithSelectedRectangle();
                        }
                        else // còn nếu chuột nhả ra mà ko phải là việc chọn multi
                        {
                            if (this._selectedSprites.Count == 0)
                            {
                                // thì với 1 selected rectangle nhỏ hơn được khởi tạo khi nhấn chuột, 1 sprite sẽ được select
                                this.GetSelectedSpriteWithPointClick();
                            }
                        }



                        // nếu ko có sprite nào được chọn và có item menu nào đó được chọn
                        if (this._selectedIndexMenuItemBottom != -1 && this._selectedSprites.Count == 0)
                        {
                            // mà nếu là item menu loại structure -> user muốn mua structure và thả vào map
                            try
                            {
                                if (this._menuItemsBottom[this._selectedIndexMenuItemBottom] is Structure && this._playerIsUser.CheckConditionToBuyStructure((Structure)this._menuItemsBottom[this._selectedIndexMenuItemBottom])) // kiểm tra  resource yêu cầu có thỏa
                                {
                                    // mua structure và đặt vào vị trí trỏ chuột
                                    this.BuyStructure(mouse, this._menuItemsBottom[this._selectedIndexMenuItemBottom].Info.Name);
                                    this._selectedIndexMenuItemBottom = -1;
                                }
                            }
                            catch
                            {
                                this._mouseClickOnMap = false;
                            }
                        }
                    }
                    this._mouseClickOnMap = false;
                }
            }
            else // ban đầu chuột trái ko nhấn
            {
                if (this._menuItemsBottom.Count > 0)
                {
                    if (this._selectedIndexMenuItemBottom > -1)
                    {
                        if (this._menuItemsBottom[this._selectedIndexMenuItemBottom] is Structure)
                        {
                            GlobalDTO.CURSOR_SIZE = new System.Drawing.Size(90, 105);
                            GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = this._menuItemsBottom[this._selectedIndexMenuItemBottom].Info.Icon;
                            return;
                        }
                        else
                        {
                            GlobalDTO.CURSOR_SIZE = new System.Drawing.Size(18, 21);
                            GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = GlobalDTO.MANAGER_GAME.Cursor.TextureNomal;
                        }

                    }
                    else
                    {
                        GlobalDTO.CURSOR_SIZE = new System.Drawing.Size(18, 21);
                        GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = GlobalDTO.MANAGER_GAME.Cursor.TextureNomal;
                    }
                }
                if (this._selectedSprites.Count > 0)
                {
                    if ((this._selectedSprites[0] is Unit) && (((Unit)this._selectedSprites[0])).PlayerContainer == this._playerIsUser)
                    {
                        for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListUnitOnViewport.Count; i++)// với mỗi unit trong list các unit trên map
                        {
                            Sprite sprite = GlobalDTO.MANAGER_GAME.ListUnitOnViewport[i];
                            Texture2D image = sprite.Info.Action[sprite.CurrentStatus.Name].DirectionInfo[sprite.CurrentDirection.Name].Image[sprite.CurrentIndex];
                            if ((sprite.Position.X + image.Width >= GlobalDTO.CURRENT_COORDINATE.X) && (sprite.Position.Y + image.Height >= GlobalDTO.CURRENT_COORDINATE.Y))
                            {
                                if ((sprite.Position.X <= GlobalDTO.CURRENT_COORDINATE.X + Game.Window.ClientBounds.Width) && (sprite.Position.Y <= GlobalDTO.CURRENT_COORDINATE.Y + Game.Window.ClientBounds.Height))
                                {
                                    if (((Unit)sprite).PlayerContainer != this._playerIsUser)
                                    {
                                        Rectangle rec = new Rectangle((int)(sprite.Position.X - GlobalDTO.CURRENT_COORDINATE.X), (int)(sprite.Position.Y - GlobalDTO.CURRENT_COORDINATE.Y), (int)(image.Width * sprite.PercentSize), (int)(image.Height * sprite.PercentSize));
                                        if (rec.Contains(new Point((int)(GlobalDTO.MANAGER_GAME.Cursor.Position.X), (int)(GlobalDTO.MANAGER_GAME.Cursor.Position.Y))))
                                        {
                                            GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = GlobalDTO.MANAGER_GAME.Cursor.TextureSpecial;
                                            return;
                                        }
                                        else
                                        {
                                            GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = GlobalDTO.MANAGER_GAME.Cursor.TextureNomal;
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListStructureOnViewport.Count; i++)// với mỗi unit trong list các unit trên map
                        {
                            Sprite sprite = GlobalDTO.MANAGER_GAME.ListStructureOnViewport[i];
                            Texture2D image = sprite.Info.Action[sprite.CurrentStatus.Name].DirectionInfo[sprite.CurrentDirection.Name].Image[sprite.CurrentIndex];
                            if ((sprite.Position.X + image.Width >= GlobalDTO.CURRENT_COORDINATE.X) && (sprite.Position.Y + image.Height >= GlobalDTO.CURRENT_COORDINATE.Y))
                            {
                                if ((sprite.Position.X <= GlobalDTO.CURRENT_COORDINATE.X + Game.Window.ClientBounds.Width) && (sprite.Position.Y <= GlobalDTO.CURRENT_COORDINATE.Y + Game.Window.ClientBounds.Height))
                                {
                                    if (((Structure)sprite).PlayerContainer != this._playerIsUser)
                                    {
                                        Rectangle rec = new Rectangle((int)(sprite.Position.X - GlobalDTO.CURRENT_COORDINATE.X), (int)(sprite.Position.Y - GlobalDTO.CURRENT_COORDINATE.Y), (int)(image.Width * sprite.PercentSize), (int)(image.Height * sprite.PercentSize));
                                        if (rec.Contains(new Point((int)(GlobalDTO.MANAGER_GAME.Cursor.Position.X), (int)(GlobalDTO.MANAGER_GAME.Cursor.Position.Y))))
                                        {
                                            GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = GlobalDTO.MANAGER_GAME.Cursor.TextureSpecial;
                                            return;
                                        }
                                        else
                                        {
                                            GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = GlobalDTO.MANAGER_GAME.Cursor.TextureNomal;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    GlobalDTO.MANAGER_GAME.Cursor.CurrentTexture = GlobalDTO.MANAGER_GAME.Cursor.TextureNomal;
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
        public void MousePressOnMenu(MouseState mouse)
        {
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
                                this._selectedIndexMenuItemBottom = i;// sét lại index là chỉ số của item đó trong mãng các item
                                // nếu nhận vào menuitem là unit
                                if (this._menuItemsBottom[i] is Unit)
                                {
                                    // thêm vào list các unit đang được mua                                    
                                    ((Structure)this._selectedSprites[0]).AddToListUnitBuying(((Unit)this._menuItemsBottom[i]).Clone() as Unit);
                                }
                                // nếu nhấn vào một technology
                                else if (this._menuItemsBottom[i] is Technology)
                                {
                                    if (((ResearchStructure)this._selectedSprites[0]).CurrentTechResearch == null) // chưa có tech nào của công trình này trong giai đoạn nghiên cứu
                                    {
                                        if (((ResearchStructure)this._selectedSprites[0]).CheckConditionToReSearch((Technology)this._menuItemsBottom[i]))
                                        {
                                            ((ResearchStructure)this._selectedSprites[0]).DecreaseResourceToRearchTech((Technology)this._menuItemsBottom[i]);
                                            ((ResearchStructure)this._selectedSprites[0]).CurrentTechResearch = ((Technology)this._menuItemsBottom[i]).Clone();
                                        }
                                    }
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
        public void MouseUpOnMenu(MouseState mouse)
        {
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

        public void MousePointerOnMap(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed &&
                _selectedSprites.Count > 0 &&
                _selectedSprites[0] is Unit &&
                _playerIsUser.Code == _selectedSprites[0].CodeFaction)
            {
                currentPointer.X = mouse.X;
                currentPointer.Y = mouse.Y;
                pointerIndex = 0;
                isPointerDrawn = true;
                lastTickCount = System.Environment.TickCount;
            }

            if (isPointerDrawn)
            {
                if (GlobalFunction.PointInRectangle(this._rectangleMenuBottom, currentPointer) ||
                    GlobalFunction.PointInRectangle(this._rectangleMenuBottom, currentPointer))
                {
                    isPointerDrawn = false;
                    return;
                }

                if (System.Environment.TickCount - lastTickCount >= 100)
                {
                    pointerIndex++;
                    lastTickCount = System.Environment.TickCount;
                    if (pointerIndex > 2)
                    {
                        pointerIndex = 0;
                        isPointerDrawn = false;
                    }
                }
            }
        }

        public void DrawPointerOnMap()
        {
            if (isPointerDrawn)
            {
                System.Drawing.Size sz = new System.Drawing.Size(this.pointers[pointerIndex].Width >> 1, this.pointers[pointerIndex].Height >> 1);

                spriteBatch.Draw(this.pointers[pointerIndex],
                    new Rectangle(currentPointer.X - sz.Width, currentPointer.Y - sz.Height, this.pointers[pointerIndex].Width, this.pointers[pointerIndex].Height),
                    Color.White);
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
            Sprite newstructure = ((Structure)this._playerIsUser.ModelStructureList[name]).Clone() as Sprite;
            this.LoadUnitListToStructure(// load tập unit của Structure
                        (Structure)newstructure,
                        ((Structure)newstructure).CurrentUpgradeInfo.Id);
            newstructure.Position = tempposition;
            this._playerIsUser.BuyStructure(newstructure);
        }

        /// <summary>
        /// Load tập các unit mà Structure có thể mua
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="upgradeId"></param>
        public void LoadUnitListToStructure(Structure structure, int upgradeId)
        {
            //// lấy list tên các unit mà structure này có khả năng sinh ra
            structure.ModelUnitList = new List<Unit>();
            List<ItemInfo> uList = ((StructureDTO)structure.Info).UnitList;

            for (int i = 0; i < uList.Count; i++)
            {
                Unit unit = (Unit)_game.UnitMgr[uList[i].Name];
                if (structure.CurrentUpgradeInfo.Id >= int.Parse(uList[i].Value))
                {
                    structure.ModelUnitList.Add(unit);
                }
            }
        }

        public void SaveGame(string pathSaveFile)
        {
            XmlDocument doc = new XmlDocument();
            pathSaveFile = System.IO.Path.GetFullPath(pathSaveFile);
            doc.Load(pathSaveFile);            

            // root
            XmlElement root = doc.DocumentElement;
            if (root.ChildNodes.Count > 0)            
            {
                return;
            }

            //Map
            XmlElement map = doc.CreateElement("Map");
            root.AppendChild(map);
            map.SetAttribute("name", GlobalDTO.MANAGER_GAME.PathToMap);

            // player
            XmlElement playersNode = doc.CreateElement("Players");
            root.AppendChild(playersNode);
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.Players.Count; i++)
            {
                Player player = GlobalDTO.MANAGER_GAME.Players[i];
                XmlElement playernode = doc.CreateElement("Player");
                playernode.SetAttribute("code", player.Code.ToString());
                playersNode.AppendChild(playernode);
                if (i == 0)
                {
                    playernode.SetAttribute("type", "Player");                    
                }
                else
                {
                    playernode.SetAttribute("type", "Computer");                    
                }

                if (player.Color.ToString() == Color.Red.ToString())
                {
                    playernode.SetAttribute("color", "Red");                    
                }
                else if (player.Color.ToString() == Color.Green.ToString())
                {
                    playernode.SetAttribute("color", "Green");
                }
                else if (player.Color.ToString() == Color.Blue.ToString())
                {
                    playernode.SetAttribute("color", "Blue");
                }
                else if (player.Color.ToString() == Color.Gold.ToString())
                {
                    playernode.SetAttribute("color", "Yellow");
                }

                // lưu structure
                XmlElement structuresNode = doc.CreateElement("Structures");
                playernode.AppendChild(structuresNode);
                for (int j = 0; j < player.StructureListCreated.Count; j++)
                {
                    Sprite structure = player.StructureListCreated[j];
                    XmlElement structurenode = doc.CreateElement("Structure");
                    structuresNode.AppendChild(structurenode);
                    structurenode.SetAttribute("name", structure.Info.Name);
                    if (structure is ResearchStructure)
                    {
                        structurenode.SetAttribute("type", "ResearchStructure");
                    }
                    else
                    {
                        structurenode.SetAttribute("type", "Structure");
                    }

                    // chèn thông tin vào structure
                    XmlElement position = doc.CreateElement("Position");
                    position.SetAttribute("X", structure.Position.X.ToString());
                    position.SetAttribute("Y", structure.Position.Y.ToString());
                    structurenode.AppendChild(position);
                }

                // lưu unit
                XmlElement unitsNode = doc.CreateElement("Units");
                playernode.AppendChild(unitsNode);
                for (int j = 0; j < player.UnitListCreated.Count; j++)
                {
                    Sprite unit = player.UnitListCreated[j];
                    XmlElement unitnode = doc.CreateElement("Unit");
                    unitsNode.AppendChild(unitnode);
                    unitnode.SetAttribute("name", unit.Info.Name);
                    if (unit is ProducerUnit)
                    {
                        unitnode.SetAttribute("type", "ProducerUnit");
                    }
                    else
                    {
                        unitnode.SetAttribute("type", "Unit");
                    }
                    // chèn thông tin vào unit
                    XmlElement position = doc.CreateElement("Position");
                    position.SetAttribute("X", unit.Position.X.ToString());
                    position.SetAttribute("Y", unit.Position.Y.ToString());
                    unitnode.AppendChild(position);
                }

                // lưu technology
                XmlElement techsNode = doc.CreateElement("Technologies");
                playernode.AppendChild(techsNode);
                for (int j = 0; j < player.TechListResearch.Count; j++)
                {
                    Technology tech = player.TechListResearch[j];
                    XmlElement technode = doc.CreateElement("Technology");
                    techsNode.AppendChild(technode);
                    technode.SetAttribute("name", tech.TechInfo.Name);
                }
            }

            // resource center
            XmlElement resources = doc.CreateElement("ResourceCenters");
            root.AppendChild(resources);
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap.Count; i++)
            {
                Sprite resource = GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[i];
                XmlElement resourcenode = doc.CreateElement("ResourceCenter");
                resources.AppendChild(resourcenode);
                // chèn thông tin vào resource center
                XmlElement position = doc.CreateElement("Position");
                position.SetAttribute("X", resource.Position.X.ToString());
                position.SetAttribute("Y", resource.Position.Y.ToString());
                resourcenode.AppendChild(position);
            }
            doc.Save(pathSaveFile);
        }

        public void LoadGame(string pathSaveFile)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            XmlDocument doc = new XmlDocument();
            doc.Load(pathSaveFile);

            //load map
            XmlNode map = doc.SelectSingleNode("//Map[1]");
            string pathmap = map.Attributes["name"].Value;
            GlobalDTO.MANAGER_GAME.LoadBattleField(pathmap);

            XmlNode resources = doc.SelectSingleNode("//ResourceCenters");
            for (int i = 0; i < resources.ChildNodes.Count; i++)
            {
                XmlNode node = resources.ChildNodes[i];
                Sprite rs = ((ResourceCenter)this._game.ResourceMgr[node.Attributes["name"].Value]).Clone() as Sprite;
                rs.Position = new Vector2((float.Parse(node.ChildNodes[0].Attributes["X"].Value)), (float.Parse(node.ChildNodes[0].Attributes["Y"].Value)));
                GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap.Add(rs);
                this._game.Components.Add(rs);
            }

            ////player
            //ComputerPlayerManager comMgr = new ComputerPlayerManager(this._game);
            //XmlNode players = doc.SelectSingleNode("//Players");
            //for (int i = 0; i < players.ChildNodes.Count; i++)
            //{
            //    Player player;
            //    if(players.ChildNodes[i].Attributes["type"].Value == "Player")
            //    {
            //        player = new Player(this._game);
            //    }
            //    else
            //    {
            //        player = comMgr.Players(ComputerPlayerManager.ComputerLevel.MEDIUM);
            //    }

            //    player.Code = int.Parse(players.ChildNodes[i].Attributes["code"].Value);
            //    if(players.ChildNodes[i].Attributes["code"].Value.Contains("Red"))
            //    {
            //        player.Color = Color.Red;
            //    }
            //    else if(players.ChildNodes[i].Attributes["code"].Value.Contains("Blue"))
            //    {
            //        player.Color = Color.Blue;
            //    }
            //    else if(players.ChildNodes[i].Attributes["code"].Value.Contains("Green"))
            //    {
            //        player.Color = Color.Red;
            //    }
            //    else if(players.ChildNodes[i].Attributes["code"].Value.Contains("Yellow"))
            //    {
            //        player.Color = Color.Gold;
            //    }        
            //     foreach (KeyValuePair<String, Sprite> sModel in this._game.StructureMgr)
            //     {
            //            if (sModel.Key.Contains(player.Code.ToString()))
            //            {
            //                player.ModelStructureList.Add(sModel.Key, sModel.Value);
            //            }
            //     }
            //    foreach (KeyValuePair<String, Sprite> uModel in this._game.UnitMgr)
            //        {
            //                player.ModelUnitList.Add(uModel.Key, uModel.Value);
            //        }     
                
            //    // structure
            //    XmlNode structures = players.ChildNodes[i].ChildNodes[0];
            //    for(int j = 0 ; j < structures.ChildNodes.Count;j++)
            //    {
            //        XmlNode structurenode = structures.ChildNodes[j];
            //        Sprite structure = ((Structure)this._game.StructureMgr[structurenode.Attributes["name"].Value]).Clone()as Sprite;
            //        structure.Position = new Vector2(float.Parse(structurenode.ChildNodes[0].Attributes["X"].Value),float.Parse(structurenode.ChildNodes[0].Attributes["Y"].Value));
            //        ((Structure)structure).UnitCenterPoint = new Point(rnd.Next((int)structure.Position.X + 128, (int)structure.Position.X + 194), rnd.Next((int)structure.Position.Y + 128, (int)structure.Position.Y + 194));
            //        structure.CodeFaction = player.Code;
            //        structure.Color = player.Color;
            //        ((Structure)structure).ListUnitsBuying = new List<List<Unit>>();
            //        ((Structure)structure).ProcessingBuyUnit = null;
            //        ((Structure)structure).DelayTimeToBuild = 0;
            //        ((Structure)structure).PlayerContainer = player;
            //        player.StructureListCreated.Add(structure);
            //        GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add(structure);
            //        this._game.Components.Add(structure);
            //    }

            //    // unit
            //    XmlNode units = players.ChildNodes[i].ChildNodes[1];
            //    for(int j = 0 ; j < units.ChildNodes.Count;j++)
            //    {
            //        XmlNode unitnode = units.ChildNodes[j];
            //        Sprite unit = ((Structure)this._game.StructureMgr[unitnode.Attributes["name"].Value]).Clone()as Sprite;
            //        unit.Position = new Vector2(float.Parse(unitnode.ChildNodes[0].Attributes["X"].Value),float.Parse(unitnode.ChildNodes[0].Attributes["Y"].Value));                    
            //        unit.CodeFaction = player.Code;
            //        unit.Color = player.Color;                                        
            //        ((Unit)unit).PlayerContainer = player;
            //        player.StructureListCreated.Add(unit);
            //        GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add(unit);
            //        this._game.Components.Add(unit);
            //    }

            //    // technology
            //    XmlNode techs = players.ChildNodes[i].ChildNodes[2];
            //     for(int j = 0 ; j < techs.ChildNodes.Count;j++)
            //    {
            //        XmlNode technode = units.ChildNodes[j];          
            //         Technology tech = new Technology(this._game,technode.Attributes["name"].Value);
            //         player.TechListResearch.Add(tech);
            //    }
            //}
        }
        #endregion
    }
}