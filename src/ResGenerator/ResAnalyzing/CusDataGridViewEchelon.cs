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
    public partial class CusDataGridViewEchelon : UserControl
    {
        #region Private Members

        private List<List<ItemInfo>> _itemList;
      
        private List<ItemInfo> _selectedItems;     

        #endregion

        #region Properties

        public List<ItemInfo> SelectedItems
        {
            get { return _selectedItems; }      
        }

        public List<List<ItemInfo>> ItemList
        {
            get { return _itemList; }
            set { _itemList = value; }
        }

        #endregion

        #region Contructor

        public CusDataGridViewEchelon()
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
      
        public int Add(ItemInfo item)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            list.Add(item);
            _itemList.Add(list);
            int upgrade = _itemList.Count + 1;
            return dgvInfo.Rows.Add(upgrade.ToString(), item.Name, item.Value, item.Type);
        }             
        public void Remove(int index)
        {
             dgvInfo.Rows.RemoveAt(index);
             RemoveItem(index);
        }
        public void RemoveSelectedItem()
        {           
            for (int i = dgvInfo.SelectedRows.Count -1; i >= 0; i--)
            {
                if (!IsUpgradeNode(dgvInfo.SelectedRows[i].Index))
                {
                    _selectedItems.RemoveAt(i);
                    Remove(dgvInfo.SelectedRows[i].Index);
                }
                else
                {
                    List<ItemInfo> li = _itemList[GetUpgrade(dgvInfo.SelectedRows[i].Index)];
                    _itemList.RemoveAt(GetUpgrade(dgvInfo.SelectedRows[i].Index));
                    int j;
                    for (j = dgvInfo.SelectedRows[i].Index + li.Count; j > dgvInfo.SelectedRows[i].Index -1; j--)
                    {
                        if (dgvInfo.Rows[j].Selected)
                            i--;
                        dgvInfo.Rows.RemoveAt(j);                            
                    }                   
                }
            }    
        }
        public void LoadItem()
       {         
            try
            {
                dgvInfo.Rows.Clear();
                for (int i = 1; i <= _itemList.Count; i++)
                {
                    dgvInfo.Rows.Add(i.ToString(), "", "");
                    foreach (ItemInfo item in _itemList[i - 1])
                    {
                        dgvInfo.Rows.Add("", item.Name, item.Value);
                    }
                }        
               
            }
            catch (Exception e)
            {
                throw e;
            }            
        }
        public Boolean ChangeValue(String name, String value, String type)
        {
            return ChangeValue(new ItemInfo(name, value, type));
        }

        public Boolean ChangeValue(ItemInfo item)
        {
            int start = 0;
            for (int i = 0; i < _itemList.Count; i++)
            {
                start += 1;
                if (ChangeValue(_itemList[i], item, start))
                    return true;
                start += _itemList[i].Count;
            }
            return false;
        }

        public void Clear()
        {
            _itemList.Clear();
            dgvInfo.Rows.Clear();
        }

        #endregion

        #region Private Methods

        private int GetMaxRow(int upgradeCeil)
        {
            int result = 0;
            for (int i = 0; i <= upgradeCeil; i++)
            {
                result++;
                foreach (ItemInfo item in _itemList[i])
                {
                    result++;
                }
            }
            return result;
        }

        private void RemoveItem(int index)
        {            
            int j = -1;
            for (int i = 0; i <= _itemList.Count; i++)
            {
                j++;
                foreach (ItemInfo item in _itemList[i])
                {
                    j++;
                    if (j == index)
                    {
                        _itemList[i].Remove(item);
                        return;
                    }
                }
            }           
        }

        private Boolean IsUpgradeNode(int index)
        {
            return ((GetUpgrade(index) >= 0) && (GetChildIndex(index) == -1));
        }
      
        private int GetUpgrade(int index)
        {           
            int j = -1;
            for (int i = 0; i <= _itemList.Count; i++)
            {
                j++;
                if (j == index)
                    return i;
                if (j < index)
                {
                    foreach (ItemInfo item in _itemList[i])
                    {
                        j++;
                        if (j == index)
                        {
                            return i;
                        }                      
                    }
                }
            }
            return -1;
        }

        private int GetChildIndex(int index)
        {           
            int j = -1, k = -1;
            for (int i = 0; i <= _itemList.Count; i++)
            {
                j++;
                if (j == index)
                    return -1;
                if (j < index)
                {
                    k = -1;
                    foreach (ItemInfo item in _itemList[i])
                    {
                        j++;
                        k++;
                        if (j == index)
                        {
                            return k;
                        }
                    }
                }
            }
            return k;
        }

        private Boolean ChangeValue(List<ItemInfo> list, ItemInfo item, int start)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == item.Name)
                {
                    list[i].Value = item.Value;
                    dgvInfo.Rows[start + i].Cells[2].Value = item.Value;
                    list[i].Type = item.Type;
                    dgvInfo.Rows[start + i].Cells[3].Value = item.Type;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Override Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            dgvInfo.Width = this.Width;
            dgvInfo.Height = this.Height;
            dgvInfo.Columns[0].Width = (dgvInfo.Width - dgvInfo.RowHeadersWidth) / 7;
            dgvInfo.Columns[1].Width = 2*((dgvInfo.Width - dgvInfo.RowHeadersWidth) / 7);
            dgvInfo.Columns[2].Width = 2*((dgvInfo.Width - dgvInfo.RowHeadersWidth) / 7);
            dgvInfo.Columns[2].Width = 2 * ((dgvInfo.Width - dgvInfo.RowHeadersWidth) / 7);
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
                    if (!IsUpgradeNode(dgvInfo.SelectedRows[i].Index))
                    {
                        _selectedItems.Add(_itemList[GetUpgrade(dgvInfo.SelectedRows[i].Index)][GetChildIndex(dgvInfo.SelectedRows[i].Index)]);
                    }
                    else
                    {                       
                        for (int j = 0; j < _itemList[GetUpgrade(dgvInfo.SelectedRows[i].Index)].Count; j++)
                        {
                            _selectedItems.Add(_itemList[GetUpgrade(dgvInfo.SelectedRows[i].Index)][j]);
                        }
                    }
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
