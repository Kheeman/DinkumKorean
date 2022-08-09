using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DinkumChinese
{
    public class KoreanCheck
    {
        public class Josa
        {
            public Regex JosaRegex = new Regex(@"\(이\)가|\(와\)과|\(을\)를|\(은\)는|\(아\)야|\(이\)여|\(으\)로|\(이\)라");
            public Regex FindRegex = new Regex(@"(\S*)([\w가-힣0-9])(\([\w가-힣0-9]\))([\w가-힣0-9])");
            public Regex EngRegex = new Regex(@"^[mn]|\S[mn]e?|\S(?:[aeiom]|lu)b|(?:u|\S[aei]|[^o]o)p|(?:^i|[^auh]i|\Su|[^ei][ae]|[^oi]o)t|(?:\S[iou]|[^e][ae])c?k|\S[aeiou](?:c|ng)|foot|go+d|b[ai]g|private|^(?:app|kor)");
            public Regex EngRegex2 = new Regex(@"^[lr]|^\Sr|\Sle?|check|[hm]ook|limit");

            Dictionary<string, JosaPair> _josaPatternPaird = new Dictionary<string, JosaPair>
            {
                { "(이)가", new JosaPair("이", "가") },
                { "(와)과", new JosaPair("과", "와") },
                { "(을)를", new JosaPair("을", "를") },
                { "(은)는", new JosaPair("은", "는") },
                { "(아)야", new JosaPair("아", "야") },
                { "(이)여", new JosaPair("이여", "여") },
                { "(으)로", new JosaPair("으로", "로") },
                { "(이)라", new JosaPair("이라", "라") },
            };

            class JosaPair
            {
                public JosaPair(string josa1, string josa2)
                {
                    this.josa1 = josa1;
                    this.josa2 = josa2;
                }

                public string josa1
                { get; private set; }

                public string josa2
                { get; private set; }
            }

            public string Replace(string src)
            {
                var findMatches = FindRegex.Matches(src);
                if (findMatches.Count == 0)
                    return src;
                
                var strBuilder = new StringBuilder(src.Length);
                var lastHeadIndex = 0;
                foreach (Match findMatch in findMatches)
                {
                    var josaMatches = JosaRegex.Matches(findMatch.Value);
                    string engCheck = findMatch.Value.Replace(josaMatches[0].Value, "");
                    int index = 0;
                    
                    foreach (Match josaMatch in josaMatches)
                    {
                        var josaPair = _josaPatternPaird[josaMatch.Value];
                        index = findMatch.Index + josaMatch.Index;
                        strBuilder.Append(src, lastHeadIndex, index - lastHeadIndex);
                        if (index > 0)
                        {
                            var prevChar = src[index - 1];
                            if ((HasJong(prevChar, engCheck) && josaMatch.Value != "(으)로") ||
                                (HasJongExceptRieul(prevChar, engCheck) && josaMatch.Value == "(으)로"))

                            {
                                strBuilder.Append(josaPair.josa1);
                            }
                            else
                            {
                                strBuilder.Append(josaPair.josa2);
                            }
                        }
                        else
                        {
                            strBuilder.Append(josaPair.josa1);
                        }
                        lastHeadIndex = index + josaMatch.Length;
                    }
                    strBuilder.Append(src, lastHeadIndex, src.Length - lastHeadIndex);
                }
                return strBuilder.ToString();
            }

            static bool HasJong(char inChar, string inString)
            {
                if (inChar >= 0xAC00 && inChar <= 0xD7A3) // 가 ~ 힣
                {
                    int localCode = inChar - 0xAC00; // 가~ 이후 로컬 코드 
                    int jongCode = localCode % 28;
                    return jongCode > 0 ? true : false;
                }
                else if ((0x61 <= inChar && inChar <= 0x7A) || (0x41 <= inChar && inChar <= 0x5A))
                {
                    var matches = _josa.EngRegex.Matches(inString);
                    foreach (Match matche in matches)
                        return matche.Value.LastIndexOf(inChar) > 0 ? true : false;
                }
                else if (inChar == 0x0030 || inChar == 0x0031 || inChar == 0x0033 || inChar == 0x0036 || inChar == 0x0037 || inChar == 0x0038) //0, 1, 3, 6, 7, 8)
                    return true;

                return false;
            }

            static bool HasJongExceptRieul(char inChar, string inString)
            {
                if (inChar >= 0xAC00 && inChar <= 0xD7A3)
                {
                    int localCode = inChar - 0xAC00;
                    int jongCode = localCode % 28;
                    return (jongCode == 8 || jongCode == 0) ? false : true;
                }
                else if ((0x61 <= inChar && inChar <= 0x7A) || (0x41 <= inChar && inChar <= 0x5A))
                {
                    var matches = _josa.EngRegex2.Matches(inString);
                    foreach (Match matche in matches)
                        return matche.Value.LastIndexOf(inChar) > 0 ? false : true;
                }

                return false;
            }
        }

        public static string ReplaceJosa(string src)
        {
            return _josa.Replace(src);
        }

        static Josa _josa = new Josa();
    }
}
