using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BexMVC.ViewModels
{
    public class ZaposleniDanIndexData
    {
        public string Id { get; set; }
        public DateTime? Datum { get; set; }
        public int? StatusId { get; set; }
        public TimeSpan? VremeOd { get; set; }
        public TimeSpan? VremeDo { get; set; }
    }
}