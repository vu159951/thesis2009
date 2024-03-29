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
using GameSharedObject.Components;
using System.Threading;
using GameSharedObject;
using GameSharedObject.DTO;
using GameDemo1.Factory;
using GameSharedObject.Frames;
using GameSharedObject.Controls;
using GameSharedObject.Factory;

namespace GameDemo1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private frmLoading loading;
        private frmMain mainMenu;

        public bool isPreviousStepOK = false;
        private delegate void FunctionHandler();
        private Thread _thread; 
        public ResourceCenterManager ResourceMgr;
        public UnitManager UnitMgr;
        public StructureManager StructureMgr;
        public ResearchStructureManager ResearchStructureMgr;
        public ProducerUnitManager ProducerUnitMgr;

        #region basic method
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GlobalDTO.SCREEN_SIZE.Width;
            graphics.PreferredBackBufferHeight = GlobalDTO.SCREEN_SIZE.Height;
            graphics.ApplyChanges();
            Content.RootDirectory = GlobalDTO.RES_CONTENT_PATH;
            isPreviousStepOK = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);
            // TODO: use this.Content to load your game content here            
            this.Window.Title = "My Game";

            AudioGame au = new AudioGame(this);
            au.StopPlayingBackSound();
            au.PlayBackSound("loading");
            
            
            // LOAD MENU TO SAVE/LOAD GAME
            mainMenu = new frmMain(this);
            this.Components.Add(mainMenu);
            mainMenu.ShowControls();
            mainMenu.Menu.Button1Click += new GameMenu.Button1ClickHandler(Menu_Button1Click);
            mainMenu.Menu.Button4Click += new GameMenu.Button4ClickHandler(Menu_Button4Click);

            //Form frm = new Form(this);
            //frm.Background = Content.Load<Texture2D>("Images\\MenuPanel\\menuPanel2");
            //frm.Location = new Point(100, 100);
            //frm.Size = new System.Drawing.Size(400, 200);
            //// frm.Opacity = 45;
            ////frm.MouseDown += new Control.MouseDownHandler(frm_MouseDown);
            ////frm.MouseUp += new Control.MouseDownHandler(frm_MouseUp);
            ////frm.MouseMove += new Control.MouseDownHandler(frm_MouseMove);
            ////frm.KeyDown +=new Control.KeyDownHandler(frm_KeyDown);
            ////frm.KeyUp += new Control.KeyUpHandler(frm_KeyUp);
            //frm.IsFocus = true;
            //GameButton btn = new GameButton(this);
            //btn.ForeColor = Color.Yellow;
            //btn.Text = "love you";
            //btn.Location = new Point(5, 5);
            //// btn.Click += new Button.ClickHandler(btn_Click);
            ////btn.MouseEnter += new Control.MouseEnterHandler(btn_MouseEnter);
            ////btn.MouseLeave += new Control.MouseLeaveHandler(btn_MouseLeave);
            //this.Components.Add(frm);
            //frm.Controls.Add(btn);
        }
        private void Menu_Button1Click()
        {
            this.Window.Title = "My Game";
            this.ShowLoadingFrame();

            // Unload menu
            mainMenu.UnLoad();
        }
        private void Menu_Button4Click()
        {
            this.Exit();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
        #endregion

        #region Function
        private Random rnd = new Random(DateTime.Now.Millisecond);           
        private ManagerPlayer _managerPlayer;
        private ManagerGame _managerGame;
        private ComputerPlayerManager comMgr;
        private Player player1;
        private Player player2;
        private Player player3;
        private Player player4;

        public void AddModels()
        {
            // Bổ sung thêm hàng mẩu
            //ProducerUnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Producer.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);

            //UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Black_Angel.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);            
            //UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Elf_swordman.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            //UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Unicorn.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            //UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Wolf.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            //UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Phoenix.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            //UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Angel.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            //UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Archon_Archer.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);

            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Human_House_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Castle_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Dwarf_Domicile_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Elf_Domicile_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Frostling_House_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);            
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Tigran_Building_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);

            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Smithy_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Medical_Station_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Resource_Building_01.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);

            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Human_House_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Castle_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Dwarf_Domicile_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Elf_Domicile_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Frostling_House_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Tigran_Building_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);

            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Smithy_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Medical_Station_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Resource_Building_02.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);

            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Human_House_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Castle_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Dwarf_Domicile_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Elf_Domicile_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Frostling_House_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Tigran_Building_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);

            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Smithy_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Medical_Station_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            //ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Resource_Building_03.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);            

            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Human_House_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Castle_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Dwarf_Domicile_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Elf_Domicile_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Frostling_House_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Tigran_Building_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);

            ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Smithy_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Medical_Station_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            ResearchStructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Resource_Building_04.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);

            //ResourceMgr.Add(GlobalDTO.SPEC_RESOURCECENTER_PATH + "StoneResourceCenter.xml", GlobalDTO.OBJ_RESOURCE_CENTER_PATH, GlobalDTO.SPEC_RESOURCECENTER_PATH);
            //ResourceMgr.Add(GlobalDTO.SPEC_RESOURCECENTER_PATH + "GoldResourceCenter.xml", GlobalDTO.OBJ_RESOURCE_CENTER_PATH, GlobalDTO.SPEC_RESOURCECENTER_PATH);
            this.isPreviousStepOK = true;
        }
        private void InitializeObjects()
        {
            this.loading.Text = "Initializing main objects...";

            /// Manager element thành phần quản lý game
            _managerPlayer = new ManagerPlayer(this);
            _managerGame = new ManagerGame(this);
            comMgr = new ComputerPlayerManager(this);

            GlobalDTO.MANAGER_GAME = _managerGame;

            this.isPreviousStepOK = true;
        }
        private void InitializeMap()
        {
            this.loading.Text = "Initializing map...";

            // load map từ file mô tả bao gồm cả các terrain
            _managerGame.LoadBattleField(GlobalDTO.SPEC_MAP_PATH + GlobalDTO.MAP + ".xml");

            this.isPreviousStepOK = true;
        }
        private void InitializeModelObjects()
        {
            this.loading.Text = "Initializing model objects...";

            // Khởi tạo các đối tượng chứa hàng mẩu
            ResourceMgr = new ResourceCenterManager(this);
            UnitMgr = new UnitManager(this);
            StructureMgr = new StructureManager(this);
            ResearchStructureMgr = new ResearchStructureManager(this);
            ProducerUnitMgr = new ProducerUnitManager(this);

            this.isPreviousStepOK = true;
        }
        private void LoadModelObjectData()
        {
            this.loading.Text = "Loading model object data...";

            // Nạp dữ liệu hàng mẩu manager cho Unit, Structure,Resource center
            UnitMgr.Load(GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            ResourceMgr.Load(GlobalDTO.OBJ_RESOURCE_CENTER_PATH, GlobalDTO.SPEC_RESOURCECENTER_PATH);
            StructureMgr.Load(GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            ProducerUnitMgr.Load(GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            this.isPreviousStepOK = true;
        }
        private void GetAllStructureOfTheGame()
        {
            this.loading.Text = "Loading structures...";

            // lấy tập các structure của game
            _managerGame.ListStructureOfGame = StructureMgr.ToArray();
            // this.AddModels();

            this.isPreviousStepOK = true;
        }
        private void LoadResourceCenters()
        {
            this.loading.Text = "Loading resourcecenters...";

            // load các resource center             
            Sprite stone = ((ResourceCenter)ResourceMgr["StoneResourceCenter"]).Clone() as Sprite;
            stone.Position = new Vector2(3000, 3000);
            Sprite gold = ((ResourceCenter)ResourceMgr["GoldResourceCenter"]).Clone() as Sprite;
            gold.Position = new Vector2(1000, 2000);
            _managerGame.ListResourceCenterOnmap.Add(stone); // add resource center vào manager game để quản lý            
            _managerGame.ListResourceCenterOnmap.Add(gold); // add resource center vào manager game để quản lý            
            GlobalFunction.SetOccupiedCellsToMatrix(stone);
            GlobalFunction.SetOccupiedCellsToMatrix(gold);
            Components.Add(stone); // add vào game component để chúng được vẽ            
            Components.Add(gold); // add vào game component để chúng được vẽ            
            this.isPreviousStepOK = true;
        }
        private void LoadPlayer1()
        {
            this.loading.Text = "Loading player 1...";

            /// player 1 có code = 1
            player1 = new Player(this);
            this.InitPlayer(player1, 1, Color.Blue);
            player1.FlagImage = this.Content.Load<Texture2D>("Images//Flag//Blue");

            /////////////////// thông tin cho player 1            
            Sprite townhall01 = ((Structure)StructureMgr["Castle_01"]).Clone() as Sprite;

            _managerPlayer.LoadUnitListToStructure(
                                    (Structure)townhall01,
                                    ((Structure)townhall01).CurrentUpgradeInfo.Id);
            townhall01.Position = new Vector2(2100, 2350);
            ((Structure)townhall01).UnitCenterPoint = new Point(rnd.Next((int)townhall01.Position.X + 128, (int)townhall01.Position.X + 194), rnd.Next((int)townhall01.Position.Y + 128, (int)townhall01.Position.Y + 194));
            townhall01.CodeFaction = player1.Code;
            townhall01.Color = Color.White;
            ((Structure)townhall01).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)townhall01).ProcessingBuyUnit = null;
            ((Structure)townhall01).DelayTimeToBuild = 0;
            player1.StructureListCreated.Add(townhall01);// add structure nhà chính cho player1
            ((Structure)player1.StructureListCreated[0]).PlayerContainer = player1; // nhà chính này hiển nhiên thuộc người player1
            _managerGame.ListStructureOnMap.Add(player1.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý                        

            for (int i = 0; i < 5; i++)
            {
                Sprite blackAngel01 = ((Unit)UnitMgr["Black_Angel"]).Clone() as Sprite;
                blackAngel01.Position = new Vector2(rnd.Next(2000, 2500), rnd.Next(2000, 2500));
                blackAngel01.CodeFaction = player1.Code;
                blackAngel01.Color = player1.Color;
                //((Structure)player1.StructureListCreated[0]).OwnerUnitList.Add(blackAngel01);// producer thuộc nhà chính
                ((Unit)blackAngel01).PlayerContainer = player1;// hiển nhiên nó thuộc player 1 
                //((Unit)blackAngel01).StructureContainer = (Structure)player1.StructureListCreated[0]; // và do nhà chính sinh ra            
                _managerGame.ListUnitOnMap.Add(blackAngel01);// add nó vào list unit của manager game mà quản lý
                player1.UnitListCreated.Add(blackAngel01); // add unit producer cho player 1            
            }

            for (int i = 0; i < 3; i++)
            {
                Sprite producer01 = ((ProducerUnit)ProducerUnitMgr["Producer"]).Clone() as Sprite;
                producer01.Position = new Vector2(rnd.Next(2000, 2500), rnd.Next(2000, 2500));
                producer01.CodeFaction = player1.Code;
                producer01.Color = player1.Color;
                ((Unit)producer01).PlayerContainer = player1;// hiển nhiên nó thuộc player 1                 
                _managerGame.ListUnitOnMap.Add(producer01);// add nó vào list unit của manager game mà quản lý
                player1.UnitListCreated.Add(producer01); // add unit producer cho player 1            
            }

            for (int i = 0; i < player1.UnitListCreated.Count; i++)// add các unit và structure của player 1 vào component để vẻ chúng ra
            {
                player1.UnitListCreated[i].Color = player1.Color;
                this.Components.Add(player1.UnitListCreated[i]);
            }
            for (int i = 0; i < player1.StructureListCreated.Count; i++)
            {
                player1.StructureListCreated[i].Color = player1.Color;
                this.Components.Add(player1.StructureListCreated[i]);
                GlobalFunction.SetOccupiedCellsToMatrix(player1.StructureListCreated[i]);
            }
            this.isPreviousStepOK = true;
        }
        private void LoadPlayer2()
        {
            this.loading.Text = "Loading player 2...";

            /// player 2 có code = 2
            player2 = comMgr.Players(ComputerPlayerManager.ComputerLevel.EASY);
            this.InitPlayer(player2, 2, Color.Red);
            player2.FlagImage = this.Content.Load<Texture2D>("Images//Flag//Red");

            ////////////////////// thông tin cho player 2
            Sprite townHall2 = ((Structure)StructureMgr["Castle_02"]).Clone() as Sprite;
            _managerPlayer.LoadUnitListToStructure(
                                    (Structure)townHall2,
                                    ((Structure)townHall2).CurrentUpgradeInfo.Id);
            townHall2.Position = new Vector2(4700, 1000);
            ((Structure)townHall2).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)townHall2).UnitCenterPoint = new Point(rnd.Next((int)townHall2.Position.X + 128, (int)townHall2.Position.X + 194), rnd.Next((int)townHall2.Position.Y + 128, (int)townHall2.Position.Y + 194));
            townHall2.CodeFaction = player2.Code;
            townHall2.Color = player2.Color;
            ((Structure)townHall2).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)townHall2).ProcessingBuyUnit = null;
            ((Structure)townHall2).DelayTimeToBuild = 0;
            player2.StructureListCreated.Add(townHall2);
            ((Structure)player2.StructureListCreated[0]).PlayerContainer = player2; // nhà chính này hiển nhiên thuộc người player2
            _managerGame.ListStructureOnMap.Add(player2.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý

            for (int i = 0; i < 5; i++)
            {
                Sprite blackAngel02 = ((Unit)UnitMgr["Angel"]).Clone() as Sprite;
                blackAngel02.Position = new Vector2(rnd.Next(4400, 4800), rnd.Next(1000, 1500));
                blackAngel02.CodeFaction = player2.Code;
                blackAngel02.Color = player2.Color;
                //((Structure)player2.StructureListCreated[0]).OwnerUnitList.Add(blackAngel02);// producer thuộc nhà chính
                ((Unit)blackAngel02).PlayerContainer = player2;// hiển nhiên nó thuộc player 2 
                //((Unit)blackAngel02).StructureContainer = (Structure)player2.StructureListCreated[0]; // và do nhà chính sinh ra            
                _managerGame.ListUnitOnMap.Add(blackAngel02);// add nó vào list unit của manager game mà quản lý
                player2.UnitListCreated.Add(blackAngel02); // add unit producer cho player 2
            }
            for (int i = 0; i < 3; i++)
            {
                Sprite producer01 = ((ProducerUnit)ProducerUnitMgr["Producer"]).Clone() as Sprite;
                producer01.Position = new Vector2(rnd.Next(4400, 4800), rnd.Next(1000, 1500));
                producer01.CodeFaction = player2.Code;
                producer01.Color = player2.Color;
                //((Structure)player1.StructureListCreated[1]).OwnerUnitList.Add(producer01);// producer thuộc nhà chính
                ((Unit)producer01).PlayerContainer = player2;// hiển nhiên nó thuộc player 1 
                //((Unit)producer01).StructureContainer = (Structure)player1.StructureListCreated[1]; // và do nhà chính sinh ra            
                _managerGame.ListUnitOnMap.Add(producer01);// add nó vào list unit của manager game mà quản lý
                player2.UnitListCreated.Add(producer01); // add unit producer cho player 1            
            }

            for (int i = 0; i < player2.UnitListCreated.Count; i++)// add các unit và structure của player 2 vào component
            {
                player2.UnitListCreated[i].Color = player2.Color;
                this.Components.Add(player2.UnitListCreated[i]);
            }
            for (int i = 0; i < player2.StructureListCreated.Count; i++)
            {
                player2.StructureListCreated[i].Color = player2.Color;
                this.Components.Add(player2.StructureListCreated[i]);
                GlobalFunction.SetOccupiedCellsToMatrix(player2.StructureListCreated[i]);
            }
            this.isPreviousStepOK = true;
        }
        private void LoadPlayer3()
        {
            this.loading.Text = "Loading player 3...";

            /// player 3 có code = 3
            player3 = comMgr.Players(ComputerPlayerManager.ComputerLevel.MEDIUM);
            this.InitPlayer(player3, 3, Color.Green);
            player3.FlagImage = this.Content.Load<Texture2D>("Images//Flag//Green");

            ////////////////////// thông tin cho player 3
            Sprite Castle_03 = ((Structure)StructureMgr["Castle_03"]).Clone() as Sprite;
            _managerPlayer.LoadUnitListToStructure(
                                    (Structure)Castle_03,
                                    ((Structure)Castle_03).CurrentUpgradeInfo.Id);
            Castle_03.Position = new Vector2(2900, 700);
            ((Structure)Castle_03).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)Castle_03).UnitCenterPoint = new Point(rnd.Next((int)Castle_03.Position.X + 128, (int)Castle_03.Position.X + 194), rnd.Next((int)Castle_03.Position.Y + 128, (int)Castle_03.Position.Y + 194));
            Castle_03.CodeFaction = player3.Code;
            Castle_03.Color = player3.Color;
            ((Structure)Castle_03).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)Castle_03).ProcessingBuyUnit = null;
            ((Structure)Castle_03).DelayTimeToBuild = 0;
            player3.StructureListCreated.Add(Castle_03);
            ((Structure)player3.StructureListCreated[0]).PlayerContainer = player3; // nhà chính này hiển nhiên thuộc người player2
            _managerGame.ListStructureOnMap.Add(player3.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý

            for (int i = 0; i < 5; i++)
            {
                Sprite blackAngel02 = ((Unit)UnitMgr["Elf_swordman"]).Clone() as Sprite;
                blackAngel02.Position = new Vector2(rnd.Next(2500, 2900), rnd.Next(600, 800));
                blackAngel02.CodeFaction = player3.Code;
                blackAngel02.Color = player3.Color;
                ((Unit)blackAngel02).PlayerContainer = player3;// hiển nhiên nó thuộc player 2                 
                _managerGame.ListUnitOnMap.Add(blackAngel02);// add nó vào list unit của manager game mà quản lý
                player3.UnitListCreated.Add(blackAngel02); // add unit producer cho player 2
            }
            for (int i = 0; i < 3; i++)
            {
                Sprite producer01 = ((ProducerUnit)ProducerUnitMgr["Producer"]).Clone() as Sprite;
                producer01.Position = new Vector2(rnd.Next(2500, 2900), rnd.Next(600, 800));
                producer01.CodeFaction = player3.Code;
                producer01.Color = player3.Color;
                ((Unit)producer01).PlayerContainer = player3;// hiển nhiên nó thuộc player 1                 
                _managerGame.ListUnitOnMap.Add(producer01);// add nó vào list unit của manager game mà quản lý
                player3.UnitListCreated.Add(producer01); // add unit producer cho player 1            
            }

            for (int i = 0; i < player3.UnitListCreated.Count; i++)// add các unit và structure của player 2 vào component
            {
                player3.UnitListCreated[i].Color = player3.Color;
                this.Components.Add(player3.UnitListCreated[i]);
            }
            for (int i = 0; i < player3.StructureListCreated.Count; i++)
            {
                player3.StructureListCreated[i].Color = player3.Color;
                this.Components.Add(player3.StructureListCreated[i]);
                GlobalFunction.SetOccupiedCellsToMatrix(player3.StructureListCreated[i]);
            }
            this.isPreviousStepOK = true;
        }
        private void LoadPlayer4()
        {
            this.loading.Text = "Loading player 4...";

            /// player 4 có code = 4
            player4 = comMgr.Players(ComputerPlayerManager.ComputerLevel.VERYHARD);
            this.InitPlayer(player4, 4, Color.Gold);
            player4.FlagImage = this.Content.Load<Texture2D>("Images//Flag//Yellow");

            ////////////////////// thông tin cho player 4
            Sprite Castle_04 = ((Structure)StructureMgr["Castle_04"]).Clone() as Sprite;
            _managerPlayer.LoadUnitListToStructure(
                                    (Structure)Castle_04,
                                    ((Structure)Castle_04).CurrentUpgradeInfo.Id);
            Castle_04.Position = new Vector2(4900, 2500);
            ((Structure)Castle_04).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)Castle_04).UnitCenterPoint = new Point(rnd.Next((int)Castle_04.Position.X + 128, (int)Castle_04.Position.X + 194), rnd.Next((int)Castle_04.Position.Y + 128, (int)Castle_04.Position.Y + 194));
            Castle_04.CodeFaction = player4.Code;
            Castle_04.Color = player4.Color;
            ((Structure)Castle_04).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)Castle_04).ProcessingBuyUnit = null;
            ((Structure)Castle_04).DelayTimeToBuild = 0;
            player4.StructureListCreated.Add(Castle_04);
            ((Structure)player4.StructureListCreated[0]).PlayerContainer = player4; // nhà chính này hiển nhiên thuộc người player2
            _managerGame.ListStructureOnMap.Add(player4.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý

            for (int i = 0; i < 5; i++)
            {
                Sprite blackAngel02 = ((Unit)UnitMgr["Archon_Archer"]).Clone() as Sprite;
                blackAngel02.Position = new Vector2(rnd.Next(4400, 5200), rnd.Next(2000, 2500));
                blackAngel02.CodeFaction = player4.Code;
                blackAngel02.Color = player4.Color;
                ((Unit)blackAngel02).PlayerContainer = player4;// hiển nhiên nó thuộc player 2                 
                _managerGame.ListUnitOnMap.Add(blackAngel02);// add nó vào list unit của manager game mà quản lý
                player4.UnitListCreated.Add(blackAngel02); // add unit producer cho player 2
            }
            for (int i = 0; i < 3; i++)
            {
                Sprite producer01 = ((ProducerUnit)ProducerUnitMgr["Producer"]).Clone() as Sprite;
                producer01.Position = new Vector2(rnd.Next(4400, 5200), rnd.Next(2000, 2500));
                producer01.CodeFaction = player4.Code;
                producer01.Color = player4.Color;
                ((Unit)producer01).PlayerContainer = player4;// hiển nhiên nó thuộc player 1                 
                _managerGame.ListUnitOnMap.Add(producer01);// add nó vào list unit của manager game mà quản lý
                player4.UnitListCreated.Add(producer01); // add unit producer cho player 1            
            }

            for (int i = 0; i < player4.UnitListCreated.Count; i++)// add các unit và structure của player 2 vào component
            {
                player4.UnitListCreated[i].Color = player4.Color;
                this.Components.Add(player4.UnitListCreated[i]);
            }
            for (int i = 0; i < player4.StructureListCreated.Count; i++)
            {
                player4.StructureListCreated[i].Color = player4.Color;
                this.Components.Add(player4.StructureListCreated[i]);
                GlobalFunction.SetOccupiedCellsToMatrix(player3.StructureListCreated[i]);
            }
            this.isPreviousStepOK = true;
        }
        private void LoadPlayersToMap()
        {
            this.loading.Text = "Loading players to map...";

            ///// add manager element
            this.Components.Add(player1);
            this.Components.Add(player2);
            this.Components.Add(player3);
            this.Components.Add(player4);
            this.isPreviousStepOK = true;
        }
        private void LoadPlayersToManager()
        {
            this.loading.Text = "Loading player to manager object...";

            _managerGame.Players.Add(player1);
            _managerGame.Players.Add(player2);
            _managerGame.Players.Add(player3);
            _managerGame.Players.Add(player4);
            _managerPlayer.PlayerIsUser = player1;// choose player is user // player1 chính là user và do user điều khiển từ lớp Manager Player
            _managerPlayer.CreateMenuItemForUser();
            this.isPreviousStepOK = true;
        }
        private void LoadMinimapCursor()
        {
            this.loading.Text = "Loading cursor...";

            ///// add cursor finally
            this.Components.Add(_managerPlayer);
            this.Components.Add(_managerGame);
            this.Components.Add(_managerGame.Minimap);
            this.Components.Add(_managerGame.Cursor);

            this.isPreviousStepOK = true;
        }
        private void LoadBackSound()
        {
            this.loading.Text = "Preparing to start game...";

            GlobalDTO.CURRENT_MODEGAME = "Playing";
            AudioGame au = new AudioGame(this);
            au.StopPlayingBackSound();
            au.PlayBackSound("bgsound");

            this.isPreviousStepOK = true;
            this.loading.Unload();
            // GameSharedObject.Data.MatrixMgr.Save("C:\\xxx.oms", new MatrixDTO(this._managerGame.Map.OccupiedMatrix));
        }
        /// <summary>
        /// Load các thành phần ban đầu cho game
        /// </summary>
        public void LoadGame()
        {
            LoadAFunction(0, 1, InitializeObjects);
            if (loading.Value > 1)
                LoadAFunction(2, 32, InitializeMap);
            if (loading.Value > 33)
                LoadAFunction(34, 47, InitializeModelObjects);
            if (loading.Value > 47)
                LoadAFunction(48, 49, GetAllStructureOfTheGame);
            if (loading.Value > 49)
                LoadAFunction(50, 79, LoadModelObjectData);
            if (loading.Value > 79)
                LoadAFunction(80, 84, LoadResourceCenters);
            if (loading.Value > 84)
                LoadAFunction(85, 86, LoadPlayer1);
            if (loading.Value > 86)
                LoadAFunction(87, 88, LoadPlayer2);
            if (loading.Value > 88)
                LoadAFunction(89, 90, LoadPlayer3);
            if (loading.Value > 90)
                LoadAFunction(91, 92, LoadPlayer4);
            if (loading.Value > 92)
                LoadAFunction(93, 94, LoadPlayersToMap);
            if (loading.Value > 94)
                LoadAFunction(95, 96, LoadPlayersToManager);
            if (loading.Value > 96)
                LoadAFunction(97, 98, LoadMinimapCursor);
            if (loading.Value > 98)
                LoadAFunction(99, 100, LoadBackSound);
        }
        private void ShowLoadingFrame()
        {
            loading = new frmLoading(this);// màn hình loading
            this.Components.Add(loading);
            loading.ShowControls();
            loading.ValueChanged += new frmLoading.ValueChangedHandler(loading_ValueChanged);
            loading.Text = "Initializing...";
        }

        private void loading_ValueChanged(object sender, int value)
        {
            this.LoadGame();
        }
        private void InitPlayer(Player player, int id, Color color)
        {
            player.Init();
            player.Code = id;
            player.Color = color;
            foreach (KeyValuePair<String, Sprite> sModel in this.StructureMgr){
                if (sModel.Key.Contains(player.Code.ToString())){
                    player.ModelStructureList.Add(sModel.Key, sModel.Value);
                }
            }
            foreach (KeyValuePair<String, Sprite> uModel in this.UnitMgr){
                    player.ModelUnitList.Add(uModel.Key, uModel.Value);
            }
        }
        private void LoadAFunction(int floor, int ceiling, FunctionHandler target)
        {
            if (target == null)
                return;

            if (loading.Value <= ceiling){
                if (loading.Value == floor && this.isPreviousStepOK){
                    this.isPreviousStepOK = false;
                    loading.AutoIncrease = true;
                    loading.Increase();
                    _thread = new Thread(new ThreadStart(target));
                    _thread.Start();
                    //target();
                }

                if (loading.Value == ceiling && this.isPreviousStepOK == false){
                    loading.AutoIncrease = false;
                    return;
                }else if (loading.Value == ceiling && this.isPreviousStepOK == true){
                    loading.AutoIncrease = true;
                    return;
                }
            }
        }
        #endregion
    }
}
