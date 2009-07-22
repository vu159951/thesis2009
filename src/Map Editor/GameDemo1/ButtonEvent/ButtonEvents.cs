using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using GameDemo1.Components;
using System.Windows.Forms;
using GameDemo1.Data;

namespace GameDemo1.ButtonEvent
{
    class ButtonEvents
    {
        public static Boolean SaveButton(Game game, string filename,string map)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("Map");
                root.SetAttribute("name", map);
                doc.AppendChild(root);
                for (int i = 0; i < game.Components.Count; i++)
                {
                    if (game.Components[i] is Terrain)
                    {
                        Terrain terrain = (Terrain)game.Components[i];
                        XmlElement terrainNode = doc.CreateElement("Terrain");
                        terrainNode.SetAttribute("name",terrain.Name.Replace(" ","_"));
                        terrainNode.SetAttribute("X",terrain.Position.X.ToString());
                        terrainNode.SetAttribute("Y", terrain.Position.Y.ToString());
                        root.AppendChild(terrainNode);
                    }
                }
                if (MessageBox.Show("Save it ? ", "Save dialog", MessageBoxButtons.OKCancel) == DialogResult.OK){
                    doc.Save(Config.PATH_SAVE_FILE + filename + ".xml");
                    MatrixMgr.Save(Config.PATH_SAVE_FILE + filename + ".oms", new GameDemo1.DTO.MatrixDTO(Config.OccupiedMatrix));
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ex = new Exception("Can't save");
                return false;
            }
        }

        public static void CloseButton()
        {
            if (MessageBox.Show("Exit ?", "Message", MessageBoxButtons.OKCancel) == DialogResult.OK){
                Application.Exit();
            }
        }

        public static void BackButton(Game game)
        {
            //game.Components.Clear();
        }
    }
}
