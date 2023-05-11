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
using System.Text;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/ucn")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class UCNDigitalController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetBrand")]
        public List<UCNDigitalBrand> GetUCNBrand()
        {
            List<UCNDigitalBrand> brandList = new List<UCNDigitalBrand>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNBrand", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Client", SqlDbType.VarChar);
                cmd.Parameters["@Client"].Value = "1";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnbrand = new UCNDigitalBrand();
                    ucnbrand.BrandID = (int)rdr["BrandID"];
                    ucnbrand.Brand = (string)rdr["Brand"];
                    ucnbrand.Client = (string)rdr["Client"];

                    brandList.Add(ucnbrand);
                }
            }
            return brandList;


        }

        [Route("GetBrandCode")]
        public string GetBrandCode(string BrndName,int MaxLimt)
        {
           //var payload = {
           //     "brand" = 'Abc ABC',

           // }

           // var masterConfig =
           // {
           //     "brand" : {
           //     limit: 5,
           //     key: 'brand',
           //     prefix: ''
           // },
          //  }
            string BCode = "";
            BrndName = BrndName.Trim();
            BrndName = BrndName.Replace("  "," ");
            BrndName = RemoveSpecialCharacters(BrndName);

            int lenSrcString = BrndName.Length;
            if (lenSrcString<=MaxLimt)
            {
                BCode = BrndName;
            } //if (lenSrcString<=MaxLimt)
            else if (lenSrcString > MaxLimt)
            { 
                string[] aryWords = BrndName.Split(' ');

                int numOfWords = aryWords.Length;
                
                if (numOfWords==1) {
                    BCode = BrndName.Substring(0, MaxLimt);
                }// if (numOfWords == 1)
                else if (numOfWords==2) 
                {
                    BCode = BCode + aryWords[0].Substring(0, 3);
                    BCode = BCode + aryWords[1].Substring(0, MaxLimt-3);
                } //else if (numOfWords==2) 
                else if (numOfWords == 3)
                {
                    BCode = BCode + aryWords[0].Substring(0, 2);
                    BCode = BCode + aryWords[1].Substring(0, 2);
                    BCode = BCode + aryWords[2].Substring(0, MaxLimt-4);
                } //else if (numOfWords == 3)
                else if (numOfWords == 4)
                {
                    BCode = BCode + aryWords[0].Substring(0, 2);
                    BCode = BCode + aryWords[1].Substring(0, 1);
                    BCode = BCode + aryWords[2].Substring(0, 1);
                    BCode = BCode + aryWords[3].Substring(0, MaxLimt-4);
                }// else if (numOfWords == 4)
                else if (numOfWords == 5)
                {
                    BCode = BCode + aryWords[0].Substring(0, 1);
                    BCode = BCode + aryWords[1].Substring(0, 1);
                    BCode = BCode + aryWords[2].Substring(0, 1);
                    BCode = BCode + aryWords[3].Substring(0, 1);
                    BCode = BCode + aryWords[4].Substring(0, MaxLimt - 4);

                }// else if (numOfWords == 5) 
                else if (numOfWords == 6)
                {
                    BCode = BCode + aryWords[0].Substring(0, 1);
                    BCode = BCode + aryWords[1].Substring(0, 1);
                    BCode = BCode + aryWords[2].Substring(0, 1);
                    BCode = BCode + aryWords[3].Substring(0, 1);
                    BCode = BCode + aryWords[4].Substring(0, 1);
                    BCode = BCode + aryWords[5].Substring(0, 1);
                   // BCode = BCode.Substring(0, MaxLimt); // todo check if this is required
                }// else if (numOfWords == 6)

                //foreach (String s in aryWords)
                //{
                //    Console.WriteLine(s);
                //}
            }

                return BCode.ToUpper(); 
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9')
                    || (str[i] >= 'A' && str[i] <= 'z'
                        || (str[i] == '.' || str[i] == '_')))
                {
                    sb.Append(str[i]);
                }
            }

            return sb.ToString();
        }




        [Route("GetLanguage")]
        public List<UCNDigitalLanguage> GetUCNLanguage()
        {
            List<UCNDigitalLanguage> languageList = new List<UCNDigitalLanguage>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNLanguage", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnlanguage = new UCNDigitalLanguage();
                    ucnlanguage.LanguageID = (int)rdr["LanguageID"];
                    ucnlanguage.LanguageCode = (string)rdr["LanguageCode"];
                    ucnlanguage.Language = (string)rdr["Language"];

                    languageList.Add(ucnlanguage);
                }
            }
            return languageList;


        }
        
        [Route("GetPlatform")]
        public List<UCNDigitalPlatform> GetUCNPlatform()
        {
            List<UCNDigitalPlatform> platformList = new List<UCNDigitalPlatform>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNPlatform", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnplatform = new UCNDigitalPlatform();
                    ucnplatform.PlatformID = (int)rdr["PlatformID"];
                    ucnplatform.PlatformCode = (string)rdr["PlatformCode"];
                    ucnplatform.Platform = (string)rdr["Platform"];

                    platformList.Add(ucnplatform);
                }
            }
            return platformList;


        }
        
        [Route("GetFormat")]
        public List<UCNDigitalFormat> GetUCNFormat()
        {
            List<UCNDigitalFormat> formatList = new List<UCNDigitalFormat>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNFormat", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnformat = new UCNDigitalFormat();
                    ucnformat.FormatID = (int)rdr["FormatID"];
                    ucnformat.FormatCode = (string)rdr["FormatCode"];
                    ucnformat.Format = (string)rdr["Format"];

                    formatList.Add(ucnformat);
                }
            }
            return formatList;


        }


        [Route("GetPlatformFormat")]
        public List<UCNDigitalPlatformFormat> GetUCNPlatformFormat(int Platform)
        {
            List<UCNDigitalPlatformFormat> platformformatList = new List<UCNDigitalPlatformFormat>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNPlatformFormat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PlatformID", SqlDbType.Int);
                cmd.Parameters["@PlatformID"].Value = Platform;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnplatformformat = new UCNDigitalPlatformFormat();
                    ucnplatformformat.PlatformFormatID = (int)rdr["PlatformFormatID"];

                    ucnplatformformat.PlatformID = (int)rdr["PlatformID"];

                    ucnplatformformat.PlatformCode = (string)rdr["PlatformCode"];

                    ucnplatformformat.Platform = (string)rdr["Platform"];

                    ucnplatformformat.FormatID = (int)rdr["FormatID"];

                    ucnplatformformat.FormatCode = (string)rdr["FormatCode"];

                    ucnplatformformat.Format = (string)rdr["Format"];
                    platformformatList.Add(ucnplatformformat);
                }
            }
            return platformformatList;


        }

        [Route("GetDuration")]
        public List<UCNDigitalDuration> GetUCNDuration()
        {
            List<UCNDigitalDuration> durationList = new List<UCNDigitalDuration>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNDuration", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnduration = new UCNDigitalDuration();
                    ucnduration.DurationID = (int)rdr["DurationID"];
                    ucnduration.DurationCode = (string)rdr["DurationCode"];
                    ucnduration.Duration = (string)rdr["Duration"];

                    durationList.Add(ucnduration);
                }
            }
            return durationList;


        }




        [Route("GetRatio")]
        public List<UCNDigitalRatio> GetUCNRatio()
        {
            List<UCNDigitalRatio> ratioList = new List<UCNDigitalRatio>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnratio = new UCNDigitalRatio();
                    ucnratio.RatioID = (int)rdr["RatioID"];
                    ucnratio.RatioCode = (string)rdr["RatioCode"];
                    ucnratio.Ratio = (string)rdr["Ratio"];

                    ratioList.Add(ucnratio);
                }
            }
            return ratioList;


        }

    }
}
