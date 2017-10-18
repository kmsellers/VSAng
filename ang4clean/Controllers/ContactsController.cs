using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAng.Data.Repository;
using VSAng.Data.Repository.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VSAng.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private IRepository<Contact> _repository;

        public ContactsController(IRepository<Contact> repository)
        {
            _repository = repository;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
        {
              return await _repository.GetDocuments(x => true);
        }

        // GET: api/Contacts/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Contact> Get(Guid id)
        {
            return await _repository.GetDocument(id);
        }

        // POST: api/Contacts
        [HttpPost("[action]")]
        public async Task<Contact> PostContact()
        {

            return await _repository.AddDocument(new Contact { first_name = "n" + DateTime.Now.Ticks.ToString() });
        }
        // POST: api/Contacts
        [HttpPost] 
        public async Task<Contact> Post([FromBody]Contact value)
        {
            return await _repository.AddDocument(value);
        }
        
        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]Contact value)
        {
            await _repository.UpdateDocument(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async void Delete(Guid id)
        {
            await _repository.RemoveDocument(id);
        }
    }
}
