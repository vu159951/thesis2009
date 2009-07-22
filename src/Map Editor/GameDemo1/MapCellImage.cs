using System;
using System.Collections.Generic;
using System.Text;

namespace MapImage
{
    public class MapCellImage
    {
        #region Private Members

        private String _name;        
        private int _id;       
        private int _start;        
        private int _end;        

        #endregion

        #region Properties

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public int Start
        {
            get { return _start; }
            set { _start = value; }
        }
        public int End
        {
            get { return _end; }
            set { _end = value; }
        }

        #endregion

        #region Contructor

        public MapCellImage()
        {
            _name = "";
            _id = 0;
            _start = 0;
            _end = 0;
        }

        public MapCellImage(String name, int id, int start, int end)
        {
            _name = name;
            _id = id;
            _start = start;
            _end = end;
        }   

        #endregion
    }
}
