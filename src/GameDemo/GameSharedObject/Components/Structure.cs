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
using System.Xml;
using GameSharedObject.DTO;

namespace GameSharedObject.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public abstract class Structure : Sprite, ICloneable
    {
        #region Properties
        private Boolean _flagAttacked; // cờ bị tấn công
        private int _currentHealth; // máu hiện thời        
        private List<Sprite> _onwerUnitList; // tập các unit do nó sinh ra        
        private int _delayTimeToBuild;//thời gian trì hoãn cho mỗi lần chuyển hình trong quá trình build structure        
        private int _timeToBuildFinish;// thời gian để build xong structure và có th63 mua lính được
        private List<Resource> _requirementResource;// các tài nguyên yêu cầu cho việc xây dựng structure
        private Player _playerContainer;// player mà nó trực thuộc
        private UpgradeInfo _currentUpgradeInfo;
        private List<Unit> _modelUnitList;// tập tất cả các unit mà structure có thể tạo
        private Point _unitCenterPoint;// điểm tập trung quân lính
        private List<List<Unit>> _listUnitsBuying;// danh sách các unit đã chọn mua
        private Unit _processingBuyUnit = null;// unit đang được xử lý mua và chờ hoàn thành sau thời gian

        public Point UnitCenterPoint
        {
            get { return _unitCenterPoint; }
            set { _unitCenterPoint = value; }
        }
        public int DelayTimeToBuild
        {
            get { return _delayTimeToBuild; }
            set { _delayTimeToBuild = value; }
        }
        public List<Unit> ModelUnitList
        {
            get { return _modelUnitList; }
            set { _modelUnitList = value; }
        }
        public UpgradeInfo CurrentUpgradeInfo
        {
            get { return _currentUpgradeInfo; }
            set { _currentUpgradeInfo = value; }
        }
        public Player PlayerContainer
        {
            get { return _playerContainer; }
            set { _playerContainer = value; }
        }
        public int TimeToBuildFinish
        {
            get { return _timeToBuildFinish; }
            set { _timeToBuildFinish = value; }
        }
        public List<Resource> RequirementResources
        {
            get { return _requirementResource; }
            set { _requirementResource = value; }
        }
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }        
        public List<Sprite> OwnerUnitList
        {
            get { return _onwerUnitList; }
            set { _onwerUnitList = value; }
        }
        public Boolean FlagAttacked
        {
            get { return _flagAttacked; }
            set { _flagAttacked = value; }
        }
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


        protected int lastTickCountForChangeImage = System.Environment.TickCount;// biến đếm time cho thời gian trì hoãn
        private int _delayTimeForProcessingBuyUnit = 1000;// thời gian trì hoãn cho xử lý mua unit
        private int _lastTickCountForProcessingBuyUnit = System.Environment.TickCount;// biến đếm timer cho thời gian trì hoãn
        #endregion

        #region Basic method
        // ------------------------------------------------------------------------------------------------------
        //                          Methods
        // ------------------------------------------------------------------------------------------------------
        public Structure(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this._flagAttacked = false;
            //this._maxHealth = 100;            
            this._onwerUnitList = new List<Sprite>();
        }

        public Structure(Game game, string pathspecificationfile, Vector2 position, int codeFaction)
            :base(game)
        {
            this.PercentSize = 1.0f;// sét kích thước hình vẽ theo tỷ lệ phần trăm
            this._flagAttacked = false;// cờ bị tấn công
            this.CodeFaction = codeFaction; // mã phe                       
            this.Position = position;// for position // vị trí của structure theo hệ tọa độ của map
            this.PathSpecificationFile = pathspecificationfile;// get path to specification file // set đường dẫn tới file xml đặc tả
            this._listUnitsBuying = new List<List<Unit>>();                        

            // player mà nó trực thuộc
            this._playerContainer = null;
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
        /// Called when graphics resources need to be loaded.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: Add your load content code here

            base.LoadContent();
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Add your unload content code here

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            /// build this structure by change image
            if (this.CurrentIndex != -1 && this.CurrentIndex != this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count)
            {
                this.PerformBuild();
            }

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

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here
            // vẽ cờ trên đầu            
            Texture2D image = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image[this.CurrentIndex];
            if ((this.Position.X + image.Width >= GlobalDTO.CURRENT_COORDINATE.X) && (this.Position.Y + image.Height >= GlobalDTO.CURRENT_COORDINATE.Y))
            {
                if ((this.Position.X <= GlobalDTO.CURRENT_COORDINATE.X + Game.Window.ClientBounds.Width) && (this.Position.Y <= GlobalDTO.CURRENT_COORDINATE.Y + Game.Window.ClientBounds.Height))
                {
                    if (!GlobalDTO.MANAGER_GAME.ListStructureOnViewport.Contains(this))
                    {
                        GlobalDTO.MANAGER_GAME.ListStructureOnViewport.Add(this);
                    }                    
                }
                else
                {
                    if (GlobalDTO.MANAGER_GAME.ListStructureOnViewport.Contains(this))
                    {
                        GlobalDTO.MANAGER_GAME.ListStructureOnViewport.Remove(this);
                    }
                }
            }
            else
            {
                if (GlobalDTO.MANAGER_GAME.ListStructureOnViewport.Contains(this))
                {
                    GlobalDTO.MANAGER_GAME.ListStructureOnViewport.Remove(this);
                }
            }            

            base.Draw(gameTime);
        }

        public abstract object Clone();
        #endregion

        #region Function
        /// <summary>
        /// get name of all units that this structure can create
        /// Lấy thông tin mô tả về structure trong file xml đặc tả
        /// </summary>
        public void GetInformationStructure()
        {
            //// lấy chiều dài tối đa của máu            
            this._currentHealth = int.Parse(((StructureDTO)this.Info).InformationList["MaxHealth"].Value); // máu hiện tại
            
            //// tính ra thời gian trì hoãn thích hợp sao cho vừa hết thời gian xây dựng cho phép cũng là lúc hình cuối cùng trong tập hình được bật lên            
            this._timeToBuildFinish = int.Parse(((StructureDTO)(this.Info)).UpgradeList[this._currentUpgradeInfo.Id].Requirements["Time"].Value);
            this._delayTimeToBuild = (this._timeToBuildFinish / this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count) * 1000;
         
            //// lấy các yêu cầu về tài nguyên cần thiết cho việc xây dựng structure này            
            this._requirementResource = new List<Resource>();
            foreach (KeyValuePair<string, ItemInfo> requirement in ((StructureDTO)this.Info).UpgradeList[this._currentUpgradeInfo.Id].Requirements)
            {
                if (requirement.Value.Type == "Structure" || requirement.Value.Type == "Technology")
                {
                    continue;
                }
                Resource resource = new Resource(requirement.Value.Name, int.Parse(requirement.Value.Value));
                this._requirementResource.Add(resource);
            }           
        }

        /// <summary>
        /// change current index of set of textures to change image for action
        /// Chuyển đổi hình ảnh mô tả structure này đang trong quá trình xây dựng
        /// </summary>
        public void PerformBuild()
        {
            if ((System.Environment.TickCount - this.lastTickCountForChangeImage) > this._delayTimeToBuild)
            {
                this.lastTickCountForChangeImage = System.Environment.TickCount;
                this.CurrentIndex++;
                if (this.CurrentStatus == this.Info.Action[StatusList.DEAD.Name])
                {
                    if (this.CurrentIndex == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count)
                    {
                        this.CurrentIndex = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count - 1;
                        this.Dispose(true);
                    }
                }
                else
                {
                    if (this.CurrentIndex == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count)
                    {
                        this.CurrentIndex = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count - 1;
                    }
                }
            }
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
                    this.BuyUnit(this.ProcessingBuyUnit);//thực hiện mua unit
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

        /// <summary>
        /// Xử lý mua Unit cho Player
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="structure"></param>
        public void BuyUnit(Sprite unit)
        {
            Sprite newUnit = unit;
            Random ran = new Random(DateTime.Now.Millisecond);
            newUnit.Position = new Vector2(ran.Next(this.UnitCenterPoint.X - 100, this.UnitCenterPoint.X), ran.Next(this.UnitCenterPoint.Y - 100, this.UnitCenterPoint.Y));
            newUnit.CodeFaction = this.PlayerContainer.Code;
            newUnit.Color = this.Color;
            ((Unit)newUnit).PlayerContainer = this.PlayerContainer;// xác định player mua no
            ((Unit)newUnit).StructureContainer = this;// xác định structure đã sinh ra nó
            ((Unit)newUnit).StructureContainer.OwnerUnitList.Add(newUnit);// add nó vào tập unit của structure sinh ra nó
            this.PlayerContainer.UnitListCreated.Add(newUnit);// add vào tập unit của player mua nó
            GlobalDTO.MANAGER_GAME.ListUnitOnMap.Add(newUnit);// add vào list của manager game mà quản lý            
            GlobalDTO.MANAGER_GAME.AddComponentIntoGame(newUnit);
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
        /// Kiểm tra tài nguyên player hiện có đủ để mua unit ko
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public Boolean CheckConditionToBuyUnit(Unit unit)
        {
            for (int i = 0; i < unit.RequirementResources.Count; i++)
            {
                foreach (KeyValuePair<String, Resource> r in this.PlayerContainer.Resources)
                {
                    try
                    {
                        if (unit.RequirementResources[r.Value.Name].Name == r.Value.Name)
                        {
                            if (unit.RequirementResources[r.Value.Name].Quantity > this.PlayerContainer.Resources[r.Key].Quantity)
                            {
                                return false;// tài nguyên ko đủ
                            }
                        }
                    }
                    catch
                    { }
                }
            }
            return true; // tài nguyên đủ
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
        /// Giảm số tài nguyên hiện có sau khi đồng ý mua unit
        /// </summary>
        /// <param name="structure"></param>
        public void DecreaseResourceToBuyUnit(Unit unit)
        {
            for (int i = 0; i < unit.RequirementResources.Count; i++)
            {
                foreach (KeyValuePair<String, Resource> r in this.PlayerContainer.Resources)
                {
                    try
                    {
                        if (unit.RequirementResources[r.Value.Name].Name == r.Value.Name)
                        {
                            this.PlayerContainer.Resources[r.Key].Quantity -= unit.RequirementResources[r.Value.Name].Quantity;// giảm tài nguyên
                        }
                    }
                    catch
                    { }
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
                foreach (KeyValuePair<String, Resource> r in this.PlayerContainer.Resources)
                {
                    try
                    {
                        if (unit.RequirementResources[r.Value.Name].Name == r.Value.Name)
                        {
                            this.PlayerContainer.Resources[r.Key].Quantity += unit.RequirementResources[r.Value.Name].Quantity;// giảm tài nguyên
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