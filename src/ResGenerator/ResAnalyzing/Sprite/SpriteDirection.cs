using System;
using System.Collections.Generic;
using System.Text;
using ResAnalyzing.DTO;
using System.IO;
using System.Xml;

namespace ResAnalyzing.Sprite
{
    public class SpriteDirection
    {
        #region Private Members

        private String _path;               // đường dẫn đến thư mục chứa hướng       
        private List<SpriteImage> _imageList;   //        
        private Direction _direction;
        private Status _status;
        private int _startIndex;
        private String _folderName;

        #endregion

        #region Properties

        public String FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }
        public int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }
        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public Status Status
        {
            get { return _status; }
            set { _status = value; }
        }
      
        #endregion

        #region Contructor

        public SpriteDirection(Status status)
        {
            _status = status;
            _imageList = new List<SpriteImage>();
            _direction = new Direction();
        }

        #endregion

        #region Public Methods

        public void Load(String folderPath)
        {
            // ví dụ folder: _path = 'C:\\Sprite1\\IDLE\\S';
            // Đọc từ file data.xml lên, kiểm tra tên thư mục với tagName
            // để xác định nó thuộc hướng nào
            // ==> nạp vào Direction
            // Lấy danh sách file trog thư mục hướng, đẩy vào _imageList

            String[] files = Directory.GetFiles(folderPath, "*.png");
            List<String> ls = new List<String>();
            ls.AddRange(files);
            ls.Sort();                 
          
            int i = 0;
            for (i = 0; i < ls.Count; i++)
            {

                SpriteImage img = new SpriteImage(Status, _direction, Utilities.CreateIndexString(StartIndex + i, 3), Utilities.CreateIndexString(i + 1, 2));
                img.ImagePath = ls[i];
                img.ImgName = FolderName;               
                _imageList.Add(img);
            }
            StartIndex = StartIndex + i;
           
        }
        public void Load()
        {
            Load(Path);
        }
        public String ToXMLString()
        {
            // xml = _direction.ToXMLString();
            // gọi xml = xml.Replace("%s%", this.ToImageListXMLString());            
            String xml = _direction.ToXMLString(); ;
            xml = xml.Replace("%s%", ToImageListXMLString());
            return xml;
        }
        public override string ToString()
        {            
            return System.IO.Path.GetFileName(_path);
        }
        public void MovenRenameImage(String desFolder)
        {
            foreach (SpriteImage sImg in _imageList)
            {               
                sImg.MovenRenameImage(desFolder);
            }
        }

        public void CreateIcon(String desFolder)
        {
            try
            {
                _imageList[0].CreateIcon(desFolder);
            }
            catch (Exception) { }
        }  
     
        public void ClearImageList()
        {
            _imageList.Clear();
        }
            
        #endregion

        #region Private Methods

        private String ToImageListXMLString()
        {
            // oreach _imageList và thực hiện
            // xml = xml +  _imageList[i].ToXMLString();
            String xml = "";
            foreach (SpriteImage sImg in _imageList)
            {               
                xml += sImg.ToXMLString();
            }

            return xml;
        }

        #endregion
    }
}
