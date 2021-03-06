﻿using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Stores information on mobile app devices. These are devices that utilize the Xamarin mobile application.
    /// </summary>
    public class MobileAppDevice
    {
        public long MobileAppDeviceNum;

        ///<summary>FK to clinic.ClinicNum.</summary>
        public long ClinicNum;

        ///<summary>The name of the device.</summary>
        public string DeviceName;

        ///<summary>The unique identifier of the device. Platform specific.</summary>
        public string UniqueID;

        ///<summary>Indicates whether the device is allowed to operate the checkin app.</summary>
        public bool IsAllowed;

        ///<summary>The date and time of the last attempted login.</summary>
        public DateTime LastAttempt;

        ///<summary>The date and time of the last succesful login.</summary>
        public DateTime LastLogin;

        ///<summary>Returns a copy of this MobileAppDevice.</summary>
        public MobileAppDevice Copy()
        {
            return (MobileAppDevice)this.MemberwiseClone();
        }
    }
}
