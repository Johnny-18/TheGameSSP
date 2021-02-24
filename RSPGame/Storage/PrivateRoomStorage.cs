﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RSPGame.Models;

namespace RSPGame.Storage
{
    public class PrivateRoomStorage
    {
        private static readonly List<Room> ListRooms
            = new List<Room>();
        private static readonly object Locker = new object();

        public async Task CreateRoom(GamerInfo gamer)
        {
            if (gamer == null)
                throw new ArgumentNullException(nameof(gamer));

            var room = new Room();
            await room.AddGamer(gamer);
            Console.WriteLine("room`s id:\t" + room.GetId());

            bool acquiredLock = false;
            try
            {
                Monitor.Enter(Locker, ref acquiredLock);
                ListRooms.Add(room);
            }
            finally
            {
                if (acquiredLock) Monitor.Exit(Locker);
            }
        }

        public async Task JoinRoom(GamerInfo gamer, int id)
        {
            if (gamer == null)
                throw new ArgumentNullException(nameof(gamer));

            bool acquiredLock = false;
            try
            {
                Monitor.Enter(Locker, ref acquiredLock);

                var room = ListRooms
                    .FirstOrDefault(x => x.GetId() == id);
                if (room == null)
                {
                    Console.WriteLine("\nNo rooms with this id found!\n");
                    return;
                }
                await room.AddGamer(gamer);

                ListRooms.Remove(room);
            }
            finally
            {
                if (acquiredLock) Monitor.Exit(Locker);
            }
        }
    }
}
