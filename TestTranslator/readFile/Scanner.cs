using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class Scanner
    {
        FileStream fs;
        List<char> specialSingleCharacters;
        List<char> specialDoubleCharacters;
        List<string> specialTokens;
        char prevToken = ' ';

        public Scanner()
        {
            fillInListSpecialCharacters();
        }
        public Scanner(FileStream _fs)
        {
            this.fs = _fs;
            fillInListSpecialCharacters();
        }

        public void scan()
        {
            try
            {
                Program.createDocument();
                Parser parser = new Parser();
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    string nextLine = sr.ReadLine();
                    List<string> listOfTokens = getListOfTokens(nextLine);
                    if(!isEmpty(listOfTokens))
                        parser.parse(listOfTokens);

                }
                parser.end();
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                Program.getMainFormController().printLine(ex.Message);
            }
        }
        private bool isEmpty(List<string> listOfStrings)
        {
            return (listOfStrings.Count == 0);
        }
        private void fillInListSpecialCharacters()
        {
            specialSingleCharacters = new List<char>();
            specialSingleCharacters.Add(',');
            specialSingleCharacters.Add('.');
            specialSingleCharacters.Add(';');
            specialSingleCharacters.Add(':');
            specialSingleCharacters.Add('(');
            specialSingleCharacters.Add(')');
            specialSingleCharacters.Add('\\');
            specialSingleCharacters.Add('<');
            specialSingleCharacters.Add('>');
            specialSingleCharacters.Add('=');
            specialSingleCharacters.Add('[');
            specialSingleCharacters.Add(']');

            specialDoubleCharacters = new List<char>();
            specialDoubleCharacters.Add('/');
            specialDoubleCharacters.Add('*');

            specialTokens = new List<string>();
            specialTokens.Add("//");
            specialTokens.Add("/*");
            specialTokens.Add("*/");


        }

        public List<string> getListOfTokens(string line)
        {
            bool newToken = true;
            List<string> listOfTokens = new List<string>();
            string token = "";

            char[] characters = line.ToArray();

            foreach (char c in characters)
            {
                if (newToken)
                {
                    if (c != ' ')
                    {
                        if (isSingleSpecialCharacter(c))
                        {
                            token = c.ToString();
                            addTokenToList(token, listOfTokens);
                        }
                        /*else if(isDoubleSpecialCharacter(c))
                        {
                            token = c.ToString();
                            newToken = false;
                        }*/
                        else
                        {
                            token = c.ToString();
                            newToken = false;
                        }
                    }

                }
                else
                {
                    if (c != ' ')
                    {
                        if (isSpecialToken(token + c))
                        {
                            token += c;
                            addTokenToList(token, listOfTokens);
                            newToken = true;
                        }
                        else if(isSingleSpecialCharacter(c))
                        {
                            addTokenToList(token, listOfTokens);
                            token = c.ToString();
                            addTokenToList(token, listOfTokens);
                            newToken = true;
                        }
                        else if(isDoubleSpecialCharacter(c))
                        {
                            addTokenToList(token, listOfTokens);
                            token = c.ToString();
                            newToken = false;//
                        }
                        else
                        {
                            token += c.ToString();
                        }
                    }
                    else
                    {
                        addTokenToList(token, listOfTokens);
                        newToken = true;
                        token = " ";
                    }
                }
            }

            if (!newToken)
                addTokenToList(token, listOfTokens);

            return listOfTokens;
        }

        private void addTokenToList(string token, List<string> listOfTokens)
        {
            if (!token.Equals("") && !token.Equals(" "))
            {
                listOfTokens.Add(token);
            }
        }

        public bool isSingleSpecialCharacter(char c)
        {
            return specialSingleCharacters.Contains(c);
        }
        public bool isDoubleSpecialCharacter(char c)
        {
            return specialDoubleCharacters.Contains(c);
        }
        public bool isSpecialToken(string token)
        {
            return specialTokens.Contains(token);
        }

        bool canBeDoubleToken(char c)
        {
            return (prevToken == ' ' && isDoubleSpecialCharacter(c));
        }
    }
}
