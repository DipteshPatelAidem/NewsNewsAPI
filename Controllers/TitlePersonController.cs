using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using FullStack.API.Models;
using System.Web.Http.Cors;

using System.Configuration;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/titleperson")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TitlePersonController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetTitlePerson")]
        public List<TitlePerson> GetTitlePerson()
        {
            List<TitlePerson> titlepersonList = new List<TitlePerson>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select GenericID,GenericName from vwMstTitlePerson", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var titleperson = new TitlePerson();

                    titleperson.GenericID = (string)rdr["GenericID"];
                    titleperson.GenericName = rdr["GenericName"].ToString();
                    titlepersonList.Add(titleperson);
                }
            }
            return titlepersonList;
        }
    }
}
