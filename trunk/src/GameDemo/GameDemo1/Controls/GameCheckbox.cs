using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Frames;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GameSharedObject.Controls
{
    public class GameCheckbox: Checkbox
    {
        private string _text;
        private SpriteFont _font;
        private Color _foreColor;
        private SoundEffect sound;
        private float _volume;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
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


        public GameCheckbox(Game game)
            : base(game)
        {
            this.bgCheck = game.Content.Load<Texture2D>("Images\\Button\\Checkbox1\\Checked");
            this.bgUncheck = game.Content.Load<Texture2D>("Images\\Button\\Checkbox1\\UnChecked");
            this.Font = game.Content.Load<SpriteFont>("Images\\Button\\JXOnlineI\\Font");
            this.sound = game.Content.Load<SoundEffect>("Images\\Button\\JXOnlineI\\Stick");
            this.Background = bgUncheck;
            this.ForeColor = Color.Yellow;

            this._text = "New Checkbox";
            Vector2 size = _font.MeasureString(_text);
            int height = (int)size.Y;
            this.Size = new System.Drawing.Size((int)size.X + height + 10, height);
            this._volume = 0.5f;
        }

        protected override void OnClick(EventArgs e)
        {
            if (Setting.SOUND_ON)
                sound.Play(this._volume);
            base.OnClick(e);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 size = _font.MeasureString(_text);
            Vector2 pos = new Vector2(
                (this.Size.Width - size.X) / 2 + this.Parent.Location.X + this.Location.X + 10,
                (this.Size.Height - size.Y) / 2 + this.Parent.Location.Y + this.Location.Y);
            spriteBatch.DrawString(_font, _text, pos, _foreColor);
        }
    }
}
