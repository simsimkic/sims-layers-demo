using System.Collections.Generic;
using SimsLayersDemo.Model;
using SimsLayersDemo.Service;

namespace SimsLayersDemo.Controller
{
    public class LoanController
    {
        private readonly LoanService _service;

        public LoanController(LoanService service)
        {
            _service = service;
        }

        public IEnumerable<Loan> GetAll()
        {
            return _service.GetAll();
        }

        public Loan Create(Loan loan)
        {
            return _service.Create(loan);
        }
    }
}
