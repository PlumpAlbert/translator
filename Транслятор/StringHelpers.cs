namespace Транслятор {

    public static class StringHelpers {

        public static string RemoveWhiteSpaces(this string s) {
            string res = string.Empty;
            foreach (var c in s) {
                if (c != ' ')
                    res += c;
            }

            return res;
        }

        public static char GetLastChar(this string s) {
            if (!string.IsNullOrWhiteSpace(s))
                return s[s.Length - 1];
            return 'a';
        }

    }

}