﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RSPGame.Models;
using RSPGame.UI.Game;
using RSPGame.UI.PlayRequests;

namespace RSPGame.UI.Menus
{
    public class PrivateRoomMenu
    {
        private Session _currentSession;

        private readonly HttpClient _client;

        public PrivateRoomMenu(HttpClient client, Session currentSession)
        {
            _client = client;
            _currentSession = currentSession;
        }

        public Task Start()
        {
            while (true)
            {
                int num;
                Console.WriteLine("1.\tCreate room");
                Console.WriteLine("2.\tJoin room");
                Console.WriteLine("3.\tBack");

                while (true)
                {
                    Console.Write("Enter the number: ");
                    if (!int.TryParse(Console.ReadLine(), out num)) Console.WriteLine("The only numbers can be entered. Try again");
                    else if (num < 1 || num > 3) Console.WriteLine("Incorrect number. Try again");
                    else break;
                }
                Console.WriteLine();
                switch (num)
                {
                    case 1:
                        var json = RoomRequests.CreateRoom(_client, _currentSession.GamerInfo)?.Result;
                        if (json == null) break;
                        var id1 = JsonConvert.DeserializeObject<int>(json);

                        Console.WriteLine($"\nRoom with id {id1} has been created!");
                        Console.WriteLine("\nWaiting for opponent\n\n");

                        var result1 = GameRequests.GetGame(_client, id1)?.ToArray();
                        if (result1 == null) break;

                        var opponent1 = result1
                            .Where(x => !x.Equals(_currentSession.GamerInfo.UserName))?
                            .First();

                        new GameLogic().StartGame(_client, _currentSession.GamerInfo.UserName, opponent1, id1);
                        break;

                    case 2:
                        Console.Write("Enter the id of the desired room: ");

                        if (!int.TryParse(Console.ReadLine(), out var id2))
                        {
                            Console.WriteLine("\nERROR:\tThe only numbers can be entered. Try again\n\n");
                            break;
                        }
                        else if (id2 < 1 || id2 > 1000)
                        {
                            Console.WriteLine("\nERROR:\tIncorrect number. Try again\n\n");
                            break;
                        }
                        
                        if (RoomRequests.JoinRoom(_client, _currentSession.GamerInfo, id2) == null) break;

                        var result2 = GameRequests.GetGame(_client, id2)?.ToArray();
                        var opponent2 = result2
                            .Where(x => !x.Equals(_currentSession.GamerInfo.UserName))?
                            .First();

                        new GameLogic().StartGame(_client, _currentSession.GamerInfo.UserName, opponent2, id2);

                        break;

                    case 3:
                        return Task.CompletedTask;
                }
            }
        }
    }
}