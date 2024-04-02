using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Shared
{
    public static class Defults
    { 
        public static string defultUpdateNumber = "16";
        private static int GetUpdateNumber(bool isPrint = false)
        {
            //check if File Exist
            
            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot",isPrint? "UpdateFilesNumber.txt" : "updateNumber.txt");
            var isFileExist = File.Exists(path);
            if (!isFileExist)
            {
                File.Create(path).Close();
                File.WriteAllText(path, defultUpdateNumber);
            }
            var fileValue = File.ReadAllText(path);
            var tryParse = int.TryParse(fileValue, out var value);
            if (!tryParse)
            {
                File.WriteAllText(path, defultUpdateNumber);
                return int.Parse(defultUpdateNumber);
            }
            return int.Parse(fileValue);

        }
        public static string GetOfflineVersion()
        {
            string versionNumber = "";
            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "offlineVersion.txt");
            if(File.Exists(path))
            {
                versionNumber = File.ReadAllText(path);
            }
            else { 
                File.Create(path).Close();
                File.WriteAllText(path, "1.0.0");
                versionNumber = File.ReadAllText(path);
            }
            return versionNumber;
        }
        public static int updateNumber
        {
            get { return GetUpdateNumber(); }
        }
        public static int UpdateFilesNumber
        {
            get { return GetUpdateNumber(true); }
        }
        public static string ChangeUpdateNumberValuePassword = "(*^&*^*^HGJBBjhvdashdvashRTYRt1r231tr21KVJBVBJ";
    }
}
