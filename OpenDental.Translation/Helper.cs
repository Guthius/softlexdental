/**
 * Copyright (C) 2019 Dental Stars SRL
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using System;
using System.Collections.Generic;

namespace OpenDental.Translation
{
    /// <summary>
    /// This class is to assist in translation of dynamic elements, such as enum fields,
    /// dates, time spans, etc...
    /// </summary>
    public static class Helper
    {
        public static List<Tuple<string, TEnum>> Translate<TEnum>() where TEnum : Enum
        {
            var results = new List<Tuple<string, TEnum>>();
            var type = typeof(TEnum);
            var key = "E_" + type.Name + "_";

            foreach (var member in type.GetMembers())
            {
                var name = member.Name;

                var translation = Language.ResourceManager.GetString(key + member.Name);
                if (string.IsNullOrEmpty(translation))
                {
                    var attributes = member.GetCustomAttributes(typeof(TranslationAttribute), false);
                    if (attributes.Length > 0)
                    {
                        name = ((TranslationAttribute)attributes[0]).Value;
                    }
                }

                results.Add(new Tuple<string, TEnum>(name, (TEnum)Enum.Parse(type, member.Name)));
            }

            return results;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TranslationAttribute : Attribute
    {
        public TranslationAttribute(string value) => Value = value;

        public string Value { get; }
    }
}
