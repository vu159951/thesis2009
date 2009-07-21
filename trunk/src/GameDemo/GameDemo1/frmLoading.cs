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
using System.IO;
using GameSharedObject.DTO;
using GameSharedObject.Controls;


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class frmLoading : GameScene
    {
        private GameLabel label;
        private GameProgressbar pBar;


        public String Text
        {
            get { return this.label.Text; }
            set { this.label.Text = value; }
        }
        public int Value{
            get { return pBar.Value; }
        }
        public bool AutoIncrease{
            get { return pBar.AutoIncrease;}
            set{ pBar.AutoIncrease = value;}
        }

        public delegate void ValueChangedHandler(object sender, int value);
        public event ValueChangedHandler ValueChanged;
        protected void OnValueChanged(int value)
        {
            if (ValueChanged != null)
                this.ValueChanged(this, value);
        }

        public frmLoading(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this.Background = game.Content.Load<Texture2D>("Images\\Background\\style001");
        }

        public void ShowControls()
        {
            if (pBar == null && label == null){
                pBar = new GameProgressbar(this.Game);
                pBar.Location = new Point(100, 580);
                pBar.Size = new System.Drawing.Size(800, 50);
                pBar.ValueChanged += new GameSharedObject.Frames.Progressbar.ValueChangedHandler(pBar_ValueChanged);
                label = new GameLabel(this.Game);
                label.Location = new Point(600, 500);
                label.TextChanged += new GameSharedObject.Frames.Label.TextChangedHandler(label_TextChanged);
                label.ForeColor = Color.Brown;

                this.Game.Components.Add(pBar);
                this.Game.Components.Add(label);
            }
        }
        public void Increase()
        {
            pBar.Increase();
        }
        private void pBar_ValueChanged(object sender, int value)
        {
            this.OnValueChanged(value);
        }
        private void label_TextChanged(object sender, EventArgs e)
        {
            int x = this.Game.Window.ClientBounds.Width >> 1;
            int y = this.Game.Window.ClientBounds.Height >> 1;
            Vector2 sz = this.label.Font.MeasureString(this.label.Text);
            this.label.Location = new Point(x - (int)(sz.X / 2), this.label.Location.Y);
        }
    }
}