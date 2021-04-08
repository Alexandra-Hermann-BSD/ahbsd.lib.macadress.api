using System.Collections.Generic;
using System.Net.NetworkInformation;
using ahbsd.lib;
using ahbsd.lib.macadress.api;
using NUnit.Framework;
using RestSharp;

namespace Test
{
    public class Tests
    {
        IList<PhysicalAddress> adresses1;
        IList<string> adresses2;

        [SetUp]
        public void Setup()
        {
            adresses1 = new List<PhysicalAddress>();
            adresses2 = new List<string>();

            adresses1.Add(PhysicalAddress.Parse("98-52-3D-0D-7B-EB"));
            adresses1.Add(PhysicalAddress.Parse("2C-91-AB-8C-52-BB"));
            adresses1.Add(PhysicalAddress.Parse("90-20-C2-3C-E5-E0"));

            adresses2.Add("00:80:77:52:B2:1F");
            adresses2.Add("8C:DC:D4:ED:F2:80");
            adresses2.Add("F0:23:B9:EC:64:9A");

            MacVendor.OnLastResponseChanged += MacVendor_OnLastResponseChanged;
        }

        private void MacVendor_OnLastResponseChanged(object sender, ChangeEventArgs<IRestResponse> e)
        {
            
        }

        [Test]
        public void Test1()
        {
            IList<string> results = new List<string>(6);

            foreach (var item in adresses1)
            {
                results.Add(MacVendor.GetVendor(item));
            }

            foreach (var item in adresses2)
            {
                results.Add(MacVendor.GetVendor(item));
            }

            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            try
            {
                MacVendor.GetVendor("This is definite bullshit!");
            }
            catch (System.Exception ex)
            {
            }

            Assert.Pass();
        }
    }
}