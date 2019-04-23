using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class OneLineComment
    {
        private string message;

        public OneLineComment(string comment)
        {
            message = comment;
        }

        public string getMessage()
        {
            return message;
        }
    }
    public class MultipleLineComment
    {
        private List<string> message;

        public MultipleLineComment(List<string> comment)
        {
            message = new List<string>();
            message = comment;
        }

        public List<string> getMessage()
        {
            return message;
        }
    }
}
