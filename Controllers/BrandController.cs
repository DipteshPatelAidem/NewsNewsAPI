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
    [RoutePrefix("api/brand")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BrandController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetBrand")]

        public List<Brand> GetBrand()
        {
            List<Brand> brandList = new List<Brand>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                //     SqlCommand cmd = new SqlCommand("select distinct BrandCode,BrandID,BrandName,AdvertiserName,Advertisercode from vwBrand order by brandName", con);
                SqlCommand cmd = new SqlCommand("select distinct BrandCode,BrandID,(BrandName +' - '+AdvertiserName) as BrandName,AdvertiserName,Advertisercode from vwBrand order by brandName", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var brand = new Brand();
                    brand.BrandCode = (int)rdr["BrandCode"];
                    brand.BrandID = (string)rdr["BrandID"];
                    brand.BrandName = rdr["BrandName"].ToString();
                    brand.AdvertiserName = rdr["AdvertiserName"].ToString();
                    brandList.Add(brand);
                }
            }
            return brandList;
        }

    }
}
