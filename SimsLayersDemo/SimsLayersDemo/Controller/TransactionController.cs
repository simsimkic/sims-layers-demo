using System.Collections.Generic;
using SimsLayersDemo.Model;
using SimsLayersDemo.Service;

namespace SimsLayersDemo.Controller
{
    public class TransactionController
    {
        private readonly TransactionService _service;

        public TransactionController(TransactionService service)
        {
            _service = service;
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _service.GetAll();
        }

        public Transaction Create(Transaction loan)
        {
            return _service.Create(loan);
        }
    }
}
