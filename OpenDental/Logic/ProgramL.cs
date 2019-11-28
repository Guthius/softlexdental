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
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public class ProgramL
    {
        /// <summary>
        ///     <para>
        ///         Determines whether the specified type represents a valid (e.g. usable) bridge 
        ///         type.
        ///     </para>
        ///     <para>
        ///         If the type is not valid; the <paramref name="errorMessage"/> parameter will be
        ///         updated with the reason why it is not considered valid. This message is 
        ///         included with the error message that is displayed to users when they try to use
        ///         a brige of the given invalid <paramref name="type"/>.
        ///     </para>
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="errorMessage">The error, set if the type is not valid.</param>
        /// <returns>True if the type is valid; otherwise, false.</returns>
        private static bool IsValidBridgeType(Type type, out string errorMessage)
        {
            errorMessage = "";

            if (!type.IsClass)
            {
                errorMessage = "The bridge is not a class.";
            }
            else if (!type.IsPublic)
            {
                errorMessage = "The bridge is marked as private.";
            }
            else if (type.IsAbstract)
            {
                errorMessage = "The bridge type is marked as abstract.";
            }
            else if (!typeof(IBridge).IsAssignableFrom(type))
            {
                errorMessage = "The bridge type does not derive from the correct base class.";
            }

            return errorMessage.Length == 0;
        }

        /// <summary>
        /// Typically used when user clicks a button to a Program link.
        /// This method attempts to identify and execute the program based on the given programNum.
        /// </summary>
        public static void Execute(long programId, Patient patient)
        {
            var program = Program.GetById(programId);

            if (program == null)
            {
                MessageBox.Show(
                    "Error, program entry not found in database.", 
                    "Program", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (patient != null && Preference.GetBool(PreferenceName.ShowFeaturePatientClone))
            {
                patient = Patients.GetOriginalPatientForClone(patient);
            }

            // Find the type for the requested program.
            var type = Type.GetType(program.TypeName);
            if (type == null)
            {
                MessageBox.Show(
                    "The bridge interface for the selected program could not be found. " +
                    "Make sure the program bridge plugin is installed correctly and try again.",
                    "Program",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Make sure the type is valid usable type.
            if (!IsValidBridgeType(type, out var errorMessage))
            {
                MessageBox.Show(
                    "The bridge interface configured for this program is invalid. " + errorMessage,
                    "Program",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                var bridge = (IBridge)Activator.CreateInstance(type);
                if (bridge != null)
                {
                    bridge.Send(program.Id, patient);
                }
            }
            catch (Exception exception)
            {
                // TODO: We might want more friendly and informative message here...

                MessageBox.Show(
                    exception.Message,
                    "Program",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (program.TypeName == ProgramName.XVWeb.ToString())
            {
                XVWebBridge.SendData(program, patient);
                return;
            }
        }

        public static void LoadToolbar(ODToolBar toolBar, ToolBarsAvail toolBarsAvail)
        {
            var toolButItems = ToolButItems.GetForToolBar(toolBarsAvail);
            foreach (var toolButItem in toolButItems)
            {
                var program = Program.GetById(toolButItem.ProgramNum);
                if (program == null)
                {
                    continue;
                }

                // TODO: Set the button image...

                if (toolBarsAvail != ToolBarsAvail.MainToolbar)
                {
                    toolBar.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
                }

                var button = new ODToolBarButton(toolButItem.ButtonText, null, "", program);

                AddDropDown(button, program);

                toolBar.Buttons.Add(button);
            }
        }

        /// <summary>
        ///     <para>
        ///         Adds a drop down menu if this program requires it.
        ///     </para>
        /// </summary>
        private static void AddDropDown(ODToolBarButton button, Program program)
        {
            if (program.TypeName == typeof(OryxBridge).FullName)
            {
                var menuItem = new MenuItem
                {
                    Index = 0,
                    Text = "User Settings"
                };

                menuItem.Click += OryxBridge.MenuItemUserSettingsClick;

                var contextMenu = new ContextMenu();

                contextMenu.MenuItems.AddRange(new MenuItem[] {
                        menuItem,
                });

                button.Style = ODToolBarButtonStyle.DropDownButton;
                button.DropDownMenu = contextMenu;
            }
        }
    }
}
