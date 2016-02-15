using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBCodeFirst
{
    class Words
    {
        /// <summary>
        /// 是否保留字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static Boolean isReservedWords(string word)
        {
            string[] ReservedWords = { "namespace", "int", "static", "class" };
            Boolean exists = ReservedWords.Contains(word.ToLower());
            return exists;
        }

        /// <summary>
        /// 重写保留字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string reWriteWord(string word)
        {
            word = word.Replace(" ", "_");
            if (isReservedWords(word))
            {
                return "@" + word;
            }
            else
            {
                return word;
            }
        }

        /// <summary>
        /// 单词变成单数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToSingular(string word)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(word))
                return plural1.Replace(word, "${keep}y");
            else if (plural2.IsMatch(word))
                return plural2.Replace(word, "${keep}");
            else if (plural3.IsMatch(word))
                return plural3.Replace(word, "${keep}");
            else if (plural4.IsMatch(word))
                return plural4.Replace(word, "${keep}");

            return word;
        }
        /// <summary>
        /// 单词变成复数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToPlural(string word)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzh])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(word))
                return plural1.Replace(word, "${keep}ies");
            else if (plural2.IsMatch(word))
                return plural2.Replace(word, "${keep}s");
            else if (plural3.IsMatch(word))
            {
                string wright1 = word.Substring(word.Length - 1);
                if (wright1 == "s")
                {
                    return word; //不改变
                }
                else {
                    return plural3.Replace(word, "${keep}es");
                }
            }
            else if (plural4.IsMatch(word))
                return plural4.Replace(word, "${keep}s");

            return word;
        }

    }
}
