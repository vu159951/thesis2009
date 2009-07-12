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
        private Dictionary<String,Resource> _resources;// list of resources which this player have // tập các tài nguyên hiện player đang có        
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
        private int _delayTimeForProcessingBuyUnit = 1000;// thời gian trì hoãn cho xử lý mua unit
        private int _lastTickCountForProcessingBuyUnit = System.Environment.TickCount;// biến đếm timer cho thời gian trì hoãn
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
            this._resources = new Dictionary<string, Resource>();
            this._resources.Add("Stone",new Resource("Stone", 10000));
            this._resources.Add("Gold", new Resource("Gold", 10000));
                                   
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
            // xử lý mua unit
            if (this.ListUnitsBuying.Count > 0 && this.ProcessingBuyUnit == null) // có unit đang được đợi mua và chưa có unit nào đang trong quá trình xử lý mua
            {
                this.ProcessingBuyUnit = this.ListUnitsBuying[0][0];// đẩy unit đầu tiên trong danh sách vào để xử lý
            }
            if (this.ProcessingBuyUnit != null) // nếu có unit đang trong quá trình xử lý mua
            {
                // xử lý mua unit đó
                this.ProcessBuyUnitWithTime();
            }

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
                foreach (KeyValuePair<String,Resource> resource in this._resources)
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
        /// Kiểm tra tài nguyên player hiện có đủ để mua unit ko
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public Boolean CheckConditionToBuyUnit(Unit unit)
        {
            for (int i = 0; i < unit.RequirementResources.Count; i++)
            {
                foreach (KeyValuePair<String,Resource> r in this._resources)
                {
                    if (unit.RequirementResources[i].Name == r.Value.Name)
                    {
                        if (unit.RequirementResources[i].Quantity > this._resources[r.Key].Quantity)
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
                foreach (KeyValuePair<String,Resource> r in this._resources)
                {
                    if (unit.RequirementResources[i].Name == r.Value.Name)
                    {
                        this._resources[r.Key].Quantity -= unit.RequirementResources[i].Quantity;// giảm tài nguyên
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
                foreach (KeyValuePair<String,Resource> r in this._resources)
                {
                    if (unit.RequirementResources[i].Name == r.Value.Name)
                    {
                        this._resources[r.Key].Quantity += unit.RequirementResources[i].Quantity;// giảm tài nguyên
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
            //newstructure.Position = tempposition;
            newStructure.CodeFaction = this.Code;
            newStructure.Color = this.Color;
            Random ran = new Random(DateTime.Now.Millisecond);            
            // change position to draw structure which have just been built
            Texture2D img = newStructure.Info.Action[newStructure.CurrentStatus.Name].DirectionInfo[newStructure.CurrentDirection.Name].Image[0];
            newStructure.Position -= new Vector2(img.Width / 2, img.Height / 2);
            ((Structure)newStructure).UnitCenterPoint = new Point(ran.Next((int)newStructure.Position.X + 128, (int)newStructure.Position.X + 194), ran.Next((int)newStructure.Position.Y + 128, (int)newStructure.Position.Y + 194));
            ((Structure)newStructure).PlayerContainer = this; // xác định player sở hữu
            this.StructureListCreated.Add(newStructure);// thêm vào tập các structure của player mua nó            
            this.DecreaseResourceToBuyStructure((Structure)newStructure);
            GlobalDTO.MANAGER_GAME.ListStructureOnMap.Add(newStructure);// đưa vào list structure của manager game
            GlobalDTO.MANAGER_GAME.AddComponentIntoGame((IGameComponent)newStructure);// đưa vào game component để nó được vẽ
            // play sound "start built"
            AudioGame au = new AudioGame(this.Game);
            au.PlaySoundEffectGame("startbuild", 0.15f, 0.0f);
            au.Dispose();
        }

        /// <summary>
        /// Add unit muốn mua vào danh sách đợi
        /// giàm tài nguyên của player
        /// </summary>
        /// <param name="unit"></param>
        public void AddToListUnitBuying(Unit unit)
        {
            if (this.ListUnitsBuying.Count == 0) // chưa có unit nào đang mua
            {
                if (this.CheckConditionToBuyUnit(unit)) // kiểm tra tài nguyên đủ ko
                {
                    List<Unit> list1 = new List<Unit>();
                    list1.Add(unit);
                    this.DecreaseResourceToBuyUnit(unit); // giảm tài nguyên của player khi mua
                    this.ListUnitsBuying.Add(list1);
                }
                return;
            }
            for (int i = 0; i < this.ListUnitsBuying.Count; i++)// nếu trong list các unit đang mua ko rỗng ->> duyệt qua từng list unit con trong đó
            {
                if (this.ListUnitsBuying[i][0].Info.Name == unit.Info.Name) // nếu unit mún mua trước đó đã được user mua vài con rồi --> add thêm vào list của unit cùng loại
                {
                    if (this.CheckConditionToBuyUnit(unit))
                    {
                        this.ListUnitsBuying[i].Add(unit);
                        this.DecreaseResourceToBuyUnit(unit);
                    }
                    return;
                }
            }
            // nếu đây là loại unit hoàn toàn mới mà người dùng chưa mua --> thêm loại này vào
            if (this.CheckConditionToBuyUnit(unit))
            {
                List<Unit> list = new List<Unit>();
                list.Add(unit);
                this.DecreaseResourceToBuyUnit(unit);
                this.ListUnitsBuying.Add(list);
            }
            return;
        }

        /// <summary>
        /// Hủy mua 1 unit nào đó
        /// </summary>
        /// <param name="unit"></param>
        public void CancelBuyUnit(Unit unit)
        {
            for (int i = 0; i < this.ListUnitsBuying.Count; i++) // tìm trong list các unit đã chọn mua
            {
                if (this.ListUnitsBuying[i][0].Info.Name == unit.Info.Name) // tìm đúng loại unit muốn hủy
                {
                    if (this.ListUnitsBuying[i].Count == 1)// nếu hủy unit đang trong quá trình mua
                    {
                        this.ProcessingBuyUnit = null; // thì set unit đang xử lý mua về lại null
                    }
                    this.ListUnitsBuying[i].RemoveAt(this.ListUnitsBuying[i].Count - 1); // giảm số lượng mua xuống
                    // thu hồi tài nguyên cho player 
                    this.RevokeResourceFromUnit(unit);
                    if (this.ListUnitsBuying[i].Count == 0) // nếu đã hủy hết, ko mua con nào cả
                    {
                        this.ListUnitsBuying.RemoveAt(i); // xóa luôn cả list các unit cùng loại
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Xử lý mua Unit cho Player
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="structure"></param>
        public void BuyUnit(Sprite unit, Structure structure)
        {
            Sprite newUnit = unit;
            Random ran = new Random(DateTime.Now.Millisecond);
            newUnit.Position = new Vector2(ran.Next(structure.UnitCenterPoint.X - 100, structure.UnitCenterPoint.X), ran.Next(structure.UnitCenterPoint.Y - 100, structure.UnitCenterPoint.Y));
            newUnit.CodeFaction = this.Code;
            newUnit.Color = this.Color;
            ((Unit)newUnit).PlayerContainer = this;// xác định player mua no
            ((Unit)newUnit).StructureContainer = structure;// xác định structure đã sinh ra nó
            ((Unit)newUnit).StructureContainer.OwnerUnitList.Add(newUnit);// add nó vào tập unit của structure sinh ra nó
            this.UnitListCreated.Add(newUnit);// add vào tập unit của player mua nó
            GlobalDTO.MANAGER_GAME.ListUnitOnMap.Add(newUnit);// add vào list của manager game mà quản lý            
            GlobalDTO.MANAGER_GAME.AddComponentIntoGame(newUnit);
        }

        /// <summary>
        /// Xử lý quá trình mua Unit theo thời gian
        /// </summary>
        public void ProcessBuyUnitWithTime()
        {
            if ((System.Environment.TickCount - this._lastTickCountForProcessingBuyUnit) > this._delayTimeForProcessingBuyUnit)
            {
                this._lastTickCountForProcessingBuyUnit = System.Environment.TickCount;
                this.ProcessingBuyUnit.TimeToBuyFinish--;
                if (this.ProcessingBuyUnit.TimeToBuyFinish <= 0)
                {
                    // đã hết thời gian yêu cầu cho lính
                    this.BuyUnit(this.ProcessingBuyUnit, this.ProcessingBuyUnit.StructureContainer);//thực hiện mua unit
                    // xóa khỏi danh sách và chờ đợi 1 unit khác được xử lý mua;
                    this.ListUnitsBuying[0].RemoveAt(0);
                    if (this.ListUnitsBuying[0].Count == 0)
                    {
                        this.ListUnitsBuying.RemoveAt(0);
                    }                    
                    this.ProcessingBuyUnit = null;                    
                }
            }
        }
        #endregion
    }
}