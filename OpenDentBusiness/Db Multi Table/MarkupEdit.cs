using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    public class MarkupEdit
    {
        public static string TranslateToXhtml(string markupText, bool isPreviewOnly, bool hasWikiPageTitles = false, bool isEmail = false, bool canAggregate = true)
        {
            return markupText;
        }

        /// <summary>
        /// This method removes HTML tags and wiki page links from the wiki page text.
        /// </summary>
        public static string ConvertToPlainText(string rawWikipageText)
        {
            rawWikipageText = Regex.Replace(rawWikipageText, @"(?<!&)<.+?(?<!&)>", "", RegexOptions.IgnoreCase);
            rawWikipageText = Regex.Replace(rawWikipageText, @"\[\[.+?\]\]", "", RegexOptions.IgnoreCase);
            return rawWikipageText;
        }
    }
}
