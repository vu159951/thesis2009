using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ResAnalyzing.DTO;

namespace ResAnalyzing
{ 
    public partial class CustomDataGridView : UserControl
    {
        #region Private Members

        private List<ItemInfo> _itemList;
        private List<ItemInfo> _selectedItems;     

        #endregion

        #region Properties

        public List<ItemInfo> SelectedItems
        {
            get { return _selectedItems; }      
        }

        public List<ItemInfo> ItemList
        {
            get { return _itemList; }
            set { _itemList = value; }
        }    
  
        #endregion

        #region Contructor

        public CustomDataGridView()
        {
            _selectedItems = new List<ItemInfo>();
            InitializeComponent();
            this.dgvInfo.ScrollBars = ScrollBars.Vertical;  
        }

        #endregion

        #region Delegates

        public delegate void Process();
        public event Process OnSelectedItem;

        #endregion

        #region Public Methods

        public int Add(String name, String value)
        {
            _itemList.Add(new ItemInfo(name, value));
           return dgvInfo.Rows.Add(name, value);           
        }
        public int Add(ItemInfo item)
        {
            _itemList.Add(item);
            return dgvInfo.Rows.Add(item.Name, item.Value);
        }             
        public void Remove(int index)
        {
             dgvInfo.Rows.RemoveAt(index);
             _itemList.RemoveAt(index);
        }
        public void RemoveSelectedItem()
        {           
            for (int i = dgvInfo.SelectedRows.Count -1; i >= 0; i--)
            {
                _selectedItems.RemoveAt(i);
                Remove(dgvInfo.SelectedRows[i].Index);               
            }    
        }
        public void LoadItem()
       {         
            try
            {
                dgvInfo.Rows.Clear();               
                foreach (ItemInfo item in _itemList)
                {
                    dgvInfo.Rows.Add(item.Name, item.Value);
                }
               
            }
            catch (Exception e)
            {
                throw e;
            }            
        }
        public Boolean ChangeValue(String name, String value)
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                if (_itemList[i].Name == name)
                {
                    _itemList[i].Value = value;
                    dgvInfo.Rows[i].Cells[1].Value = value;
                    return true;
                }
            }
            return false;
        }
        public Boolean ChangeValue(ItemInfo item)
        {
            return ChangeValue(item.Name, item.Value);
        }

        public void Clear()
        {
            _itemList.Clear();
            dgvInfo.Rows.Clear();
        }

        #endregion

        #region Override Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            dgvInfo.Width = this.Width;
            dgvInfo.Height = this.Height;
            dgvInfo.Columns["colName"].Width = (dgvInfo.Width - dgvInfo.RowHeadersWidth) / 2;
            dgvInfo.Columns["colValue"].Width = (dgvInfo.Width - dgvInfo.RowHeadersWidth) / 2;
            base.OnPaint(e);
        }
        #endregion

        #region Events

        private void dgvInfo_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                _selectedItems.Clear();
                for (int i = 0; i < dgvInfo.SelectedRows.Count; i++)
                {
                    _selectedItems.Add(_itemList[dgvInfo.SelectedRows[i].Index]);
                  
                }
                if (OnSelectedItem != null)                              
                    OnSelectedItem();               
            }
            catch (Exception)
            {
                ;
            }
        }       

        #endregion                        
    }
}
