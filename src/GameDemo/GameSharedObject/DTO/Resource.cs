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


namespace GameSharedObject.DTO
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Resource
    {
        private string _name; // tên tài nguyên
        private int _quantity; // số lượng

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        // ----------------------------------------------------------------------------------
        // -------------- Methods-
        // ----------------------------------------------------------------------------------
        public Resource()
        {
            // TODO: Construct any child components here
            this._name = "";
            this._quantity = 0;
        }

        public Resource(string name, int quantity)
        {
            this._name = name;
            this._quantity = quantity;
        }
    }
}