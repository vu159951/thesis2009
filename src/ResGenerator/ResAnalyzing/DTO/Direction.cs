using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ResAnalyzing.DTO
{
    public class Direction
    {
        #region Private Members

        private String _name;
        private String _id;

        #endregion

        #region Properties

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }
    
        #endregion

        #region Public Methods

        public String ToXMLString()
        {
            // Kiểm tra động theo dữ liệu trong data.xml
            // Load từ data.xml lên và thực hiện kiểm tra
            // ứng với mỗi name sẽ lấy tagName ra và tạo 1 cặp thẻ
            // ví dụ: if (...) return "<S>%s%<S>"; v.v...

            String result = "";

            XmlElement node = Utilities.GetElementByAttributeValue(Name, "tagName",  "Direction");

            try
            {
                result += "<" + node.GetAttribute("tagName") + ">"
                           + "%s%"
                           + "</" + node.GetAttribute("tagName") + ">";
                Id = node.GetAttribute("id");
            }
            catch (Exception)
            {
            }
            return result;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
