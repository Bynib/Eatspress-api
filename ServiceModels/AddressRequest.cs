using System;
using System.ComponentModel.DataAnnotations;

namespace Eatspress.ServiceModels
{
    public class AddressRequest
    {
        public string Unit_No { get; set; }
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public int Zip_Code { get; set; }
    }
}
