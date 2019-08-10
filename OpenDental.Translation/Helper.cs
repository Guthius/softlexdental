using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental.Translation
{
    /// <summary>
    /// This class is to assist in translation of dynamic elements, such as enum fields,
    /// dates, time spans, etc...
    /// </summary>
    public static class Helper
    {
        public static Dictionary<string, TEnum> Translate<TEnum>() where TEnum : Enum
        {
            var results = new Dictionary<string, TEnum>();

            var type = typeof(TEnum);

            foreach (var member in type.GetMembers())
            {
                var name = member.Name;

                var attributes = member.GetCustomAttributes(typeof(TranslationKeyAttribute), false);
                if (attributes.Length > 0)
                {
                    var translationKey = ((TranslationKeyAttribute)attributes[0]).Key;
                    if (!string.IsNullOrEmpty(translationKey))
                    {
                        var translation = Language.ResourceManager.GetString(translationKey);
                        if (!string.IsNullOrEmpty(translation))
                        {
                            name = translation;
                        }
                    }
                }

                results.Add(name, (TEnum)Enum.Parse(type, member.Name));
            }

            return results;
        }
    }

    public class TranslationKeyAttribute : Attribute
    {
        public string Key { get; }
    }
}
