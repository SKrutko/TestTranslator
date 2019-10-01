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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            fs = _fs;
            fillInListSpecialCharacters();
        }

        public void scan()
        {
            try
            {
                Program.createDocument();
                Parser parser = new Parser();
                StreamReader sr = new StreamReader(fs);
                Logger.Info("Scanning started");
                while (!sr.EndOfStream)
                {
                    string nextLine = sr.ReadLine();
                    List<scannerResponse> listOfTokens = getListOfTokens(nextLine);
                    int lengthOfLine = listOfTokens.Count;
                    for(int i = 0; i < lengthOfLine; i++)
                        parser.parse(listOfTokens.ElementAt(i).token, listOfTokens.ElementAt(i).space, i == lengthOfLine - 1);

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
            specialSingleCharacters.Add('"');


            specialDoubleCharacters = new List<char>();
            specialDoubleCharacters.Add('/');
            specialDoubleCharacters.Add('*');

            specialTokens = new List<string>();
            specialTokens.Add("//");
            specialTokens.Add("/*");
            specialTokens.Add("*/");


        }

        public List<scannerResponse> getListOfTokens(string line)
        {
            bool newToken = true;
            List<scannerResponse> listOfTokens = new List<scannerResponse>();
            string token = "";
            bool space = false;

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
                            addTokenToList(token, space, listOfTokens);
                        }
                        else
                        {
                            token = c.ToString();
                            newToken = false;
                        }
                    }
                    else 
                        space = true;

                }
                else
                {
                    if (c != ' ')
                    {
                        if (isSpecialToken(token + c))
                        {
                            token += c;
                            addTokenToList(token, space, listOfTokens);
                            newToken = true;
                        }
                        else if(isSingleSpecialCharacter(c))
                        {
                            addTokenToList(token, space, listOfTokens);
                            token = c.ToString();
                            space = false;
                            addTokenToList(token, space, listOfTokens);
                            newToken = true;
                        }
                        else if(isDoubleSpecialCharacter(c))
                        {
                            addTokenToList(token, space, listOfTokens);
                            space = false;
                            token = c.ToString();
                            newToken = false;
                        }
                        else
                        {
                            token += c.ToString();
                        }
                    }
                    else
                    {
                        addTokenToList(token, space, listOfTokens);
                        newToken = true;
                        token = " ";
                        space = true;
                    }
                }
            }

            if (!newToken)
                addTokenToList(token, space, listOfTokens);

            return listOfTokens;
        }

        private void addTokenToList(string token, bool space, List<scannerResponse> listOfTokens)
        {
            if (!token.Equals("") && !token.Equals(" "))
            {
                listOfTokens.Add(new scannerResponse(token, space));
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
    }
}
