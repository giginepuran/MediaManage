using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManage.classes
{
    using System.IO;
    public struct Location
    {
        
        public string Address { get; }
        public string AddressType { get; }

        public Location(string address, string addressType="default")
        {
            this.Address = address;

            if (addressType != "default")
                AddressType = addressType;
            else if (address.Length == 11)
                this.AddressType = "YT";
            else if (File.Exists(address))
                this.AddressType = "Local";
            else
                this.AddressType = "Unknown";
        }
    }
}
