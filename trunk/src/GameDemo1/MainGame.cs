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


            this._managerGame = new ManagerGame(this);
                /// for map 
            this._managerGame.LoadBattleField(this, Config.PATH_TO_BATTLEFIELD + "Map_1.xml");
                /// load resource center
            ResourceCenter rock = new ResourceCenter(this, "Rock", 10000, Config.PATH_TO_RESOURCECENTER_XML + "RockResourceCenter.xml", new Vector2(2500, 1600));
            ResourceCenter gold = new ResourceCenter(this, "Gold", 10000, Config.PATH_TO_RESOURCECENTER_XML + "GoldResourceCenter.xml", new Vector2(4800, 1800));
            this.Components.Add(rock);
            this.Components.Add(gold);

                /// for player
                /// player 1
            Player player1 = new Player(this);
            player1.Code = 1;
            player1.Color = Color.White;
            player1.Structures.Add(new Structure(this, Config.PATH_TO_STRUCTURE_XML + "Military.xml", new Vector2(2000, 2300),1));// add structure
            this._managerGame.ListStructureOnMap.Add(player1.Structures[0]);
            player1.Structures.Add(new Structure(this, Config.PATH_TO_STRUCTURE_XML + "Animal.xml", new Vector2(2000, 2500),1));
            this._managerGame.ListStructureOnMap.Add(player1.Structures[1]);
            player1.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Phoenix.xml", new Vector2(2700, 2100), 1));// add Unit
            this._managerGame.ListUnitOnMap.Add(player1.Units[0]);
            player1.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Phoenix.xml", new Vector2(2400, 2000), 1));
            this._managerGame.ListUnitOnMap.Add(player1.Units[1]);
            player1.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Phoenix.xml", new Vector2(2500, 2300), 1));
            this._managerGame.ListUnitOnMap.Add(player1.Units[2]);
            player1.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Angel.xml", new Vector2(2300, 2300), 1));
            this._managerGame.ListUnitOnMap.Add(player1.Units[3]);
            player1.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Angel.xml", new Vector2(2400, 2300), 1));
            this._managerGame.ListUnitOnMap.Add(player1.Units[4]);
            player1.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Angel.xml", new Vector2(2200, 2100), 1));
            this._managerGame.ListUnitOnMap.Add(player1.Units[5]);
            for (int i = 0; i < player1.Units.Count; i++)
            {
                ((Structure)player1.Structures[0]).Units.Add(player1.Units[i]);
            }
            for (int i = 0; i < player1.Units.Count; i++)// add unit and structure into game
            {
                player1.Units[i].Color = player1.Color;
                this.Components.Add(player1.Units[i]);
            }
            for (int i = 0; i < player1.Structures.Count; i++)
            {
                this.Components.Add(player1.Structures[i]);
            }
                /// player 2
            Player player2 = new Player(this);
            player2.Code = 2;
            player2.Color = Color.Red;
            player2.Structures.Add(new Structure(this, Config.PATH_TO_STRUCTURE_XML + "Military.xml", new Vector2(4000, 1300),2));// add structure
            this._managerGame.ListStructureOnMap.Add(player2.Structures[0]);
            player2.Structures.Add(new Structure(this, Config.PATH_TO_STRUCTURE_XML + "Animal.xml", new Vector2(4000, 1500),2));
            this._managerGame.ListStructureOnMap.Add(player2.Structures[1]);
            player2.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Phoenix.xml", new Vector2(4700, 1100), 2));// add Unit
            this._managerGame.ListUnitOnMap.Add(player2.Units[0]);
            player2.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Phoenix.xml", new Vector2(4400, 1000), 2));
            this._managerGame.ListUnitOnMap.Add(player2.Units[1]);
            player2.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Phoenix.xml", new Vector2(4500, 1300), 2));
            this._managerGame.ListUnitOnMap.Add(player2.Units[2]);
            player2.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Angel.xml", new Vector2(4300, 1300), 2));
            this._managerGame.ListUnitOnMap.Add(player2.Units[3]);
            player2.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Angel.xml", new Vector2(4400, 1300), 2));
            this._managerGame.ListUnitOnMap.Add(player2.Units[4]);
            player2.Units.Add(new Unit(this, Config.PATH_TO_UNIT_XML + "Angel.xml", new Vector2(4200, 1100), 2));
            this._managerGame.ListUnitOnMap.Add(player2.Units[5]);
            for (int i = 0; i < player2.Units.Count; i++)
            {
                ((Structure)player2.Structures[0]).Units.Add(player2.Units[i]);
            }
            for (int i = 0; i < player2.Units.Count; i++)// add to game
            {
                player2.Units[i].Color = player2.Color;
                this.Components.Add(player2.Units[i]);
            }
            for (int i = 0; i < player2.Structures.Count; i++)
            {
                this.Components.Add(player2.Structures[i]);
            }
            this._managerGame.Players.Add(player1);
            this._managerGame.CreateMenuItemIsStructure();
            this._managerGame.Players.Add(player2);

                // add cursor finally
            this.Components.Add(this._managerGame);
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


            // click mouse
            this._managerGame.MousePressedOnMap(this);
            this._managerGame.MouseUpOnMap(this);
            this._managerGame.MouseClickOnMenu();


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
