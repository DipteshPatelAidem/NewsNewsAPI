using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FullStack.API.Models;
//using System.Web.Http.Cors;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/Channel")]
          [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ChannelController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;
        //public List<Channel> GetChannels()
        //{
        //    List<Channel> channelList = new List<Channel> ();
        //    using (SqlConnection con = new SqlConnection(CS))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM MstChannel Order by ChannelName", con);
        //        cmd.CommandType = CommandType.Text;
        //        con.Open();

        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            var Channel = new Channel();

        //            Channel.ChannelCode= (int)rdr["ChannelCode"];
        //            Channel.ChannelID = (string)rdr["ChannelID"];
        //            Channel.ChannelName = rdr["ChannelName"].ToString();
        //            channelList.Add(Channel);
        //        }
        //        return channelList;


        //    }
        //}
        [Route("GetChannels")]
       public List<Channel> GetChannels(int uCode)
        {
            List<Channel> channelList = new List<Channel>();
            using (SqlConnection con = new SqlConnection(CS))
            {

                SqlCommand cmd = new SqlCommand("select* from MstChannel where channelCode in (SELECT distinct channelcode FROM vwSMUserChannel where usercode = " + uCode.ToString() + ")  Order by ChannelName", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var Channel = new Channel();

                    Channel.ChannelCode = (int)rdr["ChannelCode"];
                    Channel.ChannelID = (string)rdr["ChannelID"];
                    Channel.ChannelName = rdr["ChannelName"].ToString();
                    channelList.Add(Channel);
                }
                return channelList;


            }
        }


        [Route("GetSMChannels")]
        public List<Channel> GetSMChannels(int UserCode)
        {
            List<Channel> channelList = new List<Channel>();
            using (SqlConnection con = new SqlConnection(CS))
            {

                SqlCommand cmd = new SqlCommand("select* from MstChannel where channelCode in (SELECT distinct channelcode FROM vwSMUserChannel where usercode = " + UserCode.ToString() + ")  Order by ChannelName", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var Channel = new Channel();

                    Channel.ChannelCode = (int)rdr["ChannelCode"];
                    Channel.ChannelID = (string)rdr["ChannelID"];
                    Channel.ChannelName = rdr["ChannelName"].ToString();
                    channelList.Add(Channel);
                }
                return channelList;


            }
        }

    }
}
