using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Components;
using Microsoft.Xna.Framework;
using GameSharedObject.DTO;
using System.IO;
using System.Reflection;

namespace GameSharedObject.Factory
{
    public class ComputerPlayerManager
    {
        protected String ASM_EXTENSION = ".dll";
        protected String NS = "GameComputer";
        public enum ComputerLevel
        {
            EASY,
            MEDIUM,
            HARD,
            VERYHARD
        }


        private Player[] _players;
        public Player Players(ComputerLevel level)
        {
            switch (level)
            {
                case ComputerLevel.MEDIUM:
                    return _players[1];
                case ComputerLevel.HARD:
                    return _players[2];
                case ComputerLevel.VERYHARD:
                    return _players[3];
                default:
                    return _players[0];
            }
        }


        public ComputerPlayerManager(Game game)
        {
            _players = new Player[4];
            this.Load(game);
        }

        private void Load(Game game)
        {
            string[] files = Directory.GetFiles(GlobalDTO.AI_ACTION_PATH, "*" + this.ASM_EXTENSION, SearchOption.TopDirectoryOnly);

            foreach (string dllFile in files){
                Assembly asm = Assembly.LoadFrom(dllFile);
                String name = Path.GetFileNameWithoutExtension(dllFile);
                Type t = asm.GetType(this.NS + "." + name);
                // BindingFlags enumeration specifies flags that control binding and 
                // the way in which the search for members and types is conducted by reflection. 
                // The following specifies the Access Control of the bound type
                BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public
                    | BindingFlags.NonPublic | BindingFlags.Instance;
                // Construct an instance of the type and invoke the member method
                Object obj = t.InvokeMember(name, bflags |
                    BindingFlags.CreateInstance, null, null,
                    new object[]{
                    game}
                    );
                Player player = (Player)obj;

                switch(name.ToUpper()){
                    case "EASY":
                        _players[0] = player;
                        break;
                    case "MEDIUM":
                        _players[1] = player;
                        break;
                    case "HARD":
                        _players[2] = player;
                        break;
                    case "VERYHARD":
                        _players[3] = player;
                        break;
                }
            }
        }
    }
}
