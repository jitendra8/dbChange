using dbChange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbChange.Repository
{
    public interface IPersonRepository
    {
        List<Person> GetAllPersons();
    }
}
