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
    public class CreateTerrains : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Manager _manager;

        public CreateTerrains()
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

            /// 
            frmMain frm = new frmMain();
            frm.ShowDialog();

            ///menager obj
            this._manager = new Manager(this);

            // for map 
            this._manager.Filename = frm.Filename;
            this._manager.Mapstr = frm.map;
            this._manager.Map = new RhombusMap(this, Config.PATH_TO_MAP + this._manager.Mapstr.Replace(" ","_") + ".txt", Config.START_COORDINATE);
            Config.OccupiedMatrix = new int[Config.MAP_SIZE_IN_CELL.Width, Config.MAP_SIZE_IN_CELL.Height];

            // Initialize occupied matrix
            for (int j = 0; j < Config.MAP_SIZE_IN_CELL.Height; j++){
                for (int i = 0; i < Config.MAP_SIZE_IN_CELL.Width; i++){
                    Config.OccupiedMatrix[i, j] = 0;
                }
            }
            this.Components.Add(this._manager.Map);

            // add cursor finally
            this.Components.Add(this._manager);
            this.Components.Add(this._manager.Cursor);            
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

            ///
            /// Envents on "create terrain scence"
            ///            
            {
                this._manager.MousePressedOnMap(this);// click mouse on map
                this._manager.MouseClickOnMenu(this);// click mouse on menu
            }


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
