using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameSharedObject.Frames
{
    public abstract class Control : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        public delegate void MouseEnterHandler(object sender, MouseEventArgs e);
        public event MouseEnterHandler MouseEnter;
        public delegate void MouseLeaveHandler(object sender, MouseEventArgs e);
        public event MouseLeaveHandler MouseLeave;
        public delegate void MouseDownHandler(object sender, MouseEventArgs e);
        public event MouseDownHandler MouseDown;
        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseDownHandler MouseMove;
        public delegate void MouseUpHandler(object sender, MouseEventArgs e);
        public event MouseDownHandler MouseUp;
        public delegate void KeyDownHandler(object sender, KeyEventArgs e);
        public event KeyDownHandler KeyDown;
        public delegate void KeyUpHandler(object sender, KeyEventArgs e);
        public event KeyUpHandler KeyUp;

        protected string _name;
        protected Point _location;
        protected System.Drawing.Size _size;
        protected Texture2D _background;
        protected int _opacity;
        protected bool _isFocus;
        protected Control _parent;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public System.Drawing.Size Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }
        public int Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }
        public bool IsFocus
        {
            get { return _isFocus; }
            set { _isFocus = value; }
        }
        public Control Parent
        {
          get { return _parent; }
          set { _parent = value; }
        }

        #region Local variable
        private ButtonState[] lastMousebtnState;
        private Point lastMousePos;
        private Keys lastKeyState;
        private bool isMouseEnter;
        #endregion

        public Control(Game game)
            : base(game)
        {
            this._opacity = 100;
            this.spriteBatch = game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            this.lastMousebtnState = new ButtonState[5];
            for(int i=0; i<5; i++)
                lastMousebtnState[i] = ButtonState.Released;
            this.lastMousePos = new Point(-1, -1);
            this._isFocus = false;
            this.lastKeyState = Keys.None;
            this.isMouseEnter = false;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (!this.Game.IsActive)
                return;

            // TODO: Add your update code here
            MouseState mState = Mouse.GetState();
            KeyboardState kState = Keyboard.GetState();

            // Action for mouse event
            if (IsMouseOnControl(mState)){
                int value1 = IsAnyMouseButtonPressed(mState);
                int value2 = IsAnyMouseButtonReleased(mState);

                if (!this.isMouseEnter){
                    this.isMouseEnter = true;
                    this.OnMouseEnter(new MouseEventArgs());
                }

                if (value1 >= 0 /* exist mouse button pressed*/ && value1 <= 4){
                    lastMousebtnState[value1] = ButtonState.Pressed;
                    this.OnMouseDown(new MouseEventArgs(GetMouseButton(value1)));
                }
                if (value2 >= 0 /* exist mouse button pressed*/ && value2 <= 4){
                    lastMousebtnState[value2] = ButtonState.Released;
                    this.OnMouseUp(new MouseEventArgs(GetMouseButton(value2)));
                }
                if (mState.X != lastMousePos.X || mState.Y != lastMousePos.Y){
                    lastMousePos.X = mState.X;
                    lastMousePos.Y = mState.Y;
                    this.OnMouseMove(new MouseEventArgs(GetMouseButton(value1)));
                }
            }else if (this.isMouseEnter){
                this.isMouseEnter = false;
                this.OnMouseLeave(new MouseEventArgs());
            }

            // Action for keyboard event
            if (this._isFocus){
                // Add keys to event queue
                Keys[] keys = kState.GetPressedKeys();
                
                // Get a key from the queue => TODO
                if (keys.Length > 0){
                    Keys key = keys[0];
                    if (kState.IsKeyDown(key)){
                        lastKeyState = key;
                        this.OnKeyDown(new KeyEventArgs(key));
                    }
                }
                if (lastKeyState != Keys.None && kState.IsKeyUp(lastKeyState)){
                    this.OnKeyUp(new KeyEventArgs(lastKeyState));
                    lastKeyState = Keys.None;
                }
            }
            
            base.Update(gameTime);
        }

        protected virtual void OnMouseEnter(MouseEventArgs e)
        {
            if (MouseEnter != null)
                this.MouseEnter(this, e);
        }
        protected virtual void OnMouseLeave(MouseEventArgs e)
        {
            if (MouseLeave != null)
                this.MouseLeave(this, e);
        }
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            if (this.MouseMove != null)
                this.MouseMove(this, e);
        }
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (this.MouseDown != null)
                this.MouseDown(this, e);
        }
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            if (this.MouseUp != null)
                this.MouseUp(this, e);
        }
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            if (this.KeyDown != null)
                this.KeyDown(this, e);
        }
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            if (this.KeyUp != null)
                this.KeyUp(this, e);
        }

        protected abstract bool IsMouseOnControl(MouseState state);
        private int IsAnyMouseButtonPressed(MouseState state)
        {
            if (state.LeftButton == ButtonState.Pressed
                && lastMousebtnState[0] == ButtonState.Released)
                return 0;
            if (state.RightButton == ButtonState.Pressed
                && lastMousebtnState[1] == ButtonState.Released)
                return 1;
            if (state.MiddleButton == ButtonState.Pressed
                && lastMousebtnState[2] == ButtonState.Released)
                return 2;
            if (state.XButton1 == ButtonState.Pressed
                && lastMousebtnState[3] == ButtonState.Released)
                return 3;
            if (state.XButton2 == ButtonState.Pressed
                && lastMousebtnState[4] == ButtonState.Released)
                return 4;
            return -1;
        }
        private int IsAnyMouseButtonReleased(MouseState state)
        {
            if (state.LeftButton == ButtonState.Released
                && lastMousebtnState[0] == ButtonState.Pressed)
                return 0;
            if (state.RightButton == ButtonState.Released
                && lastMousebtnState[1] == ButtonState.Pressed)
                return 1;
            if (state.MiddleButton == ButtonState.Released
                && lastMousebtnState[2] == ButtonState.Pressed)
                return 2;
            if (state.XButton1 == ButtonState.Released
                && lastMousebtnState[3] == ButtonState.Pressed)
                return 3;
            if (state.XButton2 == ButtonState.Released
                && lastMousebtnState[4] == ButtonState.Pressed)
                return 4;
            return -1;
        }
        private MouseButtons GetMouseButton(int buttonCode)
        {
            switch (buttonCode)
            {
                case 0:
                    return MouseButtons.Left;
                case 1:
                    return MouseButtons.Right;
                case 2:
                    return MouseButtons.Middle;
                case 3:
                    return MouseButtons.XButton1;
                case 4:
                    return MouseButtons.XButton2;
                default:
                    return MouseButtons.None;
            }
        }
     }

    public class MouseEventArgs
    {
        private MouseButtons _button;
        private int _x;
        private int _y;
        private int _delta;

        public MouseButtons Button { get { return _button; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public int Delta { get { return _delta; } }
        public Point Location
        {
            get
            {
                MouseState state = Mouse.GetState();
                return new Point(state.X, state.Y);
            }
        }

        public MouseEventArgs()
        {
            MouseState state = Mouse.GetState();
            this._x = state.X;
            this._y = state.Y;
            this._delta = state.ScrollWheelValue;
        }
        public MouseEventArgs(MouseButtons btn)
        {
            MouseState state = Mouse.GetState();
            this._x = state.X;
            this._y = state.Y;
            this._delta = state.ScrollWheelValue;
            this._button = btn;
        }
    }
    public class KeyEventArgs
    {
        private bool _alt;
        private bool _control;
        private bool _shift;
        private Keys _keyCode;
        private Keys _keyData;
        private int _keyValue;

        public bool Alt { get { return _alt; } }
        public bool Control { get { return _control; } }
        public bool Shift { get { return _shift; } }
        public Keys KeyCode { get { return _keyCode; } }
        public Keys KeyData { get { return _keyData; } }
        public int KeyValue { get { return _keyValue; } }

        public KeyEventArgs()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.RightAlt) || state.IsKeyDown(Keys.LeftAlt))
                this._alt = true;
            if (state.IsKeyDown(Keys.RightControl) || state.IsKeyDown(Keys.LeftControl))
                this._control = true;
            if (state.IsKeyDown(Keys.RightShift) || state.IsKeyDown(Keys.LeftShift))
                this._shift = true;
            Keys key = state.GetPressedKeys()[0];
            if (state.IsKeyDown(key))
            {
                _keyCode = key;
                _keyData = key;
                _keyValue = (int)key;
            }
        }
        public KeyEventArgs(Keys key)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.RightAlt) || state.IsKeyDown(Keys.LeftAlt))
                this._alt = true;
            if (state.IsKeyDown(Keys.RightControl) || state.IsKeyDown(Keys.LeftControl))
                this._control = true;
            if (state.IsKeyDown(Keys.RightShift) || state.IsKeyDown(Keys.LeftShift))
                this._shift = true;
            _keyCode = key;
            _keyData = key;
            _keyValue = (int)key;
        }
    }
    public enum MouseButtons
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2,
        None
    }
    public sealed class ControlCollection
    {
        private List<Control> _list;
        public delegate void AddingHandler(object sender, Control ctl);
        public event AddingHandler Adding;
        private void OnAdding(Control ctl)
        {
            if (Adding != null)
                this.Adding(this, ctl);
        }

        public Control this[int index]
        {
            get{return _list[index];}
        }
        public int Count
        {
            get{return _list.Count;}
        }

        public ControlCollection()
        {
            _list = new List<Control>();
        }
        public void Add(Control item)
        {
            this.OnAdding(item);
           if (item.Location.X >= 0 && item.Location.X <= item.Parent.Size.Width &&
               item.Location.Y >= 0 && item.Location.Y <= item.Parent.Size.Height &&
               item.Size.Width >= 0 && item.Size.Width <= item.Parent.Size.Width - item.Location.X &&
               item.Size.Height >= 0 && item.Size.Height <= item.Parent.Size.Height - item.Location.Y)
               _list.Add(item);
            else throw new Exception("Child control(s) is out of boundary!");
        }
    }
}
