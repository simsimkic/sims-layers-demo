using System.Collections.Generic;
using SimsLayersDemo.Model;
using SimsLayersDemo.Service;

namespace SimsLayersDemo.Controller
{
    public class ClientController
    {
        private readonly ClientService _service;

        public ClientController(ClientService service)
        {
            _service = service;
        }

        public IEnumerable<Client> GetAll()
        {
            return _service.GetAll();
        }
    }
}
