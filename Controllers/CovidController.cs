using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace covid19_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidController : Controller
    {
        [HttpGet]
        public ActionResult<List<CovidData>> GetData()
        {
            List<CovidData> Users = new List<CovidData>();
            bool tableExists = false;

            using (var connection = new SqliteConnection("Data Source = myDB.db"))
            {

                connection.Open();
                string query = "SELECT name FROM sqlite_sequence WHERE name='CovidData'";

                using (SqliteCommand cmd = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                            tableExists = true;
                    }

                }

                if (tableExists == false)
                {
                    var createTable = connection.CreateCommand();
                    createTable.CommandText = @"CREATE TABLE CovidData (
                'ID' INTEGER NOT NULL UNIQUE, 'FullName' TEXT NOT NULL , 'Age' integer not null,'City' text not null,'State'text not null,
                   'Country' text not null,'Diagnosis' text not null default '', primary key('ID' autoincrement))";

                    createTable.ExecuteNonQuery();
                }



                var getAll = connection.CreateCommand();
                getAll.CommandText = @"select * from CovidData";
                getAll.ExecuteNonQuery();

                SqliteDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    CovidData u = new CovidData();
                    u.Id = reader.GetInt32(0);
                    u.FullName = reader.GetString(1);
                    u.Age = reader.GetInt32(2);
                    u.City = reader.GetString(3);
                    u.State = reader.GetString(4);
                    u.Country = reader.GetString(5);
                    u.Diagnosis = reader.GetString(6);

                    Users.Add(u);

                }
                reader.Close();
                connection.Close();

            }
            return Users;
        }


        [HttpPost]

        public ActionResult<List<CovidData>> CreateData(CovidData user)
        {
            List<CovidData> users = new List<CovidData>();

            using (var connection = new SqliteConnection("Data Source= myDB.db"))
            {
                connection.Open();
                var insert = connection.CreateCommand();
                insert.CommandText = @"insert into CovidData(FullName,Age,City,State,Country,Diagnosis) 
                values('" + user.FullName + "', " + user.Age + ", '" + user.City + "', '" + user.State + "','" + user.Country + "','" + user.Diagnosis + "')";
                insert.ExecuteNonQuery();
                connection.Close();

            }
            return Ok(users);
        }
        [HttpPut("{id}")]

        public ActionResult<List<CovidData>> UpdateData(CovidData request)
        {
            List<CovidData> users = new List<CovidData>();

            using (var connection = new SqliteConnection("Data Source= myDB.db"))
            {
                connection.Open();
                var update = connection.CreateCommand();
                update.CommandText = @"update CovidData set FullName = '" + request.FullName + "', Age = " + request.Age + ",  City='" + request.City + "', State='" + request.State + "', Country='" + request.Country + "' , Diagnosis='" + request.Diagnosis
                    + "' WHERE Id= " + request.Id + " ";
                update.ExecuteNonQuery();
                connection.Close();
            }
            return users;
        }
        [HttpDelete("{id:int}")]
        public ActionResult<List<CovidData>> DeleteData(int id)
        {
            List<CovidData> users = new List<CovidData>();

            using (var connection = new SqliteConnection("Data Source= myDB.db"))
            {
                connection.Open();
                var delete = connection.CreateCommand();
                delete.CommandText = @"delete from CovidData where Id = " + id + " ";
                delete.ExecuteNonQuery();
                connection.Close();
            }

            return Ok(users);
        }
    }

}