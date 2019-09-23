namespace OpenDentBusiness
{
    /// <summary>
    /// A single autonote template.
    /// </summary>
    public class AutoNote : DataRecord
    {
        public string Name;
        public string MainText;

        /// <summary>
        /// FK to definition.DefNum.
        /// This is the AutoNoteCat definition category (DefCat=41), for categorizing autonotes.
        /// Uncategorized autonotes will be set to null.
        /// </summary>
        public long? CategoryId;

        public AutoNote Copy()
        {
            return (AutoNote)this.MemberwiseClone();
        }

        #region Cache Pattern

        private class AutoNoteCache : CacheListAbs<AutoNote>
        {
            protected override List<AutoNote> GetCacheFromDb()
            {
                string command = "SELECT * FROM autonote ORDER BY AutoNoteName";
                return Crud.AutoNoteCrud.SelectMany(command);
            }
            protected override List<AutoNote> TableToList(DataTable table)
            {
                return Crud.AutoNoteCrud.TableToList(table);
            }
            protected override AutoNote Copy(AutoNote autoNote)
            {
                return autoNote.Copy();
            }
            protected override DataTable ListToTable(List<AutoNote> listAutoNotes)
            {
                return Crud.AutoNoteCrud.ListToTable(listAutoNotes, "AutoNote");
            }
            protected override void FillCacheIfNeeded()
            {
                AutoNotes.GetTableFromCache(false);
            }
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static AutoNoteCache _autoNoteCache = new AutoNoteCache();

        public static List<AutoNote> GetDeepCopy(bool isShort = false)
        {
            return _autoNoteCache.GetDeepCopy(isShort);
        }

        public static List<AutoNote> GetWhere(Predicate<AutoNote> match, bool isShort = false)
        {
            return _autoNoteCache.GetWhere(match, isShort);
        }

        public static bool GetExists(Predicate<AutoNote> match, bool isShort = false)
        {
            return _autoNoteCache.GetExists(match, isShort);
        }

        private static AutoNote GetFirstOrDefault(Func<AutoNote, bool> match, bool isShort = false)
        {
            return _autoNoteCache.GetFirstOrDefault(match, isShort);
        }

        ///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
        public static DataTable RefreshCache()
        {
            return GetTableFromCache(true);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            _autoNoteCache.FillCacheFromTable(table);
        }

        ///<summary>Always refreshes the ClientWeb's cache.</summary>
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _autoNoteCache.GetTableFromCache(doRefreshCache);
        }

        #endregion Cache Pattern

        ///<summary></summary>
        public static long Insert(AutoNote autonote)
        {
            return Crud.AutoNoteCrud.Insert(autonote);
        }

        ///<summary></summary>
        public static void Update(AutoNote autonote)
        {
            Crud.AutoNoteCrud.Update(autonote);
        }

        ///<summary></summary>
        public static void Delete(long autoNoteNum)
        {
            string command = "DELETE FROM autonote "
                + "WHERE AutoNoteNum = " + POut.Long(autoNoteNum);
            Db.NonQ(command);
        }

        public static string GetByTitle(string autoNoteTitle)
        {
            //No need to check RemotingRole; no call to db.
            AutoNote autoNote = GetFirstOrDefault(x => x.Name == autoNoteTitle);
            return (autoNote == null ? "" : autoNote.MainText);
        }

        ///<summary>Returns true if there is a valid AutoNote for the passed in AutoNoteName.</summary>
        public static bool IsValidAutoNote(string autoNoteTitle)
        {
            //No need to check RemotingRole; no call to db.
            AutoNote autoNote = GetFirstOrDefault(x => x.Name == autoNoteTitle);
            return autoNote != null;
        }

        ///<summary>Sets the autonote.Category=0 for the autonote category DefNum provided.  Returns the number of rows updated.</summary>
        public static long RemoveFromCategory(long autoNoteCatDefNum)
        {
            string command = "UPDATE autonote SET Category=0 WHERE Category=" + POut.Long(autoNoteCatDefNum);
            return Db.NonQ(command);
        }
    }
}
