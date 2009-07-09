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
                // tính toán vị trí đặt structure dựa vào trỏ chuột
                Vector2 tempposition = new Vector2(point.X, point.Y);

                // tạo Structure từ tên của Structure tương ứng được chọn từ menu bottom
                string name = structure.Info.Name;
                Sprite newstructure = ((Structure)this._game.StructureMgr[name]).Clone() as Sprite;
                this.LoadUnitListToStructure(
                                        (Structure)newstructure,
                                        ((Structure)newstructure).CurrentUpgradeInfo.Id);
                newstructure.Position = tempposition;
                newstructure.CodeFaction = player.Code;
                newstructure.Color = player.Color;
                //Structure newstructure = new Structure(this.Game, GlobalDTO.SPEC_STRUCTURE_PATH + this._menuItemsBottom[this._selectedIndexMenuItemBottom].Info.Name + ".xml", tempposition, this._playerIsUser.Code);
                //// change position to draw structure which have just been built
                Texture2D img = newstructure.Info.Action[newstructure.CurrentStatus.Name].DirectionInfo[newstructure.CurrentDirection.Name].Image[0];
                newstructure.Position -= new Vector2(img.Width / 2, img.Height / 2);
                ((Structure)newstructure).PlayerContainer = player; // xác định player sở hữu
                player.StructureListCreated.Add(newstructure);// thêm vào tập các structure của player mua nó
                this.AddComponentIntoGame((IGameComponent)newstructure);// đưa vào game component để nó được vẽ
                player.DecreaseResourceToBuyStructure(structure);
                GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add(newstructure);// đưa vào list structure của manager game
                //// play sound "start built"
                AudioGame au = new AudioGame(this._game);
                au.PlaySoundEffectGame("startbuild", 0.15f, 0.0f);
                au.Dispose();
            }
        }

        public void AddComponentIntoGame(IGameComponent component)
        {
            IGameComponent cursor = this.Game.Components[this.Game.Components.Count - 1];
            IGameComponent minimap = this.Game.Components[this.Game.Components.Count - 2];
            IGameComponent managergame = this.Game.Components[this.Game.Components.Count - 3];
            IGameComponent managerplayer = this.Game.Components[this.Game.Components.Count - 4];

            // remove cursor and managerplayer
            // tạm thời lấy con trỏ chuột và menu ra tập các game component
            this.Game.Components.Remove(cursor);
            this.Game.Components.Remove(minimap);
            this.Game.Components.Remove(managergame);
            this.Game.Components.Remove(managerplayer);
            // add component
            // add component muốn add vào
            this.Game.Components.Add(component);
            // add cursor and managerplayer again
            // add con trỏ chuột vào menu vào lại
            this.Game.Components.Add(managerplayer);
            this.Game.Components.Add(managergame);
            this.Game.Components.Add(minimap);
            this.Game.Components.Add(cursor);
            // add to listunit and list structure on map
            // add component vào list quản lý game
            if (component is Structure)
            {
                GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add((Structure)component);
            }
            else if (component is Unit)
            {
                GlobalDTO.MANAGER_GAME.ListUnitOnMap.Add((Unit)component);
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
    }
}
