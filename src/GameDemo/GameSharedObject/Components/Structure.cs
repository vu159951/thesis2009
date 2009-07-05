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
        private List<Technology> _technologies; // tập các tectnology
        private List<Sprite> _onwerUnitList; // tập các unit do nó sinh ra        
        private int _delayTimeToBuild;//thời gian trì hoãn cho mỗi lần chuyển hình trong quá trình build structure
        private int _timeToBuildFinish;// thời gian để build xong structure và có th63 mua lính được
        private List<Resource> _requirementResource;// các tài nguyên yêu cầu cho việc xây dựng structure
        private Player _playerContainer;// player mà nó trực thuộc
        private UpgradeInfo _currentUpgradeInfo;
        private List<Unit> _modelUnitList;// tập tất cả các unit mà structure có thể tạo

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
        public List<Technology> Technologies
        {
            get { return _technologies; }
            set { _technologies = value; }
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

        protected int lastTickCountForChangeImage = System.Environment.TickCount;// biến đếm time cho thời gian trì hoãn

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
            this._technologies = new List<Technology>();
            this._onwerUnitList = new List<Sprite>();
        }

        public Structure(Game game, string pathspecificationfile, Vector2 position, int codeFaction)
            :base(game)
        {
            this.PercentSize = 1.0f;// sét kích thước hình vẽ theo tỷ lệ phần trăm
            this._flagAttacked = false;// cờ bị tấn công
            this.CodeFaction = codeFaction; // mã phe           
            this._technologies = new List<Technology>(); // set of techs // tập các technology
            this.Position = position;// for position // vị trí của structure theo hệ tọa độ của map
            this.PathSpecificationFile = pathspecificationfile;// get path to specification file // set đường dẫn tới file xml đặc tả
            //this.GetSetOfTexturesForSprite(pathspecificationfile);// get texture // load tập hình

            // get name of all units which it can create, 
            // time to build finish, and set delay time to build
            // max health
            // required resource
            //this.GetInformationStructure(); // lấy thông tin trong file đặc tả

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


            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here

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
            //XmlDocument doc = new XmlDocument();
            //doc.Load(this.PathSpecificationFile);
            //// get list units which can create
            //// lấy list tên các unit mà structure này có khả năng sinh ra
            this._modelUnitList = new List<UnitOfStructure>();
            for (int i = 0; i < ((StructureDTO)this.Info).UnitList.Count; i++)
            {
                UnitOfStructure item;
                if (this.CurrentUpgradeInfo.Id >= int.Parse(((StructureDTO)this.Info).UnitList[i].Value))
                {
                    item.flag = true;
                }
                else
                {
                    item.flag = false;
                }
                //item.unitOfGame = new Unit(GlobalDTO.GAME);
                //item.unitOfGame.Info = new UnitDTO();
                //item.unitOfGame.Info = GlobalDTO.UNIT_DATA_READER.Load(GlobalDTO.SPEC_UNIT_PATH + ((StructureDTO)this.Info).UnitList[i].Name + ".xml");
                //item.unitOfGame.CurrentStatus = item.unitOfGame.Info.Action[StatusList.IDLE.Name];
                //item.unitOfGame.CurrentDirection = item.unitOfGame.Info.Action[item.unitOfGame.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];
                //item.unitOfGame.GetInformationUnit();
                //this._allUnitOfStructure.Add(item);
            }

                //// get max health
                //// lấy chiều dài tối đa của máu            
                this._currentHealth = int.Parse(((StructureDTO)this.Info).InformationList["MaxHealth"].Value); // máu hiện tại

            ////calculate delay time to build this structure
            //// tính ra thời gian trì hoãn thích hợp sao cho vừa hết thời gian xây dựng cho phép cũng là lúc hình cuối cùng trong tập hình được bật lên
            //XmlNode timenode = doc.SelectSingleNode("//Time");// get time to build finish this structure
            //this._timeToBuildFinish = int.Parse(timenode.Attributes[0].Value);
            //this._delayTimeToBuild = ( this._timeToBuildFinish / this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count) * 1000; // get delay time to build this structure
            this._timeToBuildFinish = int.Parse(((StructureDTO)(this.Info)).UpgradeList[this._currentUpgradeInfo.Id].Requirements["Time"].Value);
            this._delayTimeToBuild = (this._timeToBuildFinish / this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count) * 1000;


            ////get set of resource which require to build this resource
            //// lấy các yêu cầu về tài nguyên cần thiết cho việc xây dựng structure này            
            this._requirementResource = new List<Resource>();
            foreach (KeyValuePair<string,ItemInfo> requirement in ((StructureDTO)this.Info).UpgradeList[this._currentUpgradeInfo.Id].Requirements)
            {
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
                if (this.CurrentIndex == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count)
                {
                    this.CurrentIndex = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count - 1;
                }
            }
        }
        #endregion
    }
}