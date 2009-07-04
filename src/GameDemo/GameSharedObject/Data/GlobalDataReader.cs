using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.DTO;
using System.Xml;

namespace GameSharedObject.Data
{
    public class GlobalDataReader
    {
        private XmlDocument xmlDoc;
        public GlobalDataReader()
        {
            xmlDoc = new System.Xml.XmlDocument();
        }

        public void Load(string xmlPath)
        {
            this.xmlDoc.Load(xmlPath);
            GlobalDTO.SCREEN_SIZE = new System.Drawing.Size(int.Parse(xmlDoc.SelectSingleNode("SCREEN_SIZE").ChildNodes[0].InnerText), int.Parse(xmlDoc.SelectSingleNode("SCREEN_SIZE").ChildNodes[1].InnerText));
            GlobalDTO.MENU_PANEL_BOTTOM_SIZE = new System.Drawing.Size(int.Parse(xmlDoc.SelectSingleNode("MENU_PANEL_BOTTOM_SIZE").ChildNodes[0].InnerText),int.Parse(xmlDoc.SelectSingleNode("MENU_PANEL_BOTTOM_SIZE").ChildNodes[1].InnerText));
            GlobalDTO.MENU_PANEL_TOP_SIZE = new System.Drawing.Size(int.Parse(xmlDoc.SelectSingleNode("MENU_PANEL_TOP_SIZE").ChildNodes[0].InnerText), int.Parse(xmlDoc.SelectSingleNode("MENU_PANEL_TOP_SIZE").ChildNodes[1].InnerText));
            GlobalDTO.CURRENT_CELL_SIZE = new System.Drawing.Size(int.Parse(xmlDoc.SelectSingleNode("CURRENT_CELL_SIZE").ChildNodes[0].InnerText), int.Parse(xmlDoc.SelectSingleNode("CURRENT_CELL_SIZE").ChildNodes[1].InnerText));
            GlobalDTO.SPEED_SCROLL = new Microsoft.Xna.Framework.Vector2(int.Parse(xmlDoc.SelectSingleNode("SPEED_SCROLL").ChildNodes[0].InnerText), int.Parse(xmlDoc.SelectSingleNode("SPEED_SCROLL").ChildNodes[1].InnerText));
            GlobalDTO.MAP_SIZE_IN_CELL = new System.Drawing.Size(int.Parse(xmlDoc.SelectSingleNode("MAP_SIZE_IN_CELL").ChildNodes[0].InnerText), int.Parse(xmlDoc.SelectSingleNode("MAP_SIZE_IN_CELL").ChildNodes[1].InnerText));
            GlobalDTO.CURSOR_SIZE = new System.Drawing.Size(int.Parse(xmlDoc.SelectSingleNode("CURSOR_SIZE").ChildNodes[0].InnerText), int.Parse(xmlDoc.SelectSingleNode("CURSOR_SIZE").ChildNodes[1].InnerText));
            GlobalDTO.START_COORDINATE = new Microsoft.Xna.Framework.Vector2(int.Parse(xmlDoc.SelectSingleNode("START_COORDINATE").ChildNodes[0].InnerText), int.Parse(xmlDoc.SelectSingleNode("START_COORDINATE").ChildNodes[1].InnerText));
            GlobalDTO.CURRENT_COORDINATE = GlobalDTO.START_COORDINATE;

            GlobalDTO.SPEC_RESOURCECENTER_PATH = xmlDoc.SelectSingleNode("SPEC_RESOURCECENTER_PATH").InnerText;
            GlobalDTO.SPEC_STRUCTURE_PATH = xmlDoc.SelectSingleNode("SPEC_STRUCTURE_PATH").InnerText;
            GlobalDTO.SPEC_TERRAIN_PATH = xmlDoc.SelectSingleNode("SPEC_TERRAIN_PATH").InnerText;
            GlobalDTO.SPEC_UNIT_PATH = xmlDoc.SelectSingleNode("SPEC_UNIT_PATH").InnerText;
            GlobalDTO.SPEC_MAP_PATH = xmlDoc.SelectSingleNode("SPEC_MAP_PATH").InnerText;
            GlobalDTO.SPEC_MAP_PATH = xmlDoc.SelectSingleNode("SPEC_PARTICLE_PATH").InnerText;

            GlobalDTO.RES_CURSOR_PATH = xmlDoc.SelectSingleNode("RES_CURSOR_PATH").InnerText;
            GlobalDTO.RES_FONT_PATH = xmlDoc.SelectSingleNode("RES_FONT_PATH").InnerText;

            GlobalDTO.RES_MINI_MAP_PATH = xmlDoc.SelectSingleNode("RES_MINI_MAP_PATH").InnerText;
            GlobalDTO.RES_RHOMBUS_MAP_PATH = xmlDoc.SelectSingleNode("RES_RHOMBUS_MAP_PATH").InnerText;
            GlobalDTO.RES_SQUARE_MAP_PATH = xmlDoc.SelectSingleNode("RES_SQUARE_MAP_PATH").InnerText;

            GlobalDTO.RES_SELECTION_PATH = xmlDoc.SelectSingleNode("RES_SELECTION_PATH").InnerText;
            GlobalDTO.PATH_TO_HEALTHIMAGE = xmlDoc.SelectSingleNode("PATH_TO_HEALTHIMAGE").InnerText;
            GlobalDTO.PATH_TO_MENU_IMAGE = xmlDoc.SelectSingleNode("PATH_TO_MENU_IMAGE").InnerText;

            GlobalDTO.MINIMAP = xmlDoc.SelectSingleNode("MINIMAP").InnerText;
            GlobalDTO.CURSOR = xmlDoc.SelectSingleNode("CURSOR").InnerText;
            GlobalDTO.MENU_PANEL_BOTTOM = xmlDoc.SelectSingleNode("MENU_PANEL_BOTTOM").InnerText;
            GlobalDTO.MENU_PANEL_TOP = xmlDoc.SelectSingleNode("MENU_PANEL_TOP").InnerText;
            GlobalDTO.MAP = xmlDoc.SelectSingleNode("MAP").InnerText;
        }
    }
}