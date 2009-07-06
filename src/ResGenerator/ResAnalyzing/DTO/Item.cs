using System;
using System.Collections.Generic;
using System.Text;

namespace ResAnalyzing.DTO
{
    public abstract class Item
    {        
        #region Private Members

        protected String _name;             
 
        #endregion

        #region Properties

        public virtual String Name
        {
            get { return _name; }
            set { _name = value; }
        }           
    
        #endregion

        #region Contructors     

        #endregion

        #region Public Methods

        public virtual String ToXMLString()
        {            
            return ""; 
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
