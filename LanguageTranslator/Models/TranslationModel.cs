using System.ComponentModel.DataAnnotations;

namespace YourProjectName.Models
{
    public class TranslationModel
    {
        [Required(ErrorMessage = "Please enter text to translate.")]
        public string InputText { get; set; }

        [Required(ErrorMessage = "Please select the input language.")]
        public string InputLanguage { get; set; }

        [Required(ErrorMessage = "Please select the output language.")]
        public string OutputLanguage { get; set; }

        public string TranslatedText { get; set; }
    }
}
