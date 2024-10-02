using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration03.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    // Define the standard CRUD operations
    T GetById(int id);
    IEnumerable<T> GetAll();
    int Add(T entity);
    void Update(T entity);
    void Delete(int id);
}
