using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuTranslation.Models
{
    public class TranslationRequestDTO
    {
        public int ContentId { get; set; }
        public string SourceLang { get; set; }
        public IEnumerable<string> Languages { get; set; }
    }
}
