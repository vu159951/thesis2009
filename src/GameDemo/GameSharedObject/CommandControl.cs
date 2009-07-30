using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameSharedObject;
using GameSharedObject.DTO;
using GameSharedObject.Components;

namespace GameSharedObject
{
    public class CommandControl: Microsoft.Xna.Framework.GameComponent
    {        
        public CommandControl(Game game)
            : base(game)
        {
            
        }

        public static Sprite SelectUnit(int id, Player player)
        {
            Sprite unit = player.UnitListCreated[id];
            return unit;
        }

        public static Sprite SelectStructure(int id, Player player)
        {
            Sprite structure = player.StructureListCreated[id];
            return structure;
        }

        public static Player SelectPlayer()
        {
            Player player = null;
            Random ran = new Random(DateTime.Now.Millisecond);
            while (player == null)
            {                
                player = GlobalDTO.MANAGER_GAME.Players[ran.Next(0, GlobalDTO.MANAGER_GAME.Players.Count)];                
            }
            return player;
        }

        public static void Move(Unit unit, Point endpoint)
        {
            unit.EndPoint = endpoint;
            unit.CreateMovingVector();
        }

        public static void Attack(Unit unit1,Sprite unit2)
        {
            unit1.EndPoint = new Point((int)(unit2.Position.X + unit2.BoundRectangle.Width / 2), (int)(unit2.Position.Y + unit2.BoundRectangle.Height / 2));
            unit1.CreateMovingVector();
        }

        public static void Idle(Unit unit)
        {
            unit.MovingVector = Vector2.Zero;
            unit.EndPoint = Point.Zero;
            unit.CurrentStatus = ((UnitDTO)unit.Info).Action[StatusList.IDLE.Name];
            unit.CurrentIndex = 0;
        }
        
        public static void BuyBuildStructure(Player player, Structure structure, Vector2 point)
        {
            if (player.CheckConditionToBuyStructure(structure) == true)
            {
                Sprite newStructure = structure;                
                /// xác định các thuộc tính          
                structure.Position = point + (new Vector2(100, 100));
                newStructure.CodeFaction = player.Code;
                newStructure.Color = player.Color;
                // xác định vị trí xuất hiện
                Texture2D img = newStructure.Info.Action[newStructure.CurrentStatus.Name].DirectionInfo[newStructure.CurrentDirection.Name].Image[0];
                newStructure.Position -= new Vector2(img.Width / 2, img.Height / 2);
                // xác định điểm tập trung quân
                Random ran = new Random(DateTime.Now.Millisecond);
                ((Structure)newStructure).UnitCenterPoint = new Point(ran.Next((int)newStructure.Position.X + 128, (int)newStructure.Position.X + 194), ran.Next((int)newStructure.Position.Y + 128, (int)newStructure.Position.Y + 194));
                ((Structure)newStructure).ListUnitsBuying = new List<List<Unit>>();
                ((Structure)newStructure).PlayerContainer = player; // xác định player sở hữu            
                player.StructureListCreated.Add(newStructure);// thêm vào tập các structure của player mua nó            
                player.DecreaseResourceToBuyStructure((Structure)newStructure);// giảm tài nguyên tương ứng
                GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add(newStructure);// đưa vào list structure của manager game
                GlobalDTO.MANAGER_GAME.AddComponentIntoGame((IGameComponent)newStructure);// đưa vào game component để nó được vẽ            
                // play sound "start built"
                AudioGame au = new AudioGame(GlobalDTO.GAME);
                au.PlaySoundEffectGame("startbuild", 0.15f, 0.0f);
                au.Dispose();
                GlobalFunction.SetOccupiedCellsToMatrix(newStructure);
            }
        }                        

        public static void BuyUnit(Player player)
        {
            if (player.StructureListCreated.Count == 0)
            {
                return;
            }
            Random ran = new Random(DateTime.Now.Millisecond);
            Structure structure = (Structure)player.StructureListCreated[ran.Next(0, player.StructureListCreated.Count)];
            if (structure is ResearchStructure)
            {
                return;
            }
            Unit unit = structure.ModelUnitList[ran.Next(0, structure.ModelUnitList.Count)].Clone() as Unit;
            structure.AddToListUnitBuying(unit);
        }

        public static void RollBackBuyUnit(Player player)
        {
            Random ran = new Random(DateTime.Now.Millisecond);
            if (player.StructureListCreated.Count <= 0)
            {
                return;
            }
            Structure structure = (Structure)CommandControl.SelectStructure(ran.Next(0, player.StructureListCreated.Count), player);
            if (structure.ListUnitsBuying.Count > 0)
            {
                int a = ran.Next(0, structure.ListUnitsBuying.Count);
                if (structure.ListUnitsBuying[a].Count > 0)
                {
                    int b = ran.Next(0, structure.ListUnitsBuying[a].Count);
                    Unit unit = structure.ListUnitsBuying[a][b];
                    if (structure.ListUnitsBuying.Count > 0)
                    {
                        structure.CancelBuyUnit(unit);
                    }
                }
            }
        }

        public static void ExploitResource(ProducerUnit producerUnit, ResourceCenter resourceCenter)
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

        public static void ResearchTechnology(Player player)
        {
            if (player.StructureListCreated.Count == 0)
            {
                return;
            }
            List<int> temp = new List<int>();
            for (int i = 0; i < player.StructureListCreated.Count; i++)
            {
                if (player.StructureListCreated[i] is ResearchStructure)
                {
                    temp.Add(i);
                }
            }
            if (temp.Count == 0)
            {
                return;
            }
            Random ran = new Random(DateTime.Now.Millisecond);
            ResearchStructure rstructure = (ResearchStructure)player.StructureListCreated[temp[ran.Next(0, temp.Count)]];
            for (int i = 0; i < rstructure.ListTechnology.Count; i++)
            {
                if (rstructure.CheckConditionToReSearch(rstructure.ListTechnology[i]) == true)
                {
                    rstructure.DecreaseResourceToRearchTech(rstructure.ListTechnology[i]);
                    rstructure.CurrentTechResearch = rstructure.ListTechnology[i].Clone();
                }
            }
        }

        public static void IncreaseResource(Player player, int value, string resourceName)
        {
            player.Resources[resourceName].Quantity += value;
        }

        public static void SetResource(Player player, int value, string resourceName)
        {
            player.Resources[resourceName].Quantity = value;
        }
    }
}
