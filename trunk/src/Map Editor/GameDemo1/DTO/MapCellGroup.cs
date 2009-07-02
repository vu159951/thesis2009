using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class MapCellGroup
    {
        private String _name;
        private int _id;
        private int _startIndex;
        private int _endIndex;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }
        public int EndIndex
        {
            get { return _endIndex; }
            set { _endIndex = value; }
        }
    }

    public class MapCellGroupCollection : Dictionary<int, MapCellGroup>
    {
        private int _quantity = 0;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }
}
