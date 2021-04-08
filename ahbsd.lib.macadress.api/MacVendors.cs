//
//  Copyright 2021  Alexandra Hermann – Beratung, Software, Design
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using RestSharp;

namespace ahbsd.lib.macadress.api
{
    /// <summary>
    /// Simple class to get the vendor of a used MAC-Adress.
    /// </summary>
    /// <remarks>See https://macvendors.com/api </remarks>
    public static class MacVendor
    {
        /// <summary>
        /// The rest client.
        /// </summary>
        private static readonly IRestClient client;

        /// <summary>
        /// The last response.
        /// </summary>
        private static IRestResponse _lastResponse;

        /// <summary>
        /// Occures, when the <see cref="LastResponse"/> has changed.
        /// </summary>
        public static event ChangeEventHandler<IRestResponse> OnLastResponseChanged;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static MacVendor()
        {
            client = new RestClient("https://api.macvendors.com/");
        }

        /// <summary>
        /// Gets the RestClient.
        /// </summary>
        /// <value>The RestClient.</value>
        [ReadOnly(true)]
        public static IRestClient Client => client;

        /// <summary>
        /// Gets the vendor by a MAC adress as string.
        /// </summary>
        /// <param name="macAdress">The MAC adress.</param>
        /// <returns>
        /// The vendor or <c>null</c> if nothing was found.
        /// </returns>
        /// <exception cref="Exception">If the given MAC adress isn't formed correct.</exception>
        public static string GetVendor(string macAdress)
        {
            string result = null;

            IRestRequest request = new RestRequest(macAdress.Trim(), DataFormat.None);
            IRestResponse response = client.Get(request);

            LastResponse = response;

            if (response.IsSuccessful)
            {
                result = response.Content.Trim();
            }

            return result;
        }

        /// <summary>
        /// Gets the vendor by a MAC adress.
        /// </summary>
        /// <param name="macAdress">The MAC adress.</param>
        /// <returns>
        /// The vendor or <c>null</c> if nothing was found.
        /// </returns>
        public static string GetVendor(PhysicalAddress macAdress)
        {
            string result = null;

            IRestRequest request = new RestRequest(macAdress.ToString(), DataFormat.None);
            IRestResponse response = client.Get(request);

            LastResponse = response;

            if (response.IsSuccessful)
            {
                result = response.Content.Trim();
            }

            return result;
        }

        /// <summary>
        /// Gets the last <see cref="IRestResponse"/>.
        /// </summary>
        /// <value>The last response.</value>
        public static IRestResponse LastResponse
        {
            get => _lastResponse;
            private set
            {
                if (value != null && !value.Equals(_lastResponse))
                {
                    ChangeEventArgs<IRestResponse> cea
                        = new ChangeEventArgs<IRestResponse>(_lastResponse, value);
                    _lastResponse = value;
                    OnLastResponseChanged?.Invoke(null, cea);
                }
            }
        }
    }
}
