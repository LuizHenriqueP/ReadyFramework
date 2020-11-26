using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Whitebox.Controllers
{
    public class ValuesController : ReadyController
    {
        // GET api/values
        [Route("api/values")] 
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("api/values/{id}")] 
        [HttpGet]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [Route("api/values/{id}")] 
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [Route("api/values/{id}")] 
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
