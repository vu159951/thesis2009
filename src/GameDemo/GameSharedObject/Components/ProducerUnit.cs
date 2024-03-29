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


namespace GameSharedObject.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class ProducerUnit : Unit
    {
        #region Properties
        private Resource _currentResourceExploiting;// tài nguyên hiện tại mà producer đang khai thác và mang trong người(nếu chuyển qua khai thác 1 tài nguyên khác thì tài nguyên hiện thời mất hết)                                                    
        private ResourceCenter _currentResourceCenterExploiting;// mõ tài nguyên hiện tại mà producer đang khai thác                                                                
        private int _maxExploit;// số lượng tài nguyên tối đa mà producer có thể mang                                
        private int _speedExploit;// tốc độ khai thác tài nguyên
                                  

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
        public ResourceCenter CurrentResourceCenterExploiting
        {
            get { return _currentResourceCenterExploiting; }
            set { _currentResourceCenterExploiting = value; }
        }

        private int _delayTimeGetResource = 500;// thời gian trì hoãn giữa 2 lần lấy tài nguyên(thời gian trì hoãn tăng tài nguyên khi lấy)                                                
        private int _lastTickCountForGetResource = System.Environment.TickCount;// biến đếm thời gian dựa vào thời gian trì hoãn
                                                                               
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
            //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get textures
            this.GetInformationUnit();// get information about health, power, radius attack, radius detect
                                      // lấy thông tin về producer trong file xml đặc tả
            this.WhomIHit = null;
            
            // loại tài nguyên hiện tài mà producer đang khai thác
            this._currentResourceExploiting = null;
            
            // số lượng khai thác tối đa
            this._maxExploit = 100;            
            // tốc độ khai thác
            this._speedExploit = 1;            
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
            // nếu hiện tại producer đang trong tình trạng
            // khai thác mỏ tài nguyên nào đó _currentResourceCenterExploiting != null
            if (this._currentResourceExploiting != null && this._currentResourceCenterExploiting != null)
            {
                // kiểm tra mõ đó có còn nằm trong phạm vi để khai thác ko hay đã chạy ra khỏi phạm vi khai thác của mỏ rồi
                if (this.StillInScopeExploit())
                {
                    if (this.CurrentStatus == this.Info.Action[StatusList.EXPLOIT.Name] && this._currentResourceExploiting.Quantity <= int.Parse(((UnitDTO)this.Info).InformationList["MaxExploit"].Value)) // nếu đang trong trạng thái khai thác thì tăng số lượng tài nguyên đang đang thác lên -> tăng currentResourceExploiting
                    {
                        this.PerformGetResource();
                    }
                }
            }
            else if (
                        this._currentResourceCenterExploiting == null   
                        ||(this._currentResourceExploiting == null)// hoặc nếu hiện tại ko có tài nguyên nào mà nó mang trong người
                        || (this._currentResourceExploiting != null && this._currentResourceExploiting.Quantity < int.Parse(((UnitDTO)this.Info).InformationList["MaxExploit"].Value))// hoặc có mang tài nguyên nào đó mà tài nguyên đó chưa đầy
                     )
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
        /// tăng quantity của tài nguyên hiện tại đang khai thác
        /// </summary>
        public void PerformGetResource()
        {
            if ((System.Environment.TickCount - this._lastTickCountForGetResource) > this._delayTimeGetResource)
            {
                this._lastTickCountForGetResource = System.Environment.TickCount;
                // nếu đã tăng đến chỉ số max -> ngưng khai thác
                //if (this._currentResourceExploiting.Quantity >= int.Parse(((UnitDTO)this.Info).InformationList["MaxExploit"].Value))
                //{
                //    // loại tài nguyên này ko còn khả năng mang thêm vào người nữa
                //    this._currentResourceCenterExploiting = null;
                //    // chuyển về trạng thái ko khai thác tài nguyên
                //    this.MovingVector = Vector2.Zero;
                //    this.EndPoint = Point.Zero;
                //    this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];// chuyển trạng thái thành đứng yên
                //    this.CurrentIndex = 0;
                //    //this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// load lại bộ hình
                //    //(return townhall)
                //}
                //else // nếu  vẫn chưa -> khai thác tiếp
                //{
                    //this._currentResourceCenterExploiting.ResourceInfo.Quantity -= int.Parse(((UnitDTO)this.Info).InformationList["SpeedExploit"].Value);// trừ bớt quantity của nguồn tài nguyên
                    //this._currentResourceExploiting.Quantity += int.Parse(((UnitDTO)this.Info).InformationList["SpeedExploit"].Value);// tăng số tài nguyên đang mang trên người producer
                    foreach (KeyValuePair<String,Resource> r in this.PlayerContainer.Resources)
                    {
                        if (r.Value.Name == this._currentResourceExploiting.Name)
                        {
                            this._currentResourceCenterExploiting.ResourceInfo.Quantity -= int.Parse(((UnitDTO)this.Info).InformationList["SpeedExploit"].Value);
                            this.PlayerContainer.Resources[r.Key].Quantity += int.Parse(((UnitDTO)this.Info).InformationList["SpeedExploit"].Value);
                            return;
                        }
                    }
                //}
            }
        }

        /// <summary>
        /// Check ResourceCenter still in border exploiting
        /// kiểm tra mõ tài nguyên có còn trong phạm vi intersect để khai thác không vì có khi producer đã chạy đi chổ khác mà ko khai thác ở
        /// mỏ tài nguyên này nữa.
        /// </summary>
        public Boolean StillInScopeExploit()
        {
            // nếu mỏ tài nguyên ko còn trong phạm vi khai thác
            Point centerpoint = new Point((int)this.Position.X + this.BoundRectangle.Width / 2, (int)this.Position.Y + this.BoundRectangle.Height / 2);// lấy tâm ảnh producer trong tọa độ map
            Rectangle tempResourceCenter = new Rectangle((int)this._currentResourceCenterExploiting.Position.X, (int)this._currentResourceCenterExploiting.Position.Y, (int)(this._currentResourceCenterExploiting.BoundRectangle.Width), (int)(this._currentResourceCenterExploiting.BoundRectangle.Height)); // lấy rectangle của resource center trong tọa độ map
            if (!tempResourceCenter.Contains(centerpoint))
            {
                this._currentResourceCenterExploiting = null;// chuyển thành null
                return false;
            }
            return true;
        }

        /// <summary>
        /// find resource center
        /// lặp để tìm mỏ tài nguyên có rectangle bị intersect để chấp nhận khai thác mỏ tài nguyên này
        /// </summary>
        public void FindToResourceCenter()
        {
            // duyệt trên tất cả các mỏ tài nguyên của game
            Point centerpoint = new Point((int)this.Position.X + this.BoundRectangle.Width / 2, (int)this.Position.Y + this.BoundRectangle.Height / 2);// lấy tâm ảnh của producer trong tọa độ map
            for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap.Count; i++)
            {
                // nếu có mỏ nào có rectangle intersect với producer, nghĩa là mỏ này đã trong phạm vi khai thác
                // -> chấp nhận khai thác mỏ này
                Rectangle tempResourceCenter = new Rectangle((int)GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[i].Position.X, (int)GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[i].Position.Y, (int)(GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[i].BoundRectangle.Width), (int)(GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[i].BoundRectangle.Height)); // lấy rectangle của resource center trong tọa độ map
                if (tempResourceCenter.Contains(centerpoint))
                {
                    // chuyển thành trạng thái khai thác tài nguyên cho producer
                    this._currentResourceCenterExploiting = (ResourceCenter)GlobalDTO.MANAGER_GAME.ListResourceCenterOnmap[i];
                    if (this._currentResourceExploiting != null)// nếu hiện tại nó đang mang 1 tài nguyên nào đó trên người
                    {
                        // nếu đây là mỏ chứa tài nguyên mới so với loại tài nguyên mà nó mang theo bên người
                        if (this._currentResourceExploiting.Name != this._currentResourceCenterExploiting.ResourceInfo.Name)
                        {
                            // khởi tạo tài nguyên hiện tại mà producer sắp khai thác để mang về
                            this.EndPoint = Point.Zero;
                            this.MovingVector = Vector2.Zero;
                            this._currentResourceExploiting = new Resource(this._currentResourceCenterExploiting.ResourceInfo.Name, 0);
                            this.CurrentStatus = this.Info.Action[StatusList.EXPLOIT.Name];// chuyển trạng thái
                            this.CurrentIndex = 0;
                            //this.GetSetOfTexturesForSprite(this.PathSpecificationFile); // load lại bộ hình
                            return;
                        }
                        else// nếu đây vẫn là mỏ chứa tài nguyên mà hiện nó đã mang trên người
                        {
                            // nhưng số lượng chưa đầy
                            if (this._currentResourceExploiting.Quantity < int.Parse(((UnitDTO)this.Info).InformationList["MaxExploit"].Value))
                            {
                                this.EndPoint = Point.Zero;
                                this.MovingVector = Vector2.Zero;
                                this.CurrentStatus = this.Info.Action[StatusList.EXPLOIT.Name];// chuyển trạng thái
                                this.CurrentIndex = 0;
                                //this.GetSetOfTexturesForSprite(this.PathSpecificationFile); // load lại bộ hình
                                return;
                            }
                            else// còn số lượng đã đầy thì ko cần khai thác mỏ này nữa
                            {
                                this._currentResourceCenterExploiting = null;
                                return;
                            }
                        }
                    }
                    else// nếu hiện tại nó chưa có mang theo tài nguyên nào bên mình
                    {
                        this.EndPoint = Point.Zero;
                        this.MovingVector = Vector2.Zero;
                        this._currentResourceExploiting = new Resource(this._currentResourceCenterExploiting.ResourceInfo.Name, 0);
                        this.CurrentStatus = this.Info.Action[StatusList.EXPLOIT.Name];// chuyển trạng thái
                        this.CurrentIndex = 0;
                        //this.GetSetOfTexturesForSprite(this.PathSpecificationFile); // load lại bộ hình
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// tự động bỏ chạy nếu bị đánh vì producer ko có khả năng tấn công lại đối thủ
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