using System;
using System.Collections.Generic;
using System.Text;
using DataConnectionBase;

namespace OpenDentBusiness
{
    /// <summary>
    /// Please ignore this class. It's used only for testing.
    /// </summary>
    public class SchemaCrudTest
    {
        public static void AddTableTempcore()
        {
            Db.NonQ("DROP TABLE IF EXISTS tempcore");
            Db.NonQ(@"CREATE TABLE tempcore (
                TempCoreNum bigint NOT NULL auto_increment PRIMARY KEY,
                TimeOfDayTest time NOT NULL DEFAULT '00:00:00',
                TimeStampTest timestamp,
                DateTest date NOT NULL DEFAULT '0001-01-01',
                DateTimeTest datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
                TimeSpanTest time NOT NULL DEFAULT '00:00:00',
                CurrencyTest double NOT NULL,
                BoolTest tinyint NOT NULL,
                TextSmallTest varchar(255) NOT NULL,
                TextMediumTest text NOT NULL,
                TextLargeTest text NOT NULL,
                VarCharTest varchar(255) NOT NULL
                ) DEFAULT CHARSET=utf8");
        }

        public static void AddColumnEndClob()
        {
            Db.NonQ("ALTER TABLE tempcore ADD ColEndClob text NOT NULL");
        }

        public static void AddColumnEndInt()
        {
            Db.NonQ("ALTER TABLE tempcore ADD ColEndInt int NOT NULL");
        }

        public static void AddColumnEndTimeStamp()
        {
            Db.NonQ("ALTER TABLE tempcore ADD ColEndTimeStamp timestamp");
            Db.NonQ("UPDATE tempcore SET ColEndTimeStamp = NOW()");
        }

        public static void AddIndex()
        {
            Db.NonQ("ALTER TABLE tempcore ADD INDEX(ColEndInt)");
        }

        public static void DropColumn()
        {
            Db.NonQ("ALTER TABLE tempcore DROP COLUMN TextLargeTest");
        }

        //AddColumnAfter
        //DropColumnTimeStamp
        //DropIndex
        //etc.
    }
}