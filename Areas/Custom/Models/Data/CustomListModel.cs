using System.Collections.Generic;
using DIHA.Models;

namespace DIHA.Areas.Custom.Models.CustomViewModels
{
        public class CustomListModel
        {
            public int totalCustoms { get; set; }
            public int countPages { get; set; }

            public int ITEMS_PER_PAGE { get; set; } = 10;

            public int currentPage { get; set; }

        }

}