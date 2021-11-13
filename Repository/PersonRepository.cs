using dbChange.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbChange.Repository
{
    public class PersonRepository: IPersonRepository
    {
        private readonly IHubContext<SignalServer> _context;
        string connectionString = "";
        public PersonRepository(IConfiguration configuration,
                                    IHubContext<SignalServer> context)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }
        public List<Person> GetAllPersons()
        {
            var persons = new List<Person>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDependency.Start(connectionString);

                string commandText = "select ID, FirstName, LastName, Gender, Salary from dbo.Persons";

                SqlCommand cmd = new SqlCommand(commandText, conn);

                SqlDependency dependency = new SqlDependency(cmd);

                dependency.OnChange += new OnChangeEventHandler(dbChangeNotification);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var person = new Person
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Salary = Convert.ToInt32(reader["Salary"])
                    };

                    persons.Add(person);
                }
            }

            return persons;
        }

        private void dbChangeNotification(object sender, SqlNotificationEventArgs e)
        {
            _context.Clients.All.SendAsync("refreshPersons");
        }
    }
}
