﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sprint4
{
    public class LevelBuilder
    {
        public Dictionary<string, CollectableFactory.CollectableType> itemDictionary = new Dictionary<string, CollectableFactory.CollectableType>();
        public Dictionary<string, BlockFactory.BlockType> blockDictionary = new Dictionary<string, BlockFactory.BlockType>();
        public Dictionary<string, EnemyFactory.EnemyType> enemyDictionary = new Dictionary<string, EnemyFactory.EnemyType>();
        public Dictionary<string, SpriteFactory.sprites> backgroundDictionary = new Dictionary<string, SpriteFactory.sprites>();
        ISpriteFactory factory;
        BlockFactory blockFactory;
        EnemyFactory enemyFactory;
        CollectableFactory collectableFactory;
        Mario mario;
        int spacingIncrement = 16;
        Level level;

        public LevelBuilder(Level level)
        {
            this.level = level;
            factory = new SpriteFactory();
            blockFactory = new BlockFactory();
            enemyFactory = new EnemyFactory();
            collectableFactory = new CollectableFactory();
            itemDictionary.Add("F", CollectableFactory.CollectableType.fireFlower);
            itemDictionary.Add("C", CollectableFactory.CollectableType.coin);
            itemDictionary.Add("SM", CollectableFactory.CollectableType.superMushroom);
            itemDictionary.Add("1U", CollectableFactory.CollectableType.oneUp);
            itemDictionary.Add("*", CollectableFactory.CollectableType.star);
            backgroundDictionary.Add("bush1", SpriteFactory.sprites.bush1);
            backgroundDictionary.Add("bush2", SpriteFactory.sprites.bush2);
            backgroundDictionary.Add("bush3", SpriteFactory.sprites.bush3);
            backgroundDictionary.Add("exit", SpriteFactory.sprites.exit);
            blockDictionary.Add("Pi", BlockFactory.BlockType.pipe);
            enemyDictionary.Add("K", EnemyFactory.EnemyType.Koopa);
            enemyDictionary.Add("Tdin", EnemyFactory.EnemyType.Dino);
            enemyDictionary.Add("Bill", EnemyFactory.EnemyType.Bill);
            enemyDictionary.Add("Sdin", EnemyFactory.EnemyType.SmashedDino);
            blockDictionary.Add("Wing", BlockFactory.BlockType.winged);
            blockDictionary.Add("X", BlockFactory.BlockType.used);
            blockDictionary.Add("?", BlockFactory.BlockType.question);
            blockDictionary.Add("!", BlockFactory.BlockType.exclamation);
            blockDictionary.Add("B", BlockFactory.BlockType.brick);
            blockDictionary.Add("g", BlockFactory.BlockType.ground);
            blockDictionary.Add("ug", BlockFactory.BlockType.undergroundFloor);
            blockDictionary.Add("ur", BlockFactory.BlockType.undergroundRoof);
            blockDictionary.Add("ul", BlockFactory.BlockType.undergroundLeftWall);
            blockDictionary.Add("ult", BlockFactory.BlockType.undergroundLeftTop);
            blockDictionary.Add("ulb", BlockFactory.BlockType.undergroundLeftBottom);
            blockDictionary.Add("uri", BlockFactory.BlockType.undergroundRightWall);
            blockDictionary.Add("urt", BlockFactory.BlockType.undergroundRightTop);
            blockDictionary.Add("urb", BlockFactory.BlockType.undergroundRightBottom);
            blockDictionary.Add("lpi", BlockFactory.BlockType.leftPipe);
            blockDictionary.Add("dpi", BlockFactory.BlockType.downPipe);
            blockDictionary.Add("l", BlockFactory.BlockType.leftEdge);
            blockDictionary.Add("r", BlockFactory.BlockType.rightEdge);
            blockDictionary.Add("?SM", BlockFactory.BlockType.quesMush);
            blockDictionary.Add("?C", BlockFactory.BlockType.quesCoin);
            blockDictionary.Add("?1U", BlockFactory.BlockType.ques1up);
            blockDictionary.Add("?*", BlockFactory.BlockType.quesStar);
            blockDictionary.Add("?F", BlockFactory.BlockType.quesFlower);
        }

        public Mario Build(string fileName)
        {
            float xCoord =0, yCoord = 0;
            StreamReader sr;
            sr = File.OpenText(Game1.GetInstance().Content.RootDirectory + fileName);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                yCoord+=spacingIncrement;
                xCoord = 0;
                string[] words = line.Split(',');
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i] == "M")
                    {
                        mario = new Mario(new Vector2(xCoord, yCoord));
                    }
                    if(itemDictionary.ContainsKey(words[i]))
                    {
                        ICollectable item = collectableFactory.build(itemDictionary[words[i]], new Vector2(xCoord, yCoord));
                        level.levelItems.Add(item);
                    }
                    if (backgroundDictionary.ContainsKey(words[i]))
                    {
                        if (words[i] == "exit")
                        {
                            level.exitPosition = new Vector2(xCoord, yCoord);
                        }
                        KeyValuePair<IAnimatedSprite, Vector2> item = new KeyValuePair<IAnimatedSprite,
                            Vector2>(factory.build(backgroundDictionary[words[i]]), new Vector2(xCoord, yCoord));
                        level.levelBackgroundObjects.Add(item);
                    }
                    if (blockDictionary.ContainsKey(words[i]))
                    {
                        Block block = blockFactory.build(blockDictionary[words[i]], new Vector2(xCoord, yCoord));
                        level.levelBlocks.Add(block);
                    }
                    if (enemyDictionary.ContainsKey(words[i]))
                    {
                       Enemy enemy = enemyFactory.build(enemyDictionary[words[i]], new Vector2(xCoord, yCoord));
                       level.levelEnemies.Add(enemy);
                    }
                    xCoord+=spacingIncrement;
                }
            }
            return mario;
        }

    }
}
