using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Xml;
using GameDemo1;

namespace MapImage
{
    public partial class ImageListView : UserControl
    {
        #region Private Members

        private String _path;
       
        private List<MapCellImage> _selectedItems;
       
        #endregion

        #region Properties

        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }       
        internal List<MapCellImage> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; }
        }  

        #endregion

        #region Contructor

        public ImageListView()
        {
            _selectedItems = new List<MapCellImage>();
            InitializeComponent();
            this.listView1.HideSelection = false;
        }

        #endregion

        #region Delegates

        public delegate void Process();
        public event Process MyOnClick;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            try
            {
                //determine our valid file extensions
                string validExtensions = "*.jpg,*.jpeg,*.gif,*.png,*.bmp";

                //create a string array of our filters by plitting the
                //string of valid filters on the delimiter
                string[] extFilter = validExtensions.Split(new char[] { ',' });

                //ArrayList to hold the files with the certain extensions
                ArrayList files = new ArrayList();

                //DirectoryInfo instance to be used to get the files
                DirectoryInfo DI = new DirectoryInfo(this.Path);

                //loop through each extension in the filter
                foreach (string extension in extFilter)
                {
                    //add all the files that match our valid extensions
                    //by using AddRange of the ArrayList
                    files.AddRange(DI.GetFiles(extension));
                }

                try
                {
                    FileInfo f0 = (FileInfo)files[0];
                    Image im0 = Image.FromFile(f0.FullName);
                    imageList1.ImageSize = new Size(im0.Width, im0.Height);
                }
                catch (Exception) { }

                foreach (FileInfo f in files)
                {
                    Image im = Image.FromFile(f.FullName);
                    this.imageList1.Images.Add(im);
                    this.listView1.Items.Add(f.Name);
                    this.listView1.Items[this.listView1.Items.Count - 1].ImageIndex = this.imageList1.Images.Count - 1;
                }
                this.listView1.Items[0].Selected = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region Private Methods

        private XmlElement GetElementById(String id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Config.IMAGE_SPECIFICATION);
            foreach (XmlElement ele in doc.GetElementsByTagName("MapCellGroup"))
            {
                if (ele.GetAttribute("id") == id)
                    return ele;
            }
            return null;
        }

        #endregion

        #region Override Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            this.listView1.Width = this.Width;
            this.listView1.Height = this.Height;
            base.OnPaint(e);
        }

        #endregion
       
        #region Events

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _selectedItems.Clear();
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    XmlElement ele = GetElementById(listView1.SelectedItems[i].ImageIndex.ToString());
                    MapCellImage mImg = new MapCellImage();
                    mImg.Id = int.Parse(ele.GetAttribute("id"));
                    mImg.Name = ele.GetAttribute("name");
                    mImg.Start = int.Parse(ele.GetAttribute("start"));
                    mImg.End = int.Parse(ele.GetAttribute("end"));
                    _selectedItems.Add(mImg);
                }

                if (MyOnClick != null)
                    MyOnClick();
            }
            catch{}
        }

        #endregion                                                              
    }
}
