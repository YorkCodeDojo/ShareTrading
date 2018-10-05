﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ShareTradingAPI.ViewModels
{
    public class SellRequest
    {
        public Guid AccountNumber { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int MinCost { get; set; }
    }
}