using Shared.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ViewModels
{
    public class DeliveryMethodViewModel
    {
        public List<DeliveryMethodToReturnDto> DeliveryMethods { get; set; }
        public int? SelectedDeliveryMethodId { get; set; }
    }
}
