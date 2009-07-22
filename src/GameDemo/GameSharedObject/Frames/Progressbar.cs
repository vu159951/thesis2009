using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Frames
{
    public class Progressbar: Control
    {
        private Color _foreColor;
        private int _value;
        private bool _autoIncrease;
        private String text;
        private int lastTickCount;
        
        public delegate void ValueChangedHandler(object sender, int value);
        public event ValueChangedHandler ValueChanged;
        protected void OnValueChanged(int value)
        {
            if (this.ValueChanged != null)
                this.ValueChanged(this, value);
        }
        protected SpriteFont font;
        protected Texture2D barTexture;

        public int Value
        {
            get { return _value; }
            set 
            {
                if (value < 0)
                    _value = 0;
                else if (value > 100)
                    _value = 100;
                else _value = value; 
            }
        }
        public bool AutoIncrease
        {
            get { return _autoIncrease; }
            set { _autoIncrease = value; }
        }
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        public Progressbar(Game game)
            : base(game)
        {
            this._value = 0;
            this._autoIncrease = false;
            this.text = "0%";
            lastTickCount = Environment.TickCount;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: nothing
            if (_value == 100)
                return;

            if (_autoIncrease && Environment.TickCount - lastTickCount >= 500){
                _value++;
                lastTickCount = Environment.TickCount;
            }
            this.text = String.Format("{0}%", _value);
            this.OnValueChanged(_value);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // TODO: Add your draw code here
            spriteBatch.Draw(_background,
                new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height),
                new Color(Color.White, (float)this._opacity * 0.01f));
            int gama = (int)(this.Size.Width * 0.065f);
            spriteBatch.Draw(barTexture,
                            new Rectangle(this.Location.X + gama,
                                this.Location.Y + (int)(this.Size.Height * 0.25f),
                                (int)((this.Size.Width - 2 * gama) * _value * 0.01f),
                                (int)(this.Size.Height * 0.5f)),
                            new Color(Color.White, (float)this._opacity * 0.01f));

            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(
                this.Location.X + (int)((this.Size.Width - (int)size.X)>>1),
                this.Location.Y + (int)((this.Size.Height - (int)size.Y)>>1));
            spriteBatch.DrawString(font, text, pos, _foreColor);
        }
        public virtual void Start()
        {
            _autoIncrease = true;
        }
        public void Increase()
        {
            if (_value == 100)
                return;
            else _value++;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            // TODO: nothing
        }
        protected override bool IsMouseOnControl(Microsoft.Xna.Framework.Input.MouseState state)
        {
            throw new NotImplementedException();
        }
    }
}
