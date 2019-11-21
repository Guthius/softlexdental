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
        public static readonly BridgePreference ProgramPath = 
            new BridgePreference(ProgramPreferenceName.ProgramPath, "Executable path", BridgePreferenceType.FilePath);

        public static readonly BridgePreference CommandLineArguments = 
            new BridgePreference(ProgramPreferenceName.CommandLineArguments, "Command line arguments", BridgePreferenceType.String);

        public static readonly BridgePreference UseChartNumber = 
            new BridgePreference(ProgramPreferenceName.UseChartNumber, "Use patient chart number instead of ID", BridgePreferenceType.Boolean);

        public static readonly BridgePreference DateFormat = 
            new BridgePreference(ProgramPreferenceName.DateFormat, "Birthdate format (default yyyyMMdd)", BridgePreferenceType.String);

        /// <summary>
        ///     <para>
        ///         Creates a custom preference with the specified details.
        ///     </para>
        /// </summary>
        /// <param name="key">The preference key.</param>
        /// <param name="description">A description of the preference.</param>
        /// <param name="type">The data type of the preference.</param>
        /// <returns></returns>
        public static BridgePreference Custom(string key, string description, BridgePreferenceType type) =>
            new BridgePreference(key, description, type);

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
        /// Initializes a new instance of the <see cref="BridgePreference"/> class.
        /// </summary>
        /// <param name="key">The preference key.</param>
        /// <param name="description">A description of the preference.</param>
        /// <param name="type">The preference data type.</param>
        public BridgePreference(string key, string description, BridgePreferenceType type)
        {
            Key = key;
            Description = description;
            Type = type;
        }
    }
}
