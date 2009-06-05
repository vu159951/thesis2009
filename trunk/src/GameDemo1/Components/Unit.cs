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

namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class Unit : Sprite
    {
        #region Properties
        // =====================================================================================================
        // ============================================= Properties ============================================
        // =====================================================================================================
        private Vector2 _movingVector; // moving vector for moving Npc // véctơ dịch chuyển
        private int _maxHealth; // health of Npc// số lượng máu tối đa
        private int _currentHealth;    // current health of this unit // máu hiện tại
        private int _power;// power of Ncp// sức mạnh tấn công
        private int _radiusDetect; // radius to detect members of faction // bán kính phát hiện đối phương
        private int _radiusAttack; // radius to attack members of faction// bán kính tấn công
        private int _speed;// speed to move// tốc độ di chuyển
        private Point _endPoint;// end point to move// điêm kết thúc hiên tại khi nó đang di chuyển
        private Sprite _whomIHit = null;// current opposition which unit or structure is hitted by this unit// đối thủ mà nó đang đánh
        private Level _level;// current level of unit// cấp độ của unit
        private List<Resource> _requirementResource;// list resources which require to create this unit// các tài nguyên đòi hỏi để mua unit này
        private Boolean _flagBeAttacked = false;// thể hiện trạng thái nó có đang bị tấn công ko

        public Boolean FlagBeAttacked
        {
            get { return _flagBeAttacked; }
            set { _flagBeAttacked = value; }
        }
        public List<Resource> RequirementResource
        {
            get { return _requirementResource; }
            set { _requirementResource = value; }
        }
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
        public Level Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public int RadiusAttack
        {
            get { return _radiusAttack; }
            set { _radiusAttack = value; }
        }
        public int RadiusDetect
        {
            get { return _radiusDetect; }
            set { _radiusDetect = value; }
        }
        public int Power
        {
            get { return _power; }
            set { _power = value; }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }
        public Vector2 MovingVector
        {
            get { return _movingVector; }
            set { _movingVector = value; }
        }

        private int _delayTimeToChangeImage = 35;// delay time to change image // thời gian trì hoãn cho mỗi lần chuyển hình trong tâp hình thể hiện hành động
        private int _lastTickCountForChangeImage = System.Environment.TickCount;// counter delay time for change texture// biến đếm timer cho chuyên ảnh
        private int _delayTimeToMove = 20;// delay time to move// thời gian trì hoãn cho mỗi lần dịch chuyển
        private int _lastTickCountForMove = System.Environment.TickCount;// counter delay time for moving Npc// biến đếm timer cho dịch chuyển
        private int _delayTimeToDecreaseHealth_WhomIHit = 350;// delay time to decrease health // thời gian trì hoãn để giảm máu đối thủ
        private int _lastTickCountForDecreaseHealth_WhomIHit = System.Environment.TickCount;// counter delay time for decrease and die// biến đếm của nó
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
            this.PercentSize = 0.5f;
            this._movingVector = new Vector2(0, 0);// now is IDLE
            this.CodeFaction = codeFaction;// code to tell difference between this with another Npc or Structure
            this.Position = position;// for postion on map
            this.PathSpecificationFile = pathspecificationfile;//get file for specification
            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get textures
            this.GetInformationUnit();// get information about health, power, radius attack, radius detect, requiremed resource
            this._whomIHit = null;
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

            /// perform action of this unit by change image
            /// thực hiện chuyển đổi hình ảnh để mô tả hành động
            if (this.CurrentIndex != -1)
            {
                this.PerformAction();
            }

            /// perform move Npc
            /// Di chuyển trên màn hình
            if (this.CurrentStatus == Status.WALK || this.CurrentStatus == Status.FLY && (this._movingVector != Vector2.Zero))
            {
                this.Move();
                this.CantMoveAtBorder(); // nếu chạm biên thì dừng lại
            }
            

            /// detect members of another faction to attack
            if (!(this is ProducerUnit)) // nếu đây ko phải là 1 producer
            {
                if (this._whomIHit == null) // if don't have opposition // tìm kiếm cquan6 địch
                {
                    this.SearchForAttack();
                }
                else if (this._whomIHit != null) // if having opposition // nếu đã tìm ra quân địch
                {
                    this.CheckOpposition(); // kiểm tra còn trong phạm vi đánh ko
                    this.DecreaseHealthOf_WhomIHit(); // giảm máu của kẻ bị đánh
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
            base.Draw(gameTime);
        }

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
            // load file xml
            XmlDocument doc = new XmlDocument();
            doc.Load(this.PathSpecificationFile);
            this._maxHealth = int.Parse(doc.SelectSingleNode("//MaxHealth[1]").Attributes[0].Value); // số máu tối đa
            this._currentHealth = this._maxHealth;// máu hiện thời
            this._power = int.Parse(doc.SelectSingleNode("//Power[1]").Attributes[0].Value);// sức mạnh
            this._radiusAttack = int.Parse(doc.SelectSingleNode("//RadiusAttack[1]").Attributes[0].Value);// phạm vi tấn công
            this._radiusDetect = int.Parse(doc.SelectSingleNode("//RadiusDetect[1]").Attributes[0].Value);// phạm vi phát hiện đối phương
            this._speed = int.Parse(doc.SelectSingleNode("//Speed[1]").Attributes[0].Value);// tốc độ di chuyển

            // lấy tập các yêu cầu để mua unit này
            this._requirementResource = new List<Resource>();// list resource which this structure require to build
            XmlNode requirementnode = doc.SelectSingleNode("//Requirements");
            foreach (XmlNode node in requirementnode.ChildNodes)
            {
                Resource resource = new Resource(node.Name, int.Parse(node.Attributes[0].Value));
                this._requirementResource.Add(resource);
            }
            this.Name = doc.DocumentElement.Name;
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
                    this.Position += this._movingVector; // di chuyển unit với vécto di chuyển
                    Rectangle temp1 = new Rectangle(this.BoundRectangle.X + (int)this.CurrentRootCoordinate.X, this.BoundRectangle.Y + (int)this.CurrentRootCoordinate.Y, this.BoundRectangle.Width, this.BoundRectangle.Height);// tính ra rectangle bao trùm unit
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
                                this.CurrentStatus = Status.IDLE;
                                this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                            }
                        }
                        else // đây chỉ là 1 unit bình thường, không phải là người khai thác
                        {
                            // cho nó đứng yên
                            this.CurrentStatus = Status.IDLE;
                            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
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
            Point cell1 = transform.PointToCell(Transform.Vector2ToPoint(this.Position));// cell contain postion(top,left) of this
            Vector2 sizetexture = new Vector2(this.TextureSprites[this.CurrentIndex].Width, this.TextureSprites[this.CurrentIndex].Height);// size of current texture of its
            Point cell2 = transform.PointToCell(Transform.Vector2ToPoint(this.Position + sizetexture));// cell contain postion(bottom,right) of this

            if (cell1.X < 1 || cell2.X >= Config.MAP_SIZE_IN_CELL.Width - 2 || cell1.Y < 1 || cell2.Y >= Config.MAP_SIZE_IN_CELL.Height - 2)
            {
                // bắt unit dừng lại nếu nó chạy đến biên vùng chiến trận
                this.CurrentStatus = Status.IDLE;
                this._movingVector = Vector2.Zero;
                this._endPoint = Point.Zero;
                // set lại 1 tập hình mới cho hành động đứng yên
                this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get new set of texture
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
            this._movingVector = GlobalFunction.CreateMovingVector(this._endPoint, this.Position, this._speed);

            // change status
            // thay đổi trạng thái của unit
            if (this.Name == "Angel" || this.Name == "Phoenix") // nếu là các unit có khả năng bay
            {
                this.CurrentStatus = Status.FLY;
            }
            else
            {
                this.CurrentStatus = Status.WALK;
            }
            // change texture
            // thay đổi tập các hình ảnh thể hiện động tác unit
            float x = this._endPoint.X - (int)this.Position.X;
            float y = this._endPoint.Y - (int)this.Position.Y;
            float degree = MathHelper.ToDegrees((float)Math.Atan2(x, y));
            this.ChangeDirectionByDegree(degree);// đổi hướng di chuyển nhờ vào góc độ của vector di chuyển
            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);

            /// change endpoint
            /// xử lý endpoint 1 tí để unit dừng đúng chổ
            if (this.CurrentDirection == Direction.S || this.CurrentDirection == Direction.WS || this.CurrentDirection == Direction.ES)
            {
                this._endPoint.Y += this.BoundRectangle.Height / 2;
            }
            else if (this.CurrentDirection == Direction.N || this.CurrentDirection == Direction.WN || this.CurrentDirection == Direction.EN)
            {
                this._endPoint.Y -= this.BoundRectangle.Height / 2;
            }
            if(this.CurrentDirection == Direction.W || this.CurrentDirection == Direction.WN || this.CurrentDirection == Direction.WS)
            {
                this._endPoint.X -= this.BoundRectangle.Width / 2;
            }
            else if (this.CurrentDirection == Direction.E || this.CurrentDirection == Direction.EN || this.CurrentDirection == Direction.ES)
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
                this.CurrentDirection = Direction.S;
            }
            else if ((22.5 <= degree) && (degree <= 67.5))
            {
                this.CurrentDirection = Direction.ES;
            }
            else if ((67.5 <= degree) && (degree <= 112.5))
            {
                //this.CurrentDirection = Direction.E;
                this.CurrentDirection = Direction.EN;
            }
            else if ((112.5 <= degree) && (degree <= 157.5))
            {
                this.CurrentDirection = Direction.EN;
            }
            else if ((157.5 <= degree && degree <= 180.0) || (-179.9 <= degree && degree <= -157.5))
            {
                this.CurrentDirection = Direction.N;
            }
            else if (-157.5 <= degree && degree <= -112.5)
            {
                this.CurrentDirection = Direction.WN;
            }
            else if (-112.5 <= degree && degree <= -67.5)
            {
                //this.CurrentDirection = Direction.W;
                this.CurrentDirection = Direction.WS;
            }
            else if (-67.5 <= degree && degree <= -22.5)
            {
                this.CurrentDirection = Direction.WS;
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
                if (this.CurrentStatus == Status.DEAD)// if it die// nếu unit này đã bị đánh chết
                {
                    if (this.CurrentIndex == this.TextureSprites.Count)// hình ảnh cuối cùng trong tập ảnh sẽ thực thi và unit này được giải phóng khỏi vùng nhớ
                    {
                        this.CurrentIndex = this.TextureSprites.Count - 1;// đã đến hình ảnh cuối của tập hình dead
                        this.Dispose(true);
                    }
                }
                else// các trạng thái khác vẫn được lặp hành động 1 cách bình thường
                {
                    if (this.CurrentIndex == this.TextureSprites.Count)
                    {
                        this.CurrentIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Search for a unit or structure of another faction to attack
        /// Tìm kiếm trong danh sách các unit và structure một thành viên của phe khác để tấn công
        /// </summary>
        public void SearchForAttack()
        {
            for (int i = 0; i < ManagerGame._listUnitOnMap.Count; i++)// duyệt qua danh sách các unit -> ưu tiên đánh các thành viên là unit của đối thủ
            {
                Sprite anotherunit = ManagerGame._listUnitOnMap[i];
                if (this.AttackOpposition(anotherunit)) // phát hiện 1 unit trong tầm tấn công
                {
                    this._endPoint = Point.Zero;// ko di chuyển nữa
                    ((Unit)anotherunit).FlagBeAttacked = true;// bật cờ cho unit kia là nó bị tấn công
                    this._whomIHit = anotherunit;// set thuộc tính kẻ bị tấn công (whomIHit) cho unit này là unit đối thủ của nó
                    return;
                }
            }

            for (int i = 0; i < ManagerGame._listStructureOnMap.Count; i++)// duyệt qua danh sách các structure
            {
                // tương tự như trên
                Sprite anotherstructure = ManagerGame._listStructureOnMap[i];
                if (this.AttackOpposition(anotherstructure))
                {
                    this._endPoint = Point.Zero;
                    this._whomIHit = anotherstructure;
                    return;
                }
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
                // unit xác định vị trí của mình theo tọa độ map
                Vector2 locationCoordinate1 = new Vector2(this.Position.X + (int)(this.TextureSprites[0].Width / 2), this.Position.Y + (int)(this.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
                // unit xác định vị trí đối phương theo tọa độ map
                Vector2 locationCoordinate2 = new Vector2(opposition.Position.X + (int)(opposition.TextureSprites[0].Width / 2), opposition.Position.Y + (int)(opposition.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
                // tính ra khoảng cách giữa 2 bên
                int r = Convert.ToInt32(Math.Sqrt(Math.Pow((locationCoordinate1.X - locationCoordinate2.X), 2) + Math.Pow((locationCoordinate1.Y - locationCoordinate2.Y), 2)));// get distance between this and anotherunit
                // nếu khoảng cách là trong phạm vi tân công
                if (r <= this._radiusAttack)// if being in attacked area
                {
                    if (this.CurrentStatus != Status.DEAD)// unit vẫn chưa chết thì sẽ có thể tấn công
                    {
                        // chuyển qua trạng thái tán công
                        this.CurrentStatus = Status.ATTACK;// switch to ATTACK status
                        // decide detection to attack
                        Vector2 attacvector = locationCoordinate2 - locationCoordinate1;
                        // xác định hướng tấn công dựa vào véctơ hướng giữa vị trí 2 kẻ này
                        float degree = MathHelper.ToDegrees((float)Math.Atan2(attacvector.X, attacvector.Y));
                        this.ChangeDirectionByDegree(degree);
                        this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// thay đổi tập hình thành hành động tấn công
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
            // xác định lại khoảng cách 2 bên
            Vector2 locationCoordinate1 = new Vector2(this.Position.X + (int)(this.TextureSprites[0].Width / 2), this.Position.Y + (int)(this.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
            Vector2 locationCoordinate2 = new Vector2(this._whomIHit.Position.X + (int)(this._whomIHit.TextureSprites[0].Width / 2), this._whomIHit.Position.Y + (int)(this._whomIHit.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
            int r = Convert.ToInt32(Math.Sqrt(Math.Pow((locationCoordinate1.X - locationCoordinate2.X), 2) + Math.Pow((locationCoordinate1.Y - locationCoordinate2.Y), 2)));// get distance between this and anotherunit
            if (r > this._radiusAttack)// if not being in attacked area
            // khoảng cách đã lớn hơn khoảng cách tấn công
            {
                if (this.CurrentStatus == Status.ATTACK)// nếu nó đang trong trang thái tấn công
                {
                    // chuyển thành đứng yên và thay đổi tập hình phù hợp
                    this.CurrentStatus = Status.IDLE;// switch to IDLE status
                    this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
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
                        ((Unit)this._whomIHit).CurrentHealth -= this._power;// giảm máu của kẻ mà nó đánh băng chình power của nó
                        if (((Unit)this._whomIHit).CurrentHealth <= 0)// nếu đối thủ của nó đã bị giảm hết máu
                        {
                            this._whomIHit.CurrentStatus = Status.DEAD;// chuyển đối thủ nó qua trạng thái chết
                            this._whomIHit.GetSetOfTexturesForSprite(this._whomIHit.PathSpecificationFile);// xác lập tập hình dead cho đối thủ của nó
                            ((Unit)this._whomIHit).WhomIHit = null;// đối thủ dù của nó đang đánh ai thì cũng chuyển WhomIHit của đối thủ nó sang NULL -> chết rồi thì ko còn đánh ai đươc nữa
                            ManagerGame._listUnitOnMap.Remove(this._whomIHit);// loại bỏ đối thủ của nó ra khỏi mảng các unit luôn
                            this._whomIHit = null;// chuyển whomIHit của nó thành null
                            this.CurrentStatus = Status.IDLE;// chuyển tập hình thành đứng yên, sau đó nếu nó xác định ai đo ở gần nữa thì nó lại đánh
                            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                        }
                    }
                    else // is structure// nếu kẻ mà unit này đánh là 1 structure
                    {
                        // làm tương tự
                        ((Structure)this._whomIHit).CurrentHealth -= this._power;
                        if (((Structure)this._whomIHit).CurrentHealth <= 0)
                        {
                            this._whomIHit.CurrentStatus = Status.DEAD;
                            this._whomIHit.GetSetOfTexturesForSprite(this._whomIHit.PathSpecificationFile);
                            ManagerGame._listStructureOnMap.Remove(this._whomIHit);
                            this._whomIHit = null;
                            this.CurrentStatus = Status.IDLE;
                            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                        }
                    }
                }
                catch
                { }
            }
        }

        #endregion
    }
}