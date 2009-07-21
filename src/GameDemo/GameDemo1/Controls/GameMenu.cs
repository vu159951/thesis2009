﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Frames;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Controls
{
    public class GameMenu: Form
    {
        public GameMenu(Game game)
            : base(game)
        {
            this.Background = game.Content.Load<Texture2D>("Images\\Button\\Menu\\menu01");
            this.Size = new System.Drawing.Size(418, 408);
            //this.MouseDown += new Control.MouseDownHandler(frm_MouseDown);
            //this.MouseUp += new Control.MouseDownHandler(frm_MouseUp);
            //this.MouseMove += new Control.MouseDownHandler(frm_MouseMove);
            //this.KeyDown +=new Control.KeyDownHandler(frm_KeyDown);
            //this.KeyUp += new Control.KeyUpHandler(frm_KeyUp);
            //this.IsFocus = true;
        }

        public void ShowControls()
        {
            GameButton btn1 = new GameButton(this.Game);
            btn1.ForeColor = Color.Yellow;
            btn1.Text = "New campaign";
            btn1.Location = new Point(80, 65);
            btn1.Click += new Button.ClickHandler(btn1_Click);
            this.Controls.Add(btn1);

            GameButton btn1a = new GameButton(this.Game);
            btn1a.ForeColor = Color.Yellow;
            btn1a.Text = "Load";
            btn1a.Location = new Point(80, 125);            
            this.Controls.Add(btn1a);

            GameButton btn2 = new GameButton(this.Game);
            btn2.ForeColor = Color.Yellow;
            btn2.Text = "Settings";
            btn2.Location = new Point(80, 185);
            this.Controls.Add(btn2);

            GameButton btn3 = new GameButton(this.Game);
            btn3.ForeColor = Color.Yellow;
            btn3.Text = "About";
            btn3.Location = new Point(80, 245);
            this.Controls.Add(btn3);

            GameButton btn4 = new GameButton(this.Game);
            btn4.ForeColor = Color.Yellow;
            btn4.Text = "Exit";
            btn4.Location = new Point(80, 305);
            this.Controls.Add(btn4);
        }

        private void btn1_Click(object sender, MouseEventArgs e) 
        {
            GlobalFunction.MessageBox("Test function");
        }
    }
}
