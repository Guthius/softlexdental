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

        /**
         * TODO: This class needs some more work.
         * 
         * The main purpose of this class is to serve as a description of a preference used by a
         * IBridge implementation. This information is used by the program setup forms.
         *
         * Right now a BridgePreference simply contains the internal key of the preference, a 
         * description and a data type. Ideally it would be nice to also be able to specify a
         * help text, a scope (e.g. is the preference configured per clinic, per user, per computer
         * or globally) a validation action and a browse action.
         * 
         * The constructor should be private, and construction of BridgePreference instances
         * should only be done through th 'Custom' method which should be renamed to something
         * more appropriate (e.g. 'Define' or 'Make').
         * 
         * The predefined preferences should be dropped. And also the ProgramPreferenceName class
         * should be dropped.
         */

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
