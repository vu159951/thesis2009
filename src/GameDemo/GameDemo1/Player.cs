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
using GameDemo1.Components;
using GameDemo1.DTO;


namespace GameDemo1
{
                        /// <summary>
                        /// struct mô tả các unit và structure của game, thể hiện việc player có được mua hay xây dựng trong thời điểm hiện tại ko
                        /// </summary>
                        public struct UnitOfGame
                        {
                            public Boolean flag;// true -> player được mua. false -> hiện tại player ko được mua
                            public Sprite unitOfGame;// thành phần để clone ra
                        }
                        public struct StructureOfGame
                        {
                            public Boolean flag;// true -> player được xây. false -> hiện tại player ko được xây
                            public Sprite structureOfGame;// thành phần để clone ra
                        }




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
        private List<Sprite> _units; // list of units,are possessive units of this player // tập các unit thuộc player này
        private List<Sprite> _structures;// list of structures, are possessive structures of this player // tập các structure thuộc player này
        private List<Resource> _resources;// list of resources which this player have // tập các tài nguyên hiện player đang có
        private List<UnitOfGame> _listUnitOfGameInPlayer; // list các unit của game được miêu tả dưới struct, nếu thành phần flag trong struct là true nghĩa là player được phép mua loại lính này trong thời điểm hiện tại
        private List<StructureOfGame> _listStructureOfGameInPlayer; // list các structure của game được miêu tả dưới struct, nếu thành phần flag trong struct là true nghĩa là player được phép xây loại structure này trong thời điểm hiện tại

        public List<StructureOfGame> ListStructureOfGameInPlayer
        {
            get { return _listStructureOfGameInPlayer; }
            set { _listStructureOfGameInPlayer = value; }
        }
        public List<UnitOfGame> ListUnitOfGameInPLayer
        {
            get { return _listUnitOfGameInPlayer; }
            set { _listUnitOfGameInPlayer = value; }
        }
        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
        public List<Sprite> Structures
        {
            get { return _structures; }
            set { _structures = value; }
        }
        public List<Sprite> Units
        {
            get { return _units; }
            set { _units = value; }
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
            this._structures = new List<Sprite>();
            this._units = new List<Sprite>();

            // resource
            // tài nguyên hiện có
            this._resources = new List<Resource>();
            this._resources.Add(new Resource("Stone", 10000));
            this._resources.Add(new Resource("Gold", 10000));           

            // khởi tạo list các unit và structure của game trong player
            this._listStructureOfGameInPlayer = new List<StructureOfGame>();
            DirectoryInfo dir = new DirectoryInfo(GlobalDTO.SPEC_STRUCTURE_PATH);
            foreach (FileInfo f in dir.GetFiles())
            {
                StructureOfGame temp;
                temp.flag = true;
                temp.structureOfGame = new Structure(game, GlobalDTO.SPEC_STRUCTURE_PATH + f.Name, Vector2.Zero, 0);
                this._listStructureOfGameInPlayer.Add(temp);
            }
            this._listUnitOfGameInPlayer = new List<UnitOfGame>();
            dir = new DirectoryInfo(GlobalDTO.SPEC_UNIT_PATH);
            foreach (FileInfo f in dir.GetFiles())
            {
                UnitOfGame temp;
                temp.flag = true;
                temp.unitOfGame = new Unit(game, GlobalDTO.SPEC_UNIT_PATH + f.Name, Vector2.Zero, 0);
                this._listUnitOfGameInPlayer.Add(temp);
            }
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
            for (int i = 0; i < structure.RequirementResource.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (structure.RequirementResource[i].NameRerource == this._resources[j].NameRerource)
                    {
                        if (structure.RequirementResource[i].Quantity > this._resources[j].Quantity)
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
            for (int i = 0; i < structure.RequirementResource.Count; i++)
            {
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (structure.RequirementResource[i].NameRerource == this._resources[j].NameRerource)
                    {
                        this._resources[j].Quantity -= structure.RequirementResource[i].Quantity;// giảm tài nguyên
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
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (unit.RequirementResources[i].NameRerource == this._resources[j].NameRerource)
                    {
                        if (unit.RequirementResources[i].Quantity > this._resources[j].Quantity)
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
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (unit.RequirementResources[i].NameRerource == this._resources[j].NameRerource)
                    {
                        this._resources[j].Quantity -= unit.RequirementResources[i].Quantity;// giảm tài nguyên
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
                for (int j = 0; j < this._resources.Count; j++)
                {
                    if (unit.RequirementResources[i].NameRerource == this._resources[j].NameRerource)
                    {
                        this._resources[j].Quantity += unit.RequirementResources[i].Quantity;// giảm tài nguyên
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
        /// update lại cờ của các unit và structure trong game được lưu trong player
        ///     - tham số cuối là true, bật cờ thành true
        ///     - tham số cuối là false, bật cờ thành false
        /// </summary>
        /// <param name="name"></param>
        public void UpdateFlagOfListUnitAndStructureOfGameInPlayer(string[] names, Boolean onoff)
        {
            for (int j = 0; j<names.Length; j++)
            {
                for (int i = 0; i < this._listUnitOfGameInPlayer.Count; i++)
                {
                    if (this._listUnitOfGameInPlayer[i].unitOfGame.Name == names[j])
                    {
                        UnitOfGame temp;
                        temp.flag = onoff;
                        temp.unitOfGame = this._listUnitOfGameInPlayer[i].unitOfGame;
                        this._listUnitOfGameInPlayer.RemoveAt(i);
                        this._listUnitOfGameInPlayer.Add(temp);
                    }
                }
                for (int i = 0; i < this._listUnitOfGameInPlayer.Count; i++)
                {
                    if (this._listUnitOfGameInPlayer[i].unitOfGame.Name == names[j])
                    {
                        StructureOfGame temp;
                        temp.flag = onoff;
                        temp.structureOfGame = this._listStructureOfGameInPlayer[i].structureOfGame;
                        this._listStructureOfGameInPlayer.RemoveAt(i);
                        this._listStructureOfGameInPlayer.Add(temp);
                    }
                }
            }
        }
        #endregion
    }
}