using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// When some objects are deleted, we sometimes need a way to track them for synching purposes. 
    /// Other objects already have fields for IsHidden or PatStatus which track deletions just fine.  
    /// Those types of objects will not use this table.
    /// </summary>
    public class DeletedObject
    {
        // TODO: What's the point? Drop this?


        ///<summary>Primary key.</summary>
        public long DeletedObjectNum;

        ///<summary>Foreign key to a number of different tables, depending on which type it is.</summary>
        public long ObjectNum;

        ///<summary>Enum:DeletedObjectType </summary>
        public DeletedObjectType ObjectType;

        ///<summary>Updated any time the row is altered in any way.</summary>
        public DateTime DateTStamp;
    }
}