using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using GameSharedObject.Components;
using GameSharedObject.DTO;

namespace GameSharedObject.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class ResearchStructure : Structure
    {
        #region Properties        
        private List<Technology> _listTechnology;
        private Technology _currentTechResearch = null;

        public Technology CurrentTechResearch
        {
            get { return _currentTechResearch; }
            set { _currentTechResearch = value; }
        }
        public List<Technology> ListTechnology
        {
            get { return _listTechnology; }
            set { _listTechnology = value; }
        }


        private int _delaytimeToResearch = 1000;
        private int _lastTimer = System.Environment.TickCount;
        #endregion

        #region Basic method  
        public ResearchStructure(Game game)
            : base(game)
        {
            this._listTechnology = new List<Technology>();
            this._currentTechResearch = null; ;            
        }
        public ResearchStructure(Game game, string pathspecificationfile, Vector2 position, int codeFaction)
            : base(game)
        {
            // TODO: Construct any child components here
            this.PercentSize = 1.0f;// sét kích thước hình vẽ theo tỷ lệ phần trăm
            this.FlagAttacked = false;// cờ bị tấn công
            this.CodeFaction = codeFaction; // mã phe                       
            this.Position = position;// for position // vị trí của structure theo hệ tọa độ của map
            this.PathSpecificationFile = pathspecificationfile;// get path to specification file // set đường dẫn tới file xml đặc tả
            this.ListUnitsBuying = new List<List<Unit>>();
            this._listTechnology = new List<Technology>();            
            // player mà nó trực thuộc
            this.PlayerContainer = null;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (this._currentTechResearch != null)
            {
                this.ResearchTechnology();
            }
            base.Update(gameTime);
        }                
        #endregion

        #region Function
        public void GetListTechnology()
        {
            for (int i = 0; i< ((StructureDTO)this.Info).TechList.Count; i++)
            {
                Technology tech = new Technology(this.Game, ((StructureDTO)this.Info).TechList[i].Name);
                this._listTechnology.Add(tech);
            }
        }

        public void ResearchTechnology()
        {
            if ((System.Environment.TickCount - this._lastTimer) > this._delaytimeToResearch)
            {
                this._lastTimer = System.Environment.TickCount;
                this._currentTechResearch.TechInfo.Upgrade.Requirements["Time"].Value = (int.Parse(this._currentTechResearch.TechInfo.Upgrade.Requirements["Time"].Value) - 1).ToString() ;
                if (int.Parse(this._currentTechResearch.TechInfo.Upgrade.Requirements["Time"].Value) <= 0)
                {
                    // đã hết thời gian yêu cầu cho lính
                    this.PlayerContainer.TechListResearch.Add(this._currentTechResearch);
                    this._currentTechResearch = null;
                }
            }
        }

        public Boolean CheckConditionToReSearch(Technology tech)
        {            
            foreach (KeyValuePair<String, ItemInfo> r in tech.TechInfo.Upgrade.Requirements)
            {                
                foreach (KeyValuePair<String, Resource> R in this.PlayerContainer.Resources)
                {
                    try
                    {
                        if (r.Value.Name == R.Value.Name)
                        {
                            if (int.Parse(r.Value.Value) > R.Value.Quantity)
                            {
                                return false;// tài nguyên ko đủ
                            }
                        }
                    }
                    catch
                    { }
                }
            }

            for (int i = 0; i < this.PlayerContainer.TechListResearch.Count; i++)
            {
                if (tech.TechInfo.Name == this.PlayerContainer.TechListResearch[i].TechInfo.Name)
                {
                    return false;
                }
            }
                return true; // tài nguyên đủ
            }

        public void DecreaseResourceToRearchTech(Technology tech)
        {
            foreach (KeyValuePair<String,ItemInfo> r in tech.TechInfo.Upgrade.Requirements)
            {                
                foreach (KeyValuePair<String, Resource> R in this.PlayerContainer.Resources)
                {
                    try
                    {
                        if (r.Value.Name == R.Value.Name)
                        {
                            this.PlayerContainer.Resources[r.Key].Quantity -= int.Parse(r.Value.Value);// giảm tài nguyên
                        }
                    }
                    catch
                    { }
                }
            }
        }        
        #endregion
    }
}