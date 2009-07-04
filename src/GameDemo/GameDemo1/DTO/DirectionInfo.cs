using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDemo1.DTO
{
    public class DirectionInfo
    {
        private List<Texture2D> _image;// tập các hình trong 1 hướng
        private int _id;// mã của hướng
        private string _name;// tên của hướng

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public List<Texture2D> Image
        {
            get { return _image; }
            set { _image = value; }
        }
       
        public DirectionInfo()
        {
            this._image = new List<Texture2D>();            
        }

        public DirectionInfo(int id, string name)
        {
            this._image = new List<Texture2D>();
            this._id = id;
            this._name = name;
        }
    }
}
