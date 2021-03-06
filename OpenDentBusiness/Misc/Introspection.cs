﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class Introspection
    {
        ///<summary>The dictionary of testing overrides.  This variable should ONLY be used from within the DictOverrides property.</summary>
        private static Dictionary<IntrospectionEntity, string> _dictOverrides;

        ///<summary>Fills _dictOverrides when it is null and returns it.  Will always return null if the IntrospectionItems preference is not present in the database.
        ///This getter will check the preference cache for the aforementioned preference until it finds it.
        ///Once found, _dictOverrides will be instantiated and filled with the contents of the preference.  Once instatiated, this getter will never repopulate the dictionary.
        ///If the preference is present in the database but is malformed JSON, _dictOverrides will be an empty dictionary which will throw exceptions later on in the program.</summary>
        private static Dictionary<IntrospectionEntity, string> DictOverrides
        {
            get
            {
                if (_dictOverrides != null || !Preference.Exists(PreferenceName.IntrospectionItems))
                {
                    return _dictOverrides;
                }
                //The dictionary of overrides has not been filled before or the preference does not exist in the database.
                try
                {
                    string introspectionItems = Preference.GetString(PreferenceName.IntrospectionItems);//Cache call so it is fine to do this a lot.  Purposefully throws exceptions.
                                                                                                        //At this point we know the database has the IntrospectionItems preference so we need to instantiate _dictOverrides.
                    _dictOverrides = new Dictionary<IntrospectionEntity, string>();
                    //Try an deserialize the preference value into the dictionary.
                    //If this throws, the engineer will get a UE later in the program stating that their corresponding IntrospectionEntity could not be found (dictionary not filled out correctly).
                    _dictOverrides = JsonConvert.DeserializeObject<Dictionary<IntrospectionEntity, string>>(introspectionItems);
                }
                catch
                {

                }
                return _dictOverrides;
            }
        }

        ///<summary>Returns true if the IntrospectionItems preference is present within the preference cache.  Otherwise; false.</summary>
        public static bool IsTestingMode
        {
            get
            {
                return (DictOverrides != null);
            }
        }

        ///<summary>Returns the defaultValue passed in if the entity cannot be found in the global dictionary of testing overrides.
        ///Purposefully throws an exception (not meant to be caught) if the IntrospectionItems preference is present in the database (should be missing in general)
        ///and does not contain the entity that was passed in.  This will mean that the preference is malformed or is out of date and the preference value needs to be updated.</summary>
        public static string GetOverride(IntrospectionEntity entity, string defaultValue = "")
        {
            if (DictOverrides != null)
            {
                //DictOverrides was not null so we can assume it has the IntrospectionEntity passed in.
                //If the dictionary does not have the entity passed in then we will purposefully throw an exception tailored for engineers.
                string overrideStr;
                if (!DictOverrides.TryGetValue(entity, out overrideStr))
                {
                    throw new ApplicationException("Testing mode is on and the following introspection entity is not present in the IntrospectionItems preference: " + entity.ToString());
                }
                return overrideStr;
            }
            //The database does not have the IntrospectionItems preference so return defaultValue.
            return defaultValue;
        }

        ///<summary>Only used for unit tests. SHOULD NOT be used otherwise.</summary>
        public static void ClearDictOverrides()
        {
            //This wouldn't be the end of the world if a non-unit test class does this because it just causes the dictionary to automatically refresh itself later.
            _dictOverrides = null;
        }

        /// <summary>
        /// Holds 3rd party API information. IF AN ENTRY IS ADDED TO THIS ENUM PLEASE UPDATE THE QUERY BELOW.
        /// </summary>
        public enum IntrospectionEntity
        {
            DentalXChangeDwsDebugURL,
            DentalXChangeDeaDebugURL,
            DoseSpotDebugURL,
            PayConnectRestDebugURL,
        }
    }
}

/*TIP:  Hold Alt and drag to select the query without C# comment garbage if you are unfortunate enough to need to run this.*/
/*

 INSERT INTO preference (PrefName,ValueString)
 VALUES('IntrospectionItems',
	'{
		"DentalXChangeDwsDebugURL":"https://prelive2.dentalxchange.com/dws/DwsService",
		"DentalXChangeDeaDebugURL":"https://prelive2.dentalxchange.com/dea/DeaPartnerService",
		"DoseSpotDebugURL":"https://my.staging.dosespot.com/webapi",
		"PayConnectRestDebugURL":"https://https://prelive2.dentalxchange.com/pay/rest/PayService"
	}'
 );

*/