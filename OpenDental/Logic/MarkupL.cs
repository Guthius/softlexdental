﻿using OpenDentBusiness;
using SLDental.Storage;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental
{
    public class MarkupL
    {
        private static string _lanThis = "MarkupEdit";

        public static void AddTag(string tagStart, string tagClose, TextBox textBox)
        {
            int selectionStart = textBox.SelectionStart;

            textBox.SelectedText = tagStart + textBox.SelectedText + tagClose;
            textBox.SelectionStart = selectionStart + tagStart.Length;
            textBox.SelectionLength = 0;
        }


        ///<summary>Validates content, and keywords.  isForSaving can be false if just validating for refresh.</summary>
        public static bool ValidateMarkup(TextBox textBox, bool isForSaving, bool showMsgBox = true, bool isEmail = false)
        {
            MatchCollection matches;
            //xml validation----------------------------------------------------------------------------------------------------
            string s = textBox.Text;
            //"<",">", and "&"-----------------------------------------------------------------------------------------------------------
            s = s.Replace("&", "&amp;");
            s = s.Replace("&amp;<", "&lt;");//because "&" was changed to "&amp;" in the line above.
            s = s.Replace("&amp;>", "&gt;");//because "&" was changed to "&amp;" in the line above.
            s = "<body>" + s + "</body>";
            XmlDocument doc = new XmlDocument();
            StringReader reader = new StringReader(s);
            try
            {
                doc.Load(reader);
            }
            catch (Exception ex)
            {
                if (showMsgBox)
                {
                    MessageBox.Show(ex.Message);
                }
                return false;
            }
            try
            {
                //we do it this way to skip checking the main node itself since it's a dummy node.
                if (!isEmail)
                {//We are allowing any XHTML markup in emails.
                    MarkupEdit.ValidateNodes(doc.DocumentElement.ChildNodes);
                }
            }
            catch (Exception ex)
            {
                if (showMsgBox)
                {
                    MessageBox.Show(ex.Message);
                }
                return false;
            }
            //Cannot have CR within tag definition---------------------------------------------------------------------------------
            //(?<!&) means only match strings that do not start with an '&'. This is so we can continue to use '&' as an escape character for '<'.
            //<.*?> means anything as short as possible that is contained inside a tag
            MatchCollection tagMatches = Regex.Matches(textBox.Text, "(?<!&)<.*?>", RegexOptions.Singleline);
            for (int i = 0; i < tagMatches.Count; i++)
            {
                if (tagMatches[i].ToString().Contains("\n"))
                {
                    if (showMsgBox)
                    {
                        MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(tagMatches[i].Index) + " - "
                            + Lans.g(_lanThis, "Tag definitions cannot contain a return line:") + " " + tagMatches[i].Value.Replace("\n", ""));
                    }
                    return false;
                }
            }
            //wiki image validation-----------------------------------------------------------------------------------------------------
            if (!isEmail)
            {
                string wikiImagePath = "";
                try
                {
                    wikiImagePath = WikiPages.GetWikiPath();//this also creates folder if it's missing.
                }
                catch
                {

                    //do nothing, the wikiImagePath is only important if the user adds an image to the wiki page and is checked below
                }
                matches = Regex.Matches(textBox.Text, @"\[\[(img:).*?\]\]");// [[img:myimage.jpg]]
                if (matches.Count > 0 && Preferences.AtoZfolderUsed == DataStorageType.InDatabase)
                {
                    if (showMsgBox)
                    {
                        MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(matches[0].Index) + " - "
                            + Lans.g(_lanThis, "Cannot use images in wiki if storing images in database."));
                    }
                    return false;
                }
                if (isForSaving)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        string imgPath = Storage.Default.CombinePath(wikiImagePath, matches[i].Value.Substring(6).Trim(']'));
                        if (!Storage.Default.FileExists(imgPath))
                        {
                            if (showMsgBox)
                            {
                                MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(matches[i].Index) + " - "
                                    + Lans.g(_lanThis, "Not allowed to save because image does not exist:") + " " + imgPath);
                            }
                            return false;
                        }
                    }
                }
            }
            //Email image validation----------------------------------------------------------------------------------------------
            if (isEmail)
            {
                string emailImagePath = "";
                try
                {
                    emailImagePath = ImageStore.GetEmailImagePath();
                }
                catch
                {
                }
                matches = Regex.Matches(textBox.Text, @"\[\[(img:).*?\]\]");
                if (isForSaving)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        string imgPath = Storage.Default.CombinePath(emailImagePath, matches[i].Value.Substring(6).Trim(']'));
                        if (!Storage.Default.FileExists(imgPath))
                        {
                            if (showMsgBox)
                            {
                                MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(matches[i].Index) + " - "
                                    + Lans.g(_lanThis, "Not allowed to save because image does not exist: ") + " " + imgPath);
                            }
                        }
                    }
                }
            }
            //List validation-----------------------------------------------------------------------------------------------------
            matches = Regex.Matches(textBox.Text, @"\[\[(list:).*?\]\]");// [[list:CustomList]]
            foreach (Match match in matches)
            {
                if (!WikiLists.CheckExists(match.Value.Substring(7).Trim(']')))
                {
                    if (showMsgBox)
                    {
                        MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(match.Index) + " - "
                            + Lans.g(_lanThis, "Wiki list does not exist in database:") + " " + match.Value.Substring(7).Trim(']'));
                    }
                    return false;
                }
            }
            //spacing around bullets-----------------------------------------------------------------------------------------------
            string[] lines = textBox.Text.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("*"))
                {
                    if (!lines[i].StartsWith("*"))
                    {
                        if (showMsgBox)
                        {
                            MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + (i + 1) + " - "
                                + Lans.g(_lanThis, "Stars used for lists may not have a space before them."));
                        }
                        return false;
                    }
                    if (lines[i].Trim().StartsWith("* "))
                    {
                        if (showMsgBox)
                        {
                            MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + (i + 1) + " - "
                                + Lans.g(_lanThis, "Stars used for lists may not have a space after them."));
                        }
                        return false;
                    }
                }
                if (lines[i].Trim().StartsWith("#"))
                {
                    if (!lines[i].StartsWith("#"))
                    {
                        if (showMsgBox)
                        {
                            MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + (i + 1) + " - "
                                + Lans.g(_lanThis, "Hashes used for lists may not have a space before them."));
                        }
                        return false;
                    }
                    if (lines[i].Trim().StartsWith("# "))
                    {
                        if (showMsgBox)
                        {
                            MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + (i + 1) + " - "
                                + Lans.g(_lanThis, "Hashes used for lists may not have a space after them."));
                        }
                        return false;
                    }
                }
            }
            //Invalid characters inside of various tags--------------------------------------------
            matches = Regex.Matches(textBox.Text, @"\[\[.*?\]\]");
            foreach (Match match in matches)
            {
                if (match.Value.Contains("\"") && !match.Value.StartsWith("[[color:") && !match.Value.StartsWith("[[font:"))
                {//allow colored text to have quotes.
                    if (showMsgBox)
                    {
                        MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(match.Index) + " - "
                            + Lans.g(_lanThis, "Link cannot contain double quotes:") + " " + match.Value);
                    }
                    return false;
                }
                //This is not needed because our regex doesn't even catch them if the span a line break.  It's just interpreted as plain text.
                //if(match.Value.Contains("\r") || match.Value.Contains("\n")) {
                //	MessageBox.Show(Lan.g(this,"Link cannot contain carriage returns: ")+match.Value);
                //	return false;
                //}
                if (match.Value.StartsWith("[[img:")
                    || match.Value.StartsWith("[[keywords:")
                    || match.Value.StartsWith("[[file:")
                    || match.Value.StartsWith("[[folder:")
                    || match.Value.StartsWith("[[list:")
                    || match.Value.StartsWith("[[color:")
                     || match.Value.StartsWith("[[font:"))
                {
                    //other tags
                }
                else
                {
                    if (match.Value.Contains("|"))
                    {
                        if (showMsgBox)
                        {
                            MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(match.Index) + " - "
                                + Lans.g(_lanThis, "Internal link cannot contain a pipe character:") + " " + match.Value);
                        }
                        return false;
                    }
                }
            }
            //Table markup rigorously formatted----------------------------------------------------------------------
            //{|
            //!Width="100"|Column Heading 1!!Width="150"|Column Heading 2!!Width="75"|Column Heading 3
            //|- 
            //|Cell 1||Cell 2||Cell 3 
            //|-
            //|Cell A||Cell B||Cell C 
            //|}
            //Although rarely needed, it might still come in handy in certain cases, like paste, or when user doesn't add the |} until later, and other hacks.
            matches = Regex.Matches(s, @"\{\|\n.+?\n\|\}", RegexOptions.Singleline);
            //matches = Regex.Matches(textContent.Text,
            //	@"(?<=(?:\n|<body>))" //Checks for preceding newline or beggining of file
            //	+@"\{\|.+?\n\|\}" //Matches the table markup.
            //	+@"(?=(?:\n|</body>))" //Checks for following newline or end of file
            //	,RegexOptions.Singleline);
            foreach (Match match in matches)
            {
                lines = match.Value.Split(new string[] { "{|\n", "\n|-\n", "\n|}" }, StringSplitOptions.RemoveEmptyEntries);
                if (!lines[0].StartsWith("!"))
                {
                    if (showMsgBox)
                    {
                        MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(match.Index) + " - "
                            + Lans.g(_lanThis, "The second line of a table markup section must start with ! to indicate column headers."));
                    }
                    return false;
                }
                if (lines[0].StartsWith("! "))
                {
                    if (showMsgBox)
                    {
                        MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(match.Index) + " - "
                            + Lans.g(_lanThis, "In the table, at line 2, there cannot be a space after the first !"));
                    }
                    return false;
                }
                string[] cells = lines[0].Substring(1).Split(new string[] { "!!" }, StringSplitOptions.None);//this also strips off the leading !
                for (int c = 0; c < cells.Length; c++)
                {
                    if (!Regex.IsMatch(cells[c], @"^(Width="")\d+""\|"))
                    {//e.g. Width="90"| 
                        if (showMsgBox)
                        {
                            MessageBox.Show(Lans.g(_lanThis, "Error at line:") + " " + textBox.GetLineFromCharIndex(match.Index) + " - "
                            + Lans.g(_lanThis, "In the table markup, each header must be formatted like this: Width=\"#\"|..."));
                        }
                        return false;
                    }
                }
                for (int i = 1; i < lines.Length; i++)
                {//loop through the lines after the header
                    if (!lines[i].StartsWith("|"))
                    {
                        if (showMsgBox)
                        {
                            MessageBox.Show(Lans.g(_lanThis, "Table rows must start with |.  At line ") + (i + 1).ToString() + Lans.g(_lanThis, ", this was found instead:")
                                + lines[i]);
                        }
                        return false;
                    }
                }
            }
            return true;
        }
    }
}