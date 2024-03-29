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
    public abstract class Unit : Sprite, ICloneable
    {
        #region Properties
        // =====================================================================================================
        // ============================================= Properties ============================================
        // =====================================================================================================
        private Vector2 _movingVector; // moving vector for moving Npc // véctơ dịch chuyển
        private int _currentHealth;    // current health of this unit // máu hiện tại        
        private Point _endPoint;// end point to move// điêm kết thúc hiên tại khi nó đang di chuyển
        private Sprite _whomIHit = null;// current opposition which unit or structure is hitted by this unit// đối thủ mà nó đang đánh
        private Dictionary<String, Resource> _requirementResources;// list resources which require to create this unit// các tài nguyên đòi hỏi để mua unit này

        public Dictionary<String, Resource> RequirementResources
        {
            get { return _requirementResources; }
            set { _requirementResources = value; }
        }
        private Boolean _flagBeAttacked = false;// thể hiện trạng thái nó có đang bị tấn công ko
        private int _timeToBuyFinish;// thời gian để mua xong unit này
        private Structure _structureContainer;// structure tạo ra nó
        private Player _playerContainer;// player mà no trực thuộc        
        private Particle _particleAttack;

        public Particle ParticleAttack
        {
            get { return _particleAttack; }
            set { _particleAttack = value; }
        }
        public Player PlayerContainer
        {
            get { return _playerContainer; }
            set { _playerContainer = value; }
        }
        public Structure StructureContainer
        {
            get { return _structureContainer; }
            set { _structureContainer = value; }
        }
        public int TimeToBuyFinish
        {
            get { return _timeToBuyFinish; }
            set { _timeToBuyFinish = value; }
        }
        public Boolean FlagBeAttacked
        {
            get { return _flagBeAttacked; }
            set { _flagBeAttacked = value; }
        }
        //public Dictionary<String,Resource> RequirementResources
        //{
        //    get { return _requirementResources; }
        //    set { _requirementResources = value; }
        //}
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }
        public Sprite WhomIHit
        {
            get { return _whomIHit; }
            set { _whomIHit = value; }
        }
        public Point EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }
        public Vector2 MovingVector
        {
            get { return _movingVector; }
            set { _movingVector = value; }
        }

        private int _delayTimeToChangeImage = 25;// delay time to change image // thời gian trì hoãn cho mỗi lần chuyển hình trong tập hình thể hiện hành động
        private int _lastTickCountForChangeImage = System.Environment.TickCount;// counter delay time for change texture// biến đếm timer cho chuyển ảnh
        private int _delayTimeToMove = 15;// delay time to move// thời gian trì hoãn giữa lần dịch chuyển theo pixel
        private int _lastTickCountForMove = System.Environment.TickCount;// counter delay time for moving Npc// biến đếm timer cho dịch chuyển
        private int _delayTimeToDecreaseHealth_WhomIHit = 350;// delay time to decrease health // thời gian trì hoãn để giảm máu đối thủ
        private int _lastTickCountForDecreaseHealth_WhomIHit = System.Environment.TickCount;// counter delay time for decrease and die// biến đếm của nó
        private int _delayTimeToParticle = 40;// thời gian trì hoãn giữa các lần chuyển particle
        private int _lastTickCountForParticle = System.Environment.TickCount;
        #endregion

        #region Basic methods
        // =====================================================================================================
        // ============================================= basic Methods ================================================
        // =====================================================================================================
        public Unit(Game game)
            :base(game)
        {
        
        }
        public Unit(Game game, string pathspecificationfile, Vector2 position, int codeFaction)
            : base(game)
        {
            // TODO: Construct any child components here
            this._movingVector = new Vector2(0, 0);// now is IDLE
            this.CodeFaction = codeFaction;// code to tell difference between this with another Npc or Structure
            this.Position = position;// for postion on map
            this.PathSpecificationFile = pathspecificationfile;//get file for specification
            this.CurrentIndex = 0;
            //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get textures
            this.GetInformationUnit();// get information about health, power, radius attack, radius detect, requiremed resource
            this._whomIHit = null;// người bị đó tấn công
            this._playerContainer = null; // player mà nó trực thuộc
            this._structureContainer = null;// player mà nó trực thuộc
            this._particleAttack = new Particle(game, ((UnitDTO)this.Info).InformationList["AttackParticle"].Value);
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
            ////// TODO: Add your update code here
            
            /// thực hiện chuyển đổi hình ảnh để mô tả hành động
            if (this.CurrentIndex != -1)
            {
                this.PerformAction();
            }
            
            
            /// Di chuyển trên màn hình
            if (this.CurrentStatus == this.Info.Action[StatusList.MOVE.Name] && (this._movingVector != Vector2.Zero))
            {
                this.Move();
                this.CantMoveAtBorder(); // nếu chạm biên thì dừng lại
            }
            
            // phát hiện
            if (this.CurrentStatus == this.Info.Action[StatusList.IDLE.Name])
            {
                //this.SearchForDetect();
            }

            /// detect members of another faction to attack
            if (!(this is ProducerUnit)) // nếu đây ko phải là 1 producer
            {
                if (this._whomIHit == null || this.CurrentStatus == this.Info.Action[StatusList.IDLE.Name]) // if don't have opposition // tìm kiếm quân địch
                {
                    this._delayTimeToChangeImage = 25;// change speed for changing image(faster)
                    this.SearchForAttack();
                }
                else if (this._whomIHit != null) // if having opposition // nếu đã tìm ra quân địch
                {
                    this._delayTimeToChangeImage = 40; // change speed for changing image(slower)
                    this.CheckOpposition(); // kiểm tra còn trong phạm vi đánh ko
                    this.DecreaseHealthOf_WhomIHit(); // giảm máu của kẻ bị đánh
                    this.PerformParticle();
                }                
            }            

            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            if (GlobalDTO.CURRENT_MODEGAME == "Playing")
            {
                Texture2D image = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image[this.CurrentIndex];
                // vẽ hình ảnh particle nếu nó có particle và đang tấn công
                if (this._particleAttack != null && this._particleAttack.ParticleInfo.Image.Count > 0)
                {
                    //Texture2D image = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image[this.CurrentIndex];
                    if ((this.Position.X + image.Width >= GlobalDTO.CURRENT_COORDINATE.X) && (this.Position.Y + image.Height >= GlobalDTO.CURRENT_COORDINATE.Y))
                    {
                        if ((this.Position.X <= GlobalDTO.CURRENT_COORDINATE.X + Game.Window.ClientBounds.Width) && (this.Position.Y <= GlobalDTO.CURRENT_COORDINATE.Y + Game.Window.ClientBounds.Height))
                        {
                            if (this._whomIHit != null && this._particleAttack != null)
                            {
                                Texture2D particle = this._particleAttack.ParticleInfo.Image[this._particleAttack.IndexImage];
                                spriteBatch.Draw(particle, new Rectangle((int)((this._whomIHit.Position.X - GlobalDTO.CURRENT_COORDINATE.X) + this._whomIHit.BoundRectangle.Width / 4), (int)((this._whomIHit.Position.Y - GlobalDTO.CURRENT_COORDINATE.Y) + this._whomIHit.BoundRectangle.Height / 4), 64, 64), Color.White);
                            }
                        }
                    }
                }

                // vẽ cờ trên đầu            
                if ((this.Position.X + image.Width >= GlobalDTO.CURRENT_COORDINATE.X) && (this.Position.Y + image.Height >= GlobalDTO.CURRENT_COORDINATE.Y))
                {
                    if ((this.Position.X <= GlobalDTO.CURRENT_COORDINATE.X + Game.Window.ClientBounds.Width) && (this.Position.Y <= GlobalDTO.CURRENT_COORDINATE.Y + Game.Window.ClientBounds.Height))
                    {
                        if (!GlobalDTO.MANAGER_GAME.ListUnitOnViewport.Contains(this))
                        {
                            GlobalDTO.MANAGER_GAME.ListUnitOnViewport.Add(this);
                        }
                        spriteBatch.Draw(this.PlayerContainer.FlagImage, new Rectangle((int)(this.Position.X + ((this.BoundRectangle.Width - 16) / 2) - GlobalDTO.CURRENT_COORDINATE.X), (int)(this.Position.Y - GlobalDTO.CURRENT_COORDINATE.Y), 16, 16), Color.White);
                    }
                    else
                    {
                        if (GlobalDTO.MANAGER_GAME.ListUnitOnViewport.Contains(this))
                        {
                            GlobalDTO.MANAGER_GAME.ListUnitOnViewport.Remove(this);
                        }
                    }
                }
                else
                {
                    if (GlobalDTO.MANAGER_GAME.ListUnitOnViewport.Contains(this))
                    {
                        GlobalDTO.MANAGER_GAME.ListUnitOnViewport.Remove(this);
                    }
                }

            }
            base.Draw(gameTime);
        }

        public abstract object Clone();
        #endregion

        #region function
        // ------------------------------------------------------------------------------------------------------------------
        //                                              Function
        // ------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Get information about power, health, radius attack, radius detect
        /// lấy tất cả thông tin của unit này bao gồm : máu, sức tấn công, bán kính tán công, bán kính phát hiện ra đối thủ, tốc độ di chuyển của nó
        /// những yêu cầu về tài nguyên để có thể mua được unit này
        /// </summary>
        public void GetInformationUnit()
        {
            //// load file xml
            //XmlDocument doc = new XmlDocument();
            //doc.Load(this.PathSpecificationFile);
            //this._maxHealth = int.Parse(doc.SelectSingleNode("//MaxHealth[1]").Attributes[0].Value); // số máu tối đa
            this._currentHealth = int.Parse(((UnitDTO)this.Info).InformationList["MaxHealth"].Value);// máu hiện thời
            //this._power = int.Parse(doc.SelectSingleNode("//Power[1]").Attributes[0].Value);// sức mạnh
            //this._radiusAttack = int.Parse(doc.SelectSingleNode("//RadiusAttack[1]").Attributes[0].Value);// phạm vi tấn công
            //this._radiusDetect = int.Parse(doc.SelectSingleNode("//RadiusDetect[1]").Attributes[0].Value);// phạm vi phát hiện đối phương
            //this._speed = int.Parse(doc.SelectSingleNode("//Speed[1]").Attributes[0].Value);// tốc độ di chuyển
            this._timeToBuyFinish = int.Parse(((UnitDTO)this.Info).Upgrade[1].Requirements["Time"].Value);// thời gian để mua unit này

            //// lấy tập các yêu cầu để mua unit này
            this._requirementResources = new Dictionary<string,Resource>();// list resource which this structure require to build            
            foreach (KeyValuePair<string, ItemInfo> requirement in ((UnitDTO)this.Info).Upgrade[1].Requirements)
            {
                Resource resource = new Resource(requirement.Value.Name, int.Parse(requirement.Value.Value));
                this._requirementResources.Add(resource.Name,resource);
            }  
            //this.Name = doc.DocumentElement.Name;
        }

        /// Kiểm tra vật cản di chuyển
        public void CheckOccupie()
        {
            try
            {
                Vector2 position = this.Position + this._movingVector;
                Point centerpoint = new Point((int)(position.X + this.Size.Width / 2), (int)(position.Y + this.Size.Height / 2));
                Point cell = GlobalDTO.MANAGER_GAME.Map.Transform.PointToCell(centerpoint);
                if (GlobalDTO.MANAGER_GAME.Map.OccupiedMatrix[cell.X, cell.Y] == 0)
                {
                    this.Position += this._movingVector;
                }
                else
                {
                    this._movingVector = Vector2.Zero;
                    this._endPoint = Point.Zero;
                    // cho nó đứng yên
                    this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
                    this.CurrentIndex = 0;
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Move Npc on map
        /// </summary>
        public void Move()
        {
            if ((System.Environment.TickCount - this._lastTickCountForMove) > this._delayTimeToMove)
            {
                this._lastTickCountForMove = System.Environment.TickCount;
                if (this._endPoint != Point.Zero)
                {
                    //this.Position += this._movingVector; // di chuyển unit với vécto di chuyển
                    this.CheckOccupie();
                    Rectangle temp1 = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.BoundRectangle.Width, this.BoundRectangle.Height);// tính ra rectangle bao trùm unit theo hệ tọa độ toàn map
                    Rectangle temp2 = new Rectangle(this._endPoint.X, this._endPoint.Y, this.BoundRectangle.Width / 2, this.BoundRectangle.Height / 2);// tính ra rectangle điểm đến                    
                    if (temp1.Intersects(temp2)) // nếu 2 rectangle này chạm nhau
                    {
                        // dừng unit này lại
                        this._movingVector = Vector2.Zero;
                        this._endPoint = Point.Zero;
                        if (this is ProducerUnit) // nếu đây là 1 producer, thử kiểm tra nó còn trong vùng khai thác mõ ko
                        {
                            ((ProducerUnit)this).FindToResourceCenter();// nếu trong vùng khai thác mỏ thì thuộc tính CurrentResourceCenterExploiting sẽ khác NULL
                            if (((ProducerUnit)this).CurrentResourceCenterExploiting == null)// nếu ko có mỏ nào trong phạm vi khai thác thì cứ để nó đứng yên
                            {
                                // cho nó đứng yên như 1 unit bình thường vẫn làm sau khi dừng di chuyển
                                this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
                                this.CurrentIndex = 0;
                                //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                            }
                        }
                        else // đây chỉ là 1 unit bình thường, không phải là người khai thác
                        {
                            // cho nó đứng yên
                            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
                            this.CurrentIndex = 0;
                            //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// forbid to move out of battle map
        /// Cấm unit di chuyển ra ngoài vùng bản đồ chiến trận
        /// </summary>
        public void CantMoveAtBorder()
        {
            Transform transform = Transform.CURRENT;// get transform
            Texture2D image = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image[this.CurrentIndex];
            //Point cell1 = transform.PointToCell(Transform.Vector2ToPoint(this.Position));// cell contain postion(top,left) of this
            Vector2 sizetexture = new Vector2(image.Width, image.Height);// size of current texture of its
            //Point cell2 = transform.PointToCell(Transform.Vector2ToPoint(this.Position + sizetexture));// cell contain postion(bottom,right) of this

            //if (cell1.X < 0 || cell2.X >= Config.MAP_SIZE_IN_CELL.Width - 1 || cell1.Y < -1|| cell2.Y >= Config.MAP_SIZE_IN_CELL.Height)
            Point cell = transform.PointToCell(Transform.Vector2ToPoint(this.Position + sizetexture / 2));
            if (cell.X <= 0 || cell.X >= GlobalDTO.MAP_SIZE_IN_CELL.Width - 2 || cell.Y <= 0 || cell.Y >= GlobalDTO.MAP_SIZE_IN_CELL.Height -2 )
            {
                // bắt unit dừng lại nếu nó chạy đến biên vùng chiến trận
                this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
                this._movingVector = Vector2.Zero;
                this._endPoint = Point.Zero;
                // set lại 1 tập hình mới cho hành động đứng yên
                this.CurrentIndex = 0;
                //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get new set of texture
            }
        }

        /// <summary>
        /// Create moving vector for this unit
        /// Tạo ra vecto di chuyển để thực hiện move unit
        /// </summary>
        public void CreateMovingVector()
        {
            // calculate moving vector
            // tính toán ra vector dịch chuyển
            this._movingVector = GlobalFunction.CreateMovingVector(this._endPoint, this.Position, int.Parse(((UnitDTO)this.Info).InformationList["Speed"].Value));

            // change status
            // thay đổi trạng thái của unit

            this.CurrentStatus = this.Info.Action[StatusList.MOVE.Name];
            // change texture
            // thay đổi tập các hình ảnh thể hiện động tác unit
            float x = this._endPoint.X - (int)this.Position.X;
            float y = this._endPoint.Y - (int)this.Position.Y;
            float degree = MathHelper.ToDegrees((float)Math.Atan2(x, y));
            this.ChangeDirectionByDegree(degree);// đổi hướng di chuyển nhờ vào góc độ của vector di chuyển
            this.CurrentIndex = 0;
            //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);

            /// change endpoint
            /// xử lý endpoint 1 tí để unit dừng đúng chổ
            if (this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.WS.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.ES.Name])
            {
                this._endPoint.Y += this.BoundRectangle.Height / 2;
            }
            else if (this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.N.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.WN.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.EN.Name])
            {
                this._endPoint.Y -= this.BoundRectangle.Height / 2;
            }
            if (this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.W.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.WN.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.WS.Name])
            {
                this._endPoint.X -= this.BoundRectangle.Width / 2;
            }
            else if (this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.E.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.EN.Name] || this.CurrentDirection == this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.ES.Name])
            {
                this._endPoint.X += this.BoundRectangle.Width / 2;
            }
        }

        /// <summary>
        /// Change direction of this unit by degree
        /// tính toán ra hướng cần thiết nhờ vào góc độ của véctơ di chuyển
        /// </summary>
        /// <param name="degree"></param>
        public void ChangeDirectionByDegree(float degree)
        {
            if ((-22.5 <= degree) && (degree <= 22.5))
            {
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];
            }
            else if ((22.5 <= degree) && (degree <= 67.5))
            {
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.ES.Name];
            }
            else if ((67.5 <= degree) && (degree <= 112.5))
            {                
                //this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.E.Name];
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.EN.Name];
            }
            else if ((112.5 <= degree) && (degree <= 157.5))
            {
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.EN.Name];
            }
            else if ((157.5 <= degree && degree <= 180.0) || (-179.9 <= degree && degree <= -157.5))
            {
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.N.Name];
            }
            else if (-157.5 <= degree && degree <= -112.5)
            {
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.WN.Name];
            }
            else if (-112.5 <= degree && degree <= -67.5)
            {
                //this.CurrentDirection = Direction.W;
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.WS.Name];
                //this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.W.Name];
            }
            else if (-67.5 <= degree && degree <= -22.5)
            {
                this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.WS.Name];
            }
        }

        /// <summary>
        /// change current index of set of textures to change image for action
        /// thay đổi index của tập ảnh để mô tả hoạt động của unit
        /// </summary>
        public void PerformAction()
        {
            if ((System.Environment.TickCount - this._lastTickCountForChangeImage) > this._delayTimeToChangeImage)
            {
                this._lastTickCountForChangeImage = System.Environment.TickCount;
                this.CurrentIndex++;
                if (this.CurrentStatus == this.Info.Action[StatusList.DEAD.Name])// if it die// nếu unit này đã bị đánh chết
                {
                    int count = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count;
                    if (this.CurrentIndex == count)// hình ảnh cuối cùng trong tập ảnh sẽ thực thi và unit này được giải phóng khỏi vùng nhớ
                    {
                        this.CurrentIndex = count - 1;// đã đến hình ảnh cuối của tập hình dead
                        this.Dispose(true);
                    }
                }
                else// các trạng thái khác (Walk, fly, attack, hit ...) vẫn được lặp hành động 1 cách bình thường
                {
                    int count = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image.Count;
                    if (this.CurrentIndex == count)
                    {
                        // nhưng trạng thái attack được thêm vào sound effect 'sword' -> tiếng binh khí đánh nhau khi tấn công unit
                        if (!(this is ProducerUnit))
                        {
                            if (this.CurrentStatus == this.Info.Action[StatusList.ATTACK.Name])
                            {
                                this.PlayAttackSound();
                            }
                        }
                        else
                        {
                            if (this.CurrentStatus == this.Info.Action[StatusList.EXPLOIT.Name])
                            {
                                this.PlayAttackSound();
                            }
                        }
                        this.CurrentIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Phát âm thanh tân cống của unit
        /// </summary>
        public void PlayAttackSound()
        {
            AudioGame au = new AudioGame(this.Game);
            // xác định vị trí loa phát,
            float pan = 0.0f;
            if (this.Position.X > GlobalDTO.CURRENT_COORDINATE.X + GlobalDTO.SCREEN_SIZE.Width) // unit bên phải thì phát loa phải
            {
                pan = 1.0f;
            }
            else if (this.Position.X < GlobalDTO.CURRENT_COORDINATE.X)//unit bên trái thì phát loa trái
            {
                pan = -1.0f;
            }
            // xác định cường độ âm thanh
            float volume = 0.01f;
            if ( // nếu ngoài phạm vi màn hình thì giảm bớt
                    !(GlobalDTO.CURRENT_COORDINATE.X < this.Position.X && this.Position.X < GlobalDTO.CURRENT_COORDINATE.X + GlobalDTO.SCREEN_SIZE.Width
                    &&
                    GlobalDTO.CURRENT_COORDINATE.Y < this.Position.Y && this.Position.Y < GlobalDTO.CURRENT_COORDINATE.Y + GlobalDTO.SCREEN_SIZE.Height)
                )
            {
                volume = 0.005f;
            }
            au.PlaySoundEffectGame("sword", volume, pan);
            au.Dispose();
        }

        /// <summary>
        /// Search for a unit or structure of another faction to attack
        /// Tìm kiếm trong danh sách các unit và structure một thành viên của phe khác để tấn công
        /// </summary>
        public void SearchForAttack()
        {
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListUnitOnMap.Count; i++)// duyệt qua danh sách các unit -> ưu tiên đánh các thành viên là unit của đối thủ
            {
                Sprite anotherunit = GlobalDTO.MANAGER_GAME.ListUnitOnMap[i];
                if (this.AttackOpposition(anotherunit)) // phát hiện 1 unit trong tầm tấn công
                {
                    this._endPoint = Point.Zero;// ko di chuyển nữa
                    ((Unit)anotherunit).FlagBeAttacked = true;// bật cờ cho unit kia là nó bị tấn công
                    this._whomIHit = anotherunit;// set thuộc tính kẻ bị tấn công (whomIHit) cho unit này là unit đối thủ của nó
                    return;
                }
                else if (this.DetectOpposition(anotherunit))
                {
                    this._endPoint = new Point((int)(anotherunit.Position.X + anotherunit.BoundRectangle.Width / 2), (int)(anotherunit.Position.Y + anotherunit.BoundRectangle.Height / 2));
                    this.CreateMovingVector();
                    return;
                }
            }

            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListStructureOnMap.Count; i++)// duyệt qua danh sách các structure
            {
                // tương tự như trên
                Sprite anotherstructure = GlobalDTO.MANAGER_GAME.ListStructureOnMap[i];
                if (this.AttackOpposition(anotherstructure))
                {
                    this._endPoint = Point.Zero;
                    this._whomIHit = anotherstructure;
                    return;
                }
                else if (this.DetectOpposition(anotherstructure))
                {
                    this._endPoint = new Point((int)(anotherstructure.Position.X + anotherstructure.BoundRectangle.Width / 2), (int)(anotherstructure.Position.Y + anotherstructure.BoundRectangle.Height / 2));
                    this.CreateMovingVector();
                    return;
                }
            }
        }       

        /// <summary>
        /// Kiểm tra đối thủ trong phạm vi phát hiện
        /// </summary>
        /// <param name="opposition"></param>
        /// <returns></returns>
        public Boolean DetectOpposition(Sprite opposition)
        {
            if (this.CurrentStatus == this.Info.Action[StatusList.MOVE.Name] || this.CurrentStatus == this.Info.Action[StatusList.ATTACK.Name])
            {
                return false;
            }
            if (opposition.CurrentStatus == opposition.Info.Action[StatusList.MOVE.Name])
            {
                return false;
            }
            if (opposition.CodeFaction != this.CodeFaction) // if anotherunit is member of another faction, detect and attack
            // đối thủ có mã thuộc phe khác
            {
                Texture2D img1 = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image[0];
                Texture2D img2 = opposition.Info.Action[opposition.CurrentStatus.Name].DirectionInfo[opposition.CurrentDirection.Name].Image[0];
                // unit xác định vị trí của mình theo tọa độ map
                Vector2 locationCoordinate1 = new Vector2(this.Position.X + (int)(img1.Width / 2), this.Position.Y + (int)(img1.Height / 2));// get coordinate for location of anotherunit
                // unit xác định vị trí đối phương theo tọa độ map
                Vector2 locationCoordinate2 = new Vector2(opposition.Position.X + (int)(img2.Width / 2), opposition.Position.Y + (int)(img2.Height / 2));// get coordinate for location of anotherunit
                // tính ra khoảng cách giữa 2 bên
                int r = Convert.ToInt32(Math.Sqrt(Math.Pow((locationCoordinate1.X - locationCoordinate2.X), 2) + Math.Pow((locationCoordinate1.Y - locationCoordinate2.Y), 2)));// get distance between this and anotherunit
                // nếu khoảng cách là trong phạm vi tân công
                if (r <= int.Parse(((UnitDTO)this.Info).InformationList["RadiusDetect"].Value))// if being in attacked area
                {
                    if (this.CurrentStatus != this.Info.Action[StatusList.DEAD.Name])// unit vẫn chưa chết thì sẽ có thể tấn công
                    {                       
                        return true;// trả lại true nếu đối thủ trong phạm vi phát hiện
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Acctack a member of another faction
        /// Xác định rằng đối thủ này có đánh được ko
        /// </summary>
        /// <param name="anotherunit"></param>
        public Boolean AttackOpposition(Sprite opposition)
        {
            if (opposition.CodeFaction != this.CodeFaction) // if anotherunit is member of another faction, detect and attack
            // đối thủ có mã thuộc phe khác
            {
                Texture2D img1 = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image[0];
                Texture2D img2 = opposition.Info.Action[opposition.CurrentStatus.Name].DirectionInfo[opposition.CurrentDirection.Name].Image[0];
                // unit xác định vị trí của mình theo tọa độ map
                Vector2 locationCoordinate1 = new Vector2(this.Position.X + (int)(img1.Width / 2), this.Position.Y + (int)(img1.Height / 2));// get coordinate for location of anotherunit
                // unit xác định vị trí đối phương theo tọa độ map
                Vector2 locationCoordinate2 = new Vector2(opposition.Position.X + (int)(img2.Width / 2), opposition.Position.Y + (int)(img2.Height / 2));// get coordinate for location of anotherunit
                // tính ra khoảng cách giữa 2 bên
                int r = Convert.ToInt32(Math.Sqrt(Math.Pow((locationCoordinate1.X - locationCoordinate2.X), 2) + Math.Pow((locationCoordinate1.Y - locationCoordinate2.Y), 2)));// get distance between this and anotherunit
                // nếu khoảng cách là trong phạm vi tân công
                if (r <= int.Parse(((UnitDTO)this.Info).InformationList["RadiusAttack"].Value))// if being in attacked area
                {
                    if (this.CurrentStatus != this.Info.Action[StatusList.DEAD.Name])// unit vẫn chưa chết thì sẽ có thể tấn công
                    {
                        // chuyển qua trạng thái tấn công
                        this.CurrentStatus = this.Info.Action[StatusList.ATTACK.Name];// switch to ATTACK status
                        // decide detection to attack
                        Vector2 attacvector = locationCoordinate2 - locationCoordinate1;
                        // xác định hướng tấn công dựa vào véctơ hướng giữa vị trí 2 kẻ này
                        float degree = MathHelper.ToDegrees((float)Math.Atan2(attacvector.X, attacvector.Y));
                        this.ChangeDirectionByDegree(degree);
                        this.CurrentIndex = 0;
                        //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// thay đổi tập hình thành hành động tấn công
                        return true;// trả lại true nếu đối thủ này sẽ bị tấn công
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check opposition if it die or flee
        /// kiểm tra đối thủ đã chết chưa, hay đã chạy mất
        /// hoặc chính bản thân unit này đã chạy ra khỏi chỗ nó đang đánh
        /// </summary>
        public void CheckOpposition()
        {
            Texture2D img1 = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[this.CurrentDirection.Name].Image[0];
            Texture2D img2 = this._whomIHit.Info.Action[this._whomIHit.CurrentStatus.Name].DirectionInfo[this._whomIHit.CurrentDirection.Name].Image[0];
            // xác định lại khoảng cách 2 bên
            Vector2 locationCoordinate1 = new Vector2(this.Position.X + (int)(img1.Width / 2), this.Position.Y + (int)(img1.Height / 2));// get coordinate for location of anotherunit
            Vector2 locationCoordinate2 = new Vector2(this._whomIHit.Position.X + (int)(img2.Width / 2), this._whomIHit.Position.Y + (int)(img2.Height / 2));// get coordinate for location of anotherunit
            int r = Convert.ToInt32(Math.Sqrt(Math.Pow((locationCoordinate1.X - locationCoordinate2.X), 2) + Math.Pow((locationCoordinate1.Y - locationCoordinate2.Y), 2)));// get distance between this and anotherunit
            if (r > int.Parse(((UnitDTO)this.Info).InformationList["RadiusAttack"].Value))// if not being in attacked area
            // khoảng cách đã lớn hơn khoảng cách tấn công
            {
                if (this.CurrentStatus == this.Info.Action[StatusList.ATTACK.Name])// nếu nó đang trong trang thái tấn công
                {
                    // chuyển thành đứng yên và thay đổi tập hình phù hợp
                    this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];// switch to IDLE status
                    this.CurrentIndex = 0;
                    //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                }
                // nếu người nó đánh là 1 unit thì bật lại cờ cho đối phương
                if (this._whomIHit is Unit)
                {
                    ((Unit)this._whomIHit).FlagBeAttacked = false;// bật lại cờ cho đối thủ
                }
                this._whomIHit = null;// chuyển whomIHit thành null
                return;
            }
        }

        /// <summary>
        /// Decrease health of sprite which this unit attacked
        /// Giảm máu của đối thủ mà nó đang tân công sau 1 khoảng thời gian trì hoãn
        /// </summary>
        public void DecreaseHealthOf_WhomIHit()
        {
            if ((System.Environment.TickCount - this._lastTickCountForDecreaseHealth_WhomIHit) > this._delayTimeToDecreaseHealth_WhomIHit)
            {
                this._lastTickCountForDecreaseHealth_WhomIHit = System.Environment.TickCount;
                try
                {
                    if (this._whomIHit is Unit) // is unit // nếu kẻ mà unit này đang đánh là 1 unit
                    {
                        ((Unit)this._whomIHit).CurrentHealth -= int.Parse(((UnitDTO)this.Info).InformationList["Power"].Value);// giảm máu của kẻ mà nó đánh băng chình power của nó
                        if (((Unit)this._whomIHit).CurrentHealth <= 0)// nếu đối thủ của nó đã bị giảm hết máu
                        {
                            this._whomIHit.CurrentStatus = this._whomIHit.Info.Action[StatusList.DEAD.Name];// chuyển đối thủ nó qua trạng thái chết
                            this._whomIHit.CurrentIndex = 0;
                            //this._whomIHit.GetSetOfTexturesForSprite(this._whomIHit.PathSpecificationFile);// xác lập tập hình dead cho đối thủ của nó
                            ((Unit)this._whomIHit).WhomIHit = null;// đối thủ dù của nó đang đánh ai thì cũng chuyển WhomIHit của đối thủ nó sang NULL -> chết rồi thì ko còn đánh ai đươc nữa
                            GlobalDTO.MANAGER_GAME.ListUnitOnMap.Remove(this._whomIHit);// loại bỏ đối thủ của nó ra khỏi mảng các unit luôn
                            this._whomIHit = null;// chuyển whomIHit của nó thành null
                            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];// chuyển tập hình thành đứng yên, sau đó nếu nó xác định ai đo ở gần nữa thì nó lại đánh
                            this.CurrentIndex = 0;
                            //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                        }
                    }
                    else // is structure// nếu kẻ mà unit này đánh là 1 structure
                    {
                        // làm tương tự
                        ((Structure)this._whomIHit).CurrentHealth -= int.Parse(((UnitDTO)this.Info).InformationList["Power"].Value);
                        if (((Structure)this._whomIHit).CurrentHealth <= 0)
                        {
                            this._whomIHit.CurrentStatus = this._whomIHit.Info.Action[StatusList.DEAD.Name];
                            this._whomIHit.CurrentIndex = 0;
                            //this._whomIHit.GetSetOfTexturesForSprite(this._whomIHit.PathSpecificationFile);
                            GlobalDTO.MANAGER_GAME.ListStructureOnMap.Remove(this._whomIHit);
                            this._whomIHit = null;
                            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
                            this.CurrentIndex = 0;
                            //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                        }
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Thay đổi hình ảnh của tập hình particle
        /// </summary>
        public void PerformParticle()
        {
            if (this._particleAttack != null && 
                this._particleAttack.ParticleInfo.Image.Count > 0){
                if (this._whomIHit != null){
                    if ((System.Environment.TickCount - this._lastTickCountForParticle) > this._delayTimeToParticle)
                    {
                        this._lastTickCountForParticle = System.Environment.TickCount;
                        this._particleAttack.IndexImage++;
                        if (this._particleAttack.IndexImage == this._particleAttack.ParticleInfo.Image.Count)
                        {
                            this._particleAttack.IndexImage = 0;
                        }
                    }
                }
            }
        }
        #endregion
    }
}