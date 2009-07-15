using System;
using System.Collections;
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
using System.Xml;
using GameSharedObject;
using GameSharedObject.DTO;
using GameSharedObject.Data;

namespace GameDemo1
{
    public class Computer:Player,IComputer
    {
        private AIDTO _ai;
        private string[] _actionNames;
        private int[] _actionIds;

        private int _delayTime = 0;
        private int _lastTickCount = System.Environment.TickCount;

        public AIDTO Ai
        {
            get { return _ai; }
            set { _ai = value; }
        }

        public Computer(MainGame game)
            : base(game)
        {
            this._ai = new AIDTO();
        }
        
        public void Init()
        {
            AIDataReader reader = new AIDataReader();
            this._ai.Id = 1;
            this._ai = reader.Load(GlobalDTO.SPEC_AI_PATH + GlobalDTO.ACTION_AI + GlobalDTO.SPEC_EXTENSION,this._ai.Id);
            this._delayTime = this._ai.Time;
            this._actionNames = new string[this._ai.Actions.Count];
            this._actionIds = new int[100];

            int i = 0;
            int k = 0;
            foreach(KeyValuePair<String,ItemInfo> action in this._ai.Actions)
            {
                this._actionNames[i] = action.Key;                
                for (int j = 0; j < int.Parse(action.Value.Value); j++)
                {
                    this._actionIds[k] = i;
                    k++;
                }
                i++;
            }            
        }

        public void Play()
        {
            if ((System.Environment.TickCount - this._lastTickCount) > this._delayTime * 1000)
            {
                this._lastTickCount = System.Environment.TickCount;
                Random ran = new Random(DateTime.Now.Millisecond);
                int idAction = 1;//this._actionIds[ran.Next(0, 59)];
                string nameAction = this._actionNames[idAction];
                if (nameAction == "Move")
                {
                    Sprite selectUnit = CommandControl.SelectUnit(ran.Next(0, this.UnitListCreated.Count), this);
                    if (selectUnit.CurrentStatus.Name == StatusList.MOVE.Name || selectUnit.CurrentStatus.Name == StatusList.ATTACK.Name)
                    {
                        return;
                    }
                    CommandControl.Move((Unit)selectUnit, new Point(ran.Next((int)selectUnit.Position.X - 100, (int)selectUnit.Position.X + 100), ran.Next((int)selectUnit.Position.Y - 100, (int)selectUnit.Position.Y + 100)));
                }
                else if (nameAction == "Attack")
                {
                    if (this.UnitListCreated.Count > 0)
                    {
                        Sprite selectUnit = CommandControl.SelectUnit(ran.Next(0, this.UnitListCreated.Count), this);
                        if (selectUnit is ProducerUnit || selectUnit.CurrentStatus.Name == StatusList.MOVE.Name || ((Unit)selectUnit).WhomIHit != null || selectUnit.CurrentStatus.Name == StatusList.ATTACK.Name)
                        {
                            return;
                        }
                        int i = ran.Next(1, 3);
                        Sprite sprite = null;
                        Player player = CommandControl.SelectPlayer();
                        if (i == 1)
                        {
                            if (player.UnitListCreated.Count > 0)
                            {
                                sprite = CommandControl.SelectUnit(ran.Next(0, player.UnitListCreated.Count), player);
                            }
                        }
                        else
                        {
                            if (player.StructureListCreated.Count > 0)
                            {
                                sprite = CommandControl.SelectStructure(ran.Next(0, player.StructureListCreated.Count), player);
                            }
                        }
                        if (sprite != null)
                        {
                            CommandControl.Attack((Unit)selectUnit, sprite);
                        }
                    }
                }
                else if (nameAction == "Idle")
                {
                    Sprite selectUnit = CommandControl.SelectUnit(ran.Next(0, this.UnitListCreated.Count), this);
                    CommandControl.Idle((Unit)selectUnit);
                }
                else if (nameAction == "ExploitResource")
                {
                    ProducerUnit producerUnit = null;
                    List<ProducerUnit> temp = new List<ProducerUnit>();
                    for (int i = 0; i < this.UnitListCreated.Count; i++)
                    {
                        if (this.UnitListCreated[i] is ProducerUnit && this.UnitListCreated[i].CurrentStatus.Name != StatusList.EXPLOIT.Name)
                        {
                            temp.Add((ProducerUnit)this.UnitListCreated[i]);
                        }
                    }
                    if (temp.Count == 0)
                    {
                        return;
                    }                    
                    producerUnit = temp[ran.Next(0, temp.Count)];
                    ResourceCenter resource = (ResourceCenter)GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[ran.Next(0, GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap.Count - 1)];
                    CommandControl.ExploitResource(producerUnit, resource);
                }
                else if (nameAction == "BuyUnit")
                {
                               
                }
                else if (nameAction == "BuyStructure")
                {
                    
                }
                else if (nameAction == "RollBackBuyUnit")
                {
                    Structure structure = (Structure)CommandControl.SelectStructure(ran.Next(0, this.StructureListCreated.Count), this);
                    int a = ran.Next(0,structure.ListUnitsBuying.Count);
                    int b = ran.Next(0,structure.ListUnitsBuying[a].Count);
                    Unit unit = structure.ListUnitsBuying[a][b];
                    CommandControl.RollBackBuyUnit(structure, unit);
                }
                else if (nameAction == "UpgradeStructure")
                { }
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.Play();
            base.Update(gameTime);
        }
    }
}
