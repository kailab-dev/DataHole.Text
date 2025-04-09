
namespace DataHole.Text
{
    public class Hangul
    {
        public Hangul()
        {

        }

        [Obsolete("최초 버전이 char단위를 처리했는데 string 단위까지 확장되며 실질적으로 외부에서 사용할 이유가 사라져서 리펙토링 과정에서 언제든 사라질 수 있습니다.",false)]
        internal Hangul(char choseong, char jungseong, char jongseong)
        {
            Choseong = choseong;
            Jungseong = jungseong;
            Jongseong = jongseong;
        }

        private static string m_ChoSungTbl = "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ";
        private static string m_JungSungTbl = "ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ";
        private static string m_JongSungTbl = " ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";
        private static ushort m_UniCodeHangulBase = 0xAC00;
        private static ushort m_UniCodeHangulLast = 0xD79F;

        char m_Choseong;
        /// <summary>
        /// 초성
        /// </summary>
        public char Choseong
        {
            get;
            set;
        }

        /// <summary>
        /// 중성
        /// </summary>
        public char Jungseong
        {
            get;
            set;
        }

        /// <summary>
        /// 종성
        /// </summary>
        public char Jongseong
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Hangul.MergeJaso(Choseong, Jungseong, Jongseong);
        }


        #region 자소 결합 및 분리
        //자소 결합
        public static string MergeJaso(char[] hanChar)
        {
            int ChoSungPos, JungSungPos, JongSungPos;
            int nUniCode;

            ChoSungPos = m_ChoSungTbl.IndexOf(hanChar[0]);//jaso.Substring(0, 1));
            JungSungPos = m_JungSungTbl.IndexOf(hanChar[1]);//jaso.Substring(1, 1));
            JongSungPos = m_JongSungTbl.IndexOf(hanChar[2]);

            nUniCode = m_UniCodeHangulBase + (ChoSungPos * 21 + JungSungPos) * 28 + JongSungPos;
            char temp = Convert.ToChar(nUniCode);

            return temp.ToString();
        }

        public static string MergeJaso(char choseong, char jungseong)
        {
            int ChoSungPos, JungSungPos, JongSungPos;
            int nUniCode;

            ChoSungPos = m_ChoSungTbl.IndexOf(choseong);//jaso.Substring(0, 1));
            JungSungPos = m_JungSungTbl.IndexOf(jungseong);//jaso.Substring(1, 1));
            JongSungPos = 0;// (jongSung == null) ? 0 : m_JongSungTbl.IndexOf(jongSung);

            nUniCode = m_UniCodeHangulBase + (ChoSungPos * 21 + jungseong) * 28 + JongSungPos;
            char temp = Convert.ToChar(nUniCode);

            return temp.ToString();
        }

        /// <summary>
        /// 자소 결합
        /// </summary>
        /// <param name="choseong"></param>
        /// <param name="jungseong"></param>
        /// <param name="jongseong"></param>
        /// <returns></returns>
        public static string MergeJaso(char choseong, char jungseong, char jongseong)
        {
            int ChoSungPos, JungSungPos, JongSungPos;
            int nUniCode;

            ChoSungPos = m_ChoSungTbl.IndexOf(choseong);//jaso.Substring(0, 1));
            JungSungPos = m_JungSungTbl.IndexOf(jungseong);//jaso.Substring(1, 1));
            JongSungPos = m_JongSungTbl.IndexOf(jongseong);

            nUniCode = m_UniCodeHangulBase + (ChoSungPos * 21 + JungSungPos) * 28 + JongSungPos;
            char temp = Convert.ToChar(nUniCode);


            return temp.ToString();
        }

        public static string MergeJaso(string str)
        {
            str = str + " ";
            string _ret = "";
            string _tmp = "";
            int strLen = str.Length;

            int curIdx = 0;
            List<char> charBuffer = new List<char>();

            while (curIdx < strLen)
            {
                charBuffer.Clear();
                if (Hangul.IsHangul(str[curIdx].ToString()))
                {
                    if (Hangul.IsHangulJaum(str[curIdx].ToString()))
                    {
                        if (curIdx + 1 < strLen && Hangul.IsHangulMoum(str[curIdx + 1].ToString()))
                        {
                            if (curIdx + 2 < strLen && Hangul.IsHangulJaum(str[curIdx + 2].ToString()))
                            {
                                charBuffer.Add(str[curIdx]);
                                charBuffer.Add(str[curIdx + 1]);
                                if (curIdx + 3 < strLen && !Hangul.IsHangulMoum(str[curIdx + 3].ToString()))
                                {
                                    charBuffer.Add(str[curIdx + 2]);
                                    _ret = _ret + Hangul.MergeJaso(charBuffer[0], charBuffer[1], charBuffer[2]);
                                    curIdx = curIdx + 3;
                                }
                                else
                                {
                                    //charBuffer.Add(str[curIdx + 2]);
                                    _ret = _ret + Hangul.MergeJaso(charBuffer[0], charBuffer[1]);
                                    curIdx = curIdx + 2;
                                }
                            }
                            else
                            {
                                charBuffer.Add(str[curIdx]);
                                charBuffer.Add(str[curIdx + 1]);
                                _ret = _ret + Hangul.MergeJaso(charBuffer[0], charBuffer[1]);
                                curIdx = curIdx + 2;
                            }
                        }
                        else
                        {
                            //charBuffer.Add(str[curIdx]);
                            _ret = _ret + str[curIdx];
                            curIdx = curIdx + 1;
                        }
                    }
                    else
                    {
                        //charBuffer.Add(str[curIdx]);
                        _ret = _ret + str[curIdx];
                        curIdx = curIdx + 1;
                    }
                }
                else
                {
                    //charBuffer.Add(str[curIdx]);
                    _ret = _ret + str[curIdx];
                    curIdx = curIdx + 1;
                }
            }



            return _ret.Substring(0, _ret.Length - 1);
        }

        ///////=========
        // 자소 분리(스트링을 받아서..
        public static string Dividejaso(string sourceString)
        {
            string retString = "";

            for (int i = 0; i < sourceString.Length; i++)
            {

                if (IsHangulJaum(sourceString[i].ToString()) || IsHangulMoum(sourceString[i].ToString()))
                {
                    retString += sourceString[i].ToString();
                }
                else
                {
                    if (IsHangul(sourceString[i].ToString()))
                    {
                        //LanguageIdentification. sourceString[i]
                        Hangul hanChar = DivideJaso(sourceString[i]);
                        retString += Convert.ToChar(hanChar.Choseong).ToString() + Convert.ToChar(hanChar.Jungseong).ToString() +
                            ((hanChar.Jongseong.ToString().Trim() != "") ? Convert.ToChar(hanChar.Jongseong).ToString() : "");
                    }
                    else retString += sourceString[i].ToString();
                }
            }

            return retString;
        }

        //자소 분리(초성+중성+종성)
        public static Hangul DivideJaso(char hanChar)
        {
            int ChoSung, JungSung, JongSung;

            Hangul hc;

            ushort temp = 0x0000;

            try
            {
                temp = Convert.ToUInt16(hanChar); 
            }
            catch (Exception e)
            {
                //return e.ToString();
            }

            if ((temp < m_UniCodeHangulBase) || (temp > m_UniCodeHangulLast)) new Exception("Not hangul!");//hc;		

            //초성자, 중성자, 종성자 Index계산
            int nUniCode = temp - m_UniCodeHangulBase;
            ChoSung = nUniCode / (21 * 28);
            nUniCode = nUniCode % (21 * 28);
            JungSung = nUniCode / 28;
            nUniCode = nUniCode % 28;
            JongSung = nUniCode;

            hc = new Hangul(m_ChoSungTbl[ChoSung], m_JungSungTbl[JungSung], m_JongSungTbl[JongSung]);

            return hc;

        }

        //자소 분리(초성+중성+종성)
        public static char[] DivideJasoToArray(char hanChar)
        {
            int ChoSung, JungSung, JongSung;

            //HanChar hc;

            ushort temp = 0x0000;

            try
            {
                temp = Convert.ToUInt16(hanChar);   
            }
            catch (Exception e)
            {
                //return e.ToString();
            }

            if ((temp < m_UniCodeHangulBase) || (temp > m_UniCodeHangulLast)) return null;

            //초성자, 중성자, 종성자 Index계산
            int nUniCode = temp - m_UniCodeHangulBase;
            ChoSung = nUniCode / (21 * 28);
            nUniCode = nUniCode % (21 * 28);
            JungSung = nUniCode / 28;
            nUniCode = nUniCode % 28;
            JongSung = nUniCode;

            return new char[] { m_ChoSungTbl[ChoSung], m_JungSungTbl[JungSung], m_JongSungTbl[JongSung] };

        }

        #endregion

        #region 조사 활용 결합
        
 
        #endregion

        #region 문자 판별용 메서드
        // 한글이면 true, 아니면 false리턴
        public static bool IsHangul(string word)
        {
            if (word == null) return false;

            byte[] temp = System.Text.Encoding.Default.GetBytes(word);

            if (temp[0] > 128) return true;   //한글에서 한바이트는 MSB가 1,  따라서 0x80(128)과 비교

            return false;
        }

        // 한글 자음인지이면 true
        public static bool IsHangulJaum(string _hangulChar)
        {
            if (_hangulChar == null) return false;

            if (_hangulChar[0] >= 12593 && _hangulChar[0] <= 12622) return true;

            return false;
        }

        // 한글 모음인지이면 true
        public static bool IsHangulMoum(string _hangulChar)
        {
            if (_hangulChar == null) return false;

            if (_hangulChar[0] >= 12623 && _hangulChar[0] <= 12643) return true;

            return false;
            //byte[] temp = System.Text.Encoding.Default.GetBytes(_hangulChar);

            //return (temp[0] == 164 && (temp[1] >= 191 && temp[1] <= 211));//완성형 한글 모음 범위 : w[0] == 0xa4 && (w[1] >= 0xbf && w[1] <= 0xd3)
        }
        #endregion
    }
}
