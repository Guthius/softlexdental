/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
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
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace OpenDentBusiness
{
    public class Preference : DataRecordBase
    {
        static readonly IDataRecordCacheBase<Preference> cache =
            new DataRecordCacheBase<Preference>("SELECT * FROM `preferences`", FromReader);

        /// <summary>
        /// The key of the preference.
        /// </summary>
        public string Key;
        
        /// <summary>
        /// The value of the preference.
        /// </summary>
        public string Value;

        /// <summary>
        /// Reads a preference from the specified data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <returns>The preference.</returns>
        static Preference FromReader(MySqlDataReader dataReader)
        {
            return new Preference
            {
                Key = Convert.ToString(dataReader["key"]),
                Value = Convert.ToString(dataReader["value"])
            };
        }

        /// <summary>
        /// Determines whether a preference with the specified name exists in the cache.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <returns>True if the preference exists in the cache; otherwise, false.</returns>
        public static bool Exists(PreferenceName preferenceName) => Exists(preferenceName.ToString());

        /// <summary>
        /// Determines whether a preference with the specified key exists in the cache.
        /// </summary>
        /// <param name="preferenceKey">The key of the preference.</param>
        /// <returns>True if the preference exists in the cache; otherwise, false.</returns>
        public static bool Exists(string preferenceKey)
        {
            foreach (var preference in cache)
            {
                if (preference.Key.Equals(preferenceKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <returns>The preference.</returns>
        /// <exception cref="DataException">When no preference with the specified name exists.</exception>
        public static Preference GetByName(PreferenceName preferenceName) => GetByKey(preferenceName.ToString());

        /// <summary>
        /// Gets the preference with the specified key.
        /// </summary>
        /// <param name="preferenceKey">The key of the preference.</param>
        /// <returns>The preference.</returns>
        /// <exception cref="DataException">When no preference with the specified key exists.</exception>
        public static Preference GetByKey(string preferenceKey) =>
            cache.SelectOne(p => p.Key == preferenceKey);

        /// <summary>
        /// Gets the string value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>The preference value.</returns>
        public static string GetString(PreferenceName preferenceName, string defaultValue = "") =>
            GetByName(preferenceName)?.Value ?? defaultValue;

        /// <summary>
        /// Gets the string value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceKey">The key of the preference.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>The preference value.</returns>
        public static string GetString(string preferenceKey, string defaultValue = "") =>
            GetByKey(preferenceKey)?.Value ?? defaultValue;

        /// <summary>
        /// Gets the string value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <returns>The preference value.</returns>
        public static string GetStringNoCache(PreferenceName preferenceName) =>
            DataConnection.ExecuteString(
                "SELECT `value` FROM `preferences` WHERE `key` = ?key",
                    new MySqlParameter("key", preferenceName.ToString()));

        /// <summary>
        /// Gets the integer value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>The preference value.</returns>
        public static int GetInt(PreferenceName preferenceName, int defaultValue = 0)
        {
            if (int.TryParse(GetString(preferenceName), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the long value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>The preference value.</returns>
        public static long GetLong(PreferenceName preferenceName, long defaultValue = 0)
        {
            if (long.TryParse(GetString(preferenceName), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the byte value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>The preference value.</returns>
        public static byte GetByte(PreferenceName preferenceName, byte defaultValue = 0)
        {
            if (byte.TryParse(GetString(preferenceName), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the double value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>The preference value.</returns>
        public static double GetDouble(PreferenceName preferenceName, double defaultValue = 0)
        {
            if (double.TryParse(GetString(preferenceName), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the boolean value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <param name="defaultValue">The default value of the preference.</param>
        /// <returns>The preference value.</returns>
        public static bool GetBool(PreferenceName preferenceName, bool defaultValue = false)
        {
            if (bool.TryParse(GetString(preferenceName), out var result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the boolean value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <returns>The preference value.</returns>
        public static bool GetBoolNoCache(PreferenceName preferenceName)
        {
            var value =
                DataConnection.ExecuteString(
                    "SELECT `value` FROM preferences WHERE `key` = ?key",
                        new MySqlParameter("key", preferenceName.ToString()));

            if (value != null)
            {
                if (bool.TryParse(value, out var result))
                {
                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the date/time value of the preference with the specified name.
        /// Returns <see cref="DateTime.MinValue"/> if no preference with the specified name exists.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <returns>The preference value.</returns>
        public static DateTime GetDateTime(PreferenceName preferenceName)
        {
            if (DateTime.TryParse(GetString(preferenceName), out var result))
            {
                return result;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Gets the date value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <returns>The preference value.</returns>
        public static DateTime GetDate(PreferenceName preferenceName) => 
            GetDateTime(preferenceName).Date;

        /// <summary>
        /// Gets the color value of the preference with the speicied name.
        /// </summary>
        /// <param name="preferenceName">The preference name.</param>
        /// <returns>The preference value.</returns>
        public static Color GetColor(PreferenceName preferenceName) => 
            Color.FromArgb(GetInt(preferenceName));

        /// <summary>
        /// Inserts the specified preference into the database.
        /// </summary>
        /// <param name="preference"></param>
        public static void Insert(Preference preference) =>
            DataConnection.ExecuteNonQuery(
                "INSERT INTO preferences (`key`, `value`) VALUES (?key, ?value) ON DUPLICATE KEY UPDATE `value` = ?value",
                    new MySqlParameter("key", preference.Key ?? ""),
                    new MySqlParameter("value", preference.Value ?? ""));

        /// <summary>
        /// Updates the value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(PreferenceName preferenceName, long value) =>
            Update(preferenceName, value.ToString());

        /// <summary>
        /// Updates the value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(PreferenceName preferenceName, int value) =>
            Update(preferenceName, value.ToString());

        /// <summary>
        /// Updates the value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(PreferenceName preferenceName, byte value) =>
            Update(preferenceName, value.ToString());

        /// <summary>
        /// Updates the value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <param name="round">A value indicating whether to round the value to 2 decimals.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(PreferenceName preferenceName, double value, bool round = true)
        {
            if (round)
            {
                value = Math.Round(value, 2);
            }

            return Update(preferenceName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Updates the value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(PreferenceName preferenceName, bool value) =>
            Update(preferenceName, value.ToString());

        /// <summary>
        /// Updates the value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(PreferenceName preferenceName, DateTime value) =>
            Update(preferenceName, value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

        /// <summary>
        /// Updates the value of the preference with the specified name.
        /// </summary>
        /// <param name="preferenceName">The name of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(PreferenceName preferenceName, string value) =>
            Update(preferenceName.ToString(), value);

        /// <summary>
        /// Updates the value of the preference with the specified key.
        /// </summary>
        /// <param name="preferenceKey">The key of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        /// <returns>A value indicating whether the preference was updated.</returns>
        public static bool Update(string preferenceKey, string value)
        {
            value = value ?? "";

            var preference = GetByKey(preferenceKey);

            if (preference != null)
            {
                if (!preference.Value.Equals(value, StringComparison.InvariantCulture))
                {
                    preference.Value = value;

                    Insert(preference);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Deletes the preference with the specified key from the database.
        /// </summary>
        /// <param name="preferenceKey">The preference key.</param>
        public static void Delete(string preferenceKey) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `preferences` WHERE `key` = ?key",
                    new MySqlParameter("key", preferenceKey ?? ""));

        /// <summary>
        /// Refreshes the preferences cache.
        /// </summary>
        public static void Refresh() => cache.Refresh();

        #region CLEANUP

        ///<summary>For UI display when we store a zero/meaningless value as -1. Returns "0" when useZero is true, otherwise "".</summary>
        public static string GetLongHideNegOne(PreferenceName prefName, bool useZero = false)
        {
            long prefVal = GetLong(prefName);
            if (prefVal == -1)
            {
                return useZero ? "0" : "";
            }
            return POut.Long(prefVal);
        }

        ///<summary>For saving from the UI when we want "0" or "" to be saved as a -1 in the database.</summary>
        public static bool UpdateLongAsNegOne(PreferenceName prefName, string newVal)
        {
            long val = -1;
            if (!string.IsNullOrWhiteSpace(newVal))
            {
                val = PIn.Long(newVal) > 0 ? PIn.Long(newVal) : -1;
            }
            return Update(prefName, val);
        }

        /// <summary>
        /// Gets the practice wide default PrefName that corresponds to the passed in sheet type.
        /// </summary>
        public static PreferenceName GetSheetDefPref(SheetTypeEnum sheetType)
        {
            PreferenceName preferenceName;
            switch (sheetType)
            {
                case SheetTypeEnum.Consent:
                    preferenceName = PreferenceName.SheetsDefaultConsent;
                    break;
                case SheetTypeEnum.DepositSlip:
                    preferenceName = PreferenceName.SheetsDefaultDepositSlip;
                    break;
                case SheetTypeEnum.ExamSheet:
                    preferenceName = PreferenceName.SheetsDefaultExamSheet;
                    break;
                case SheetTypeEnum.LabelAppointment:
                    preferenceName = PreferenceName.SheetsDefaultLabelAppointment;
                    break;
                case SheetTypeEnum.LabelCarrier:
                    preferenceName = PreferenceName.SheetsDefaultLabelCarrier;
                    break;
                case SheetTypeEnum.LabelPatient:
                    preferenceName = PreferenceName.SheetsDefaultLabelPatient;
                    break;
                case SheetTypeEnum.LabelReferral:
                    preferenceName = PreferenceName.SheetsDefaultLabelReferral;
                    break;
                case SheetTypeEnum.LabSlip:
                    preferenceName = PreferenceName.SheetsDefaultLabSlip;
                    break;
                case SheetTypeEnum.MedicalHistory:
                    preferenceName = PreferenceName.SheetsDefaultMedicalHistory;
                    break;
                case SheetTypeEnum.MedLabResults:
                    preferenceName = PreferenceName.SheetsDefaultMedLabResults;
                    break;
                case SheetTypeEnum.PatientForm:
                    preferenceName = PreferenceName.SheetsDefaultPatientForm;
                    break;
                case SheetTypeEnum.PatientLetter:
                    preferenceName = PreferenceName.SheetsDefaultPatientLetter;
                    break;
                case SheetTypeEnum.PaymentPlan:
                    preferenceName = PreferenceName.SheetsDefaultPaymentPlan;
                    break;
                case SheetTypeEnum.ReferralLetter:
                    preferenceName = PreferenceName.SheetsDefaultReferralLetter;
                    break;
                case SheetTypeEnum.ReferralSlip:
                    preferenceName = PreferenceName.SheetsDefaultReferralSlip;
                    break;
                case SheetTypeEnum.RoutingSlip:
                    preferenceName = PreferenceName.SheetsDefaultRoutingSlip;
                    break;
                case SheetTypeEnum.Rx:
                    preferenceName = PreferenceName.SheetsDefaultRx;
                    break;
                case SheetTypeEnum.RxMulti:
                    preferenceName = PreferenceName.SheetsDefaultRxMulti;
                    break;
                case SheetTypeEnum.Screening:
                    preferenceName = PreferenceName.SheetsDefaultScreening;
                    break;
                case SheetTypeEnum.Statement:
                    preferenceName = PreferenceName.SheetsDefaultStatement;
                    break;
                case SheetTypeEnum.TreatmentPlan:
                    preferenceName = PreferenceName.SheetsDefaultTreatmentPlan;
                    break;
                case SheetTypeEnum.RxInstruction:
                    preferenceName = PreferenceName.SheetsDefaultRxInstructions;
                    break;
                default:
                    throw new Exception("Unsupported SheetTypeEnum\r\n" + sheetType.ToString());
            }
            return preferenceName;
        }

        public static IEnumerable<Preference> GetByName(params PreferenceName[] preferenceNames)
        {
            foreach (var preferenceName in preferenceNames)
            {
                var preference = GetByName(preferenceName);

                if (preference != null)
                {
                    yield return preference;
                }
            }
        }

        public static IEnumerable<Preference> GetInsHistPrefs() =>
            GetByName(
                PreferenceName.InsHistBWCodes,
                PreferenceName.InsHistDebridementCodes,
                PreferenceName.InsHistExamCodes,
                PreferenceName.InsHistPanoCodes,
                PreferenceName.InsHistPerioLLCodes,
                PreferenceName.InsHistPerioLRCodes,
                PreferenceName.InsHistPerioMaintCodes,
                PreferenceName.InsHistPerioULCodes,
                PreferenceName.InsHistPerioURCodes,
                PreferenceName.InsHistProphyCodes);
        

        /// <summary>
        /// Returns a list of all of the InsHist PrefNames.
        /// </summary>
        public static List<PreferenceName> GetInsHistPrefNames()
        {
            return new List<PreferenceName> {
                PreferenceName.InsHistBWCodes,
                PreferenceName.InsHistPanoCodes,
                PreferenceName.InsHistExamCodes,
                PreferenceName.InsHistProphyCodes,
                PreferenceName.InsHistPerioURCodes,
                PreferenceName.InsHistPerioULCodes,
                PreferenceName.InsHistPerioLRCodes,
                PreferenceName.InsHistPerioLLCodes,
                PreferenceName.InsHistPerioMaintCodes,
                PreferenceName.InsHistDebridementCodes
            };
        }

        #endregion
    }
}