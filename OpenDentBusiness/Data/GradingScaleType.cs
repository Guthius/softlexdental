/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/

namespace OpenDentBusiness
{
    /// <summary>
    /// Used in GradingScale to determine how grades are assigned.
    /// </summary>
    public enum GradingScaleType
    {
        /// <summary>
        /// User-Defined list of possible grades. Grade is calculated as an average.
        /// </summary>
        PickList = 0,

        /// <summary>
        /// Percentage Scale 0-100. Grade is calculated as an average.
        /// </summary>
        Percentage = 1,

        /// <summary>
        /// Allows point values for grades. Grade is calculated as a sum of all points out of points possible.
        /// </summary>
        Weighted = 2
    }
}