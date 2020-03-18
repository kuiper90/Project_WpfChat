using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace WpfApp_Client.Utility
{
    public class RecordFile
    {
        private FileInfo currentFile;

        private string currentPath;

        private string fileName = "ch";

        public RecordFile()
        {
            CreateWrapperForFilePath();
            if (!currentFile.Exists)
                CreateFile();
        }

        private void CreateFile()
        {
            FileStream fileStream = currentFile.Create();
            currentFile.Refresh();
            fileStream.Close();
        }

        private void CreateWrapperForFilePath()
        {
            SetCurrentPath();
            currentFile = new FileInfo(GetCurrentPath + fileName);
        }

        public string GetFileName
            => fileName;

        public FileInfo GetCurrentFile
            => currentFile;

        public string GetCurrentPath
            => currentPath;

        public void SetCurrentPath()
        {
            currentPath = Path.GetFullPath("..\\..\\");
        }

        private bool SafelyCreateDirectory(string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                DirectoryInfo destDirectory = Directory.CreateDirectory(destPath);
                return true;
            }
            return false;
        }

        public void SafelyMoveFiles()
        {
            string destPath = currentPath + "History\\";
            if (currentFile.Exists && this.SafelyCreateDirectory(destPath))
                this.MoveFiles(destPath);
        }

        private void MoveFiles(string destPath)
        {
            string[] getSourceFileNames = Directory
                                        .GetFiles(Path.GetDirectoryName(destPath))
                                        .Where(s => s.Contains(fileName))
                                        .ToArray();
            int lastIndex = 0;
            if (getSourceFileNames.Length > 0)
            {
                int currentIndex = 0;
                foreach (string str in getSourceFileNames)
                {
                    string[] fileNameArray = str.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    var result = string
                        .Concat(fileNameArray[0]
                                .Reverse()
                                .TakeWhile(char.IsNumber)
                                .Reverse());
                    int.TryParse(result, out currentIndex);

                    if (currentIndex > lastIndex)
                        lastIndex = currentIndex;
                }
                lastIndex++;
            }
            currentFile.MoveTo(destPath + fileName + lastIndex + ".bak");
        }

        public void TryWriteToFile(string message)
        {
            try
            {
                currentFile.Refresh();
                if (currentFile.Exists && currentFile.Length < 10)
                    WriteToFile(message);
                else
                {
                    SafelyMoveFiles();
                    CreateWrapperForFilePath();
                    CreateFile();
                    TryWriteToFile(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to file.");
            }
        }

        public string TryReadFromFile()
        {
            if (currentFile.Exists || currentFile.Length != 0)
            {
                SetCurrentPath();
                StreamReader streamReader = currentFile.OpenText();
                try
                {
                    return ReadFromFile(streamReader);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading from file.");
                    return null;
                }
                finally
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                        streamReader.Dispose();
                        streamReader = null;
                    }
                }
            }
            return null;
        }

        private string ReadFromFile(StreamReader streamReader)
        {
            int attempt = 5;
            string fileContent = null;
            while (attempt > 0)
            {
                fileContent = streamReader.ReadToEnd();
                if (fileContent != null)
                {
                    Console.WriteLine(fileContent);
                    //streamReader.Close();
                    return fileContent;
                }
                attempt--;
                Thread.Sleep(1000);
            }
            return null;
        }

        public void WriteToFile(string message)
        {
            //StreamWriter streamWriter = new StreamWriter(currentFile.Name, true, Encoding.Default, 1024);
            //StreamWriter streamWriter = currentFile.AppendText();

            using (StreamWriter streamWriter = currentFile.AppendText())
            {
                streamWriter.WriteLine(message);
            }
        }
    }
}