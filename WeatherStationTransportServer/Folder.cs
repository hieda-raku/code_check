using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace WeatherStationTransportServer 
{
    public class Folder
    {
        string folderPath = "";

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        public void createFolder(string path)
        {
            Directory.CreateDirectory(path);
            addpathPower(path, "lucifer-PC", "FullControl");
        }

        public void checkFolder(string path)
        {
            if (Directory.Exists(path))
            {
                addpathPower(path, "lucifer-PC", "FullControl");
                return;
            }
            else
            {
                createFolder(path);
            }
        }

        public void addpathPower(string pathname, string username, string power)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(pathname);
            if ((dirinfo.Attributes & FileAttributes.ReadOnly) != 0)
            { dirinfo.Attributes = FileAttributes.Normal; }
            //C#创建文件夹取得访问控制列表    
            //DirectorySecurity dirsecurity = dirinfo.GetAccessControl();
            //switch (power)
            //{
            //    case "FullControl": dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            //        break;
            //    case "ReadOnly": dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Read, AccessControlType.Allow));
            //        break;
            //    case "Write": dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Write, AccessControlType.Allow));
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
