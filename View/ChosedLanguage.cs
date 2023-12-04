using BackupSoftware.ViewModel;

namespace BackupSoftware.View
{
    public class ChosedLanguage
    {
        public LanguageManager chosingLanguage { get; }

        public ChosedLanguage(LanguageManager cl)
        {
            this.chosingLanguage = cl;
        }
    }
}
