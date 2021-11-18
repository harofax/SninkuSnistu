using System;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public static class LevelLoader
{
    private const string LevelLoaderLogFile = "level_loader.log";
    public struct LevelData
    {
        public char[][] Data { get; }

        public Vector3Int BoundingBox { get; }

        public LevelData(char[][] data, Vector3Int boundingBox)
        {
            this.Data = data;
            this.BoundingBox = boundingBox;
        }
    }
    public static LevelData ParseLevelData(string fileName, string path)
    {
        int xMax = -1;
        int yMax = -1;

        try
        {
            string[] rows;

            string fullPath = Path.Combine(path, fileName);

            if (File.Exists(fullPath))
            {
                rows = File.ReadAllLines(fullPath);
            }
            else
            {
                throw new ArgumentException("Failed to open " + fullPath);
            }
            
            int zMax = rows.Length;
            char[][] level = new char[zMax][];
            

            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                
                if (row.Length > xMax) xMax = row.Length;
                
                level[i] = row.ToCharArray();
                
                foreach (var tileChar in row)
                {
                    int height;
                    try
                    {
                        height = int.Parse(tileChar.ToString());
                    }
                    catch (FormatException e)
                    {
                        string logPath = Path.Combine(path, LevelLoaderLogFile);
                        using StreamWriter logFile = File.CreateText(logPath);
                        logFile.WriteLine("The character " + tileChar + " is not allowed in the level file.");
                        logFile.WriteLine("Please only use integers when making a level.");
                        logFile.WriteLine(e);
                        throw;
                    }
                    if (height > yMax) yMax = height;
                }
                
            }

            Vector3Int boundingBox = new Vector3Int(xMax, yMax, zMax);

            return new LevelData(level, boundingBox);
        }
        catch (System.Exception e)
        {
            string logPath = Path.Combine(path, LevelLoaderLogFile);
            
            using (StreamWriter logFile = File.CreateText(logPath))
            {
                logFile.WriteLine("The level file " + Path.Combine(path, fileName) +
                                  " could not be loaded.");
                logFile.WriteLine(e);
            }
            throw;
        }
    }
}