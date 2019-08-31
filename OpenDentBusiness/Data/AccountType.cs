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
    /// Identifies the type of a <see cref="Account"/>. Used in accounting for chart of accounts.
    /// </summary>
    public enum AccountType
    {
        Asset = 0,
        Liability = 1,
        Equity = 2,
        Income = 3,
        Expense = 4
    }
}