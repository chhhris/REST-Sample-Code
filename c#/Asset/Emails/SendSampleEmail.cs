/*
   SendSampleEmail.cs

   Marketo REST API Sample Code
   Copyright (C) 2016 Marketo, Inc.

   This software may be modified and distributed under the terms
   of the MIT license.  See the LICENSE file for details.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using System.Web;

namespace Samples
{

    class SendSampleEmail
    {
        private String host = "CHANGE ME"; //host of your marketo instance, https://AAA-BBB-CCC.mktorest.com
        private String clientId = "CHANGE ME"; //clientId from admin > Launchpoint
        private String clientSecret = "CHANGE ME"; //clientSecret from admin > Launchpoint
        public int id;//id of email to send
        public String emailAddress;//email address to send to
        public Boolean textOnly;//default false
        public int leadId;//optional leadId to impersonate

        public String postData()
        {
            //Assemble the URL
            String url = host + "/rest/asset/v1/email/" + id + "/sendSample.json?";
            //Serialize the Data into JSON
            String requestBody = bodyBuilder();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            StreamWriter wr = new StreamWriter(request.GetRequestStream());
            wr.Write(requestBody);
            wr.Flush();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            return reader.ReadToEnd();
        }
        private String bodyBuilder()
        {
            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs.Add("access_token", getToken());
            qs.Add("emailAddress", emailAddress);
            if (textOnly != null)
            {
                qs.Add("textOnly", textOnly.ToString());
            }
            if (leadId > 0){
                qs.Add("leadId", leadId.ToString());
            }
            return qs.ToString();
        }

        private String getToken()
        {
            String url = host + "/identity/oauth/token?grant_type=client_credentials&client_id=" + clientId + "&client_secret=" + clientSecret;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            String json = reader.ReadToEnd();
            //Dictionary<String, Object> dict = JavaScriptSerializer.DeserializeObject(reader.ReadToEnd);
            Dictionary<String, String> dict = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);
            return dict["access_token"];
        }
    }
}