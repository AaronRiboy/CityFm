    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    namespace CityFm.API.Models
    {
        public class VendorProductDTO
        {
            public string ProductCode { get; set; } = default!;
            public string? Description { get; set; }
            public decimal Price { get; set; }
        }
    }