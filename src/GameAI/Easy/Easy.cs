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
using GameSharedObject.DTO;
using GameSharedObject.Data;
using GameSharedObject;
using GameSharedObject.Components;

namespace GameComputer
{
    public class Easy : Player, IComputer
    {
        Game _game;
        protected AIDTO _ai;
        protected string[] _actionNames;
        protected int[] _actionIds;

        protected int _delayTime = 0;
        private int _lastTickCount = System.Environment.TickCount;

        public AIDTO Ai
        {
            get { return _ai; }
            set { _ai = value; }
        }

        public Easy(Game game)
            : base(game)
        {
            this._ai = new AIDTO();
            this._game = game;
        }

        public override void Init()
        {
            AIDataReader reader = new AIDataReader();
            this._ai.Id = 1;
            this._ai = reader.Load(GlobalDTO.SPEC_AI_PATH + GlobalDTO.AI_ACTION_FILE_NAME + GlobalDTO.SPEC_EXTENSION, this._ai.Id);
            this._delayTime = this._ai.Time;
            this._actionNames = new string[this._ai.Actions.Count];
            this._actionIds = new int[100];

            int i = 0;
            int k = 0;
            foreach (KeyValuePair<String, ItemInfo> action in this._ai.Actions)
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
                int idAction = this._actionIds[ran.Next(0, 100)];
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
                    if (this.UnitListCreated.Count > 0 || this.StructureListCreated.Count > 0)
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
                    ResourceCenter resource = (ResourceCenter)GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[ran.Next(0, GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap.Count)];
                    CommandControl.ExploitResource(producerUnit, resource);
                }
                else if (nameAction == "BuyUnit")
                {
                    CommandControl.BuyUnit(this);
                }
                else if (nameAction == "BuyStructure")
                {
                    string[] namestructure = new string[this.ModelStructureList.Count];
                    int i = 0;
                    foreach (KeyValuePair<String, Sprite> s in this.ModelStructureList)
                    {
                        namestructure[i] = s.Value.Info.Name;
                        i++;
                    }
                    Structure newstructure = ((Structure)this.ModelStructureList[namestructure[ran.Next(0, namestructure.Length)]]).Clone() as Structure;
                    //// l?y list tên các unit mà structure này có kh? nang sinh ra
                    newstructure.ModelUnitList = new List<Unit>();
                    List<ItemInfo> uList = ((StructureDTO)newstructure.Info).UnitList;

                    for (int k = 0; k < uList.Count; k++)
                    {
                        Unit unit = (Unit)this.ModelUnitList[uList[k].Name];
                        if (newstructure.CurrentUpgradeInfo.Id >= int.Parse(uList[k].Value))
                        {
                            newstructure.ModelUnitList.Add(unit);
                        }
                    }
                    CommandControl.BuyBuildStructure(this, newstructure, this.UnitListCreated[ran.Next(0, this.UnitListCreated.Count)].Position);
                }
                else if (nameAction == "RollBackBuyUnit")
                {
                    CommandControl.RollBackBuyUnit(this);
                }
                else if (nameAction == "Researchtechnnology")
                {
                    CommandControl.ResearchTechnology(this);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GlobalDTO.CURRENT_MODEGAME == "Playing")
            {
                this.Play();
            }
            base.Update(gameTime);
        }
    }
}
