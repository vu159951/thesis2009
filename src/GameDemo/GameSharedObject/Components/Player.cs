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


namespace GameSharedObject.Components
{   
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        #region Properties
        private List<Player> _aliedPlayers;//list player is alied with it // tập các player đồng minh
        private List<Player> _antagonisticPlayers;//list player is antagonistic with it // tập các player đối kháng
        private int _code;// code of player // mã player
        private Color _color; // color // màu
        private Texture2D _flagImage;// cờ đại diện        
        private List<Sprite> _unitListCreated; // list of units,are possessive units of this player // tập các unit thuộc player này
        private List<Sprite> _structuresListCreated;// list of structures, are possessive structures of this player // tập các structure thuộc player này
        private List<Technology> _techListResearch;// tập các technology đã nghiên cứu;        
        private Dictionary<String,Resource> _resources;// list of resources which this player have // tập các tài nguyên hiện player đang có                
        private Dictionary<String,Sprite> _modelStructureList;// tập các structure có thể mua
        private Dictionary<String, Sprite> _modelUnitList;// tập các unit có thể mua

        public Texture2D FlagImage
        {
            get { return _flagImage; }
            set { _flagImage = value; }
        }
        public Dictionary<String, Sprite> ModelStructureList
        {
            get { return _modelStructureList; }
            set { _modelStructureList = value; }
        }
        public Dictionary<String, Sprite> ModelUnitList
        {
            get { return _modelUnitList; }
            set { _modelUnitList = value; }
        }
        public List<Technology> TechListResearch
        {
            get { return _techListResearch; }
            set { _techListResearch = value; }
        }
        public Dictionary<String,Resource> Resources
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
            this._techListResearch = new List<Technology>();
            this._modelStructureList = new Dictionary<string, Sprite>();
            this._modelUnitList = new Dictionary<string, Sprite>();            

            // resource
            // tài nguyên hiện có
            this._resources = new Dictionary<string, Resource>();
            this._resources.Add("Stone",new Resource("Stone", 100000));
            this._resources.Add("Gold", new Resource("Gold", 100000));
                                   
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

        public virtual void Init() { }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here            

            base.Update(gameTime);
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
            /// kiểm tra tài nguyên xây structure
            if (this.CheckResource(structure) == false)
            {
                return false;
            }
            
            // kiểm tra công trình tiên quyết
            if (this.CheckLogicStructure(structure) == false)
            {
                return false;
            }

            // kiểm tra điều kiện technolgy tiên quyết
            if (this.CheckLogicTechnology(structure) == false)
            {
                return false;
            }

            return true; // tài nguyên đủ
        }

        /// <summary>
        ///  kiểm tra tài nguyên để xây structure
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public Boolean CheckResource(Structure structure)
        {
            for (int i = 0; i < structure.RequirementResources.Count; i++)
            {
                foreach (KeyValuePair<String, Resource> resource in this._resources)
                {
                    if (structure.RequirementResources[i].Name == resource.Value.Name)
                    {
                        if (structure.RequirementResources[i].Quantity > this._resources[resource.Key].Quantity)
                        {
                            return false;// tài nguyên ko đủ
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra điều kiện structure tiên quyết
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public Boolean CheckLogicStructure(Structure structure)
        {
            foreach (KeyValuePair<string, ItemInfo> requirement in ((StructureDTO)structure.Info).UpgradeList[structure.CurrentUpgradeInfo.Id].Requirements)
            {
                if (requirement.Value.Type != "Structure")
                { continue; }
                int i = 0;
                for (; i < this._structuresListCreated.Count; i++)
                {
                    Sprite temp = this._structuresListCreated[i];
                    if (temp.GetType().ToString().Contains(requirement.Value.Value) && (temp.CurrentIndex == temp.Info.Action[temp.CurrentStatus.Name].DirectionInfo[temp.CurrentDirection.Name].Image.Count - 1))
                    {
                        i = 0;
                        break;
                    }
                }
                if (i == this._structuresListCreated.Count)
                {
                    return false;// điều kiện công trình tiên quyết không đủ
                }
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra điều kiện Technology tiên quyết
        /// </summary>
        /// <param name="structure"></param>
        public Boolean CheckLogicTechnology(Structure structure)
        {
            foreach (KeyValuePair<string, ItemInfo> requirement in ((StructureDTO)structure.Info).UpgradeList[structure.CurrentUpgradeInfo.Id].Requirements)
            {
                if (requirement.Value.Type != "Technology")
                { continue; }
                int i = 0;
                for (; i < this._techListResearch.Count; i++)
                {
                    Technology temp = this._techListResearch[i];
                    if (temp.TechInfo.Name == requirement.Value.Value)
                    {
                        i = 0;
                        break;
                    }
                }
                if (i == this._techListResearch.Count)
                {
                    return false;// điều kiện công trình tiên quyết không đủ
                }
            }
            return true;            
        }

        /// <summary>
        /// Giảm số tài nguyên hiện có sau khi đồng ý mua structure
        /// </summary>
        /// <param name="structure"></param>
        public void DecreaseResourceToBuyStructure(Structure structure)
        {
            for (int i = 0; i < structure.RequirementResources.Count; i++)
            {
                foreach (KeyValuePair<String,Resource> r in this._resources)
                {
                    if (structure.RequirementResources[i].Name == r.Value.Name)
                    {
                        this._resources[r.Key].Quantity -= structure.RequirementResources[i].Quantity;// giảm tài nguyên
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

        /// <summary>
        /// Xử lý mua Structure
        /// </summary>
        /// <param name="structure"></param>
        public void BuyStructure(Sprite structure)
        {
            Sprite newStructure = structure;
            /// xác định các thuộc tính            
            newStructure.CodeFaction = this.Code;
            newStructure.Color = this.Color;
            // xác định vị trí xuất hiện
            Texture2D img = newStructure.Info.Action[newStructure.CurrentStatus.Name].DirectionInfo[newStructure.CurrentDirection.Name].Image[0];            
            newStructure.Position -= new Vector2(img.Width / 2, img.Height / 2);            
            // xác định điểm tập trung quân
            Random ran = new Random(DateTime.Now.Millisecond);            
            ((Structure)newStructure).UnitCenterPoint = new Point(ran.Next((int)newStructure.Position.X + 128, (int)newStructure.Position.X + 194), ran.Next((int)newStructure.Position.Y + 128, (int)newStructure.Position.Y + 194));
            ((Structure)newStructure).ListUnitsBuying = new List<List<Unit>>();
            ((Structure)newStructure).PlayerContainer = this; // xác định player sở hữu            
            this.StructureListCreated.Add(newStructure);// thêm vào tập các structure của player mua nó            
            this.DecreaseResourceToBuyStructure((Structure)newStructure);// giảm tài nguyên tương ứng
            GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add(newStructure);// đưa vào list structure của manager game
            GlobalDTO.MANAGER_GAME.AddComponentIntoGame((IGameComponent)newStructure);// đưa vào game component để nó được vẽ            
            // play sound "start built"
            AudioGame au = new AudioGame(this.Game);
            au.PlaySoundEffectGame("startbuild", 0.01f, 0.0f);
            au.Dispose();
            GlobalFunction.SetOccupiedCellsToMatrix(newStructure);
        }               
        
        #endregion
    }
}