namespace Mukari2Patcher
{
    class Configs
    {
        internal class Settings
        {
            public static string ServerFolderUrl = "http://localhost:81/patch/client/";
            public static readonly string ServerListUrl = "http://localhost:81/patch/patchlist.txt";

            public static string BinaryName = "Metin2.exe";
            public static string ServerName = "METIN2";

            public static string ServerUrl = "http://karbust.me/";
            public static string ServerUrlForum = "http://karbust.me/";
            public static string ServerUrlRegister = "http://karbust.me/";
            public static string ServerUrlSupport = "http://karbust.me/";

            public static readonly bool CheckFilesByHash = true;
        }

        internal class File
        {
            public string Name = "";
            public long Size = 0;
            public string Hash = "";
            public bool IsUpToDate = false;
        }
    }
}
