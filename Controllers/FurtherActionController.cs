using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FullStack.API.Models;
using System.Web.Http.Cors;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/furtheraction")]

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FurtherActionController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;
        [Route("GetFurtherAction")]
        public List<FurtherAction> GetFurtherAction()
        {
            List<FurtherAction> furtheractionList = new List<FurtherAction>();

            //var furtheraction = new FurtherAction();
            //furtheraction.ActionName = "Follow up";
            //furtheractionList.Add(furtheraction);
            //var furtheraction2 = new FurtherAction();
            //furtheraction2.ActionName = "Share Assets";
            //furtheractionList.Add(furtheraction2);

            //var furtheraction3 = new FurtherAction();
            //furtheraction3.ActionName = "Share Proposal";
            //furtheractionList.Add(furtheraction3);

            //var furtheraction4 = new FurtherAction();
            //furtheraction4.ActionName = "Revised Proposal";
            //furtheractionList.Add(furtheraction4);


            //var furtheraction5 = new FurtherAction();
            //furtheraction5.ActionName = "Other";
            //furtheractionList.Add(furtheraction5);

             using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetGenericByGenericKey", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@GenericKey1", SqlDbType.Text);
                cmd.Parameters["@GenericKey1"].Value = "MstFurtherAction";

                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var furtheraction = new FurtherAction();
                    furtheraction.ActionName = rdr["GenericName"].ToString();
                    furtheractionList.Add(furtheraction);

                }
            }
           
            return furtheractionList;
        }

    }
}
