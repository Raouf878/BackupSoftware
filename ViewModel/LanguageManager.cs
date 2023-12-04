using BackupSoftware.Model;
using System.Globalization;

namespace BackupSoftware.ViewModel
{
    public class LanguageManager
    {
        private Language languagecode;
        public LanguageManager(Language lc)
        {
            languagecode = lc;
        }

        public void translate()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languagecode.BaseCode);

        }
    }
}
