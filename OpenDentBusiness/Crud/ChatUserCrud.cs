//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ChatUserCrud {
		///<summary>Gets one ChatUser object from the database using the primary key.  Returns null if not found.</summary>
		public static ChatUser SelectOne(long chatUserNum) {
			string command="SELECT * FROM chatuser "
				+"WHERE ChatUserNum = "+POut.Long(chatUserNum);
			List<ChatUser> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ChatUser object from the database using a query.</summary>
		public static ChatUser SelectOne(string command) {
			
			List<ChatUser> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ChatUser objects from the database using a query.</summary>
		public static List<ChatUser> SelectMany(string command) {
			
			List<ChatUser> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ChatUser> TableToList(DataTable table) {
			List<ChatUser> retVal=new List<ChatUser>();
			ChatUser chatUser;
			foreach(DataRow row in table.Rows) {
				chatUser=new ChatUser();
				chatUser.ChatUserNum    = PIn.Long  (row["ChatUserNum"].ToString());
				chatUser.Extension      = PIn.Int   (row["Extension"].ToString());
				chatUser.CurrentSessions= PIn.Int   (row["CurrentSessions"].ToString());
				chatUser.SessionTime    = PIn.Long  (row["SessionTime"].ToString());
				retVal.Add(chatUser);
			}
			return retVal;
		}

		///<summary>Converts a list of ChatUser into a DataTable.</summary>
		public static DataTable ListToTable(List<ChatUser> listChatUsers,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ChatUser";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ChatUserNum");
			table.Columns.Add("Extension");
			table.Columns.Add("CurrentSessions");
			table.Columns.Add("SessionTime");
			foreach(ChatUser chatUser in listChatUsers) {
				table.Rows.Add(new object[] {
					POut.Long  (chatUser.ChatUserNum),
					POut.Int   (chatUser.Extension),
					POut.Int   (chatUser.CurrentSessions),
					POut.Long  (chatUser.SessionTime),
				});
			}
			return table;
		}

		///<summary>Inserts one ChatUser into the database.  Returns the new priKey.</summary>
		public static long Insert(ChatUser chatUser) {
			return Insert(chatUser,false);
		}

		///<summary>Inserts one ChatUser into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ChatUser chatUser,bool useExistingPK) {
			if(!useExistingPK && Preferences.RandomKeys) {
				chatUser.ChatUserNum=ReplicationServers.GetKey("chatuser","ChatUserNum");
			}
			string command="INSERT INTO chatuser (";
			if(useExistingPK || Preferences.RandomKeys) {
				command+="ChatUserNum,";
			}
			command+="Extension,CurrentSessions,SessionTime) VALUES(";
			if(useExistingPK || Preferences.RandomKeys) {
				command+=POut.Long(chatUser.ChatUserNum)+",";
			}
			command+=
				     POut.Int   (chatUser.Extension)+","
				+    POut.Int   (chatUser.CurrentSessions)+","
				+    POut.Long  (chatUser.SessionTime)+")";
			if(useExistingPK || Preferences.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				chatUser.ChatUserNum=Db.NonQ(command,true,"ChatUserNum","chatUser");
			}
			return chatUser.ChatUserNum;
		}

		///<summary>Inserts one ChatUser into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ChatUser chatUser) {
			return InsertNoCache(chatUser,false);
		}

		///<summary>Inserts one ChatUser into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ChatUser chatUser,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO chatuser (";
			if(!useExistingPK && isRandomKeys) {
				chatUser.ChatUserNum=ReplicationServers.GetKeyNoCache("chatuser","ChatUserNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ChatUserNum,";
			}
			command+="Extension,CurrentSessions,SessionTime) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(chatUser.ChatUserNum)+",";
			}
			command+=
				     POut.Int   (chatUser.Extension)+","
				+    POut.Int   (chatUser.CurrentSessions)+","
				+    POut.Long  (chatUser.SessionTime)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				chatUser.ChatUserNum=Db.NonQ(command,true,"ChatUserNum","chatUser");
			}
			return chatUser.ChatUserNum;
		}

		///<summary>Updates one ChatUser in the database.</summary>
		public static void Update(ChatUser chatUser) {
			string command="UPDATE chatuser SET "
				+"Extension      =  "+POut.Int   (chatUser.Extension)+", "
				+"CurrentSessions=  "+POut.Int   (chatUser.CurrentSessions)+", "
				+"SessionTime    =  "+POut.Long  (chatUser.SessionTime)+" "
				+"WHERE ChatUserNum = "+POut.Long(chatUser.ChatUserNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ChatUser in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ChatUser chatUser,ChatUser oldChatUser) {
			string command="";
			if(chatUser.Extension != oldChatUser.Extension) {
				if(command!="") { command+=",";}
				command+="Extension = "+POut.Int(chatUser.Extension)+"";
			}
			if(chatUser.CurrentSessions != oldChatUser.CurrentSessions) {
				if(command!="") { command+=",";}
				command+="CurrentSessions = "+POut.Int(chatUser.CurrentSessions)+"";
			}
			if(chatUser.SessionTime != oldChatUser.SessionTime) {
				if(command!="") { command+=",";}
				command+="SessionTime = "+POut.Long(chatUser.SessionTime)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE chatuser SET "+command
				+" WHERE ChatUserNum = "+POut.Long(chatUser.ChatUserNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(ChatUser,ChatUser) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ChatUser chatUser,ChatUser oldChatUser) {
			if(chatUser.Extension != oldChatUser.Extension) {
				return true;
			}
			if(chatUser.CurrentSessions != oldChatUser.CurrentSessions) {
				return true;
			}
			if(chatUser.SessionTime != oldChatUser.SessionTime) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ChatUser from the database.</summary>
		public static void Delete(long chatUserNum) {
			string command="DELETE FROM chatuser "
				+"WHERE ChatUserNum = "+POut.Long(chatUserNum);
			Db.NonQ(command);
		}

	}
}