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
using System.Web.Script.Serialization;

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

        [HttpPost]
        [Route("GetUCNDigital")]
        public List<UCNDigitalPreview> GetUCNCode(dynamic ucnpreview)
        {


            List<UCNDigitalMasterConfig> ucnmasterconfiglist = GetUCNMasterConfig();


            // string ucnMasterConfig = "[{'key':'brandCode','limit':'5','prefix':'','index':'1'},{'key':'caption','limit':'4','prefix':'-','index':'2'},{'key':'duration','limit':'0','prefix':'-','index':'3'},{'key':'language','limit':'0','prefix':'-','index':'4'},{'key':'destination','limit':'0','prefix':'-','index':'5'},{'key':'format','limit':'0','prefix':'-','index':'6'},{'key':'ratio','limit':'0','prefix':'-','index':'7'}]";
            string ucnMasterConfig = "[{'key':'brandCode','limit':'5','prefix':'','index':'1'},{'key':'caption','limit':'4','prefix':'','index':'2'},{'key':'duration','limit':'0','prefix':'','index':'3'},{'key':'language','limit':'0','prefix':'','index':'4'},{'key':'destination','limit':'0','prefix':'','index':'5'},{'key':'format','limit':'0','prefix':'','index':'6'},{'key':'ratio','limit':'0','prefix':'','index':'7'}]";
            //string ucnMasterConfig = "[{'key':'caption','limit':'4','prefix':'-','index':'2'},{'key':'brandCode','limit':'5','prefix':'','index':'4'},{'key':'duration','limit':'0','prefix':'-','index':'3'},{'key':'language','limit':'0','prefix':'-','index':'1'},{'key':'destination','limit':'0','prefix':'-','index':'5'},{'key':'format','limit':'0','prefix':'-','index':'6'},{'key':'ratio','limit':'0','prefix':'-','index':'7'}]";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic masterConfig = serializer.Deserialize<dynamic>(ucnMasterConfig);
            // masterConfig =Array.Sort(masterConfig,4,1);
            int MaxLimt = 6;
            string BrndName = "";
            string prfix = "";
            //            string[] brandnames;
            string ucnCode = "";
            List<UCNDigitalPreview> ucnpreviewList = new List<UCNDigitalPreview>();
            int currentCounter = 0;
            int processCounter = 0;
            string CurrentlyProcessFor = "";



            //// get master config
            //foreach (dynamic formula in masterConfig)
            //{
            //    processCounter = 0;
            //    currentCounter++;
            //    // dynamic key = formula["key"];
            //    foreach (dynamic eachAry in formula)
            //    {
            //        if (eachAry.Key == "limit")
            //        {
            //            MaxLimt = Int32.Parse(eachAry.Value);
            //        }
            //        if ((eachAry.Key == "prefix") )
            //        {
            //            prfix = eachAry.Value;
            //        }
            //        if ((eachAry.Key== "index"))
            //        {
            //            processCounter = currentCounter;
            //        }
            //        if ((eachAry.Key == "key") )
            //        {
            //            CurrentlyProcessFor = eachAry.Value;
            //        }

            //    } // foreach (dynamic eachAry in formula)



            foreach (dynamic formulaFromMasterConfig in ucnmasterconfiglist)
            {
                MaxLimt = Int32.Parse(formulaFromMasterConfig.limit);
                CurrentlyProcessFor = formulaFromMasterConfig.key;
                prfix = formulaFromMasterConfig.prefix;

                if (CurrentlyProcessFor == "brandCode")
                {
                    BrndName = ucnpreview.brandname;
                    ucnCode = ucnCode + prfix + GenerateBrandCode((string)ucnpreview.brandname, MaxLimt);
                    CurrentlyProcessFor = "";
                } // if (CurrentlyProcessFor == "brandCode")

                if (CurrentlyProcessFor == "language")
                {
                    string lng = ucnpreview.language;
                    List<UCNDigitalLanguage> ucnlaguagelist = GetUCNLanguage();
                    UCNDigitalLanguage selectedlng = ucnlaguagelist.SingleOrDefault(x => x.Language.ToUpper() == lng.ToUpper());
                    string lngCd = selectedlng.LanguageCode.ToString();
                    ucnCode = ucnCode + prfix + lngCd; // todo Lannguage code from master
                    CurrentlyProcessFor = "";
                } //if (CurrentlyProcessFor == "language")
                if (CurrentlyProcessFor == "caption")
                {
                    string caption = (string)ucnpreview.caption;
                    ucnCode = ucnCode + GenerateCaptionCode((string)caption, MaxLimt);
                    CurrentlyProcessFor = "";
                } //if (CurrentlyProcessFor == "caption")
                if (CurrentlyProcessFor == "duration")
                {
                    decimal drn = ucnpreview.duration;
                    string strdrn = (drn).ToString();
                    ucnCode = ucnCode + prfix + (strdrn);
                    // ucnpreview.duration = Decimal.Parse("99.99"); // todo remove
                    CurrentlyProcessFor = "";
                } //if (CurrentlyProcessFor == "duration")
                if (CurrentlyProcessFor == "destination")
                {
                    ucnCode = ucnCode + prfix + GeneratePlatformCode((string)ucnpreview.platform);
                    CurrentlyProcessFor = "";
                } //if (CurrentlyProcessFor == "destination")

                if (CurrentlyProcessFor == "format")
                {
                    ucnCode = ucnCode + GenerateFormatCode((string)ucnpreview.format); ;
                    CurrentlyProcessFor = "";
                } // if (CurrentlyProcessFor == "format")
                if (CurrentlyProcessFor == "ratio")
                {
                    ucnCode = ucnCode + prfix + "";
                    CurrentlyProcessFor = "";
                } //if (CurrentlyProcessFor == "ratio")



            } // foreach (dynamic formulaFromMasterConfig in ucnmasterconfiglist)

            //           } //foreach (dynamic formula in masterConfig)

            var ucnPreviewResponse = new UCNDigitalPreview();
            ucnPreviewResponse.brandname = ucnpreview.brandname;
            ucnPreviewResponse.caption = ucnpreview.caption;
            ucnPreviewResponse.duration = ucnpreview.duration;
            ucnPreviewResponse.language = ucnpreview.language;
            ucnPreviewResponse.platform = ucnpreview.platform;
            ucnPreviewResponse.format = ucnpreview.format;
            ucnPreviewResponse.ratio = ucnpreview.ratio;

            ucnPreviewResponse.UCNdigitalCode = ucnCode.ToUpper();
            ucnpreviewList.Add(ucnPreviewResponse);
            return ucnpreviewList;
        }
        public List<UCNDigitalMasterConfig> GetUCNMasterConfig()
        {
            List<UCNDigitalMasterConfig> masterconfigList = new List<UCNDigitalMasterConfig>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("GetUCNMasterConfig", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var ucnmasterconfig = new UCNDigitalMasterConfig();
                    ucnmasterconfig.key = (string)rdr["key"];
                    ucnmasterconfig.limit = (string)rdr["limit"];
                    ucnmasterconfig.prefix = (string)rdr["prefix"];
                    ucnmasterconfig.index = (int)rdr["index"];

                    masterconfigList.Add(ucnmasterconfig);
                }
            }
            return masterconfigList;


        }
        public string GenerateBrandCode(string BrndName, int MaxLimt)
        {
            string BCode = "";
            BrndName = BrndName.Trim(); // todo handle null here
            string[] aryWords = BrndName.Split(' ');
            int numOfWords = aryWords.Length;
            // BrndName = BrndName.Replace("  "," ");
            // BrndName = RemoveSpecialCharacters(BrndName);

            if (numOfWords >= MaxLimt)
            {
                foreach (string aryWord in aryWords)
                {
                    BCode = BCode + aryWord.Substring(0, 1);
                }
            }
            else
            {
                if (numOfWords == 1)
                {
                    BCode = BrndName.Substring(0, MaxLimt);
                }
                else if (numOfWords == 2)
                {
                    BCode = BCode + aryWords[0].Substring(0, 3);
                    BCode = BCode + aryWords[1].Substring(0, MaxLimt - 3);
                }
                else if (numOfWords == 3)
                {
                    BCode = BCode + aryWords[0].Substring(0, 2);
                    BCode = BCode + aryWords[1].Substring(0, 2);
                    BCode = BCode + aryWords[2].Substring(0, MaxLimt - 4);
                } //else if (numOfWords == 3)
                else if (numOfWords == 4)
                {
                    BCode = BCode + aryWords[0].Substring(0, 2);
                    BCode = BCode + aryWords[1].Substring(0, 1);
                    BCode = BCode + aryWords[2].Substring(0, 1);
                    BCode = BCode + aryWords[3].Substring(0, MaxLimt - 4);
                }
            }

            return BCode;
        }
        public string GenerateCaptionCode(string caption, int MaxLimt)
        {
            // string cption ;
            StringBuilder sb = new StringBuilder();
            foreach (char c in caption)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Substring(0, MaxLimt);
        }
        public string GeneratePlatformCode(string platform)
        {

            string platfrm = platform;
            List<UCNDigitalPlatform> ucnPlatformlist = GetUCNPlatform();
            UCNDigitalPlatform selectedPlatfrm = ucnPlatformlist.SingleOrDefault(x => x.Platform.ToUpper() == platfrm.ToUpper());
            string pltfrmCd = selectedPlatfrm.PlatformCode.ToString();
            pltfrmCd = pltfrmCd.PadLeft(4, '0');
            return pltfrmCd;
        }
        public string GenerateFormatCode(string format)
        {
            string frmt = format;
            List<UCNDigitalFormat> ucnformatlist = GetUCNFormat();
            UCNDigitalFormat selectedFormat = ucnformatlist.SingleOrDefault(x => x.Format.ToUpper() == frmt.ToUpper());
            string formatCd = selectedFormat.FormatCode.ToString();
            return formatCd;
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


        [HttpPost]
        [Route("UCNDigitalInsert")]
        public IHttpActionResult UCNDigitalInsert(UCNDigital ucndigital)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("UCNDigitalInsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UCNdigitalCode", SqlDbType.VarChar);
                cmd.Parameters["@UCNdigitalCode"].Value = ucndigital.UCNdigitalCode;

                cmd.Parameters.Add("@BrandID", SqlDbType.Int);
                cmd.Parameters["@BrandID"].Value = ucndigital.BrandID;

                cmd.Parameters.Add("@Caption", SqlDbType.VarChar);
                cmd.Parameters["@Caption"].Value = ucndigital.Caption;

                cmd.Parameters.Add("@DurationID", SqlDbType.Int);
                cmd.Parameters["@DurationID"].Value = ucndigital.DurationID;

                cmd.Parameters.Add("@LanguageID", SqlDbType.Int);
                cmd.Parameters["@LanguageID"].Value = ucndigital.LanguageID;

                cmd.Parameters.Add("@PlatformID", SqlDbType.Int);
                cmd.Parameters["@PlatformID"].Value = ucndigital.PlatformID;

                cmd.Parameters.Add("@FormatID", SqlDbType.Int);
                cmd.Parameters["@FormatID"].Value = ucndigital.FormatID;

                cmd.Parameters.Add("@RatioID", SqlDbType.Int);
                // cmd.Parameters["@RatioID"].Value = DBNull.Value; // ucndigital.RatioID;
                if (ucndigital.RatioID.HasValue)
                {
                    cmd.Parameters["@RatioID"].Value = ucndigital.RatioID;
                }
                else
                {
                    cmd.Parameters["@RatioID"].Value = DBNull.Value; // ucndigital.RatioID;
                }

                cmd.Parameters.Add("@config", SqlDbType.Text);
                cmd.Parameters["@config"].Value = ucndigital.config;

                cmd.Parameters.Add("@EnteredBy", SqlDbType.Int);
                cmd.Parameters["@EnteredBy"].Value = ucndigital.EnteredBy;

                cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int);
                cmd.Parameters["@UpdatedBy"].Value = DBNull.Value; // ucndigital.UpdatedBy;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int);
                cmd.Parameters["@CompanyID"].Value = ucndigital.CompanyID;

                con.Open();

                cmd.ExecuteNonQuery();
            }
            return Ok();
        }
    }
}
