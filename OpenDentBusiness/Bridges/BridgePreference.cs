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

namespace OpenDentBusiness.Bridges
{
    public class BridgePreference
    {
        /// <summary>
        ///     <para>
        ///         Creates a custom preference with the specified details.
        ///     </para>
        /// </summary>
        /// <param name="key">The preference key.</param>
        /// <param name="description">A description of the preference.</param>
        /// <param name="type">The data type of the preference.</param>
        /// <param name="definitionCategory">The definition category.</param>
        /// <returns></returns>
        public static BridgePreference Define(string key, string description, BridgePreferenceType type, DefinitionCategory definitionCategory = DefinitionCategory.None) =>
            new BridgePreference(key, description, type, definitionCategory);

        /// <summary>
        /// Gets the preference key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets a description of the preference.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the data type of the preference.
        /// </summary>
        public BridgePreferenceType Type { get; }

        /// <summary>
        ///     <para>
        ///         Gets the definition category. Only used when the preference type is set to
        ///         <see cref="BridgePreferenceType.Definition"/>.
        ///     </para>
        /// </summary>
        public DefinitionCategory DefinitionCategory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BridgePreference"/> class.
        /// </summary>
        /// <param name="key">The preference key.</param>
        /// <param name="description">A description of the preference.</param>
        /// <param name="type">The preference data type.</param>
        /// <param name="definitionCategory">The definition category.</param>
        private BridgePreference(string key, string description, BridgePreferenceType type, DefinitionCategory definitionCategory)
        {
            Key = key;
            Description = description;
            Type = type;
            DefinitionCategory = definitionCategory;
        }
    }
}
