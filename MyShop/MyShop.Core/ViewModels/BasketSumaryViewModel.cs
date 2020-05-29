using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class BasketSumaryViewModel
    {
        public int BasketCount { get; set; }
        public decimal BasketTotal { get; set; }

        public BasketSumaryViewModel()
        {

        }
        public BasketSumaryViewModel(int basketCount,decimal basketTotal)
        {
            this.BasketCount = basketCount;
            this.BasketTotal = basketTotal;
        }
    }
}
