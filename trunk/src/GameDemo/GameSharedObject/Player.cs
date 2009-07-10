using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using GameSharedObject.Data;


namespace GameSharedObject
{   
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties
        private List<Player> _aliedPlayers;//list player is alied with it // tập các player đồng minh
        private List<Player> _antagonisticPlayers;//list player is antagonistic with it // tập các player đối kháng
        private int _code;// code of player // mã player
        private Color _color; // color // màu
        private List<Sprite> _unitListCreated; // list of units,are possessive units of this player // tập các unit thuộc player này
        private List<Sprite> _structuresListCreated;// list of structures, are possessive structures of this player // tập các structure thuộc player này
        private List<Resource> _resources;// list of resources which this player have // tập các tài nguyên hiện player đang có        
        private List<List<Unit>> _listUnitsBuying;// danh sách các unit đã chọn mua
        private Unit _processingBuyUnit = null;// unit đang được xử lý mua và chờ hoàn thành sau thời gian


        public Unit ProcessingBuyUnit
        {
            get { return _processingBuyUnit; }
            set { _processingBuyUnit = value; }
        }
        public List<List<Unit>> ListUnitsBuying
        {
            get { return _listUnitsBuying; }
            set { _listUnitsBuying = value; }
        }        
        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
        public List<Sprite> StructureListCreated
        {
            get { return _structuresListCreated; }
            set { _structuresListCreated = value; }
        }
        public List<Sprite> UnitListCreated
        {
            get { return _unitListCreated; }
            set { _unitListCreated = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public List<Player> AntagonisticPlayers
        {
            get { return _antagonisticPlayers; }
            set { _antagonisticPlayers = value; }
        }
        public List<Player> AliedPlayers
        {
            get { return _aliedPlayers; }
            set { _aliedPlayers = value; }
        }
        #endregion

        #region basic method
        // ----------------------------------------------------------------------------------------------
        //              Methods
        // ----------------------------------------------------------------------------------------------
        public Player(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this._aliedPlayers = new List<Player>();
            this._antagonisticPlayers = new List<Player>();
            this._code = 0;
            this._color = Color.White;
            this._structuresListCreated = new List<Sprite>();
            this._unitListCreated = new List<Sprite>();
            this._listUnitsBuying = new List<List<Unit>>();

            // resource
            // tài nguyên hiện có
            this._resources = new List<Resource>();
            this._resources.Add(new Resource("Stone", 10000));
            this._resources.Add(new Resource("Gold", 10000));           
                                   
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

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        #region Function
        /// <summary>
        /// Kiểm tra điều kiện tài nguyên để có thể mua 1 structure nào đó
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public Boolean CheckConditionToBuyStructure(Structure structure)
        {
            for (int i = 0; i < structure.RequirementResources.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (structure.RequirementResources[i].Name == this._resources[j].Name)
                    {
                        if (structure.RequirementResources[i].Quantity > this._resources[j].Quantity)
                        {
                            return false;// tài nguyên ko đủ
                        }
                    }
                }
            }
            return true; // tài nguyên đủ
        }

        /// <summary>
        /// Giảm số tài nguyên hiện có sau khi đồng ý mua structure
        /// </summary>
        /// <param name="structure"></param>
        public void DecreaseResourceToBuyStructure(Structure structure)
        {
            for (int i = 0; i < structure.RequirementResources.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (structure.RequirementResources[i].Name == this._resources[j].Name)
                    {
                        this._resources[j].Quantity -= structure.RequirementResources[i].Quantity;// giảm tài nguyên
                    }
                }
            }
        }

        /// <summary>
        /// Kiểm tra tài nguyên player hiện có đủ để mua unit ko
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public Boolean CheckConditionToBuyUnit(Unit unit)
        {
            for (int i = 0; i < unit.RequirementResources.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (unit.RequirementResources[i].Name == this._resources[j].Name)
                    {
                        if (unit.RequirementResources[i].Quantity > this._resources[j].Quantity)
                        {
                            return false;// tài nguyên ko đủ
                        }
                    }
                }
            }
            return true; // tài nguyên đủ
        }

        /// <summary>
        /// Giảm số tài nguyên hiện có sau khi đồng ý mua unit
        /// </summary>
        /// <param name="structure"></param>
        public void DecreaseResourceToBuyUnit(Unit unit)
        {
            for (int i = 0; i < unit.RequirementResources.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (unit.RequirementResources[i].Name == this._resources[j].Name)
                    {
                        this._resources[j].Quantity -= unit.RequirementResources[i].Quantity;// giảm tài nguyên
                    }
                }
            }
        }

        /// <summary>
        /// thu hồi tài nguyên từ việc hủy mua unit
        /// </summary>
        /// <param name="unit"></param>
        public void RevokeResourceFromUnit(Unit unit)
        {
            for (int i = 0; i < unit.RequirementResources.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (unit.RequirementResources[i].Name == this._resources[j].Name)
                    {
                        this._resources[j].Quantity += unit.RequirementResources[i].Quantity;// giảm tài nguyên
                    }
                }
            }
        }

        /// <summary>
        /// Thu hồi tài nguyên từ việc bán structure
        /// </summary>
        /// <param name="structure"></param>
        public void RevokeResourceFromStructure(Structure structure)
        {
 
        }               

        #endregion
    }
}