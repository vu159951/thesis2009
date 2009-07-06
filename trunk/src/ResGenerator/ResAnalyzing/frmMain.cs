using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ResAnalyzing.DTO;
using System.Xml;

namespace ResAnalyzing
{
    public partial class frmMain : Form
    {
        #region Private Members
        private List<ItemInfo> informationList;
        private List<List<ItemInfo>> requirementList;
        private List<UnitInfo> unitList;
        private ResAnalyzing.Sprite.Sprite sprite;
        private String spriteType;       
        #endregion

        #region Contructor
        public frmMain()
        {
            informationList = new List<ItemInfo>();
            requirementList = new List<List<ItemInfo>>();
            unitList = new List<UnitInfo>();            
            InitializeComponent();
        }
        #endregion

        #region Events
        private void frmMain_Load(object sender, EventArgs e)
        {

            //load Sprite type into cbo Sprite Type
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.RULE_PATH);

            foreach (XmlElement node in doc.GetElementsByTagName("SpriteType"))
            {
                foreach (XmlElement childnode in node.ChildNodes)
                {
                    cboType.Items.Add((String)childnode.GetAttribute("name"));
                }
            }
            cboType.SelectedIndex = 0;           

            this.cusDataGridViewTri1.Visible = false;
            LoadItem();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            String inputFolder = txtInputFolder.Text;
            String outputFolder = txtOutputFolder.Text;
            Boolean exportFile = chboExportImage.Checked;
            // ham chinh  
            String msg = "";
           
            try
            {
                if (inputFolder == "")
                {
                    msg = "Must have select input folder.\n";                   
                }
                if (outputFolder == "")
                {
                    msg += "Must have select output folder.";                    
                }
                if (inputFolder == "" || outputFolder == "")
                {
                    MessageBox.Show(this,msg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {                  
                    Utilities.GenXmlFile(inputFolder, outputFolder, sprite, exportFile);

                    msg = "Success in gen xml file!\n";
                    if (exportFile)
                    {
                        msg += "Success in move and rename file!";
                    }
                    MessageBox.Show( msg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
     
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            spriteType = cboType.SelectedItem.ToString();
            switch (spriteType)
            {
                case "ResourceCenter":
                    sprite = new Sprite.ResourceCenter();                 
                    informationList = sprite.InformationList;

                    this.cboPropertyType.Items.Clear();
                    this.cboPropertyType.Items.Add("Infomation");
                    cboPropertyType.SelectedIndex = 0;

                    ShowInfoBox();

                    LoadItem();
                    break;
                case "Structure":
                    sprite = new Sprite.Structure();                 
                    informationList = sprite.InformationList;
                    requirementList = sprite.RequirementList;
                    unitList = sprite.ListUnits;

                    this.cboPropertyType.Items.Clear();
                    this.cboPropertyType.Items.AddRange(new object[] {
                                                                        "Infomation",
                                                                        "Requirement",
                                                                        "ListUnits"});
                    cboPropertyType.SelectedIndex = 0;

                    ShowInfoBox();

                    LoadItem();
                    break;
                case "Terrain":
                    Config.SIMPLE_SPRITE = "Terrain";
                    sprite = new Sprite.SimpleSprite();
                    this.customDataGridView1.Clear();

                    this.cboPropertyType.Items.Clear();
                    
                    HideInfoBox();

                    LoadItem();
                    break;
                case "Particle":
                    Config.SIMPLE_SPRITE = "Particle";
                    sprite = new Sprite.SimpleSprite();
                    this.customDataGridView1.Clear();
                    
                    this.cboPropertyType.Items.Clear();

                    HideInfoBox();

                    LoadItem();
                    break;
                case "Unit":

                    sprite = new Sprite.Unit();                  
                    informationList = sprite.InformationList;
                    requirementList = sprite.RequirementList;

                    this.cboPropertyType.Items.Clear();
                    this.cboPropertyType.Items.AddRange(new object[] {
                                                                        "Infomation",
                                                                        "Requirement"});
                    cboPropertyType.SelectedIndex = 0;

                    ShowInfoBox();   
                    LoadItem();
                    break;
                default:
                    this.groupBox2.Visible = true;     
                    break;
            }
        }      
        
        private void btnInputFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtInputFolder.Text = folderBrowserDialog1.SelectedPath;
                this.txtOutputFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnOutputFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                this.txtOutputFolder.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
          
            if (cboPropertyType.SelectedIndex == 0)
            {
                informationList.Clear();
                this.customDataGridView1.Clear();
                sprite.InformationList.Clear();
                MessageBox.Show("Infomation list is cleared.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (cboPropertyType.SelectedIndex == 1)
            {             
                requirementList.Clear();
                this.cusDataGridViewTri1.Clear();
                sprite.RequirementList.Clear();
                MessageBox.Show("Requirement list is cleared.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (cboPropertyType.SelectedIndex == 2)
            {
                unitList.Clear();
                sprite.ListUnits.Clear();
                this.customDataGridView1.Clear();
                MessageBox.Show("Unit list is cleared.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No data found. Can't delete!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtInfoName.Text = "";
            txtInfoValue.Text = "";
        }

        private void btnAddInfo_Click(object sender, EventArgs e)
        {
            if (txtInfoName.Text != "" && txtInfoValue.Text != "")
            {                
                if (cboPropertyType.SelectedIndex == 0)
                {
                    ItemInfo item = new ItemInfo(txtInfoName.Text, txtInfoValue.Text);
                    if (!this.customDataGridView1.ChangeValue(item))
                        this.customDataGridView1.Add(item);                
                    this.customDataGridView1.Invalidate();
                    informationList = customDataGridView1.ItemList;
                    sprite.InformationList = informationList;
                }
                else if (cboPropertyType.SelectedIndex == 1)
                {
                    ItemInfo item = new ItemInfo(txtInfoName.Text, txtInfoValue.Text);
                    if (!this.cusDataGridViewTri1.ChangeValue(item))
                        this.cusDataGridViewTri1.Add(item);
                    this.cusDataGridViewTri1.Invalidate();
                    requirementList = cusDataGridViewTri1.ItemList;
                    sprite.RequirementList = requirementList;     
                }
                else if (cboPropertyType.SelectedIndex == 2)
                {
                    ItemInfo item = new ItemInfo(txtInfoName.Text, txtInfoValue.Text);
                    if (!this.customDataGridView1.ChangeValue(item))
                        this.customDataGridView1.Add(item);
                    this.customDataGridView1.Invalidate();
                    unitList = Utilities.ConvetInfoToUnit(customDataGridView1.ItemList);
                    sprite.ListUnits = unitList;
                }
            }
            else
            {
                MessageBox.Show("Please input name and value into InfoName and InfoValue!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cboPropertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItem();
        }      

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (customDataGridView1.SelectedItems.Count > 0)
                {                   
                    if (cboPropertyType.SelectedIndex == 0)
                    {
                        customDataGridView1.RemoveSelectedItem();
                        customDataGridView1.Invalidate();
                        informationList = customDataGridView1.ItemList;
                        sprite.InformationList = informationList;
                    }                   
                    else if (cboPropertyType.SelectedIndex == 2)
                    {
                        customDataGridView1.RemoveSelectedItem();
                        customDataGridView1.Invalidate();
                        unitList = Utilities.ConvetInfoToUnit(customDataGridView1.ItemList);
                        sprite.ListUnits = unitList;
                    }
                }
                else if (cusDataGridViewTri1.SelectedItems.Count > 0 && cboPropertyType.SelectedIndex == 1)
                {
                    cusDataGridViewTri1.RemoveSelectedItem();
                    cusDataGridViewTri1.Invalidate();
                    requirementList = cusDataGridViewTri1.ItemList;
                    sprite.RequirementList = requirementList;
                }
                else
                {
                    MessageBox.Show("No rows selected. Can't remove!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception) { ;}          
        }      

        private void customDataGridView1_OnSelectedItem()
        {
            this.txtInfoName.Text = customDataGridView1.SelectedItems[0].Name;
            this.txtInfoValue.Text = customDataGridView1.SelectedItems[0].Value;
        }

        private void cusDataGridViewTri1_OnSelectedItem()
        {
            this.txtInfoName.Text = cusDataGridViewTri1.SelectedItems[0].Name;
            this.txtInfoValue.Text = cusDataGridViewTri1.SelectedItems[0].Value;
        }

        #endregion

        #region Private Methods

        private void LoadItem()
        {
            if (cboPropertyType.SelectedIndex == 0)
            {              
                try
                {
                    this.cusDataGridViewTri1.Visible = false;
                    this.customDataGridView1.Visible = true;
                    this.customDataGridView1.ItemList = informationList;          
                    this.customDataGridView1.LoadItem();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                this.customDataGridView1.Invalidate();
            }
            else if (cboPropertyType.SelectedIndex == 1)
            {
                this.cusDataGridViewTri1.Visible = true;
                this.customDataGridView1.Visible = false;
                this.cusDataGridViewTri1.ItemList = requirementList;
                this.cusDataGridViewTri1.LoadItem();
                this.cusDataGridViewTri1.Invalidate();
            }
            else if (cboPropertyType.SelectedIndex == 2)
            {
                try
                {
                    this.cusDataGridViewTri1.Visible = false;
                    this.customDataGridView1.Visible = true;
                    this.customDataGridView1.ItemList = Utilities.ConvetUnitToInfo(unitList);
                    this.customDataGridView1.LoadItem();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                this.customDataGridView1.Invalidate();
            }
        }

        private void HideInfoBox()
        {
            this.groupBox2.Visible = false;
            this.btnGen.Height = 50;
            this.Height = 350;
        }

        private void ShowInfoBox()
        {
            this.groupBox2.Visible = true;
            this.btnGen.Height = 275;
            this.Height = 578;
        }
        #endregion       
    }
}