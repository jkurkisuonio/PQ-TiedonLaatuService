using System;
using System.Collections.Generic;
using System.Text;

namespace PQ_TiedonLaatuService
{
    public class ParserUtil
    {
        private readonly Dictionary<string, string> replaceWords;

        public ParserUtil(Dictionary<string, string> replaceWords)
        {
            this.replaceWords = replaceWords;
        }
    
    public string ReplaceWithKeyWords(string text)
        {
            foreach (var dic in this.replaceWords)
            {
                text = text.Replace("%" + dic.Key + "%", dic.Value);
            }            
            return text;
        }
    
    
    
    }
}
