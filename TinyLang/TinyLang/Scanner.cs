using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    Int, Float, String, Real,
    Repeat, Write, Read,
    If, Elseif, Else, Then, Until, Endl, Return, End, Main,
    LCurlyBrace, RCurlyBrace, LParanthesis, RParanthesis, Semicolon, PlusOp, MinusOp, DivideOp, MultiplyOp,
    EqualOp, AssignOp, AndOp, OrOp, GreaterThOp, SmallerThOp, Comma, Constant, Identifier,
     NotEqualOp, Dot , comment ,dblQ ,CommentOpen,commentClose
}

namespace TinyLang
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("real", Token_Class.Real);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("main", Token_Class.Main);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.SmallerThOp);
            Operators.Add(">", Token_Class.GreaterThOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("!", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("{", Token_Class.LCurlyBrace);
            Operators.Add("}", Token_Class.RCurlyBrace);
            Operators.Add(":=", Token_Class.AssignOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("/*", Token_Class.CommentOpen);
            Operators.Add("*/", Token_Class.commentClose);
            Operators.Add("\"", Token_Class.dblQ);



        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                //-------------------------------------------------------------------------
                // (identifier)
                if (CurrentChar >= 'A' && CurrentChar <= 'z')
                {
                    j++;
                    while (Char.IsLetterOrDigit(SourceCode[j]))
                    {

                        CurrentLexeme += SourceCode[j];
                        j++;

                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                //-------------------------------------------------------------------------
                // (Constant)
                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j++;
                    while (Char.IsNumber(SourceCode[j]) || SourceCode[j] == '.' || Char.IsLetter(SourceCode[j]))

                    {
                        CurrentLexeme += SourceCode[j];
                        j++;
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }


                //-------------------------------------------------------------------------


                //-------------------------------------------------------------------------
                // (assign operator)
                else if (CurrentChar == ':')
                {
                    if ((SourceCode.Length - 1) - i == 0) Errors.Error_List.Add(CurrentLexeme);
                    else
                    {
                        j++;
                        if (SourceCode[j] == '=')
                        {
                            CurrentLexeme += SourceCode[j];
                            i = j;
                        }
                        else i = j - 1;
                        FindTokenClass(CurrentLexeme);
                    }

                }
                //-------------------------------------------------------------------------
                // (or operator)
                else if (CurrentChar == '|')
                {
                    if ((SourceCode.Length - 1) - i == 0) Errors.Error_List.Add(CurrentLexeme);
                    else
                    {
                        j++;
                        if (SourceCode[j] == '|')
                        {
                            CurrentLexeme += SourceCode[j];
                            i = j;
                        }
                        else i = j - 1;
                        FindTokenClass(CurrentLexeme);
                    }
                }
                //-------------------------------------------------------------------------
                // (and operator)
                else if (CurrentChar == '&')
                {
                    if ((SourceCode.Length - 1) - i == 0) Errors.Error_List.Add(CurrentLexeme);
                    else
                    {
                        j++;
                        if (SourceCode[j] == '&')
                        {
                            CurrentLexeme += SourceCode[j];
                            i = j;
                        }
                        else i = j - 1;
                        FindTokenClass(CurrentLexeme);
                    }
                }
                //-------------------------------------------------------------------------
                // (not operator)
                else if (CurrentChar == '<')
                {
                    if ((SourceCode.Length - 1) - i == 0) Errors.Error_List.Add(CurrentLexeme);
                    else
                    {
                        j++;
                        if (SourceCode[j] == '>' || SourceCode[j] == '=')
                        {
                            CurrentLexeme += SourceCode[j];
                            i = j;
                        }
                        else i = j - 1;
                        FindTokenClass(CurrentLexeme);
                    }
                }
                //-------------------------------------------------------------------------
                // (great than or equal) (error)
                else if (CurrentChar == '>')
                {
                    if ((SourceCode.Length - 1) - i == 0) Errors.Error_List.Add(CurrentLexeme);
                    else
                    {
                        j++;
                        if (SourceCode[j] == '<' || SourceCode[j] == '=')
                        {
                            CurrentLexeme += SourceCode[j];
                            i = j;
                        }
                        else i = j - 1;
                        FindTokenClass(CurrentLexeme);
                    }
                }
                //-------------------------------------------------------------------------
                // .01
                else if (CurrentChar == '.') {
                    j++;
                    while (Char.IsLetterOrDigit(SourceCode[j])) {
                        CurrentLexeme += SourceCode[j];
                        j++;
                    }
                    i= j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                //-------------------------------------------------------------------------

                // (string)
                else if (CurrentChar == '\"')
                {
                    bool flag = false;

                    j++;
                    while (SourceCode[j] != '\"')
                    {
                        if (SourceCode[j] == '\n')
                        {
                            flag = true;
                            break;
                        }
                        CurrentLexeme += SourceCode[j];
                        j++;
                    }
                    if (flag)
                    {
                        Errors.Error_List.Add(CurrentLexeme);

                    }
                    else
                    {
                        CurrentLexeme += '\"';
                        FindTokenClass(CurrentLexeme);

                    }
                    i = j;
                }
                //-------------------------------------------------------------------------
                // error
                else
                {
                    if (CurrentChar == '/') {

                        if (SourceCode[j + 1] == '*')
                        {
                            j++;
                            while (!CurrentLexeme.Contains("*/"))
                            {
                                CurrentLexeme += SourceCode[j];
                                j++;
                            }
                            i = j - 1;
                            FindTokenClass(CurrentLexeme);
                        }
                        else {
                            FindTokenClass(CurrentLexeme);

                        }
                    }
                    else
                        FindTokenClass(CurrentLexeme);
                }
                //-------------------------------------------------------------------------

            }

            TinyLand_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            
            Token Tok = new Token();
            Token Tok1 = new Token();
            Token Tok2 = new Token();


            Tok.lex = Lex;
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            else if (IsString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tok.lex = Lex.Replace("\"", String.Empty);
                Tokens.Add(Tok);
                Tok1.token_type = Token_Class.dblQ;
                Tok1.lex = "\"";
                Tokens.Add(Tok1);
                Tokens.Add(Tok1);


            }

            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identifier;
                Tokens.Add(Tok);
            }

            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }

            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
            }

            else if (IsComment(Lex)) {
                Tok.token_type = Token_Class.comment;
                Tok.lex = Lex.Replace("/*", String.Empty);
                Tok.lex = Tok.lex.Replace("*/", String.Empty);
                Tokens.Add(Tok);
                Tok1.token_type = Token_Class.CommentOpen;
                Tok1.lex = "/*";
                Tok2.token_type = Token_Class.commentClose;
                Tok2.lex = "*/";
                Tokens.Add(Tok1);
                Tokens.Add(Tok2);



            }
            else
            {
                Errors.Error_List.Add(Lex);
            }
          
        }



        bool isIdentifier(string lex)
        {
            Regex Reg = new Regex(@"^[a-zA-Z]+[a-zA-Z0-9]*");
            return Reg.IsMatch(lex);
        }
        bool isConstant(string lex)
        {
            Regex Reg = new Regex(@"^[0-9]+(\.[0-9]+)?$");
            return Reg.IsMatch(lex);
        }
        public bool IsString(string lex)
        {
            return (lex.StartsWith("\"") && lex.EndsWith("\""));
        }
        public bool IsComment(string lex) {
            return (lex.StartsWith("/*") && lex.EndsWith("*/"));
        }
    }
}
