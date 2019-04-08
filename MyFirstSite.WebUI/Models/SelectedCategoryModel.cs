using System.Collections.Generic;

namespace MyFirstSite.WebUI.Models{
    public class SelectedCategoryModel{
        public string selectedCategory { get; set; } //выбранная категория, которую мы подсветим
        public IEnumerable<string> categories { get; set; }
    }
}