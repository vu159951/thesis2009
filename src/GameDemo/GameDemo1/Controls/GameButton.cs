using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GameSharedObject.Controls
{
    public class GameButton: GameSharedObject.Frames.Button
    {
        public delegate void TextChangedHandler(object sender, EventArgs e);
        public event TextChangedHandler TextChanged;
        protected void OnTextChanged(EventArgs e)
        {
            if (this.TextChanged != null)
                this.TextChanged(this, e);
        }

        private String _text;
        private SpriteFont _font;
        private Color _foreColor;
        private SoundEffect soundEnter;
        private SoundEffect soundDown;
        private float _volume;

        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                this.OnTextChanged(new EventArgs());
            }
        }
        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }
        public float Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        #region Local Variable
        private Texture2D bgNormal;
        private Texture2D bgMouseEnter;
        private Texture2D bgMouseDown;
        #endregion

        public GameButton(Game game)
            : base(game)
        {
            bgNormal = Game.Content.Load<Texture2D>("Images\\Button\\JXOnlineI\\MouseLeave");
            bgMouseEnter = Game.Content.Load<Texture2D>("Images\\Button\\JXOnlineI\\MouseEnter");
            bgMouseDown = Game.Content.Load<Texture2D>("Images\\Button\\JXOnlineI\\MouseDown");
            this.Font = game.Content.Load<SpriteFont>("Images\\Button\\JXOnlineI\\Font");
            this.soundEnter = game.Content.Load<SoundEffect>("Images\\Button\\JXOnlineI\\Glass");
            this.soundDown = game.Content.Load<SoundEffect>("Images\\Button\\JXOnlineI\\Stick");
            this.Background = bgNormal;

            this.ForeColor = Color.White;
            this.Size = new System.Drawing.Size(269, 38);
            this._text = "New button";
            this._volume = 0.5f;
        }
        protected override void OnMouseDown(GameSharedObject.Frames.MouseEventArgs e)
        {
            this.Background = bgMouseDown;
            if (Setting.SOUND_ON)
                soundDown.Play(this._volume);
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(GameSharedObject.Frames.MouseEventArgs e)
        {
            this.Background = bgMouseEnter;
            base.OnMouseUp(e);
        }
        protected override void OnMouseEnter(GameSharedObject.Frames.MouseEventArgs e)
        {
            this.Background = bgMouseEnter;
            if(Setting.SOUND_ON)
                this.soundEnter.Play(this._volume);
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(GameSharedObject.Frames.MouseEventArgs e)
        {
            this.Background = bgNormal;
            base.OnMouseLeave(e);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 size = _font.MeasureString(_text);
            Vector2 pos = new Vector2(
                (this.Size.Width - size.X) / 2 + this.Parent.Location.X + this.Location.X - 2,
                (this.Size.Height - size.Y) / 2 + this.Parent.Location.Y + this.Location.Y);
            spriteBatch.DrawString(_font, _text, pos, _foreColor);            
        }
    }
}
