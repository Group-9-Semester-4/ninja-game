using System;
using System.IO;
using APIClient.Models;
using UnityEngine;

namespace Game
{
    public class GameService
    {
        private static GameService _instance;

        public static GameService Instance => _instance ??= new GameService();

        public byte[] GetCardImage(CardResource card)
        {
            var id = card.id;

            var folder = ImageSavePath();
            var filePath = folder + id;

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            else
            {
                throw new NullReferenceException("File not cached");
            }
        }

        public string ImageSavePath()
        {
            return Application.persistentDataPath + "/card-images/";
        }
    }
}