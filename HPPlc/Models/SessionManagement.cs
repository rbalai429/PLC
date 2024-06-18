using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public static class SessionManagement
	{
		#region Global Objects

		static string vSessionVariable = "HP PLC";
		private static SessionType SessionVariableName;
		#endregion

		/// <summary>
		/// This method stores value in the session using Hash table.
		/// </summary>
		/// <param name="vKey">It stores Session Key name.</param>
		/// <param name="vSessionValue">It stores Session value.</param>
		/// <returns>It returns hash table.</returns>
		public static Hashtable StoreInSession(SessionType vKey, object vSessionValue)
		{
			Hashtable vInfinixSession = null;
			SessionVariableName = vKey;
			try
			{
				vInfinixSession = new Hashtable();

				vInfinixSession = (Hashtable)HttpContext.Current.Session[vSessionVariable];

				if (vInfinixSession != null)
				{
					if (!vInfinixSession.ContainsKey((object)vKey))
					{
						vInfinixSession.Add(vKey, vSessionValue);
					}
					else
					{
						vInfinixSession.Remove((object)vKey);
						vInfinixSession.Add(vKey, vSessionValue);
					}
					HttpContext.Current.Session[vSessionVariable] = vInfinixSession;
				}
				else
				{
					vInfinixSession = new Hashtable();
					vInfinixSession.Add(vKey, vSessionValue);
					HttpContext.Current.Session[vSessionVariable] = vInfinixSession;
				}
				return vInfinixSession;
			}
			catch
			{
				throw;

			}
		}

		/// <summary>
		/// This method will delete a session value from hash table with the help of session key.
		/// </summary>
		/// <param name="vKey">It holds the session key name.</param>
		public static void DeleteFromSession(object vKey)
		{
			Hashtable vInfinixSession = null;

			try
			{
				vInfinixSession = new Hashtable();

				vInfinixSession = (Hashtable)HttpContext.Current.Session[vSessionVariable];

				if (vInfinixSession != null)
				{
					if (vInfinixSession.ContainsKey((object)vKey))
					{
						vInfinixSession.Remove(vKey);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// This method will return the hash table with the current session.
		/// </summary>
		/// <returns>It returns the hash table.</returns>
		public static Hashtable GetSession()
		{
			Hashtable vInfinixSession = null;
			try
			{
				vInfinixSession = new Hashtable();

				vInfinixSession = (Hashtable)HttpContext.Current.Session[vSessionVariable];

				return vInfinixSession;
			}
			catch
			{
				throw;
			}
		}

		public static T GetCurrentSession<T>(SessionType variable)
		{
			Hashtable vInfinixSession = null;
			SessionVariableName = variable;
			try
			{
				vInfinixSession = new Hashtable();

				vInfinixSession = (Hashtable)HttpContext.Current.Session[vSessionVariable];

				if (vInfinixSession != null)
				{
					if (vInfinixSession.ContainsKey(SessionVariableName))
					{
						return (T)vInfinixSession[SessionVariableName];
					}
				}
				return default(T);
			}
			catch
			{
				throw;
			}
		}
	}
}