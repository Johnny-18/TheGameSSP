﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoomPrototype
{
    class Program
    {
        static async Task Main()
        {
            //var roomWithId = new RoomPrototype(44444);
            var room = new RoomPrototype();

            await room.AddGamer(new GameInfo());
            //Task.Run(() => room.AddGamer(new GameInfo()));

           Console.ReadKey();
        }
    }
}
