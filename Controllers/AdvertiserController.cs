using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using FullStack.API.Models;

using System.Web.Http.Cors;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/advertiser")]

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdvertiserController : ApiController
    {

        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetAdvertiser")]
        public List<Advertiser> GetAdvertiser()
        {
              
            List<Advertiser> advertiserList = new List<Advertiser>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select distinct AdvertiserCode,AdvertiserName,AdvertiserID from vwmstAdvertiser where ISNULL(recordflag,0)=1 order by AdvertiserName", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var advertiser = new Advertiser();

                    advertiser.AdvertiserCode = (int)rdr["AdvertiserCode"];
                    advertiser.AdvertiserID = (string)rdr["AdvertiserID"];
                    advertiser.AdvertiserName = rdr["AdvertiserName"].ToString();
                    advertiserList.Add(advertiser);
                }
            }
            return advertiserList;
        }

    }
}
