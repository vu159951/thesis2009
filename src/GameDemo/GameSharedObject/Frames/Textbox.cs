using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Frames
{
    public class Textbox : Control
    {
        private SpriteFont _font;        
        private Color _foreColor;        
        private String _text;
        private int _maxLength;

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
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        #region Local varialbe
        private Keys lastKey;
        private int keepKey;
        private int isDelete;
        #endregion

        public Textbox(Game game)
            : base(game) {
            isDelete = 0;
            keepKey = 0;
            _foreColor = Color.Yellow;
            lastKey = Keys.None;
            _maxLength = 65536;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // TODO: Add your draw code here
            spriteBatch.Draw(_background,
                new Rectangle(this.Location.X + this.Parent.Location.X, this.Location.Y + this.Parent.Location.X, this.Size.Width, this.Size.Height),
                new Color(Color.White, (float)this._opacity * 0.01f));

            Vector2 size = _font.MeasureString(_text);
            Vector2 pos = new Vector2(
                this.Parent.Location.X + this.Location.X + 5,
                this.Parent.Location.Y + this.Location.Y);
            spriteBatch.DrawString(_font, _text, pos, _foreColor);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keys keyFired = e.KeyCode;
            if (keepKey == 30){
                keepKey = 0;
                lastKey = keyFired;
                if (keyFired == Keys.Back && this._text.Length > 0){
                    this._text = this._text.Remove(this._text.Length - 1);
                }else if (keyFired == Keys.Space){
                    this._text += " ";
                }else if ((e.KeyValue >= 'A' && e.KeyValue <= 'Z') || (e.KeyValue >= '0' && e.KeyValue <= '9')){
                    if (!isGreateThanSize())
                    {
                        if (e.Shift == false)
                            this._text += keyFired.ToString().ToLower();
                        else this._text += keyFired.ToString();
                    }
                }
            }else{
                if (keyFired != lastKey)
                    keepKey = 30 ;
                else keepKey++;
            }

            if (isDelete == 40 && keyFired == Keys.Back && this._text.Length > 0){
                this._text = this._text.Remove(this._text.Length - 1);
                isDelete = 30;
            }else isDelete++;
            base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == lastKey)
                keepKey = 30;
            isDelete = 0;
            base.OnKeyUp(e);
        }
        protected override bool IsMouseOnControl(MouseState state)
        {
            if (state.X >= this.Location.X + this.Parent.Location.X &&
                state.X <= this.Location.X + this.Parent.Location.X + this.Size.Width &&
                state.Y >= this.Location.Y + this.Parent.Location.Y &&
                state.Y <= this.Location.Y + this.Parent.Location.Y + this.Size.Height)
                    return true;
            return false;
        }

        private bool isGreateThanSize()
        {
            Vector2 size = _font.MeasureString(this._text);
            if (size.X + 10 > this._size.Width || _text.Length >= _maxLength)
                return true;
            return false;
        }
    }
}
