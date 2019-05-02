// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using SectorDirector.Core.FormatModels.Common;
using SectorDirector.Core.FormatModels.Udmf.Parsing.AbstractSyntaxTree;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing
{
    public static class UdmfParser
    {
        public static IEnumerable<IGlobalExpression> Parse(IEnumerable<Token> tokens)
        {
            using (var tokenStream = tokens.GetEnumerator())
            {
                while (tokenStream.MoveNext())
                {
                    if (tokenStream.Current is IdentifierToken i)
                    {
                        switch (GetNext(tokenStream))
                        {
                            case OpenBraceToken o:
                                yield return ParseBlock(i.Id, tokenStream);
                                break;
                            case EqualsToken e:
                                yield return ParseAssignment(i.Id, tokenStream);
                                break;
                            default:
                                throw CreateError(tokenStream.Current, "open brace or equals");
                        }
                    }
                    else
                    {
                        throw CreateError<IdentifierToken>(tokenStream.Current);
                    }
                }
            }
        }

        static ParsingException CreateError(Token token, string expected)
        {
            if (token == null)
            {
                return new ParsingException("Unexpected end of file");
            }
            return new ParsingException($"Unexpected token {token.GetType().Name} (expected {expected}) on {token.Location}");
        }

        static ParsingException CreateError<TExpected>(Token token) => CreateError(token, typeof(TExpected).Name);

        static Token GetNext(IEnumerator<Token> enumerator) => enumerator.MoveNext() ? enumerator.Current : null;

        static void ExpectNext<TExpected>(IEnumerator<Token> tokenStream)
        {
            var nextToken = GetNext(tokenStream);
            if (!(nextToken is TExpected))
            {
                throw CreateError<TExpected>(nextToken);
            }
        }

        private static Assignment ParseAssignment(Identifier id, IEnumerator<Token> tokenStream)
        {
            var valueToken = GetNext(tokenStream);
            switch (valueToken)
            {
                case IntegerToken i: break;
                case FloatToken f: break;
                case BooleanToken b: break;
                case StringToken s: break;
                default:
                    throw CreateError(valueToken, "value");
            }

            ExpectNext<SemicolonToken>(tokenStream);

            return new Assignment(id, valueToken);
        }

        private static Block ParseBlock(Identifier name, IEnumerator<Token> tokenStream)
        {
            var assignments = new List<Assignment>();

            while (true)
            {
                var token = GetNext(tokenStream);
                switch (token)
                {
                    case IdentifierToken i:
                        ExpectNext<EqualsToken>(tokenStream);
                        assignments.Add(ParseAssignment(i.Id, tokenStream));
                        break;
                    case CloseBraceToken cb:
                        return new Block(name, assignments.ToImmutableArray());
                    default:
                        throw CreateError(token, "identifier or end of block");
                }
            }
        }
    }
}