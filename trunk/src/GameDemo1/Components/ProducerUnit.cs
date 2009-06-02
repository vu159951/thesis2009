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


namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ProducerUnit : Unit
    {
        #region Properties
        private Resource _currentResourceExploiting;// currrent resource which this producer is bringing on itself
                                                    // tài nguyên hiện tại mà producer đang khai thác và mang trong người(nếu chuyển qua khai thác 1 tài nguyên khác thì tài nguyên hiện thời mất hết)
        private ResourceCenter _currentResourceCenterExploiting;// current resource center which it is exploiting
                                                                // mõ tài nguyên hiện tại mà producer đang khai thác
        private int _maxExploit;// max number that this producer can exploit
                                // số lượng tài nguyên tối đa mà producer có thể mang
        private int _speedExploit;// quatity this producer can exploit in 1/60 s(one time to perform update method)
                                  // tốc độ khai thác tài nguyên

        public int SpeedExploit
        {
            get { return _speedExploit; }
            set { _speedExploit = value; }
        }
        public int MaxExploit
        {
            get { return _maxExploit; }
            set { _maxExploit = value; }
        }
        public Resource CurrentResourceExploiting
        {
            get { return _currentResourceExploiting; }
            set { _currentResourceExploiting = value; }
        }

        private int _delayTimeGetResource = 100;// delay time to get resource(increase quatity resource which it is exploiting)
                                                // thời gian trì hoãn giữa 2 lần lấy tài nguyên(thời gian trì hoãn tăng tài nguyên khi lấy)
        private int _lastTickCountForGetResource = System.Environment.TickCount;// counter delay time for increase quatity resource
                                                                               // biến đếm thời gian dựa vào thời gian trì hoãn
        #endregion

        #region Basic methods
        public ProducerUnit(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        public ProducerUnit(Game game, string pathspecificationfile, Vector2 position, int codeFaction)
            : base(game)
        {
            this.PercentSize = 0.5f;
            this.MovingVector = new Vector2(0, 0);// now is IDLE
            this.CodeFaction = codeFaction;// code to tell difference between this with another Npc or Structure
            this.Position = position;// for postion on map battle // cho vị trí trên bản đồ chiến trường
            this.PathSpecificationFile = pathspecificationfile;//get file for specification
            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get textures
            this.GetInformationUnit();// get information about health, power, radius attack, radius detect
                                      // lấy thông tin về producer trong file xml đặc tả
            this.WhomIHit = null;

            // current resource to get
            // cho tài nguyên hiện tài mà producer đang khai thác
            this._currentResourceExploiting = null;

            // max exploiting
            // số lượng khai thác tối đa
            this._maxExploit = 100;
            // speed
            // tốc độ khai thác
            this._speedExploit = 1;
            // current resource center to exploit
            // mõ tài nguyên hiện tại mà producer đang khai thác
            this._currentResourceCenterExploiting = null;
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
            // nếu hiện tại producer đang trong tình trạng khai thác mỏ tài nguyên nào đó _currentResourceCenterExploiting != null
            if (this._currentResourceCenterExploiting != null)
            {
                this.CheckCurrentResourceCenter(); // kiểm tra mõ đó có còn nằm trong phạm vi để khai thác ko
                if (this.CurrentStatus == Status.ATTACK) // nếu đang trong trạng thái khai thác thì tăng số lượng tài nguyên đang đang thác lên -> tăng currentResourceExploiting
                {
                    this.PerformGetReource(); 
                }
            }
            // nếu hiện tại ko trong tình trang khai thác tài nguyên nào
            else if (this._currentResourceCenterExploiting == null)
            {
                this.FindToResourceCenter(); // duyệt các mõ tài nguyên để xem mỏ nào trong phạm vi khai thác mà khai thác mõ đó
            }

            this.FleeIfBeAttacked();
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Explote resource
        /// Khai thác tài nguyên
        /// tăng quatity của tài nguyên hiện tại đang khai thác
        /// </summary>
        public void PerformGetReource()
        {
            if ((System.Environment.TickCount - this._lastTickCountForGetResource) > this._delayTimeGetResource)
            {
                this._lastTickCountForGetResource = System.Environment.TickCount;
                if (this._currentResourceExploiting.Quantity >= this._maxExploit) // nếu đã tăng đến chỉ số max -> ngưng khai thác
                {
                    this._currentResourceCenterExploiting = null;
                    this.MovingVector = Vector2.Zero;
                    this.EndPoint = Point.Zero;
                    this.CurrentStatus = Status.IDLE;
                    this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                    //(return townhall)
                }
                else // nếu  vẫn chưa -> khai thác tiếp
                {
                    this._currentResourceCenterExploiting.ResourceInfo.Quantity -= this._speedExploit;
                    this._currentResourceExploiting.Quantity += this._speedExploit;
                }
            }
        }

        /// <summary>
        /// Check ResourceCenter still in border exploiting
        /// </summary>
        public void CheckCurrentResourceCenter()
        {
            if (!((ResourceCenter)(this._currentResourceCenterExploiting)).BoundRectangle.Intersects(this.BoundRectangle))
            {
                this._currentResourceCenterExploiting = null;
            }
        }

        /// <summary>
        /// find rsource center
        /// </summary>
        public void FindToResourceCenter()
        {
            for (int i = 0; i < ManagerGame._listResourceCenterOnmap.Count; i++)
            {
                if (ManagerGame._listResourceCenterOnmap[i].BoundRectangle.Intersects(this.BoundRectangle))
                {
                    this.EndPoint = Point.Zero;
                    this.MovingVector = Vector2.Zero;
                    this._currentResourceCenterExploiting = (ResourceCenter)ManagerGame._listResourceCenterOnmap[i];
                    this._currentResourceExploiting = new Resource(this._currentResourceCenterExploiting.ResourceInfo.NameRerource, 0);
                    this.CurrentStatus = Status.ATTACK;
                    this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FleeIfBeAttacked()
        {
            //if (this.FlagBeAttacked == true)// if it is arttacked
            //{
            //    // it flee
            //    Random ran = new Random(DateTime.Now.Millisecond);
            //    this.EndPoint = new Point((int)this.Position.X + ran.Next(-100, 0), (int)this.Position.Y + ran.Next(-100,0));
            //    this.CreateMovingVector();                
            //}
        }
        #endregion
    }
}