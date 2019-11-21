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

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Storage class for common program preference names.
    ///     </para>
    /// </summary>
    public static class ProgramPreferenceName
    {
        /// <summary>
        /// The path of the executable to run or file to open.
        /// </summary>
        public const string ProgramPath = "program_path";

        /// <summary>
        /// the command line arguments to pass to the bridge.
        /// </summary>
        public const string CommandLineArguments = "cmd_line_args";

        /// <summary>
        /// For custom program links only. Stores the path of a file to be generated when launching the program link.
        /// </summary>
        public const string FilePath = "file_path";

        /// <summary>
        /// For custom program links only.  Stores the template of a file to be generated when launching the program link.
        /// </summary>
        public const string FileTemplate = "file_template";

        /// <summary>
        /// Indicates whether to use the chart number of the patient; when false the patient ID is used instead.
        /// </summary>
        public const string UseChartNumber = "use_chart_number";

        /// <summary>
        /// Specifies the date format to use when exporting dates.
        /// </summary>
        public const string DateFormat = "date_format";
    }
}
