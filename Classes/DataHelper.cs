using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheAnimal.Classes
{
    public class DataHelper
    {
        public DataTable GetData(string path)
        {
            var dt = new DataTable
            {
                TableName = "Animals"
            };

            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Questions", typeof(string));

            dt.ReadXml(path);

            return dt;
        }

        public List<Animal> GetAnimals(DataTable dt)
        {
            var animals = new List<Animal>();

            foreach (DataRow r in dt.Rows)
            {
                animals.Add(new Animal
                {
                    Name = r["Name"].ToString(),
                    Questions = r["Questions"].ToString()
                });
            }

            return animals;
        }

        public void AddAnimal(string Name, string Questions, string path)
        {
            var dt = GetData(path);

            dt.Rows.Add(Name, Questions);

            dt.WriteXml(path);
        }
    }
}
