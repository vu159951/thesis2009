using System;
using System.Collections;
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
using GameSharedObject.Components;
using System.Xml;
using GameSharedObject.DTO;
using GameSharedObject.Data;

namespace GameSharedObject
{
    class Computer:Player,IComputer
    {
        private AIDTO _ai;

        public AIDTO Ai
        {
            get { return _ai; }
            set { _ai = value; }
        }

        public Computer(Game game)
            : base(game)
        {
            this._ai = new AIDTO();
        }
        
        public void Init(int idlevel)
        {
            AIDataReader reader = new AIDataReader();
            this._ai = reader.Load(GlobalDTO.SPEC_AI_PATH + GlobalDTO.ACTION_AI + GlobalDTO.SPEC_EXTENSION,idlevel);
        }
        public void Play()
        {
 
        }

        public Computer Clone()
        {
            Computer computer = new Computer(GlobalDTO.GAME);
            return computer;
        }
    }
}
