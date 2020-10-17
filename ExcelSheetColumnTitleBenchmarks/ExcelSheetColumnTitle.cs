using System;
using System.Runtime.CompilerServices;

namespace ExcelSheetColumnTitleBenchmarks
{
    // https://leetcode.com/problems/excel-sheet-column-title/
    public class ExcelSheetColumnTitle
    {
        [SkipLocalsInit]
        public string ComputeColumnTitleUnsafe(int n) {
            Span<char> res = stackalloc char[7];
            byte numChars = 0;
            while (n > 0) {
                n--;
                res[^(++numChars)] = (char)('A' + n % 26);
                n /= 26;
            }
            return res[^numChars..].ToString();
        }

        public string ComputeColumnTitle(int n) {
            // maximum result size for integers is 7 characters
            Span<char> res = stackalloc char[7];
            byte numChars = 0;
            while (n > 0) {
                n--;
                // fill in array backwards, allow characters A-Z (26)
                res[^(++numChars)] = (char)('A' + n % 26);
                n /= 26;
            }
            // Return remaining slice starting `numCharsFrom` from the end
            return res[^numChars..].ToString();
        }

        // https://leetcode.com/problems/excel-sheet-column-title/discuss/609087/C-O(n)-80ms
        public string ComputeColumnTitleSimple(int n) {
            var ans = "";
            while (n > 0) {
                n--;
                ans = (char)(n % 26 + 'A') + ans;
                n /= 26;
            }
            return ans;
        }
    }
}