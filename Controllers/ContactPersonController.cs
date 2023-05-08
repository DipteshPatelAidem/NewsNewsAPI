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
    [RoutePrefix("api/ContactPerson")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ContactPersonController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetContactPersons")]
        public List<ContactPerson> GetContactPersons(string Search)
        {

            List<ContactPerson> contactpersonList = new List<ContactPerson>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                //                SqlCommand cmd = new SqlCommand("SELECT PersonCode,PersonName,CompanyName FROM vwcontactLookup where PersonName like '%" + Search + "%'", con);
                //SqlCommand cmd = new SqlCommand("SELECT top 10 PersonCode,PersonName,CompanyName,trim(isnull((select isnull(GenericName,'') from vwMstTitlePerson where GenericID=TitlePersonCode),'') + ' '+ PersonName +' - ' +ISNULL( CompanyName ,'') + '('+trim(str(ContactForCode)) +' - '+ ContactFor+')' ) as PersonName2 FROM vwcontactLookup where PersonName like '%" + Search + "%' and isnull(delFlag,0)=0 order by PersonName", con);
                SqlCommand cmd = new SqlCommand("SELECT top 10 PersonCode,PersonName,CompanyName,trim(isnull((select isnull(GenericName,'') from vwMstTitlePerson where GenericID=TitlePersonCode),'') + ' '+ PersonName +' - ' +ISNULL( CompanyName ,'') + '('+trim(str(ContactForCode)) +' - '+ ContactFor+')' ) as PersonName2,ContactFor,ContactForCode FROM vwcontactLookup where PersonName like '%" + Search + "%' and isnull(delFlag,0)=0 order by PersonName", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ContactPerson = new ContactPerson();

                    ContactPerson.PersonCode = (Guid)rdr["PersonCode"];
                    ContactPerson.PersonName = rdr["PersonName"].ToString();
                    ContactPerson.CompanyName = rdr["CompanyName"].ToString();
                    ContactPerson.PersonName2 = rdr["PersonName2"].ToString();
                    ContactPerson.ContactFor = rdr["ContactFor"].ToString();
                    ContactPerson.ContactForCode = (int)rdr["ContactForCode"];

                    contactpersonList.Add(ContactPerson);
                }
            }
            return contactpersonList;

        }

        [HttpPost]
        [Route("ContactPersonPost")]
        //  public IHttpActionResult ContactPersonInsertUpdate(Nullable<Guid> PersonCode ,int intContactTypeCode , string PersonName , int intDesignationCode, int intTitlePersonCode , int intDepartmentCode,int intContactForCode,string strContactFor, int intEnteredBy,int intDelFlag)
        public IHttpActionResult ContactPersonInsertUpdate(ContactPeople contactPeopleMet)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {

                 
                SqlCommand cmd = new SqlCommand("ContactPersonInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PersonCode", SqlDbType.UniqueIdentifier );
                cmd.Parameters["@PersonCode"].Value = contactPeopleMet.PersonCode;
                
                cmd.Parameters.Add("@ContactTypeCode", SqlDbType.Int);
                cmd.Parameters["@ContactTypeCode"].Value = contactPeopleMet.intContactTypeCode;

                cmd.Parameters.Add("@PersonName", SqlDbType.VarChar);
                cmd.Parameters["@PersonName"].Value = contactPeopleMet.PersonName;
                
                cmd.Parameters.Add("@DesignationCode", SqlDbType.Int);
                cmd.Parameters["@DesignationCode"].Value = contactPeopleMet.intDesignationCode;
                
                cmd.Parameters.Add("@TitlePersonCode", SqlDbType.Int);
                cmd.Parameters["@TitlePersonCode"].Value = contactPeopleMet.intTitlePersonCode;

                cmd.Parameters.Add("@DepartmentCode", SqlDbType.Int);
                cmd.Parameters["@DepartmentCode"].Value = contactPeopleMet.intDepartmentCode;
                 
                cmd.Parameters.Add("@ContactForCode", SqlDbType.Int);
                cmd.Parameters["@ContactForCode"].Value = contactPeopleMet.intContactForCode;
                
                cmd.Parameters.Add("@ContactFor", SqlDbType.VarChar);
                cmd.Parameters["@ContactFor"].Value = contactPeopleMet.strContactFor;

                cmd.Parameters.Add("@EnteredBy", SqlDbType.Int);
                cmd.Parameters["@EnteredBy"].Value = contactPeopleMet.intEnteredBy;

                //  cmd.Parameters.Add("@DelFlag", SqlDbType.Int);
                // cmd.Parameters["@DelFlag"].Value = contactPeopleMet.intDelFlag;



                SqlParameter parmOUT = new SqlParameter("@return", SqlDbType.VarChar,50);
                
                parmOUT.Direction =ParameterDirection.Output;
                cmd.Parameters.Add(parmOUT);


                con.Open();
                cmd.ExecuteNonQuery();
                string returnVALUE = (string)cmd.Parameters["@return"].Value;
                con.Close();
                return Ok(returnVALUE);
            }

           // return Ok();
        }
    }
}
