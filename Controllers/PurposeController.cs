using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using FullStack.API.Models;
using System.Web.Http.Cors;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/purpose")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PurposeController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetPurpose")]
        public List<Purpose> GetPurpose()
        {

            List<Purpose> purposeList = new List<Purpose>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select GenericID,GenericName from MstGeneric where GenericKey1='MstSubject' order by Genericname asc", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var purpose = new Purpose();

                    purpose.GenericID = (string)rdr["GenericID"];
                    purpose.GenericName = rdr["GenericName"].ToString();
                    purposeList.Add(purpose);
                }
            }
            return purposeList;

        }
    }
}
