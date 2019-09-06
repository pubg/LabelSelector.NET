using System;

namespace LabelSelector
{
    public class TokenConsumer
    {
        private readonly Token[] _tokens;
        private int _index;

        public bool HasNextToken => _tokens.Length - 1 > _index;
        public bool HasToken => _tokens.Length > _index;

        public TokenConsumer(Token[] tokens)
        {
            _tokens = tokens;
            _index = 0;
        }

        public Token EatToken(TokenType expectedTokenType)
        {
            var token = _tokens[_index];
            if (token.TokenType != expectedTokenType)
            {
                throw new Exception($"You expected token {expectedTokenType}, but it is token {token}");
            }

            _index += 1;
            return token;
        }

        public T EatToken<T>() where T : Token
        {
            var token = _tokens[_index];

            if (!(token is T castedToken))
            {
                var expectedTokenType = Token.TokenTypeEnumMap[typeof(T)];
                throw new Exception($"You expected token {expectedTokenType}, but it is token {token}");
            }

            _index += 1;
            return castedToken;
        }

        public Token PeekToken()
        {
            return _tokens[_index];
        }
    }
}