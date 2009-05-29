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
        private Vector2 _movingVector; // moving vector for moving Npc
        private int _maxHealth; // health of Npc
        private int _currentHealth;    // current health of this unit
        private int _power;// power of Ncp
        private int _radiusDetect; // radius to detect members of faction 
        private int _radiusAttack; // radius to attack members of faction
        private int _speed;// speed to move
        private Point _endPoint;// end point to move
        private Sprite _whomIHit = null;// current opposition which unit or structure is hitted by this unit
        private Level _level;// current level of unit
        private List<Resource> _requirementResource;// list resources which require to create this unit

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

        private int _delayTimeToChangeImage = 70;// delay time to change image
        private int _delayTimeToMove = 20;// delay time to move
        private int _delayTimeToDecreaseHealth_WhomIHit = 350;// delay time to move
        private int lastTickCountForMove = System.Environment.TickCount;// counter delay time for moving Npc
        private int lastTickCountForChangeImage = System.Environment.TickCount;// counter delay time for change texture
        private int lastTickCountForDecreaseHealth_WhomIHit = System.Environment.TickCount;// counter delay time for decrease and die
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
            if (this.CurrentIndex != -1)
            {
                this.PerformAction();
            }

            /// perform move Npc
            if (this.CurrentStatus == Status.WALK || this.CurrentStatus == Status.FLY && (this._movingVector != Vector2.Zero))
            {
                this.Move();
                this.CantMoveAtBorder();
            }
            

            /// detect members of another faction to attack
            if (this._whomIHit == null) // if don't have opposition
            {
                this.SearchForAttack();
            }
            else if (this._whomIHit != null) // if having opposition
            {
                this.CheckOpposition();
                this.DecreaseHealthOf_WhomIHit();
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
        /// </summary>
        public void GetInformationUnit()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.PathSpecificationFile);
            this._maxHealth = int.Parse(doc.SelectSingleNode("//MaxHealth[1]").Attributes[0].Value);
            this._currentHealth = this._maxHealth;
            this._power = int.Parse(doc.SelectSingleNode("//Power[1]").Attributes[0].Value);
            this._radiusAttack = int.Parse(doc.SelectSingleNode("//RadiusAttack[1]").Attributes[0].Value);
            this._radiusDetect = int.Parse(doc.SelectSingleNode("//RadiusDetect[1]").Attributes[0].Value);
            this._speed = int.Parse(doc.SelectSingleNode("//Speed[1]").Attributes[0].Value);
            //get set of resource which require to build this resource
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
            if ((System.Environment.TickCount - this.lastTickCountForMove) > this._delayTimeToMove)
            {
                this.lastTickCountForMove = System.Environment.TickCount;
                if (this._endPoint != Point.Zero)
                {
                    this.Position += this._movingVector; // moving with vector
                    Rectangle temp1 = new Rectangle(this.BoundRectangle.X + (int)this.CurrentRootCoordinate.X, this.BoundRectangle.Y + (int)this.CurrentRootCoordinate.Y, this.BoundRectangle.Width, this.BoundRectangle.Height);
                    Rectangle temp2 = new Rectangle(this._endPoint.X, this._endPoint.Y, this.BoundRectangle.Width / 2, this.BoundRectangle.Height / 2);
                    if (temp1.Intersects(temp2))
                    {
                        this._movingVector = Vector2.Zero;
                        this._endPoint = Point.Zero;
                        this.CurrentStatus = Status.IDLE;
                        this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                    }
                }
            }
        }

        /// <summary>
        /// Chamge moving vector to move Npc
        /// </summary>
        public void CantMoveAtBorder()
        {
            Transform transform = Transform.CURRENT;// get transform
            Point cell1 = transform.PointToCell(Transform.Vector2ToPoint(this.Position));// cell contain postion(top,left) of this
            Vector2 sizetexture = new Vector2(this.TextureSprites[this.CurrentIndex].Width, this.TextureSprites[this.CurrentIndex].Height);// size of current texture of its
            Point cell2 = transform.PointToCell(Transform.Vector2ToPoint(this.Position + sizetexture));// cell contain postion(bottom,right) of this

            if (cell1.X < 1 || cell2.X >= Config.MAP_SIZE_IN_CELL.Width - 2 || cell1.Y < 1 || cell2.Y >= Config.MAP_SIZE_IN_CELL.Height - 2)
            {
                this.CurrentStatus = Status.IDLE;
                this._movingVector = Vector2.Zero;
                this.GetSetOfTexturesForSprite(this.PathSpecificationFile);// get new set of texture
            }
        }

        /// <summary>
        /// Create moving vector for this unit
        /// </summary>
        public void CreateMovingVector()
        {
            // calculate moving vector
            this._movingVector = GlobalFunction.CreateMovingVector(this._endPoint, this.Position, this._speed);

            // change status
            if (this.Name == "Angel" || this.Name == "Phoenix")
            {
                this.CurrentStatus = Status.FLY;
            }
            else
            {
                this.CurrentStatus = Status.WALK;
            }
            // change texture
            float x = this._endPoint.X - (int)this.Position.X;
            float y = this._endPoint.Y - (int)this.Position.Y;
            float degree = MathHelper.ToDegrees((float)Math.Atan2(x, y));
            this.ChangeDirectionByDegree(degree);
            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);

            /// change endpoint
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
        /// </summary>
        public void PerformAction()
        {
            if ((System.Environment.TickCount - this.lastTickCountForChangeImage) > this._delayTimeToChangeImage)
            {
                this.lastTickCountForChangeImage = System.Environment.TickCount;
                this.CurrentIndex++;
                if (this.CurrentStatus == Status.DEAD)// if it die
                {
                    if (this.CurrentIndex == this.TextureSprites.Count)
                    {
                        this.CurrentIndex = this.TextureSprites.Count - 1;
                        this.Dispose(true);
                    }
                }
                else
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
        /// </summary>
        public void SearchForAttack()
        {
            for (int i = 0; i < ManagerGame._listUnitOnMap.Count; i++)
            {
                Sprite anotherunit = ManagerGame._listUnitOnMap[i];
                if (this.AttackOpposition(anotherunit))
                {
                    this._endPoint = Point.Zero;
                    this._whomIHit = anotherunit;
                    return;
                }
            }

            for (int i = 0; i < ManagerGame._listStructureOnMap.Count; i++)
            {
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
        /// </summary>
        /// <param name="anotherunit"></param>
        public Boolean AttackOpposition(Sprite opposition)
        {
            if (opposition.CodeFaction != this.CodeFaction) // if anotherunit is member of another faction, detect and attack
            {
                Vector2 locationCoordinate1 = new Vector2(this.Position.X + (int)(this.TextureSprites[0].Width / 2), this.Position.Y + (int)(this.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
                Vector2 locationCoordinate2 = new Vector2(opposition.Position.X + (int)(opposition.TextureSprites[0].Width / 2), opposition.Position.Y + (int)(opposition.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
                int r = Convert.ToInt32(Math.Sqrt(Math.Pow((locationCoordinate1.X - locationCoordinate2.X), 2) + Math.Pow((locationCoordinate1.Y - locationCoordinate2.Y), 2)));// get distance between this and anotherunit
                if (r <= this._radiusAttack)// if being in attacked area
                {
                    if (this.CurrentStatus != Status.DEAD)
                    {
                        this.CurrentStatus = Status.ATTACK;// switch to ATTACK status
                        // decide detection to attack
                        Vector2 attacvector = locationCoordinate2 - locationCoordinate1;
                        float degree = MathHelper.ToDegrees((float)Math.Atan2(attacvector.X, attacvector.Y));
                        this.ChangeDirectionByDegree(degree);
                        this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                        return true;
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
        /// </summary>
        public void CheckOpposition()
        {
            Vector2 locationCoordinate1 = new Vector2(this.Position.X + (int)(this.TextureSprites[0].Width / 2), this.Position.Y + (int)(this.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
            Vector2 locationCoordinate2 = new Vector2(this._whomIHit.Position.X + (int)(this._whomIHit.TextureSprites[0].Width / 2), this._whomIHit.Position.Y + (int)(this._whomIHit.TextureSprites[0].Height / 2));// get coordinate for location of anotherunit
            int r = Convert.ToInt32(Math.Sqrt(Math.Pow((locationCoordinate1.X - locationCoordinate2.X), 2) + Math.Pow((locationCoordinate1.Y - locationCoordinate2.Y), 2)));// get distance between this and anotherunit
            if (r > this._radiusAttack)// if not being in attacked area
            {
                if (this.CurrentStatus == Status.ATTACK)
                {
                    this.CurrentStatus = Status.IDLE;// switch to IDLE status
                    this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                }
                this._whomIHit = null;
                return;
            }
        }

        /// <summary>
        /// Decrease health of sprite which this unit attacked
        /// </summary>
        public void DecreaseHealthOf_WhomIHit()
        {
            if ((System.Environment.TickCount - this.lastTickCountForDecreaseHealth_WhomIHit) > this._delayTimeToDecreaseHealth_WhomIHit)
            {
                this.lastTickCountForDecreaseHealth_WhomIHit = System.Environment.TickCount;

                try
                {
                    if (this._whomIHit is Unit) // is unit
                    {
                        ((Unit)this._whomIHit).CurrentHealth -= this._power;
                        if (((Unit)this._whomIHit).CurrentHealth <= 0)
                        {
                            this._whomIHit.CurrentStatus = Status.DEAD;
                            this._whomIHit.GetSetOfTexturesForSprite(this._whomIHit.PathSpecificationFile);
                            ((Unit)this._whomIHit).WhomIHit = null;
                            ManagerGame._listUnitOnMap.Remove(this._whomIHit);
                            this._whomIHit = null;
                            this.CurrentStatus = Status.IDLE;
                            this.GetSetOfTexturesForSprite(this.PathSpecificationFile);
                        }
                    }
                    else // is structure
                    {
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