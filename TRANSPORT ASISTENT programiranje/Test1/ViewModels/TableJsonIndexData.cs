﻿using System.Collections.Generic;
using Bex.Models;
using System;

namespace DDtrafic.ViewModels
{
   public class TableJsonIndexData<T> where T : class
    {


        public int total;

        public IEnumerable<T> rows { get; set; }
       
    }
}