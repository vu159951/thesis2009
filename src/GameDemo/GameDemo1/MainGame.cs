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
using GameSharedObject.DTO;
using System.Threading;
using GameSharedObject;

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

            LoadingScene loading = new LoadingScene(this);
            this.Components.Add(loading);
            this._thread = new Thread(LoadGame);
            this._thread.Start();
            //this.LoadGame();
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
            /// load map từ file mô tả bao gồm cả các terrain
            _managerGame.LoadBattleField(GlobalDTO.SPEC_MAP_PATH + GlobalDTO.MAP + ".xml");
            /// load các resource center 
            StoneResourceCenter stone = new StoneResourceCenter(this, GlobalDTO.SPEC_RESOURCECENTER_PATH + "StoneResourceCenter.xml", new Vector2(2500, 1600));
            //ResourceCenter gold = new ResourceCenter(this, "Gold", 10000, GlobalDTO.SPEC_RESOURCECENTER_PATH + "GoldResourceCenter.xml", new Vector2(4800, 1800));
            _managerGame.ListResourceCenterOnmap.Add(stone); // add resource center vào manager game để quản lý
            //this._managerGame.ListResourceCenterOnmap.Add(gold);
            Components.Add(stone); // add vào game component để chúng được vẽ
            //this.Components.Add(gold);

            /// for players tạo các player và thông tin khởi đầu
            /// player 1 có code = 1
            Player player1 = new Player(this);
            player1.Code = 1;
            player1.Color = Color.White; // màu trắng

            player1.StructureListCreated.Add(new Military(this, GlobalDTO.SPEC_STRUCTURE_PATH + "Military.xml", new Vector2(2000, 2500), 1));// add structure nhà chính cho player1
            ((Structure)player1.StructureListCreated[0]).PlayerContainer = player1; // nhà chính này hiển nhiên thuộc người player1
            _managerGame.ListStructureOnMap.Add(player1.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý                        

            player1.StructureListCreated.Add(new TownHall(this, GlobalDTO.SPEC_STRUCTURE_PATH + "TownHall.xml", new Vector2(2200, 2400), 1));// add structure nhà chính cho player1
            ((Structure)player1.StructureListCreated[1]).PlayerContainer = player1; // nhà chính này hiển nhiên thuộc người player1
            _managerGame.ListStructureOnMap.Add(player1.StructureListCreated[1]);// add nó vào list các structure trong manager game để quản lý                        

            player1.UnitListCreated.Add(new Black_Angel(this, GlobalDTO.SPEC_UNIT_PATH + "Black_Angel.xml", GlobalDTO.SPEC_PARTICLE_PATH + "Boiling Fury Fire.xml", new Vector2(2200, 2300), 1)); // add unit producer cho player 1
            ((Structure)player1.StructureListCreated[0]).UnitListCreated.Add(player1.UnitListCreated[0]);// producer thuộc nhà chính
            ((Unit)player1.UnitListCreated[0]).PlayerContainer = player1;// hiển nhiên nó thuộc player 1 
            ((Unit)player1.UnitListCreated[0]).StructureContainer = (Structure)player1.StructureListCreated[0]; // và do nhà chính sinh ra            
            _managerGame.ListUnitOnMap.Add(player1.UnitListCreated[0]);// add nó vào list unit của manager game mà quản lý

            player1.UnitListCreated.Add(new Producer(this, GlobalDTO.SPEC_UNIT_PATH + "Producer.xml", "", new Vector2(2200, 2300), 1)); // add unit producer cho player 1
            ((Structure)player1.StructureListCreated[1]).UnitListCreated.Add(player1.UnitListCreated[1]);// producer thuộc nhà chính
            ((Unit)player1.UnitListCreated[1]).PlayerContainer = player1;// hiển nhiên nó thuộc player 1 
            ((Unit)player1.UnitListCreated[1]).StructureContainer = (Structure)player1.StructureListCreated[1]; // và do nhà chính sinh ra            
            _managerGame.ListUnitOnMap.Add(player1.UnitListCreated[1]);// add nó vào list unit của manager game mà quản lý

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
            ///// player 2
            Player player2 = new Player(this);
            player2.Code = 2;
            player2.Color = Color.Red;
            player2.StructureListCreated.Add(new Military(this, GlobalDTO.SPEC_STRUCTURE_PATH + "Military.xml", new Vector2(4700, 1000), 2));// add structure nhà chính cho player2
            ((Structure)player2.StructureListCreated[0]).PlayerContainer = player2; // nhà chính này hiển nhiên thuộc người player2
            _managerGame.ListStructureOnMap.Add(player2.StructureListCreated[0]);// add nó vào list các structure trong manager game để quản lý

            player2.UnitListCreated.Add(new Black_Angel(this, GlobalDTO.SPEC_UNIT_PATH + "Black_Angel.xml", GlobalDTO.SPEC_PARTICLE_PATH + "Boiling Fury Fire.xml", new Vector2(4300, 2000), 2)); // add unit producer cho player 2
            ((Structure)player2.StructureListCreated[0]).UnitListCreated.Add(player1.UnitListCreated[0]);// producer thuộc nhà chính
            ((Unit)player2.UnitListCreated[0]).PlayerContainer = player2;// hiển nhiên nó thuộc player 2 
            ((Unit)player2.UnitListCreated[0]).StructureContainer = (Structure)player2.StructureListCreated[0]; // và do nhà chính sinh ra            
            _managerGame.ListUnitOnMap.Add(player2.UnitListCreated[0]);// add nó vào list unit của manager game mà quản lý
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
            GlobalDTO.currentModeGame = "Playing";
            this._thread.Abort();
        }
        #endregion
    }
}
