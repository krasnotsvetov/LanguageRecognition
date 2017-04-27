using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LanguageRecognition.Runtime
{
    public class LexicalAnalyzer
    {
        public delegate TokenState TokenAction(LexicalAnalyzer analyzer);

        public enum TokenState
        {
            Nothing,
            Skip
        }


        public string CurrentTokenValue { get; private set; }
        public int CurrentTokenPosition { get; private set; } = 0;
        public int CurrentToken { get; private set; } = -1;
        public string CurrentTokenName { get { return TokensName[CurrentToken]; } }



        private Dictionary<int, Regex> Tokens = new Dictionary<int, Regex>();
        private Dictionary<int, string> TokensName = new Dictionary<int, string>();
        private Dictionary<int, TokenAction> TokensActions = new Dictionary<int, TokenAction>();

        private StreamReader stream;
        private char curChar;
        int eofIndex = -1;

        public LexicalAnalyzer(StreamReader sr, IVocabulary vocabulary)
        {
            stream = sr;
            this.Tokens = vocabulary.Tokens;
            this.TokensName = vocabulary.Names;
            this.TokensActions = vocabulary.Actions;
            eofIndex = TokensName.Count;
            TokensName[eofIndex] = "eof";
        }


        private int _NextToken()
        {
            if (stream.Peek() == -1)
            {
                CurrentTokenValue = null;
                CurrentToken = eofIndex;
                return CurrentToken;
            }

            SortedSet<int> acceptableTokens = new SortedSet<int>();
            Tokens.Keys.ToList().ForEach(t => acceptableTokens.Add(t));

            var sb = new StringBuilder();
            bool isFirstView = true;

            while (acceptableTokens.Count != 0)
            {
                CurrentToken = acceptableTokens.First();
                if (stream.Peek() == -1)
                {
                    break;
                }
                var toRemove = new List<int>();

                sb.Append((char)stream.Peek());
                foreach (var token in acceptableTokens)
                {
                    //TODO : implement a NFA to check for O(1). Now it's O(N) + O(N) for sb.ToString(). 
                    // 1) build a NFA from regex
                    // 2) build common NFA for all regex
                    // 3) test with common NFA
                    if (!Tokens[token].IsMatch(sb.ToString()))
                    {
                        toRemove.Add(token);
                    } 
                }

                if (isFirstView && toRemove.Count == acceptableTokens.Count)
                {
                    CurrentToken = -1;
                    CurrentTokenValue = null;
                    throw new Exception($"Unexcepted symbol at position: {CurrentTokenPosition}, Find: {(char)stream.Peek()}");
                }
                toRemove.ForEach(t => acceptableTokens.Remove(t));

                isFirstView = false;

                if (acceptableTokens.Count != 0)
                {
                    CurrentTokenPosition++;
                    curChar = (char)stream.Read();
                } else
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }

            if (CurrentToken == -1)
            {
                CurrentToken = -1;
                CurrentTokenValue = null;
                throw new Exception($"Unexcepted symbol at position: {CurrentTokenPosition}, Find: {(char)stream.Peek()}");
            }

            if (acceptableTokens.Count > 1)
            {
                Console.WriteLine($"Ambiguity to choose token. Value is {sb.ToString()}, position: {CurrentTokenPosition}");
            }


            CurrentTokenValue = sb.ToString();
            return CurrentToken;
        }

        public int NextToken()
        {
            _NextToken();
            int prevPos = CurrentTokenPosition;
            do
            {
                if (!TokensActions.ContainsKey(CurrentToken))
                {
                    break;
                }
                prevPos = CurrentTokenPosition;
                switch (TokensActions[CurrentToken]?.Invoke(this))
                {
                    case TokenState.Skip:
                        _NextToken();
                        break;
                    case TokenState.Nothing:
                        break;
                }
            } while (prevPos != CurrentTokenPosition);

            return CurrentToken;
        }
    }
}
