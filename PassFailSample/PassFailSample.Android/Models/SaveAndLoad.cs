using System;
using System.IO;
using PassFailSample.Models;
using Environment = Android.OS.Environment;
using Xamarin.Forms;
using PassFailSample.Droid.Models;
using System.Threading.Tasks;
using System.Text;
using Android.Content.PM;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace PassFailSample.Droid.Models
{
    public class SaveAndLoad: ISaveAndLoad
    {
        private string _FileName;
        public bool Initialize()
        {
            this._FileName = $"PassFail{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            return true;
        }

        public bool AppendText(string text)
        {
            try
            {
                // TODO: Add google drive api call
                var directory = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments);
                var filePath = Path.Combine(directory.Path, this._FileName);
                if (!directory.Exists())
                {
                    var dirMadeSuccessfully = directory.Mkdir();
                }

                if (File.Exists(filePath))
                {
                    File.AppendAllText(filePath, text);
                }
                else
                {
                    using (var file = File.Create(filePath))
                    {
                        file.Write(Encoding.ASCII.GetBytes(text), 0, text.Length);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string LoadText()
        {
            // TODO: Add google drive api call
            var directory = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments);
            var filePath = Path.Combine(directory.Path, this._FileName);
            return File.ReadAllText(filePath);
        }
    }
}