using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Mukari2Patcher
{
    internal class Patch
    {
        public static void Start()
        {
            if (Net.IsServerOnline())
            {
                try
                {

                    string[] patchList = Net.DownloadPatchList(Configs.Settings.ServerListUrl);

                    List<Configs.File> files = Net.ProcessPatchList(patchList);

                    var invalidFiles = from x in files
                        where x.IsUpToDate == false
                        select x;

                    IEnumerable<Configs.File> enumerable = invalidFiles as Configs.File[] ?? invalidFiles.ToArray();
                    UI.TotalSize = (enumerable.Sum(x => x.Size));
                    Net.InvalidFiles = enumerable.ToList();

                    if (Net.InvalidFiles.Count > 0)
                    {
                        Net.DownloadFile(0);
                    }
                    else
                    {
                        UI.Window.ProgressBarTotal.Value = 100;
                        UI.Window.ProgressBarCurrent.Value = 100;
                        UI.SetStartStatus(true);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(ex.Message);
                    Application.Current.Shutdown();
                }
            }
            else
            {
                MessageBox.Show(Texts.GetText("NONETWORK"));
                Application.Current.Shutdown();
            }

        }

        public class UI
        {

            public static MainWindow Window = null;
            public static long TotalSize = 0;
            public static long LastBytes = 0;

            public static void AddToFullProgress(long size)
            {
                if (size != LastBytes)
                {
                    Window.ProgressBarTotal.Value += (100f * (size - LastBytes) / TotalSize);

                    LastBytes = size;
                }
            }

            public static void UpdateSubProgress(double percent) => Window.ProgressBarCurrent.Value = percent;

            public static void SetStartStatus(bool status) => Window.StartButton.IsEnabled = status;
        }

        private class Net
        {
            private static int _fileIndex = 0;
            public static List<Configs.File> InvalidFiles = new List<Configs.File>();

            public static bool IsServerOnline()
            {
                if (!Configs.Settings.ServerFolderUrl.EndsWith("/")) Configs.Settings.ServerFolderUrl += "/";

                try
                {
                    HttpWebRequest request = WebRequest.Create(Configs.Settings.ServerFolderUrl) as HttpWebRequest;
                    request.Method = "GET";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    response.Close();

                    if (response.StatusCode != HttpStatusCode.NotFound)
                        return true;
                }
                catch (WebException wex)
                {
                    if (wex.Response == null)
                        return false;

                    if (((HttpWebResponse) wex.Response).StatusCode == HttpStatusCode.NotFound)
                        return false;

                    return true;
                }
                return false;
            }

            public static string[] DownloadPatchList(string url)
            {
                try
                {
                    WebClient client = new WebClient();

                    client.Proxy = null;
                    string @htmlCode = "";


                    htmlCode = client.DownloadString(url);

                    htmlCode = Regex.Replace(htmlCode, @"^\s*$\n|\r", "", RegexOptions.Multiline).TrimEnd();

                    return @htmlCode.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                }
                catch (UriFormatException uex)
                {
                    throw new UriFormatException(Texts.GetText("PATCHLISTDOWNLOADURIFORMATERROR", uex.Message));
                }
                catch (WebException wex)
                {
                    throw new WebException(Texts.GetText("PATCHLISTDOWNLOADCONERROR", wex.Message));
                }
                catch (Exception exception)
                {
                    throw new Exception(Texts.GetText("UNKNOWNERROR", exception.ToString()));
                }

            }

            public static List<Configs.File> ProcessPatchList(string[] list)
            {
                List<Configs.File> files = new List<Configs.File>();

                try
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        string line = list[i];
                        string[] data = line.Split(' ');
                        FileInfo localFile = new FileInfo(data[0]);
                        string fileName = data[0];
                        long fileSize = Convert.ToInt64(data[2]);
                        string fileHash = data[1];


                        Configs.File nf = new Configs.File();
                        nf.Name = fileName;
                        nf.Size = fileSize;
                        nf.Hash = fileHash;
                        nf.IsUpToDate = File.Exists(fileName) && localFile.Length == fileSize && (!Configs.Settings.CheckFilesByHash || GetMd5HashFromFile(fileName) == fileHash);
                        files.Add(nf);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(Texts.GetText("PATCHLISTPROCESSORERROR") + "\n" + ex.Message);
                }
                return files;
            }

            public static void DownloadFile(int fi)
            {
                try
                {
                    Configs.File file = InvalidFiles[fi];

                    _fileIndex = fi;

                    if (file.Name.Contains(@"\"))
                        Directory.CreateDirectory(Path.GetDirectoryName(file.Name));

                    //MessageBox.Show(Path.GetDirectoryName(file.name) + "\n" + file.name);

                    WebClient webClient = new WebClient();
                    webClient.Proxy = null;
                    webClient.DownloadProgressChanged += downloadFile_DownloadProgressChanged;
                    webClient.DownloadFileCompleted += downloadFile_DownloadFileCompleted;

                    StopWatch.Start();

                    webClient.DownloadFileAsync(new Uri(Configs.Settings.ServerFolderUrl + file.Name), file.Name);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    throw;
                }
            }

            private static readonly Stopwatch StopWatch = new Stopwatch();

            private static void downloadFile_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                UI.UpdateSubProgress(e.ProgressPercentage);
                UI.AddToFullProgress(e.BytesReceived);
            }

            private static void downloadFile_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                StopWatch.Reset();
                if (_fileIndex == InvalidFiles.Count - 1)
                    UI.SetStartStatus(true);
                else
                {
                    UI.LastBytes = 0;
                    DownloadFile(_fileIndex + 1);
                }
            }

            static string GetMd5HashFromFile(string filename)
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    var buffer = md5.ComputeHash(File.ReadAllBytes(filename));
                    var sb = new StringBuilder();
                    foreach (var t in buffer) sb.Append(t.ToString("x2"));
                    return sb.ToString();
                }
            }
        }
    }
}