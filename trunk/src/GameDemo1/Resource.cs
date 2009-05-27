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


namespace GameDemo1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Resource
    {
        private string _nameSource;
        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public string NameSource
        {
            get { return _nameSource; }
            set { _nameSource = value; }
        }

        // ----------------------------------------------------------------------------------
        // -------------- Methods-
        // ----------------------------------------------------------------------------------
        public Resource()
        {
            // TODO: Construct any child components here
        }

        public Resource(string name, int quantity)
        {
            this._nameSource = name;
            this._quantity = quantity;
        }
    }
}