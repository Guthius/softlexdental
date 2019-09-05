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
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Drawing;

namespace OpenDental
{
    public partial class DashPatPicture : ODPictureBox, IDashWidgetField
    {
        private Bitmap patientImage;
        private Document document;

        public void RefreshData(Patient patient, SheetField sheetField)
        {
            if (patient == null) return;

            try
            {
                long documentId = long.Parse(sheetField.FieldValue);
                if (document == null || documentId != document.DocNum)
                {
                    document = Documents.GetByNum(documentId, true);

                    using (var image = ImageHelper.GetFullImage(document, ImageStore.GetPatientFolder(patient)))
                    {
                        patientImage = ImageHelper.GetThumbnail(image, Math.Min(sheetField.Width, sheetField.Height));
                    }
                }
            }
            catch
            {
                patientImage?.Dispose();
                patientImage = null;
            }
        }

        public void RefreshView()
        {
            Image = patientImage;
            HasBorder = true;
            TextNullImage = "Patient Picture Unavailable";
        }
    }
}
