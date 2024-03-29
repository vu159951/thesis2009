using System;
using System.Collections.Generic;
using System.Text;
using ResAnalyzing.DTO;
using System.IO;
using System.Xml;

namespace ResAnalyzing.Sprite
{
    public class SpriteImage
    {
        #region Private Members

        private String _imagePath;  // đường dẫn đến tập tin hình ảnh trên ổ cứng       
        private Direction _direction;
        private Status _status;        
        private String _index;       
        private String _sindex;
        private String _imgName;

        #endregion

        #region Properties

        public String ImgName
        {
            get { return _imgName; }
            set { _imgName = value; }
        }
        public String ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }
        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public Status Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public String Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public String Sindex
        {
            get { return _sindex; }
            set { _sindex = value; }
        }     

        #endregion

        #region Contructor

        public SpriteImage(Status status, Direction direction, String index, String sindex)
        {
            _status = status;
            _direction = direction;
            _index = index;
            _sindex = sindex;
        }

        #endregion

        #region Public Methods

        public String ToXMLString()
        {
            // lấy tên tập tin từ _imagePath và gen ra 1 thẻ XML dạng như sau
            //<Image name="000_Black_Angel_IDLE.1.01"/>
            // luu y: cách đặt tên giống hàm GetFormattedImageNameList
            
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.SPEC_PATH);

            XmlElement node;
            node = (XmlElement)doc.GetElementsByTagName("Image")[0];

            String XMLString = node.OuterXml.Replace("%name%", CreateName());
          //  String XMLString = "<Image name= \"" + ruleName + "\"/>";
            return XMLString;
        }
      
        public override string ToString()
        {
            return Path.GetFileName(_imagePath);
        }
        public void MovenRenameImage(String desFolder)
        {
            String newPath = desFolder + "\\" + @"\" + CreateName() + ".png";
            FileInfo fi = new FileInfo(newPath);
            if (!fi.Exists)
            {
                File.Copy(_imagePath, newPath);
            }
        }

        public void CreateIcon(String desFolder)
        {
            String newPath = desFolder + "\\" + @"\" + "Icon" + ".png";
            FileInfo fi = new FileInfo(newPath);
            if (!fi.Exists)
            {
                File.Copy(_imagePath, newPath);
            }
        }
        
        #endregion

        #region Private Methods

        private String CreateName()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.RULE_PATH);

            XmlElement node;
            node = (XmlElement)doc.GetElementsByTagName("RuleName")[0];
            String ruleName = node.GetAttribute("name");
            ruleName = ruleName.Replace("%index%", _index);
            ruleName = ruleName.Replace("%name%", _imgName);
            ruleName = ruleName.Replace("%status%", Status.Name);
            ruleName = ruleName.Replace("%directionId%", _direction.Id);
            ruleName = ruleName.Replace("%s-index%", _sindex);
            return ruleName;
        }

        private List<String> GetStandardImageName()
        {
            // trả về tên hình ảnh đã được chuẩn hóa
            // vd:
            // 000_Black_Angel_IDLE.1.01
            // Load RuleName từ data.xmllên và tạo tên theo quy định đó
            // sử dụng kỹ thuật replace chuỗi %chuỗi thay thế% = nội dung cần chuẩn hóa
            // ví dụ: 
            // tagName đọc từ data.xml lên
            // RuleName đọc từ xml lên
            // RuleName = RuleName.Replace("%index%", _index);
            // RuleName = RuleName.Replace("%s-index%", _sindex);
            // v.v...           

            List<String> ls = new List<string>();

            return ls;
        }

        #endregion
    }
}
