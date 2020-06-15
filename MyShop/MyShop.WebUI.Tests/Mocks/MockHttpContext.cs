using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext :HttpContextBase
    {
        private MockRequest request;
        private MockResponse reponse;
        private HttpCookieCollection cookies;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.reponse = new MockResponse(cookies);
        }

        public override HttpRequestBase Request { get { return request; } }
        public override HttpResponseBase Response { get { return reponse; } }
    }

    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies;
        public MockResponse(HttpCookieCollection cookieCollection)
        {
            this.cookies = cookieCollection;
        }

        public override HttpCookieCollection Cookies { get { return cookies; } }
    }
    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;
        public MockRequest(HttpCookieCollection cookieCollection)
        {
            this.cookies = cookieCollection;
        }

        public override HttpCookieCollection Cookies { get { return cookies; } }
    }


}
