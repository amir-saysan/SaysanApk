namespace SaysanPwa.Application
{
    public static class ConvertEnglishToPersian
    {
        public static string GetEnglishNumber(string englishNumber)
        {
            string persianNumber = "";
            foreach (char ch in englishNumber)
            {
                persianNumber += (char)(1776 + char.GetNumericValue(ch));
            }
            return persianNumber.Replace("ۯ", "/");
        }
    }
}
