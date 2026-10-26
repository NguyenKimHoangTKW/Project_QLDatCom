using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Dat_Com.Models.DTOs
{
    public class ViewGroupMenuAdmin
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string? SearchTerm { get; set; }
    }
}