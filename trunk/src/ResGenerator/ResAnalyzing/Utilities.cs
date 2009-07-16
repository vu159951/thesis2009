using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ResAnalyzing.Sprite;
using ResAnalyzing.DTO;
using System.IO;

namespace ResAnalyzing
{
    static class Utilities
    {
        #region Public Methods

        /// <summary>
        /// is folderName exist in list tagName, 
        /// if(yes) : return tagName
        /// if(no)  : return ""
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        static public String GetTagName(String folderName, String tagName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.RULE_PATH);

            foreach (XmlElement node in doc.GetElementsByTagName(tagName))
            {
                foreach (XmlElement childNode in node.ChildNodes)
                {
                    if (childNode.GetAttribute("tagName") == folderName)
                        return folderName;
                }
            }
            return "";
        }
        
        /// <summary>
        /// tạo chuỗi số dạng  001 002 01 02  tùy vào len
        /// len = 2   01 02
        /// len = 3   001 002
        /// </summary>
        /// <param name="value"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        static public String CreateIndexString(int value, int len)
        {
            String result;
            result = value.ToString();
            if (result.Length < len)
                for (int i = 0; i <= len - result.Length; i++)
                {
                    result = "0" + result;
                }
            return result;
        }

        ///// <summary>
        /////  add ItemInfo into source
        ///// if ItemInfo.Name exist in source: replace value
        ///// if ItemInfo.Name !exist in source: add new node
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="info"></param>
        ///// <returns></returns>
        //static public String AddItemInfo(String source, ItemInfo info)
        //{
        //    String result = "";
        //    if (source.IndexOf(info.Name) != -1)
        //    {
        //        result = source.Substring(0, source.IndexOf(info.Name) + info.Name.Length + 1);               
        //        result += " value=\""+ info.Value + "\"" ;
        //        String temp = source.Substring(source.IndexOf(info.Name) + info.Name.Length);
        //        temp = temp.Substring(temp.IndexOf("/>"));
        //        result += temp;
        //    }
        //    else
        //    {
        //        result = source + info.ToXMLString();
        //    }
        //    return result;
        //}

        //static public void AddItemInfo(ref List<ItemInfo> source, ItemInfo info)
        //{
        //    foreach (ItemInfo item in source)
        //    {
        //        if (item.Name == info.Name)
        //            item.Value = info.Value;
        //        return;
        //    }
        //    source.Add(info);
        //}

        //static public void AddItemInfo(ref List<List<ItemInfo>> source, ItemInfo info)
        //{
        //    for (int i = 1; i <= source.Count; i++)
        //    {
        //        foreach (ItemInfo item in source[i-1])
        //        {
        //            if (item.Name == info.Name)
        //                item.Value = info.Value;
        //            return;
        //        }               
        //    }
        //    List<ItemInfo> list = new List<ItemInfo>();
        //    list.Add(info);
        //    source.Add(list);
        //}

        static public void GenXmlFile(String sourcePath, String desPath, ResAnalyzing.Sprite.Sprite sprite, Boolean exportImage)
        {
            sprite.Load(sourcePath);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sprite.ToXMLString());
            System.IO.Directory.CreateDirectory(desPath);
            doc.Save(desPath + "\\" + @"\" + System.IO.Path.GetFileName(sourcePath) + ".xml");
            if (exportImage)
            {
                sprite.MovenRenameImage(desPath);
            }           
            sprite.CreateIcon(desPath);
            sprite.ClearImageList();
            sprite.ClearStatusList();
        }

        static public XmlElement GetElementByAttributeValue(String attributeValue, String attributeName, String tagNameRange)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.RULE_PATH);
           
            foreach(XmlElement node in doc.GetElementsByTagName(tagNameRange))
            {
                foreach (XmlElement childnode in node.ChildNodes)
                {
                    if (childnode.GetAttribute(attributeName) == attributeValue)
                        return childnode;
                }                    
            }           
            return null;
        }

        static public String GenXMLByList(List<List<ItemInfo>> list)
        {
            String spec = "<Upgrade name=\"%upgradeName%\" id=\"%upgradeId%\">%listInfo%</Upgrade>";
            String temp, temp1 ="";
            String xml = "";           
            for (int i = 1; i <= list.Count; i++)
            {
                temp = spec.Replace("%upgradeName%", i.ToString());
                temp = temp.Replace("%upgradeId%", i.ToString());
                foreach(ItemInfo item in list[i-1])
                {
                    temp1 += item.ToXMLString();
                }
                temp = temp.Replace("%listInfo%", temp1);
                temp1 = "";
                xml += temp;
            }
                return xml;
        }

        static public String GenXMLByList(List<ItemInfo> list)
        {            
            String xml = "";
           
            foreach (ItemInfo item in list)
            {
                xml += item.ToXMLString();
            }          
      
            return xml;
        }

        static public String GenXMLByList(List<UnitInfo> list)
        {
            String xml = "";

            foreach (UnitInfo item in list)
            {
                xml += item.ToXMLString();
            }

            return xml;
        }

        static public List<ItemInfo> ConvetUnitToInfo(List<UnitInfo> unit)
        {
            List<ItemInfo> itemL = new List<ItemInfo>();
            foreach (UnitInfo u in unit)
            {
                itemL.Add(new ItemInfo(u.Type, u.Name, u.UpgradeId));
            }
            return itemL;
        }

        static public List<UnitInfo> ConvetInfoToUnit(List<ItemInfo> item)
        {
            List<UnitInfo> itemL = new List<UnitInfo>();
            foreach (ItemInfo it in item)
            {
                itemL.Add(new UnitInfo(it.Type, it.Name, it.Value));
            }
            return itemL;
        }

        #endregion
    }    
}
