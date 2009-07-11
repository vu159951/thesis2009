using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameSharedObject;
using GameSharedObject.DTO;
using GameSharedObject.Components;

namespace GameDemo1
{
    class CommandControl: Microsoft.Xna.Framework.GameComponent
    {
        private MainGame _game;

        public CommandControl(MainGame game)
            : base(game)
        {
            this._game = game;
        }

        public Sprite SelectUnit(int id, Player player)
        {
            Sprite unit = player.UnitListCreated[id];
            return unit;
        }

        public Sprite SelectStructure(int id, Player player)
        {
            Sprite structure = player.StructureListCreated[id];
            return structure;
        }

        public void Move(Unit unit, Point endpoint)
        {
            unit.EndPoint = endpoint;
            unit.CreateMovingVector();
        }

        public void Attack(Unit unit1,Unit unit2)
        {
            unit1.EndPoint = new Point((int)(unit2.Position.X + unit2.BoundRectangle.Width / 2), (int)(unit2.Position.Y + unit2.BoundRectangle.Height / 2));
            unit1.CreateMovingVector();
        }

        public void Idle(Unit unit)
        {
            unit.MovingVector = Vector2.Zero;
            unit.EndPoint = Point.Zero;
            unit.CurrentStatus = ((UnitDTO)unit.Info).Action[StatusList.IDLE.Name];
            unit.CurrentIndex = 0;
        }


        #region buy structure
        public void BuyBuildStructure(Player player, Structure structure, Vector2 point)
        {
            if (player.CheckConditionToBuyStructure(structure) == true)
            {                
                Vector2 tempposition = new Vector2(point.X, point.Y);

                // tạo Structure từ tên của Structure tương ứng được chọn từ menu bottom
                string name = structure.Info.Name;
                Sprite newstructure = ((Structure)this._game.StructureMgr[name]).Clone() as Sprite;
                this.LoadUnitListToStructure(
                                        (Structure)newstructure,
                                        ((Structure)newstructure).CurrentUpgradeInfo.Id);
                newstructure.Position = tempposition;
                player.BuyStructure(newstructure);                
            }
        }        

        public void LoadUnitListToStructure(Structure structure, int upgradeId)
        {
            //// get list units which can create
            //// lấy list tên các unit mà structure này có khả năng sinh ra
            structure.ModelUnitList = new List<Unit>();
            List<ItemInfo> uList = ((StructureDTO)structure.Info).UnitList;

            for (int i = 0; i < uList.Count; i++)
            {
                Unit unit = (Unit)_game.UnitMgr[uList[i].Name];
                if (structure.CurrentUpgradeInfo.Id >= int.Parse(uList[i].Value))
                {
                    structure.ModelUnitList.Add(unit);
                }
            }
        }
        #endregion

        public void BuyUnit(Player player, Unit unit)
        {
            string name = unit.Info.Name;
            Unit newUnit = ((Unit)this._game.UnitMgr[name]).Clone() as Unit;                        
            player.AddToListUnitBuying(newUnit);
        }

        public void ExploitResource(ProducerUnit producerUnit, ResourceCenter resourceCenter)
        {
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap.Count; i++)
            {
                if (GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[i].Info.Name == resourceCenter.Info.Name)
                {
                    producerUnit.EndPoint = new Point((int)(resourceCenter.Position.X + resourceCenter.BoundRectangle.Width / 2), (int)(resourceCenter.Position.Y + resourceCenter.BoundRectangle.Height / 2));
                    producerUnit.CreateMovingVector();
                    return;
                }
            }
        }

        public void IncreaseResource(Player player, int value, string resourceName)
        {
            player.Resources[resourceName].Quantity += value;
        }

        public void SetResource(Player player, int value, string resourceName)
        {
            player.Resources[resourceName].Quantity = value;
        }
    }
}
