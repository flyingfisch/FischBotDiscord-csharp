using System;
using System.Collections.Generic;
using FischBot.Helpers;

namespace FischBot.Services.EightBallService
{
    public class EightBallService : IEightBallService
    {
        private static readonly List<(string phrase, int level)> _eightBallResults = new List<(string phrase, int level)>
        {
            ("It is certain.", 1),
            ("It is decidedly so.", 1),
            ("Without a doubt.", 1),
            ("Yes - definitely.", 1),
            ("You may rely on it.", 1),
            ("As I see it, yes.", 1),
            ("Most likely.", 1),
            ("Outlook good.", 1),
            ("Yes.", 1),
            ("Signs point to yes.", 1),
            ("Reply hazy, try again.", 2),
            ("Ask again later.", 2),
            ("Better not tell you now.", 2),
            ("Cannot predict now.", 2),
            ("Concentrate and ask again.", 2),
            ("Don't count on it.", 3),
            ("My reply is no.", 3),
            ("My sources say no.", 3),
            ("Outlook not so good.", 3),
            ("Very doubtful.", 3)
        };

        private static readonly Random _random = new Random();

        public (string phrase, int level) GetEightBallResult()
        {
            var shuffledList = ShuffleTheList();

            var (phrase, level) = shuffledList[_random.Next(0, shuffledList.Count)];

            return (phrase, level);
        }

        // Loop through a random amount of times to scramble our list of possible response for the Magic 8ball game.
        private List<(string phrase, int level)> ShuffleTheList()
        {
            var timesToJumble = _random.Next(2, 10);
            var randomizedList = _eightBallResults;

            for (var i = 0; i < timesToJumble; i++)
            {
                randomizedList = randomizedList.Shuffle().ToList();
            }

            return randomizedList;
        }
    }
}