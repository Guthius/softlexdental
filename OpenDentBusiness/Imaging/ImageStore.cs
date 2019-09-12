using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class ImageStore
    {
        public static string GetPatientFolder(Patient patient)
        {
            var path = "0";

            string name = string.Concat(patient.LName, patient.FName);
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsLetter(name[i]))
                {
                    path = char.ToUpper(name[i]).ToString();

                    break;
                }
            }

            path = Storage.Default.CombinePath("Patients", path, patient.PatNum.ToString().PadLeft(8, '0'));
            if (!Storage.Default.DirectoryExists(path))
            {
                try
                {
                    Storage.Default.CreateDirectory(path);
                }
                catch
                {
                    throw new Exception("Error.  Could not create folder for patient: " + path);
                }
            }

            return path;
        }

        private static string CreateDirectoryIfNotExists(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!Storage.Default.DirectoryExists(path))
                {
                    Storage.Default.CreateDirectory(path);
                }
            }
            return path;
        }

        /// <summary>
        /// Will create folder if needed.  Will validate that folder exists.
        /// </summary>
        public static string GetEobFolder() => CreateDirectoryIfNotExists("EOBs");

        ///<summary>Will create folder if needed.  Will validate that folder exists.</summary>
        public static string GetAmdFolder() => CreateDirectoryIfNotExists("Amendments");

        /// <summary>
        /// Gets the folder name where provider images are stored. Will create folder if needed.
        /// </summary>
        public static string GetProviderImagesFolder() => CreateDirectoryIfNotExists("ProviderImages");

        public static string GetEmailImagePath() => CreateDirectoryIfNotExists("EmailImages");

        public static void AddMissingFilesToDatabase(Patient patient)
        {
            // Scans the patient folder and scans for files that are not in the database and imports them automatically.

        }

        public static string GetHashString(Document document, string patientPath)
        {
            var bytes = Storage.Default.ReadAllBytes(Storage.Default.CombinePath(patientPath, document.FileName));

            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(bytes);

                var stringBuilder = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        public static IEnumerable<Image> OpenImages(IEnumerable<Document> documents, string patientPath)
        {
            foreach (var document in documents)
            {
                if (document != null)
                {
                    yield return OpenImage(document, patientPath);
                }
            }
        }

        public static Image OpenImage(Document document, string patientPath)
        {
            string path = Storage.Default.CombinePath(patientPath, document.FileName);

            if (HasImageExtension(path))
            {
                using (var stream = Storage.Default.OpenRead(path))
                {
                    return Image.FromStream(stream);
                }
            }

            return null;
        }

        public static IEnumerable<Image> OpenImagesEob(EobAttach eob)
        {
            string path = Storage.Default.CombinePath(GetEobFolder(), eob.FileName);

            if (HasImageExtension(path))
            {
                if (Storage.Default.FileExists(path))
                {
                    using (var stream = Storage.Default.OpenRead(path))
                    {
                        yield return Image.FromStream(stream);
                    }
                }
                else
                {
                    throw new ApplicationException("File not found: " + path);
                }
            }
        }

        public static IEnumerable<Image> OpenImagesAmd(EhrAmendment amd)
        {
            string path = Storage.Default.CombinePath(GetAmdFolder(), amd.FileName);

            if (HasImageExtension(path))
            {
                if (Storage.Default.FileExists(path))
                {
                    using (var stream = Storage.Default.OpenRead(path))
                    {
                        yield return Image.FromStream(stream);
                    }
                }
                else
                {
                    throw new ApplicationException("File not found: " + path);
                }
            }
        }

        public static Document Import(string importFileName, long docCategory, Patient patient)
        {
            string patientPath = GetPatientFolder(patient);
            
            var document = new Document();
            if (Path.GetExtension(importFileName) == "")
            {
                try
                {
                    using (var stream = Storage.Default.OpenRead(Storage.Default.CombinePath(patientPath, importFileName)))
                    {
                        var image = Image.FromStream(stream);
                    }

                    document.FileName = ".jpg";
                }
                catch
                {
                    document.FileName = ".txt";
                }
            }
            else
            {
                document.FileName = Path.GetExtension(importFileName);
            }
            document.DateCreated = File.GetLastWriteTime(importFileName);
            document.PatNum = patient.PatNum;
            if (HasImageExtension(document.FileName))
            {
                document.ImgType = ImageType.Photo;
            }
            else
            {
                document.ImgType = ImageType.Document;
            }
            document.DocCategory = docCategory;
            document = Documents.InsertAndGet(document, patient);

            try
            {
                SaveDocument(document, importFileName, patientPath);
            }
            catch (Exception ex)
            {
                Documents.Delete(document);

                throw ex;
            }
            return document;
        }

        public static Document Import(Image image, long docCategory, ImageType imageType, Patient pat, string fileName = "")
        {
            string patFolder = GetPatientFolder(pat);

            var document = new Document
            {
                ImgType = imageType,
                FileName = fileName + ".jpg",
                DateCreated = DateTime.Now,
                PatNum = pat.PatNum,
                DocCategory = docCategory
            };

            Documents.Insert(document, pat);

            long qualityL;
            if (imageType == ImageType.Radiograph)
            {
                qualityL = 100;
            }
            else if (imageType == ImageType.Photo)
            {
                qualityL = 100;
            }
            else
            {
                qualityL = ComputerPrefs.LocalComputer.ScanDocQuality;
            }

            ImageCodecInfo myImageCodecInfo;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            myImageCodecInfo = null;
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == "image/jpeg")
                {
                    myImageCodecInfo = encoders[j];
                }
            }
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityL);
            myEncoderParameters.Param[0] = myEncoderParameter;

            try
            {
                SaveDocument(document, image, myImageCodecInfo, myEncoderParameters, patFolder);
            }
            catch
            {

                Documents.Delete(document);

                throw;
            }
            return document;
        }

        public static Document ImportForm(string form, long docCategory, Patient patient)
        {
            var formPath = Storage.Default.CombinePath("Forms", form);
            if (!Storage.Default.FileExists(formPath))
            {
                throw new Exception("Could not find file: " + formPath);
            }

            var document = new Document
            {
                FileName = Path.GetExtension(formPath),
                DateCreated = DateTime.Now,
                DocCategory = docCategory,
                PatNum = patient.PatNum,
                ImgType = ImageType.Document
            };

            Documents.Insert(document, patient);
            try
            {
                SaveDocument(document, formPath, GetPatientFolder(patient));
            }
            catch
            {
                Documents.Delete(document);

                throw;
            }
            return document;
        }

        public static Document ImportImageToMount(Bitmap image, short rotationAngle, long mountItemNum, long docCategory, Patient patient)
        {
            var document = new Document
            {
                MountItemNum = mountItemNum,
                DegreesRotated = rotationAngle,
                ImgType = ImageType.Radiograph,
                FileName = ".bmp",
                DateCreated = DateTime.Now,
                PatNum = patient.PatNum,
                DocCategory = docCategory,
                WindowingMin = Preference.GetInt(PreferenceName.ImageWindowingMin),
                WindowingMax = Preference.GetInt(PreferenceName.ImageWindowingMax)
            };

            Documents.Insert(document, patient);
            try
            {
                SaveDocument(document, image, ImageFormat.Bmp, GetPatientFolder(patient));
            }
            catch
            {
                Documents.Delete(document);

                throw;
            }
            return document;
        }

        public static EobAttach ImportEobAttach(Bitmap image, long claimPaymentNum)
        {
            string eobFolder = GetEobFolder();

            EobAttach eob = new EobAttach
            {
                FileName = ".jpg",
                DateTCreated = DateTime.Now,
                ClaimPaymentNum = claimPaymentNum
            };
            EobAttaches.Insert(eob);

            long qualityL = ComputerPrefs.LocalComputer.ScanDocQuality;

            ImageCodecInfo myImageCodecInfo;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            myImageCodecInfo = null;
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == "image/jpeg")
                {
                    myImageCodecInfo = encoders[j];
                }
            }

            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityL);
            myEncoderParameters.Param[0] = myEncoderParameter;

            try
            {
                SaveEobAttach(eob, image, myImageCodecInfo, myEncoderParameters, eobFolder);
            }
            catch
            {
                EobAttaches.Delete(eob.EobAttachNum);

                throw;
            }
            return eob;
        }


        public static EobAttach ImportEobAttach(string pathImportFrom, long claimPaymentNum)
        {
            string eobFolder = GetEobFolder();
            
            EobAttach eob = new EobAttach();
            if (Path.GetExtension(pathImportFrom) == "")
            {
                eob.FileName = ".jpg";
            }
            else
            {
                eob.FileName = Path.GetExtension(pathImportFrom);
            }

            eob.DateTCreated = File.GetLastWriteTime(pathImportFrom);
            eob.ClaimPaymentNum = claimPaymentNum;

            EobAttaches.Insert(eob);
            try
            {
                SaveEobAttach(eob, pathImportFrom, eobFolder);
            }
            catch
            {
                EobAttaches.Delete(eob.EobAttachNum);

                throw;
            }
            return eob;
        }

        public static EhrAmendment ImportAmdAttach(Bitmap image, EhrAmendment amd)
        {
            string amdFolder = GetAmdFolder();
            
            amd.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + amd.EhrAmendmentNum;
            amd.FileName += ".jpg";
            amd.DateTAppend = DateTime.Now;
            EhrAmendments.Update(amd);

            long qualityL = ComputerPrefs.LocalComputer.ScanDocQuality;
            ImageCodecInfo myImageCodecInfo;
            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();
            myImageCodecInfo = null;
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == "image/jpeg")
                {
                    myImageCodecInfo = encoders[j];
                }
            }

            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityL);
            myEncoderParameters.Param[0] = myEncoderParameter;
            try
            {
                SaveAmdAttach(amd, image, myImageCodecInfo, myEncoderParameters, amdFolder);
            }
            catch
            {
                throw;
            }

            return amd;
        }

        public static EhrAmendment ImportAmdAttach(string pathImportFrom, EhrAmendment amd)
        {
            string amdFolder = GetAmdFolder();
            string amdFilename = amd.FileName;


            amd.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + amd.EhrAmendmentNum + Path.GetExtension(pathImportFrom);

            if (Path.GetExtension(pathImportFrom) == "")
            {
                amd.FileName += ".jpg";
            }

            try
            {
                SaveAmdAttach(amd, pathImportFrom, amdFolder);
            }
            catch
            {
                throw;
            }


            amd.DateTAppend = DateTime.Now;
            EhrAmendments.Update(amd);

            CleanAmdAttach(amdFilename);

            return amd;
        }

        /// <summary>
        /// Save a Document to another location on the disk (outside of Open Dental).
        /// </summary>
        public static void Export(string exportFileName, Document document, Patient patient)
        {
            string filePath = Storage.Default.CombinePath(GetPatientFolder(patient), document.FileName);

            Storage.Default.CopyFile(filePath, exportFileName);
        }

        /// <summary>
        /// Save an EOB to another location on the disk.
        /// </summary>
        public static void ExportEobAttach(string saveToPath, EobAttach eob)
        {
            string filePath = Storage.Default.CombinePath(GetEobFolder(), eob.FileName);

            Storage.Default.CopyFile(filePath, saveToPath);
        }

        /// <summary>
        /// Save an EHR amendment to another location on the disk.
        /// </summary>
        public static void ExportAmdAttach(string saveToPath, EhrAmendment amd)
        {
            string filePath = Storage.Default.CombinePath(GetAmdFolder(), amd.FileName);

            Storage.Default.CopyFile(filePath, saveToPath);
        }

        public static void SaveDocument(Document document, Image image, ImageFormat format, string patientPath)
        {
            using (var stream = Storage.Default.OpenWrite(Storage.Default.CombinePath(patientPath, document.FileName)))
            {
                image.Save(stream, format);
            }

            LogDocument("Document Created: ", Permissions.ImageEdit, document, DateTime.MinValue); //a brand new document is always passed-in
        }

        public static void SaveDocument(Document doc, Image image, ImageCodecInfo codec, EncoderParameters encoderParameters, string patFolder)
        {
            using (var stream = Storage.Default.OpenWrite(Storage.Default.CombinePath(patFolder, doc.FileName)))
            {
                image.Save(stream, codec, encoderParameters);
            }

            LogDocument("Document Created: ", Permissions.ImageEdit, doc, DateTime.MinValue); //a brand new document is always passed-in
        }

        public static void SaveDocument(Document doc, string sourceFileName, string patientPath)
        {
            Storage.Default.CopyFile(sourceFileName, Storage.Default.CombinePath(patientPath, doc.FileName));

            LogDocument("Document Created: ", Permissions.ImageEdit, doc, DateTime.MinValue);
        }

        public static void SaveEobAttach(EobAttach eob, Bitmap image, ImageCodecInfo codec, EncoderParameters encoderParameters, string eobFolder)
        {
            using (var stream = Storage.Default.OpenWrite(Storage.Default.CombinePath(eobFolder, eob.FileName)))
            {
                image.Save(stream, codec, encoderParameters);
            }
        }

        public static void SaveAmdAttach(EhrAmendment amd, Bitmap image, ImageCodecInfo codec, EncoderParameters encoderParameters, string amdFolder)
        {
            using (var stream = Storage.Default.OpenWrite(Storage.Default.CombinePath(amdFolder, amd.FileName)))
            {
                image.Save(stream, codec, encoderParameters);
            }
        }

        public static void SaveEobAttach(EobAttach eob, string pathSourceFile, string eobFolder)
        {
            Storage.Default.CopyFile(pathSourceFile, Storage.Default.CombinePath(eobFolder, eob.FileName));
        }

        public static void SaveAmdAttach(EhrAmendment amd, string pathSourceFile, string amdFolder)
        {
            Storage.Default.CopyFile(pathSourceFile, Storage.Default.CombinePath(amdFolder, amd.FileName));
        }

        public static void DeleteDocuments(IList<Document> documents, string patientPath)
        {
            for (int i = 0; i < documents.Count; i++)
            {
                if (documents[i] == null) continue;

                // Check if document is referenced by a sheet.
                List<Sheet> sheetList = Sheets.GetForDocument(documents[i].DocNum);
                if (sheetList.Count != 0)
                {
                    string msgText = "Cannot delete image, it is referenced by sheets with the following dates:";
                    foreach (var sheet in sheetList)
                    {
                        msgText += "\r\n" + sheet.DateTimeSheet.ToShortDateString();
                    }

                    throw new Exception(msgText);
                }

                try
                {
                    string filePath = Storage.Default.CombinePath(patientPath, documents[i].FileName);
                    if (Storage.Default.FileExists(filePath))
                    {
                        Storage.Default.DeleteFile(filePath);

                        LogDocument("Document Deleted: ", Permissions.ImageDelete, documents[i], documents[i].DateTStamp);
                    }
                }
                catch
                {
                    throw new Exception("Could not delete file. It may be in use by another program, flagged as read-only, or you might not have sufficient permissions.");
                }


                Documents.Delete(documents[i]);
            }
        }

        public static void DeleteEobAttach(EobAttach eob)
        {
            string filePath = Storage.Default.CombinePath(GetEobFolder(), eob.FileName);
            if (Storage.Default.FileExists(filePath))
            {
                try
                {
                    Storage.Default.DeleteFile(filePath);
                }
                catch
                {
                }
            }

            EobAttaches.Delete(eob.EobAttachNum);
        }

        public static void DeleteAmdAttach(EhrAmendment amendment)
        {
            string filePath = Storage.Default.CombinePath(GetAmdFolder(), amendment.FileName);
            if (Storage.Default.FileExists(filePath))
            {
                try
                {
                    Storage.Default.DeleteFile(filePath);
                }
                catch
                {
                    MessageBox.Show(
                        "Delete was unsuccessful. The file may be in use.");

                    return;
                }
            }

            amendment.DateTAppend = DateTime.MinValue;
            amendment.FileName = "";
            amendment.RawBase64 = "";

            EhrAmendments.Update(amendment);
        }

        public static void CleanAmdAttach(string fileName)
        {
            var filePath = Storage.Default.CombinePath(GetAmdFolder(), fileName);
            if (Storage.Default.FileExists(filePath))
            {
                try
                {
                    Storage.Default.DeleteFile(filePath);
                }
                catch
                {
                    return;
                }
            }
        }

        public static void DeleteThumbnailImage(Document document, string patientPath)
        {
            var filePath = Storage.Default.CombinePath(patientPath, "Thumbnails", document.FileName);
            if (Storage.Default.FileExists(filePath))
            {
                try
                {
                    Storage.Default.DeleteFile(filePath);
                }
                catch
                {
                }
            }
        }

        public static string GetExtension(Document doc) => Path.GetExtension(doc.FileName).ToLower();

        public static string GetFilePath(Document doc, string patFolder) => Storage.Default.CombinePath(patFolder, doc.FileName);

        public static bool HasImageExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();

            return
                ext == ".jpg" || ext == ".jpeg" || ext == ".tga" || ext == ".bmp" ||
                ext == ".tif" || ext == ".tiff" || ext == ".gif" || ext == ".emf" ||
                ext == ".exif" || ext == ".ico" || ext == ".png" || ext == ".wmf" ||
                ext == ".tig";
        }

        /// <summary>
        /// Makes log entry for documents. Supply beginning text, permission, document, and the DateTStamp that the document was previously last edited.
        /// </summary>
        public static void LogDocument(string logMsgStart, string perm, Document doc, DateTime secDatePrevious)
        {
            string logMsg = logMsgStart + doc.FileName;

            if (doc.Description != "")
            {
                string descriptDoc = doc.Description;
                if (descriptDoc.Length > 50)
                {
                    descriptDoc = descriptDoc.Substring(0, 50);
                }
                logMsg += " " + "with description " + descriptDoc;
            }

            Definition docCat = Defs.GetDef(DefinitionCategory.ImageCats, doc.DocCategory);
            logMsg += " with category " + docCat.Description;

            SecurityLogs.MakeLogEntry(perm, doc.PatNum, logMsg, doc.DocNum, secDatePrevious);
        }
    }
}