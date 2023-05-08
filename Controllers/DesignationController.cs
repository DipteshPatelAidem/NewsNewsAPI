using System.Collections.Generic;
using System.Web.Http;
using FullStack.API.Models;
using System.Web.Http.Cors;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/designation")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DesignationController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetDesignation")]
        public List<Designation> GetDesignation()
        {
            
            List<Designation> designationList = new List<Designation>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MstDesignation", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var designation = new Designation();

                    designation.DesignationID = (string)rdr["DesignationID"];
                    designation.DesignationName = rdr["DesignationName"].ToString();
                    designationList.Add(designation);
                }
            }
            return designationList;
        }

        [HttpPost]
        [Route ("DesignationInsert")]
        public int DesignationInsert(Designation designation)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DesignationInsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DesignationID", SqlDbType.VarChar);
                cmd.Parameters["@DesignationID"].Value = designation.DesignationID;
                cmd.Parameters.Add("@DesignationName", SqlDbType.VarChar);
                cmd.Parameters["@DesignationName"].Value = designation.DesignationName;
  
                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return 0; // check
        }

        [HttpPost]
        [Route("DesignationUpdate")]
        public int DesignationUpdate(Designation designation)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("DesignationUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DesignationCode", SqlDbType.Int);
                cmd.Parameters["@DesignationCode"].Value = designation.DesignationCode;
                cmd.Parameters.Add("@DesignationID", SqlDbType.VarChar);
                cmd.Parameters["@DesignationID"].Value = designation.DesignationID;
                cmd.Parameters.Add("@DesignationName", SqlDbType.VarChar);
                cmd.Parameters["@DesignationName"].Value = designation.DesignationName;

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return 0; // check
        }

        [Route("GetPeople")]
        public List<ContactPeople> GetPeople(string name)
        {
            List<ContactPeople> contactPeopleList = new List<ContactPeople>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SELECT PersonName, CompanyName FROM vwContactLookup where PersonName like '%" + name + "%'", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var contactPeople = new ContactPeople();

                    contactPeople.PersonName = (string)rdr["PersonName"];
                    contactPeople.CompanyName = rdr["CompanyName"].ToString();
                    contactPeopleList.Add(contactPeople);
                }
            }
            return contactPeopleList;
        }

    }
}