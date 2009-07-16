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

namespace GameDemo1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Thread _thread;

        public ResourceCenterManager ResourceMgr;
        public UnitManager UnitMgr;
        public StructureManager StructureMgr;
        public ProducerUnitManager ProducerUnitMgr;

        #region basic method        
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GlobalDTO.SCREEN_SIZE.Width;
            graphics.PreferredBackBufferHeight = GlobalDTO.SCREEN_SIZE.Height;
            graphics.ApplyChanges();
            Content.RootDirectory = GlobalDTO.RES_CONTENT_PATH;
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

            LoadingScene loading = new LoadingScene(this);// màn hình loading
            this.Components.Add(loading);
            AudioGame au = new AudioGame(this);
            au.StopPlayingBackSound();
            au.PlayBackSound("loading");

            // tách tiểu trình cho load game
            this._thread = new Thread(LoadGame);
            this._thread.Start();            
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
        /// <summary>
        /// Load các thành phần ban đầu cho game
        /// </summary>
        public void LoadGame()
        {
            // thông tin demo
            /// 
            /// Manager element thành phần quản lý game
            ///             
            ManagerPlayer _managerPlayer = new ManagerPlayer(this);
            ManagerGame _managerGame = new ManagerGame(this);
            GlobalDTO.MANAGER_GAME = _managerGame;

            // load map từ file mô tả bao gồm cả các terrain
            _managerGame.LoadBattleField(GlobalDTO.SPEC_MAP_PATH + GlobalDTO.MAP + ".xml");

            // Khởi tạo các đối tượng chứa hàng mẩu
            ResourceMgr = new ResourceCenterManager(this);
            UnitMgr = new UnitManager(this);
            StructureMgr = new StructureManager(this);
            ProducerUnitMgr = new ProducerUnitManager(this);

            // thêm các mẫu
             //this.AddModels();

            // Nạp dữ liệu hàng mẩu manager cho Unit, Structure,Resource center
            UnitMgr.Load(GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            ResourceMgr.Load(GlobalDTO.OBJ_RESOURCE_CENTER_PATH, GlobalDTO.SPEC_RESOURCECENTER_PATH);
            StructureMgr.Load(GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            ProducerUnitMgr.Load(GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);

            // Load list of all structure of the game
            _managerGame.ListStructureOfGame = StructureMgr.ToArray();

            // load các resource center             
            Sprite stone = ((ResourceCenter)ResourceMgr["StoneResourceCenter"]).Clone() as Sprite;
            stone.Position = new Vector2(3000, 3000);
            Sprite gold = ((ResourceCenter)ResourceMgr["GoldResourceCenter"]).Clone() as Sprite;
            gold.Position = new Vector2(1000, 2000);
            _managerGame.ListResourceCenterOnmap.Add(stone); // add resource center vào manager game để quản lý            
            _managerGame.ListResourceCenterOnmap.Add(gold); // add resource center vào manager game để quản lý            
            Components.Add(stone); // add vào game component để chúng được vẽ            
            Components.Add(gold); // add vào game component để chúng được vẽ            

            /// for players tạo các player và thông tin khởi đầu
            
            /// player 1 có code = 1
            Player player1 = new Player(this);
            player1.Code = 1;
            player1.Color = Color.White; // màu trắng
            ///// player 2 có code = 2
            Computer player2 = new Computer(this);
            player2.Init();
            player2.Code = 2;
            player2.Color = Color.Red;

            Random ran = new Random(this.GetHashCode());

            // thông tin cho player 1            
            //Sprite military01 = ((Structure)StructureMgr["Military"]).Clone() as Sprite;
            //_managerPlayer.LoadUnitListToStructure(
            //                        (Structure)military01,
            //                        ((Structure)military01).CurrentUpgradeInfo.Id );
            //military01.Position = new Vector2(2000, 2000);
            //military01.CodeFaction = player1.Code;
            //military01.Color = player1.Color;
            //((Structure)military01).UnitCenterPoint = new Point(ran.Next((int)military01.Position.X + 128, (int)military01.Position.X + 194), ran.Next((int)military01.Position.Y + 128, (int)military01.Position.Y + 194));
            //((Structure)military01).DelayTimeToBuild = 0;
            //player1.StructureListCreated.Add(military01);// add structure nhà chính cho player1
            //((Structure)player1.StructureListCreated[0]).PlayerContainer = player1; // nhà chính này hiển nhiên thuộc người player1
            //_managerGame.ListStructureOnMap.Add(player1.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý                        

            //Sprite townhall01 = ((Structure)StructureMgr["TownHall"]).Clone() as Sprite;
            //_managerPlayer.LoadUnitListToStructure(
            //                        (Structure)townhall01,
            //                        ((Structure)townhall01).CurrentUpgradeInfo.Id);
            //townhall01.Position = new Vector2(2100, 2300);
            //((Structure)townhall01).UnitCenterPoint = new Point(ran.Next((int)townhall01.Position.X + 128, (int)townhall01.Position.X + 194), ran.Next((int)townhall01.Position.Y + 128, (int)townhall01.Position.Y + 194));
            //townhall01.CodeFaction = player1.Code;
            //townhall01.Color = player1.Color = Color.White;
            //((Structure)townhall01).DelayTimeToBuild = 0;
            //player1.StructureListCreated.Add(townhall01);// add structure nhà chính cho player1
            //((Structure)player1.StructureListCreated[1]).PlayerContainer = player1; // nhà chính này hiển nhiên thuộc người player1
            //_managerGame.ListStructureOnMap.Add(player1.StructureListCreated[1]);// add nó vào list các structure trong manager game để quản lý                        
            
            for (int i = 0; i < 5; i++)
            {                            
                Sprite blackAngel01 = ((Unit)UnitMgr["Black_Angel"]).Clone() as Sprite;
                blackAngel01.Position = new Vector2(ran.Next(2000, 2500), ran.Next(2000, 2500));
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
                producer01.Position = new Vector2(ran.Next(2000, 2500), ran.Next(2000, 2500));
                producer01.CodeFaction = player1.Code;
                producer01.Color = player1.Color;
                //((Structure)player1.StructureListCreated[1]).OwnerUnitList.Add(producer01);// producer thuộc nhà chính
                ((Unit)producer01).PlayerContainer = player1;// hiển nhiên nó thuộc player 1 
                //((Unit)producer01).StructureContainer = (Structure)player1.StructureListCreated[1]; // và do nhà chính sinh ra            
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
            }            


            // thông tin cho player 2
            //Sprite military02 = ((Structure)StructureMgr["Military"]).Clone() as Sprite;
            //_managerPlayer.LoadUnitListToStructure(
            //                        (Structure)military02,
            //                        ((Structure)military02).CurrentUpgradeInfo.Id);
            //military02.Position = new Vector2(4700, 1000);
            //((Structure)military02).ListUnitsBuying = new List<List<Unit>>();
            //((Structure)military02).UnitCenterPoint = new Point(ran.Next((int)military02.Position.X + 128, (int)military02.Position.X + 194), ran.Next((int)military02.Position.Y + 128, (int)military02.Position.Y + 194));
            //military02.CodeFaction = player2.Code;
            //military02.Color = player2.Color;
            //((Structure)military02).DelayTimeToBuild = 0;
            //player2.StructureListCreated.Add(military02);
            //((Structure)player2.StructureListCreated[0]).PlayerContainer = player2; // nhà chính này hiển nhiên thuộc người player2
            //_managerGame.ListStructureOnMap.Add(player2.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý

            for (int i = 0; i < 5; i++)
            {
                Sprite blackAngel02 = ((Unit)UnitMgr["Angel"]).Clone() as Sprite;
                blackAngel02.Position = new Vector2(ran.Next(4400, 4800), ran.Next(1000, 1500));
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
                producer01.Position = new Vector2(ran.Next(4400, 4800), ran.Next(1000, 1500));
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
            }

            ///// add manager element
            this.Components.Add(player1);
            this.Components.Add(player2);
            _managerGame.Players.Add(player1);
            _managerGame.Players.Add(player2);            
            _managerPlayer.PlayerIsUser = player1;// choose player is user // player1 chính là user và do user điều khiển từ lớp Manager Player
            _managerPlayer.CreateMenuItemIsStructure();

            ///// add cursor finally
            this.Components.Add(_managerPlayer);
            this.Components.Add(_managerGame);
            this.Components.Add(_managerGame.Minimap);
            this.Components.Add(_managerGame.Cursor);






            // xóa màn hình loading và tắt tiểu trình load, bắt đầu chế độ chơi
            this.Components.RemoveAt(0);
            GlobalDTO.CURRENT_MODEGAME = "Playing";            
            AudioGame au = new AudioGame(this);
            au.StopPlayingBackSound();
            au.PlayBackSound("bgsound");
            this._thread.Abort();            
        }

        public void AddModels()
        {
            // Bổ sung thêm hàng mẩu
            ProducerUnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Producer.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Black_Angel.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Elf_swordman.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Unicorn.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Wolf.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Phoenix.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Angel.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);
            UnitMgr.Add(GlobalDTO.SPEC_UNIT_PATH + "Archon_Archer.xml", GlobalDTO.OBJ_UNIT_PATH, GlobalDTO.SPEC_UNIT_PATH);            

            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Military.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "TownHall.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);
            StructureMgr.Add(GlobalDTO.SPEC_STRUCTURE_PATH + "Animal.xml", GlobalDTO.OBJ_STRUCTURE_PATH, GlobalDTO.SPEC_STRUCTURE_PATH);            

            ResourceMgr.Add(GlobalDTO.SPEC_RESOURCECENTER_PATH + "StoneResourceCenter.xml", GlobalDTO.OBJ_RESOURCE_CENTER_PATH, GlobalDTO.SPEC_RESOURCECENTER_PATH);
            ResourceMgr.Add(GlobalDTO.SPEC_RESOURCECENTER_PATH + "GoldResourceCenter.xml", GlobalDTO.OBJ_RESOURCE_CENTER_PATH, GlobalDTO.SPEC_RESOURCECENTER_PATH);
        }
        #endregion
    }
}
