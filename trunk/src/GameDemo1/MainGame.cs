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

namespace GameDemo1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ManagerPlayer _managerPlayer;
        ManagerGame _managerGame;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Config.SCREEN_SIZE.Width;
            graphics.PreferredBackBufferHeight = Config.SCREEN_SIZE.Height;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
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

            // thông tin demo
            /// 
            /// Manager element thành phần quản lý game
            /// 
            this._managerPlayer = new ManagerPlayer(this);
            this._managerGame = new ManagerGame(this);
            /// load map từ file mô tả bao gồm cả các terrain
            this._managerGame.LoadBattleField(Config.PATH_TO_BATTLEFIELD + "Map_1.xml");
            /// load các resource center 
            ResourceCenter stone = new ResourceCenter(this, "Stone", 10000, Config.PATH_TO_RESOURCECENTER_XML + "StoneResourceCenter.xml", new Vector2(2500, 1600));
            ResourceCenter gold = new ResourceCenter(this, "Gold", 10000, Config.PATH_TO_RESOURCECENTER_XML + "GoldResourceCenter.xml", new Vector2(4800, 1800));
            this._managerGame.ListResourceCenterOnmap.Add(stone); // add resource center vào manager game để quản lý
            this._managerGame.ListResourceCenterOnmap.Add(gold);
            this.Components.Add(stone); // add vào game component để chúng được vẽ
            this.Components.Add(gold);

            /// for players tạo các player và thông tin khởi đầu
            /// player 1 có code = 1
            Player player1 = new Player(this);
            player1.Code = 1;
            player1.Color = Color.White; // màu trắng
            
            player1.Structures.Add(new Structure(this, Config.PATH_TO_STRUCTURE_XML + "TownHall.xml", new Vector2(2000, 2500), 1));// add structure nhà chính cho player1
            ((Structure)player1.Structures[0]).PlayerContainer = player1; // nhà chính này hiển nhiên thuộc người player1
            this._managerGame.ListStructureOnMap.Add(player1.Structures[0]);// add nó vào list các structure trong manager game để quản lý                        

            player1.Units.Add(new ProducerUnit(this, Config.PATH_TO_UNIT_XML + "Producer.xml", new Vector2(2200, 2300), 1)); // add unit producer cho player 1
            ((Structure)player1.Structures[0]).Units.Add(player1.Units[0]);// producer thuộc nhà chính
            ((Unit)player1.Units[0]).PlayerContainer = player1;// hiển nhiên nó thuộc player 1 
            ((Unit)player1.Units[0]).StructureContainer = (Structure)player1.Structures[0]; // và do nhà chính sinh ra            
            this._managerGame.ListUnitOnMap.Add(player1.Units[0]);// add nó vào list unit của manager game mà quản lý

            for (int i = 0; i < player1.Units.Count; i++)// add các unit và structure của player 1 vào component để vẻ chúng ra
            {
                player1.Units[i].Color = player1.Color;
                this.Components.Add(player1.Units[i]);
            }
            for (int i = 0; i < player1.Structures.Count; i++)
            {
                player1.Structures[i].Color = player1.Color;
                this.Components.Add(player1.Structures[i]);
            }
            /// player 2
            Player player2 = new Player(this);
            player2.Code = 2;
            player2.Color = Color.Red;
            player2.Structures.Add(new Structure(this, Config.PATH_TO_STRUCTURE_XML + "TownHall.xml", new Vector2(4700, 1000), 2));// add structure nhà chính cho player2
            ((Structure)player2.Structures[0]).PlayerContainer = player2; // nhà chính này hiển nhiên thuộc người player2
            this._managerGame.ListStructureOnMap.Add(player2.Structures[0]);// add nó vào list các structure trong manager game để quản lý

            player2.Units.Add(new ProducerUnit(this, Config.PATH_TO_UNIT_XML + "Producer.xml", new Vector2(4300, 2000), 2)); // add unit producer cho player 2
            ((Structure)player2.Structures[0]).Units.Add(player1.Units[0]);// producer thuộc nhà chính
            ((Unit)player2.Units[0]).PlayerContainer = player2;// hiển nhiên nó thuộc player 2 
            ((Unit)player2.Units[0]).StructureContainer = (Structure)player2.Structures[0]; // và do nhà chính sinh ra            
            this._managerGame.ListUnitOnMap.Add(player2.Units[0]);// add nó vào list unit của manager game mà quản lý
            for (int i = 0; i < player2.Units.Count; i++)// add các unit và structure của player 2 vào component
            {
                player2.Units[i].Color = player2.Color;
                this.Components.Add(player2.Units[i]);
            }
            for (int i = 0; i < player2.Structures.Count; i++)
            {
                player2.Structures[i].Color = player2.Color;
                this.Components.Add(player2.Structures[i]);
            }

            /// add manager element
            this._managerGame.Players.Add(player1);
            this._managerGame.Players.Add(player2);
            this._managerPlayer.PlayerIsUser = player1;// choose player is user // player1 chính là user và do user điều khiển từ lớp Manager Player
            this._managerPlayer.CreateMenuItemIsStructure();

            /// add cursor finally
            
            this.Components.Add(this._managerPlayer);            
            this.Components.Add(this._managerGame);
            this.Components.Add(this._managerGame.Minimap);
            this.Components.Add(this._managerGame.Cursor);
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
    }
}
