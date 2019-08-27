using System.Collections.Generic;

namespace Mukari2Patcher
{
    internal class Texts
    {
        private static readonly Dictionary<string, string> Text = new Dictionary<string, string>
        {
            {"UNKNOWNERROR",                    "An unknown error occurred. Contact the administrator.\nError: {0}"},
            {"MISSINGBINARY",                   "O jogo não pode ser iniciado pois o ficheiro {0} está em falta."},
            //{"CANNOTSTART",                     "Não foi possível iniciar o jogo. Execute-me como administrador."},
            {"NONETWORK",                       "Server, or your network connection is offline."},
            //{"CONNECTING",                      "A ligar ao servidor..."},
            //{"LISTDOWNLOAD",                    "A baixar a lista de actualizações..."},
            //{"CHECKCOMPLETE",                   "Todos os ficheiros verificados com sucesso."},
            //{"DOWNLOADCOMPLETE",                "Todos os ficheiros baixados com sucesso."},
            {"PATCHLISTDOWNLOADURIFORMATERROR", "Ocorreu um erro no formato do URL.\nError: {0}" },
            {"PATCHLISTDOWNLOADCONERROR",       "There was an error connecting to the url.\nError: {0}"},
            //{"PATCHLISTDOWNLOADERROR",          "Ocorreu um erro desconhecido ao efetuar o download da lista de atualizações.\nError: {0}"},
            {"PATCHLISTPROCESSORERROR",         "Something went wrong with the patchlist.\nError: {0}"},
        };

        public static string GetText(string key, params object[] arguments)
        {
            foreach (var currentItem in Text)
            {
                if (currentItem.Key == key)
                {
                    return string.Format(currentItem.Value, arguments);
                }
            }

            return null;
        }
    }
}