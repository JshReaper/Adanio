using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoginServer.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        //[Authorize]
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            string value = request.Content.ReadAsStringAsync().Result;
            int separator = value.IndexOf(':');
            string name = value.Substring(0, separator);
            string password = value.Substring(separator + 1);
            if(SharpCode.DatabaseHandler.Instance.SignUp(name, password))
            {
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }
        }

        // PUT api/values/5
        [Authorize]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [Authorize]
        public void Delete(int id)
        {
        }
    }

}
