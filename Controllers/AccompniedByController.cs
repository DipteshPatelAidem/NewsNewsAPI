using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FullStack.API.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

using System.Web.Http.Cors;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/accompniedby")]

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccompniedByController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;
        [Route("GetAccompniedBy")]
        public List<AccompaniedBy> GetAccompninedBy()
        {

            List<AccompaniedBy> accompaniedbyList = new List<AccompaniedBy>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select distinct UserCode,UserName,GroupHead,dol from vwUserTeam a where (a.Todate is NULL or a.Todate > getdate()) and ( dol is null or dol >getdate())", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var accompaniedby = new AccompaniedBy();

                    accompaniedby.UserCode = (int)rdr["usercode"];
                    accompaniedby.UserName = rdr["UserName"].ToString();
                    accompaniedbyList.Add(accompaniedby);
                }
            }
            return accompaniedbyList;
        }

    }
}
