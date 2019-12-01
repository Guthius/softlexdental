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
using CodeBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    /// <summary>
    /// Bridge to Apteryx's XVWeb
    /// </summary>
    public class XVWebBridge : Bridge
    {
        //
        // TODO: The core application needs to have a better way to intergrate imaging bridges
        //       like this one. At the moment there are a lot of direct calls from core modules to
        //       this class that make it impossible to decouple this class from the main program at
        //       this moment.
        //       
        //       A API should be build to allow plugins to integrate with the Chart and Image 
        //       modules.
        //

        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("url", "XVWeb URL", BridgePreferenceType.String),
            BridgePreference.Define("username", "Username", BridgePreferenceType.String),
            BridgePreference.Define("password", "Password", BridgePreferenceType.String),
            BridgePreference.Define("date_format", "Date Format (default MM/dd/yyyy)", BridgePreferenceType.String),
            BridgePreference.Define("image_category", "Image Category", BridgePreferenceType.Definition),
            BridgePreference.Define("save_images", "Save Images", BridgePreferenceType.Boolean)
        };

        private static readonly Dictionary<long, Token> xvwebTokenCache = new Dictionary<long, Token>();
        private static readonly object xvwebTokenLock = new object();

        /// <summary>
        /// Represents a authorization token for the XVWeb service.
        /// </summary>
        private struct Token
        {
            /// <summary>
            /// The actual token.
            /// </summary>
            public string AccessToken;

            /// <summary>
            /// The token type.
            /// </summary>
            public string TokenType;

            /// <summary>
            /// The date and time on which the token was issued.
            /// </summary>
            public DateTime Issued;

            /// <summary>
            /// The date and time on which the token expires.
            /// </summary>
            public DateTime Expires;

            /// <summary>
            /// Gets a value indicating whether the token is expired.
            /// </summary>
            public bool Expired
            {
                get
                {
                    if (Expires <= DateTime.Now.AddSeconds(-10) || AccessToken == null)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XVWebBridge"/> class.
        /// </summary>
        public XVWebBridge() : base(
            "Apteryx XVWeb", 
            "XVWeb® is a fully-functional DICOM server that can accept images from any DICOM-compliant imaging software.", 
            "https://apteryx.com/product/xvweb/", 
            preferences)
        {
        }

        /// <summary>
        /// Gets a string that represents the specified gender.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns>A string that represents the gender.</returns>
        private static string GetGender(PatientGender gender)
        {
            switch (gender)
            {
                case PatientGender.Male:
                    return "M";

                case PatientGender.Female:
                    return "F";
            }
            return "O";
        }

        /// <summary>
        /// Gets the query string for the specified patient.
        /// </summary>
        /// <param name="programId">The program ID.</param>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        private static string GetQueryString(long programId, Patient patient)
        {
            string Tidy(string input) => HttpUtility.UrlEncode(input);
            
            var queryString =
                "?patientid=" + Tidy(GetPatientId(programId, patient)) +
                "&lastname=" + Tidy(patient.LName) +
                "&firstname=" + Tidy(patient.FName) +
                "&gender=" + GetGender(patient.Gender);

            if (patient.Birthdate.Year > 1880)
            {
                var dateFormat = ProgramPreference.GetString(programId, "date_format", "MM/dd/yyyy");

                queryString += "&birthdate=" + patient.Birthdate.ToString(dateFormat);
            }

            return queryString;
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public override void Send(long programId, Patient patient)
        {
            var programUrl = ProgramPreference.GetString(programId, "url", "https://demo2.apteryxweb.com/");

            var queryString = patient != null ? GetQueryString(programId, patient) : "";

            try
            {
                if (!string.IsNullOrWhiteSpace(programUrl))
                {
                    Process.Start(programUrl + queryString);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Name, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     <para>
        ///         Makes a request to the XVWeb service to obtain a authorization token.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="forceRefresh">
        ///     Value indicating whether to force a refresh of the authorization token. When true
        ///     a new authorization token will be retrieved even if the cached token is still 
        ///     valid.
        /// </param>
        /// <returns>The authorization token.</returns>
        private static string GetAuthorizationToken(long programId, bool forceRefresh = false)
        {
            string token = "";

            lock (xvwebTokenLock)
            {
                // If we already have a token and it is still valid we reuse it.
                if (xvwebTokenCache.TryGetValue(programId, out var xvwebToken) && !forceRefresh && !xvwebToken.Expired)
                {
                    return xvwebToken.TokenType + " " + xvwebToken.AccessToken;
                }
            }

            Encryption.TryDecrypt(ProgramPreference.GetString(programId, "password"), out var password);

            var username = ProgramPreference.GetString(programId, "username");

            var request = 
                (HttpWebRequest)WebRequest.Create(
                    GetRequestUri(programId, "api/token"));

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            // Send the request.
            using (var stream = request.GetRequestStream())
            {
                var postData = JsonConvert.SerializeObject(new
                {
                    username,
                    password
                });

                var postDataBytes = Encoding.UTF8.GetBytes(postData);

                stream.Write(postDataBytes, 0, postDataBytes.Length);
                stream.Close();
            }

            // Receive and parse the response.
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    throw new ApplicationException(
                        "Invalid XVWeb credentials. Please check your username and password in the XVWeb bridge setup.");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw
                        new ApplicationException("Unable to connect to XVWeb. Response from XVWeb: " + response.StatusDescription);
                }

                using (var responseStream = response.GetResponseStream())
                using (var responseStreamReader = new StreamReader(responseStream))
                {
                    var responseData = responseStreamReader.ReadToEnd();
                    var responseToken = JToken.Parse(responseData);

                    var authorizationToken = new Token
                    {
                        TokenType = responseToken["token_type"].ToString(),
                        AccessToken = responseToken["access_token"].ToString(),
                        Issued = responseToken["issued"].ToObject<DateTime>(),
                        Expires = responseToken["expires"].ToObject<DateTime>().ToLocalTime()
                    };

                    token = authorizationToken.TokenType + " " + authorizationToken.AccessToken;
                    lock (xvwebTokenLock)
                    {
                        xvwebTokenCache[programId] = authorizationToken;
                    }
                }
            }

            return token;
        }

        /// <summary>
        ///     <para>
        ///         Performs a basic GET request and returns the stream for the response.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestToken">The authorization token to use for the request.</param>
        /// <param name="retryIfUnauthorized">
        ///     Value indicating whether to retry the request if the initial request fails due to 
        ///     being unauthorized. If a request is unauthorized it typically means the specified
        ///     <paramref name="requestToken"/> is no longer valid. Before retrying a new token
        ///     is obtained.
        /// </param>
        /// <param name="contentType">The content type to use for the request.</param>
        /// <returns>The request response stream.</returns>
        private static Stream GetRequestResponseStream(long programId, string requestUri, string requestToken, bool retryIfUnauthorized = true, string contentType = "application/json")
        {
            var request = 
                (HttpWebRequest)WebRequest.Create(requestUri.ToString());

            request.Method = "GET";
            request.ContentType = contentType;
            request.Accept = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, requestToken);

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException exception)
            {
                // In case the token has expired, get a new token and try again.
                if (retryIfUnauthorized && ((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Unauthorized) 
                {
                    return GetRequestResponseStream(programId, requestUri, GetAuthorizationToken(programId, true), false, contentType);
                }

                throw;
            }

            return response.GetResponseStream();
        }

        /// <summary>
        ///     <para>
        ///         Performs a basic GET request and returns the response as a string.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestToken">The authorization token to use for the request.</param>
        /// <param name="retryIfUnauthorized">
        ///     Value indicating whether to retry the request if the initial request fails due to 
        ///     being unauthorized. If a request is unauthorized it typically means the specified
        ///     <paramref name="requestToken"/> is no longer valid. Before retrying a new token
        ///     is obtained.
        /// </param>
        /// <param name="contentType">The content type to use for the request.</param>
        /// <returns></returns>
        private static string GetRequestResponseAsString(long programId, string requestUri, string requestToken, bool retryIfUnauthorized = true, string contentType = "application/json")
        {
            using (var stream = GetRequestResponseStream(programId, requestUri, requestToken, retryIfUnauthorized, contentType))
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        ///     <para>
        ///         Performs a basic GET request and returns the response as a JSON object.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestToken">The authorization token to use for the request.</param>
        /// <param name="retryIfUnauthorized">
        ///     Value indicating whether to retry the request if the initial request fails due to 
        ///     being unauthorized. If a request is unauthorized it typically means the specified
        ///     <paramref name="requestToken"/> is no longer valid. Before retrying a new token
        ///     is obtained.
        /// </param>
        /// <param name="contentType">The content type to use for the request.</param>
        /// <returns></returns>
        private static JToken GetRequestResponseAsJson(long programId, string requestUri, string requestToken, bool retryIfUnauthorized = true) =>
            JObject.Parse(
                GetRequestResponseAsString(programId, requestUri, requestToken, retryIfUnauthorized, "application/json"));

        /// <summary>
        ///     <para>
        ///         A simple get request that saves XVWeb Id's for the object it is getting
        ///     </para>
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestToken"></param>
        /// <returns></returns>
        private static List<long> GetRequestIds(long programId, string requestUri, string requestToken)
        {
            JToken GetRecord(int record)
            {
                var uriBuilder = new UriBuilder(requestUri);
                if (record > 0)
                {
                    uriBuilder.Query += (uriBuilder.Query == "" ? "" : "&") + "NextRecord=" + record;
                }

                return GetRequestResponseAsJson(programId, uriBuilder.ToString(), requestToken);
            }

            var recordIds = new List<long>();

            int nextRecord = 0;
            do
            {
                var response = GetRecord(nextRecord);

                var records = response["Records"];
                if (records != null)
                {
                    recordIds.AddRange(
                        records.Select(
                            record => (long)record["Id"]));
                }

                nextRecord = (int)response["NextRecord"];
                if (nextRecord == 0 || nextRecord > (int)response["TotalRecords"])
                {
                    break;
                }
            }
            while (true);

            return recordIds;
        }

        /// <summary>
        ///     <para>
        ///         Gets all images in the series with the specified ID.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="seriesId">The ID of the image series.</param>
        /// <param name="requestToken"></param>
        /// <returns></returns>
        private static List<ApteryxImage> GetImages(long programId, long seriesId, string requestToken)
        {
            var requestUri = GetRequestUri(programId, "Image", "series=" + seriesId);

            string GetRecord(int record)
            {
                var newRequestUri = requestUri;
                if (record > 0)
                {
                    newRequestUri += "&NextRecord=" + record;
                }

                return GetRequestResponseAsString(programId, newRequestUri, requestToken);
            }
            
            var images = new List<ApteryxImage>();

            int nextRecord = 0;
            do
            {
                var responseData = GetRecord(nextRecord);
                var response = JObject.Parse(responseData);

                var records = response["Records"];
                if (records != null && records.Count() > 0)
                {
                    try
                    {
                        ApteryxImageCollection rootImage = 
                            JsonConvert.DeserializeObject<ApteryxImageCollection>(
                                responseData);

                        images.AddRange(rootImage.Records);
                    }
                    catch
                    {
                    }
                }

                nextRecord = (int)response["NextRecord"];
                if (nextRecord == 0 || nextRecord > (int)response["TotalRecords"])
                {
                    break;
                }
            }
            while (true);

            return images;
        }

        /// <summary>
        ///     <para>
        ///         Downloads the image data for the specified <paramref name="apteryxImage"/>.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="apteryxImage">The image data.</param>
        /// <param name="progressHandler"></param>
        /// <returns>The image.</returns>
        public static Image GetImage(long programId, ApteryxImage apteryxImage, IProgressHandler progressHandler)
        {
            var requestUri = GetRequestUri(programId, "bitmap/" + apteryxImage.Id);
            var requestToken = GetAuthorizationToken(programId);

            int bytesRead;
            long totalBytesRead = 0;
            byte[] buffer = new byte[10240];

            using (var responseStream = GetRequestResponseStream(programId, requestUri, requestToken, true, "image/jpeg"))
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytesRead += bytesRead;
                        if (totalBytesRead != apteryxImage.FileSize)
                        {
                            progressHandler.UpdateBytesRead(totalBytesRead);
                        }

                        memoryStream.Write(buffer, 0, bytesRead);
                    }

                    progressHandler.CloseProgress();
                }
                catch (Exception exception)
                {
                    progressHandler.DisplayError(exception.Message);
                }
                
                return Image.FromStream(memoryStream);
            }
        }

        /// <summary>
        ///     <para>
        ///         Saves the specified image to local storage and creates a document in the 
        ///         database that links the image to specified <paramref name="patient"/>. If 
        ///         saving images is not enabled on the bridge this does nothing.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="apteryxImage"></param>
        /// <param name="image">The image to save.</param>
        /// <param name="patient">The patient.</param>
        /// <returns>
        ///     Null if saving images is not enabled or the given image already exists;
        ///     otherwise the newly created document.
        /// </returns>
        public static Document SaveApteryxImageToDocument(long programId, ApteryxImage apteryxImage, Bitmap image, Patient patient)
        {
            bool saveImages = ProgramPreference.GetBool(programId, "save_images", true);

            // If they want to save and it doesn't already exist in DB.
            if (!saveImages || Documents.DocExternalExists(apteryxImage.Id.ToString(), ExternalSourceType.XVWeb)) 
            {
                return null;
            }

            var imageCategoryId = ProgramPreference.GetLong(programId, "image_category");

            // Store the image in the database.

            var document = 
                ImageStore.Import(
                    image,
                    imageCategoryId,
                    ImageType.Photo, patient);

            document.ToothNumbers = apteryxImage.FormattedTeeth;
            document.DateCreated = apteryxImage.AcquisitionDate;
            document.Description = document.ToothNumbers;
            document.ExternalGUID = apteryxImage.Id.ToString();
            document.ExternalSource = ExternalSourceType.XVWeb;

            Documents.Update(document);

            return document;
        }

        /// <summary>
        ///     <para>
        ///         Gets the thumbnail for the specified <paramref name="apteryxImage"/>.
        ///     </para>
        /// </summary>
        /// <param name="programId">The program ID.</param>
        /// <param name="apteryxImage">The image.</param>
        /// <returns>The thumbnail for the given image.</returns>
        public static Bitmap GetThumbnail(long programId, ApteryxImage apteryxImage)
        {
            var requestToken = GetAuthorizationToken(programId);
            var requestUri = GetRequestUri(programId, "bitmap/thumbnail/" + apteryxImage.Id);

            using (var responseStream = GetRequestResponseStream(programId, requestUri, requestToken, contentType: "image/jpeg"))
            {
                return new Bitmap(responseStream);
            }
        }

        /// <summary>
        ///     <para>
        ///         Gets all thumbnails for the specified <paramref name="patient"/>.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="idsToExclude">
        ///     The ID's if the images whose thumbnai lt exclude from the list.
        /// </param>
        /// <returns>All thumbnuals for the specified patient.</returns>
        public static IEnumerable<ApteryxThumbnail> GetThumbnails(long programId, Patient patient, IEnumerable<string> idsToExclude)
        {
            var apteryxThumbnails = new List<ApteryxThumbnail>();
            var apteryxImages = GetImages(programId, patient);

            apteryxImages.RemoveAll(x => x.Id.ToString().In(idsToExclude));

            foreach (var apteryxImage in apteryxImages)
            {
                var apteryxThumbnail = new ApteryxThumbnail
                {
                    Thumbnail = GetThumbnail(programId, apteryxImage),
                    Image = apteryxImage,
                    PatNum = patient.PatNum
                };

                apteryxThumbnails.Add(apteryxThumbnail);

                yield return apteryxThumbnail;
            }
        }

        /// <summary>
        ///     <para>
        ///         Gets all images for the specified patient.
        ///     </para>
        ///     <para>
        ///         Depending on the number of images in XVWeb this can take a long time.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <returns>All images for the specified patient.</returns>
        public static List<ApteryxImage> GetImages(long programId, Patient patient)
        {
            var requestUri = GetRequestUri(programId, "patient", "PrimaryId=" + patient.PatNum + "&lastname=" + patient.LName + "&firstname=" + patient.FName);
            var requestToken = GetAuthorizationToken(programId);

            var images = new List<ApteryxImage>();
            var patientIds = GetRequestIds(programId, requestUri, requestToken);

            if (patientIds.Count < 1) return images;

            var actions = new List<Action>();

            // Get studies for all patients.
            var studyIds = new List<long>();
            foreach (var patientId in patientIds)
            { 
                actions.Add(new Action(() =>
                {
                    var ids = 
                        GetRequestIds(
                            programId, 
                            GetRequestUri(programId, "study", "patient=" + patientId),
                            requestToken);

                    if (ids.Count > 0)
                    {
                        lock (studyIds)
                        {
                            studyIds.AddRange(ids);
                        }
                    }
                }));
            }

            ODThread.RunParallel(actions, TimeSpan.FromMinutes(1));

            if (studyIds.Count == 0) return images;

            actions.Clear();

            // Get series for all studies for all patients.
            var seriesIds = new List<long>();
            foreach (var studyId in studyIds)
            { 
                actions.Add(new Action(() =>
                {
                    var ids = 
                        GetRequestIds(
                            programId,
                            GetRequestUri(programId, "series", "study=" + studyId),
                            requestToken);

                    lock (seriesIds)
                    {
                        seriesIds.AddRange(ids);
                    }
                }));
            }

            ODThread.RunParallel(actions, TimeSpan.FromMinutes(1));

            if (seriesIds.Count < 1) return images;

            actions.Clear();

            // Get images for all series for all studies for all patients.
            foreach (var seriesId in seriesIds)
            { 
                actions.Add(new Action(() =>
                {
                    var seriesImages = 
                        GetImages(
                            programId, 
                            seriesId,
                            requestToken);

                    lock (images)
                    {
                        images.AddRange(seriesImages);
                    }
                }));
            }

            ODThread.RunParallel(actions, TimeSpan.FromMinutes(1));

            return images;
        }

        /// <summary>
        ///     <para>
        ///         Constructs a URI for a request to the XVWeb API.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private static string GetRequestUri(long programId, string path = "", string query = "")
        {
            var baseUrl = ProgramPreference.GetString(programId, "url");
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            var uriBuilder = new UriBuilder(baseUrl);

            if (!string.IsNullOrEmpty(path))
            {
                uriBuilder.Path += path;
            }

            if (!string.IsNullOrEmpty(query))
            {
                uriBuilder.Query = query;
            }

            return uriBuilder.ToString();
        }

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether the <see cref="XVWebBridge"/> bridge can display 
        ///         images in the program.
        ///     </para>
        /// </summary>
        public static bool IsDisplayingImagesInProgram
        {
            get
            {
                var program = Program.GetByType<XVWebBridge>();
                if (program != null && program.Enabled)
                {
                    if (!string.IsNullOrEmpty(ProgramPreference.GetString(program.Id, "url")) &&
                        !string.IsNullOrEmpty(ProgramPreference.GetString(program.Id, "username")) &&
                        !string.IsNullOrEmpty(ProgramPreference.GetString(program.Id, "password")))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private class ApteryxImageCollection
        {
            public int TotalRecords { get; set; }

            public int NextRecord { get; set; }

            public ApteryxImage[] Records { get; set; }
        }
    }
}
