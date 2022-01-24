using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cunning_Linguist
{
    public class Linguist
    {
        private Dictionary<string, int> _words;

        public Linguist()
        {
            _words = ReadWordsFromFile("wordlist.txt");
            Score();
        }

        public void Process(string[] FixedLetters, string GoodLetters, string BadLetters)
        {
            var fixedLettersString = string.Join("", FixedLetters);
            foreach (var word in _words)
            {
                _words[word.Key] = 1;
            }

            if (string.IsNullOrWhiteSpace(GoodLetters) && string.IsNullOrWhiteSpace(BadLetters) && string.IsNullOrWhiteSpace(fixedLettersString))
            {
                Score();
                return;
            }

            var knownTokens = GoodLetters.ToLower().OrderBy(x => x).Distinct().ToList();
            var badTokens = BadLetters.ToLower().OrderBy(x => x).Distinct().Where(w => !knownTokens.Contains(w)).ToList();

            foreach (var token in badTokens)
            {
                foreach (var word in _words.Where(w => w.Value > 0))
                {
                    if (word.Key.Contains(token))
                    {
                        _words[word.Key] = 0;
                    }
                }
            }

            foreach (var token in knownTokens)
            {
                foreach (var word in _words.Where(w => w.Value > 0))
                {
                    if (!word.Key.Contains(token))
                    {
                        _words[word.Key] = 0;
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (string.IsNullOrEmpty(FixedLetters[i]))
                {
                    continue;
                }

                foreach (var word in _words.Where(w => w.Value > 0))
                {
                    var wordToken = word.Key[i].ToString();
                    if (wordToken != FixedLetters[i])
                    {
                        _words[word.Key] = 0;
                    }
                    else
                    {
                        _words[word.Key] = 1;
                    }
                }
            }

            Score();
        }

        private void Score()
        {
            var words = _words.Where(w => w.Value > 0).ToList();
            var tokenScores = new Dictionary<char, int>();

            foreach (var word in words)
            {
                foreach (var token in word.Key)
                {
                    if (tokenScores.ContainsKey(token))
                    {
                        tokenScores[token]++;
                    }
                    else
                    {
                        tokenScores[token] = 1;
                    }
                }
            }

            foreach (var word in words)
            {
                var wordScore = 1;
                var usedTokens = "";
                foreach (var token in word.Key)
                {
                    if (!usedTokens.Contains(token))
                    {
                        wordScore += tokenScores[token];
                        usedTokens += token;
                    }
                }

                _words[word.Key] = wordScore;
            }
        }

        public string GetSuggestedWord()
        {
            var remainingWords = _words.Where(w => w.Value > 0).OrderByDescending(w => w.Value).ToList();

            if (remainingWords.Count == 0)
            {
                return "None!";
            }
            else
            {
                return remainingWords[0].Key;
            }
        }

        public string GetAllSuggestedWords()
        {
            return string.Join(", ", _words.Where(w => w.Value > 0).OrderByDescending(w => w.Value).Skip(1).Select(w => w.Key));
        }

        private Dictionary<string, int> ReadWordsFromFile(string fileName)
        {
            var words = File.ReadAllLines(fileName);
            var dictionary = new Dictionary<string, int>();

            foreach (var word in words)
            {
                dictionary[word] = 1;
            }

            return dictionary;
        }
    }
}