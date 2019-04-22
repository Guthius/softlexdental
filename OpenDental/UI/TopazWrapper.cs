using System;
using System.Windows.Forms;
using OpenDentBusiness;
using Topaz;

namespace OpenDental.UI
{
    public class TopazWrapper
    {
        // The plugin calls in this class were added in order to use a Topaz pad in a RDP/Citrix remote session environment so that signature pads can be
        // plugged in on the client computer while the signature displays in the SigPlusNET UI control on the terminal server.  The signature pad and the
        // UI control are always kept in sync, i.e. the state, encryption and compression modes, key data and key string, signature string etc.  This means
        // the GetTopaz... methods could retrieve data from the UI control and not the remote signature pad.  The GetTopazNumberOfTabletPoints method returns
        // 0 if sent to the remote device, so it must be called on the local UI control.

        // The SetTopaz... methods have a plugin call in order for HSB signature pads to be used in a RDP/Citrix remote session environment.  It may not be
        // necessary to send all of the SetTopaz... methods to the device connected to the client machine.  The encryption and compression modes and the
        // key data and key string are only necessary when saving the signature, not during signature capture.  Since the captured signature is duplicated
        // on the server in the SigPlusNET control in the OD UI, we can set the compression, encryption, key data, and key string on the control before
        // getting the signature string and validate it using the control as well.  We will leave the plugin calls just in case they are needed in the future.

        public static Control GetTopaz()
        {
            Control retVal = new SigPlusNET();
            retVal.HandleDestroyed += new EventHandler(topazWrapper_HandleDestroyed);
            return retVal;
        }

        /// <summary>
        /// This allows third-party software to unload device/clean-up.
        /// </summary>
        static void topazWrapper_HandleDestroyed(object sender, EventArgs e)
        {
            Plugin.Trigger(null, "TopazWrapper_HandleDestroyed");
        }

        /// <summary>
        /// 0 = disable signature capture.
        /// 1 = enable capture.
        /// </summary>
        public static int GetTopazState(Control topaz)
        {
            int stateCur = 0;

            if (topaz is SigPlusNET sigPlusNet)
            {
                stateCur = sigPlusNet.GetTabletState();
            }
            return Plugin.Filter(null, "TopazWrapper_GetTopazState", stateCur, topaz);
        }

        /// <summary>
        /// Returns 0 if the signature is not valid, i.e. the compression or encryption modes are not the same used when the signature was saved,
        /// or the key data is different than that used to save the signature, or the signature string is blank, etc. 
        /// Valid signatures should consist of at least 200 points, as this represents approximately 1 second of active signature time.  
        /// We will consider any value greater than 0 to mean a signature is valid.
        /// </summary>
        public static int GetTopazNumberOfTabletPoints(Control topaz)
        {
            return ((SigPlusNET)topaz).NumberOfTabletPoints();
        }

        public static string GetTopazString(Control topaz)
        {
            // If in remote session, always get the sig string from the UI control, 
            // not the remote sig pad, because a form can have more than one sig box
            // and the sig pad will only have the most recent sig string.
            return ((SigPlusNET)topaz).GetSigString();
        }

        public static void ClearTopaz(Control topaz)
        {
            if (topaz is SigPlusNET sigPlusNet)
            {
                sigPlusNet.ClearTablet();
                sigPlusNet.SetTabletLogicalXSize(2000);
                sigPlusNet.SetTabletLogicalYSize(600);
            }

            Plugin.Trigger(null, "TopazWrapper_ClearTopaz", topaz);
        }

        public static void SetTopazCompressionMode(Control topaz, int compressionMode)
        {
            if (topaz is SigPlusNET sigPlusNet) sigPlusNet.SetSigCompressionMode(compressionMode);

            Plugin.Trigger(null, "TopazWrapper_SetTopazCompressionMode", topaz, compressionMode);
        }

        public static void SetTopazEncryptionMode(Control topaz, int encryptionMode)
        {
            if (topaz is SigPlusNET sigPlusNet) sigPlusNet.SetEncryptionMode(encryptionMode);

            Plugin.Trigger(null, "TopazWrapper_SetTopazEncryptionMode", topaz, encryptionMode);
        }

        public static void SetTopazKeyString(Control topaz, string str)
        {
            if (topaz is SigPlusNET sigPlusNet) sigPlusNet.SetKeyString(str);

            Plugin.Trigger(null, "TopazWrapper_SetTopazKeyString", topaz, str);
        }

        public static void SetTopazAutoKeyData(Control topaz, string data)
        {
            if (topaz is SigPlusNET sigPlusNet)
            {
                sigPlusNet.AutoKeyStart();
                sigPlusNet.SetAutoKeyData(data);
                sigPlusNet.AutoKeyFinish();
            }

            Plugin.Trigger(null, "TopazWrapper_SetTopazAutoKeyData", topaz, data);
        }

        public static void SetTopazSigString(Control topaz, string signature)
        {
            if (topaz is SigPlusNET sigPlusNet) sigPlusNet.SetSigString(signature);

            // This hook shouldn't be implemented or necessary, we only set/get the sig string 
            // on the local UI control, so this command shouldn't be sent to the remote connection sig pad
            Plugin.Trigger(null, "TopazWrapper_SetTopazSigString", topaz, signature);
        }

        /// <summary>
        /// 0 = disable signature capture
        /// 1 = enable
        /// </summary>
        public static void SetTopazState(Control topaz, int state)
        {
            if (Plugin.Filter(null, "TopazWrapper_SetTopazState", false, topaz, state))
            {
                return;
            }
            if (topaz is SigPlusNET sigPlusNet) sigPlusNet.SetTabletState(state);
        }
    }
}