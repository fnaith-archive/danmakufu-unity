using System;

namespace Gstd
{
    namespace GstdUtility
    {
        sealed class Token
        {
            private TokenType type;
            private string element = "";
            private int posStart;
            private int posEnd;
            public Token()
            {
                type = TokenType.TK_UNKNOWN;
                posStart = 0;
                posEnd = 0;
            }
            public Token(TokenType type, string element, int start, int end)
            {
                this.type = type;
                this.element = element;
                posStart = start;
                posEnd = end;
            }
            public TokenType GetTokenType()
            {
                return type;
            }
            public string GetElement()
            {
                return element;
            }
            /*std::string GetElementA();

            int GetStartPointer(){return posStart_;}
            int GetEndPointer(){return posEnd_;}

            int GetInteger();
            double GetReal();
            bool GetBoolean();*/
            public string GetString()
            {
                if (type != TokenType.TK_STRING)
                {
                    throw new Exception("Token::GetString:");//�f�[�^�̃^�C�v���Ⴂ�܂�");
                }
                return element.Substring(1, element.Length - 2);
            }
            public string GetIdentifier()
            {
                if (type != TokenType.TK_ID)
                {
                    throw new Exception("Token::GetIdentifier:");//�f�[�^�̃^�C�v���Ⴂ�܂�");
                }
                return element;
            }

            /*std::string GetStringA();
            std::string GetIdentifierA();*/
        }
    }
}
