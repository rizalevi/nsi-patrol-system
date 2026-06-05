using System.Security.AccessControl;

namespace NSI.Infra.Helpers
{
	public static class DirectoryHelper
	{
		public static void CreateIfNotExistDirectory(string path)
		{

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		/*	method ini hanya support di host yg pakai OS Windows. usage:
			DirectoryHelper.SetPermission(path,
				new List<FileSystemAccessRule>()
				{
					new FileSystemAccessRule
					{
						"Everyone",
						FileSystemRights.Write | FileSystemRights.Modify,
						AccessControlType.Allow
					},
					new FileSystemAccessRule
					{
						"BUILTIN\\Administrators",
						FileSystemRights.FullControl,
						AccessControlType.Allow
					},
				}
			)
		*/
		public static void SetPermissionToExistingDirectory(string path, FileSystemAccessRule[] listAccessRule)
		{
			if (Directory.Exists(path))
			{
				// Get the directory's current security settings
				DirectoryInfo dirInfo = new DirectoryInfo(path);
				DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

				if (listAccessRule != null)
				{
					foreach (var accessRule in listAccessRule)
					{
						dirSecurity.AddAccessRule(accessRule);
					}
				}

				// Apply the updated security settings to the directory
				dirInfo.SetAccessControl(dirSecurity);
			}
		}

		public static void CreateDirectoryWithPermissionIfNotExists(string path, FileSystemAccessRule[] listAccessRule)
		{
			CreateIfNotExistDirectory(path);
			SetPermissionToExistingDirectory(path, listAccessRule);
		}

		//eof
	}
}
