using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Cache;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Demo_CsharpBetweenSrvAndDll
{
    class srv_operation
    {
        public string BuildJson_Enroll(string encMinutiae, string eSkey, string iv, string id, int fp_idx, int privilege)
        {
            //put together as new serialize json string as server need
            json_srv_enroll json_to_srv = new json_srv_enroll();
            string ret_str;
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer seralizer = new DataContractJsonSerializer(typeof(json_srv_enroll));

                //assign one json to another json 
                json_to_srv.encMinutiae = encMinutiae;
                json_to_srv.eSkey = eSkey;
                json_to_srv.iv = iv;
                json_to_srv.clientUserId = id; 
                json_to_srv.fpIndex = fp_idx;
                json_to_srv.privilege = privilege;
                //write to stream
                seralizer.WriteObject(ms, json_to_srv);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                ret_str = sr.ReadToEnd();
                sr.Close();
            }
            return ret_str;
        }
        public string BuildJson_Verify(string encMinutiae, string eSkey, string iv, string id, int fp_idx, int privilege)
        {
            return BuildJson_Enroll(encMinutiae, eSkey, iv, id, fp_idx, privilege);
        }

        public string BuildJson_Identify(string encMinutiae, string eSkey, string iv)
        {
            //put together as new serialize json string as server need
            json_srv_indetify json_to_srv = new json_srv_indetify();
            string ret_str;
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer seralizer = new DataContractJsonSerializer(typeof(json_srv_indetify));

                //assign one json to another json 
                json_to_srv.encMinutiae = encMinutiae;
                json_to_srv.eSkey = eSkey;
                json_to_srv.iv = iv;

                //write to stream
                seralizer.WriteObject(ms, json_to_srv);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                ret_str = sr.ReadToEnd();
                sr.Close();
            }
            return ret_str;
        }

        public string BuildJson_Delete(string clientUserId, string deleteData)
        {
            //put together as new serialize json string as server need
            json_srv_delete json_to_srv = new json_srv_delete();
            string ret_str;
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer seralizer = new DataContractJsonSerializer(typeof(json_srv_delete));

                //assign one json to another json 
                json_to_srv.clientUserId = clientUserId;
                json_to_srv.deleteData = deleteData;

                //write to stream
                seralizer.WriteObject(ms, json_to_srv);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                ret_str = sr.ReadToEnd();
                sr.Close();
            }
            return ret_str;
        }
        public string ReComposeJson_Enroll(string json_in, string id, int fp_idx, int privilege)
        {
            json_get_minutiae json_from_WebAPI;
            //de-serialize json string from web api.
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json_in)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(json_get_minutiae));
                json_from_WebAPI = (json_get_minutiae)deseralizer.ReadObject(ms);// //反序列化ReadObject
            }

            //put together as new serialize json string as server need
            json_srv_enroll json_to_srv = new json_srv_enroll();
            string ret_str;
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer seralizer = new DataContractJsonSerializer(typeof(json_srv_enroll));

                //assign one json to another json 
                json_to_srv.encMinutiae = json_from_WebAPI.data.encMinutiae;
                json_to_srv.eSkey = json_from_WebAPI.data.eSkey;
                json_to_srv.iv = json_from_WebAPI.data.iv;
                json_to_srv.clientUserId = id; // richTextBox_id.Text;

                //write to stream
                seralizer.WriteObject(ms, json_to_srv);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                ret_str = sr.ReadToEnd();
                sr.Close();
            }
            return ret_str;
        }



        public string ReComposeJson_Identify(string json_in)
        {
            json_get_minutiae json_from_WebAPI;
            //de-serialize json string from web api.
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json_in)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(json_get_minutiae));
                json_from_WebAPI = (json_get_minutiae)deseralizer.ReadObject(ms);// //反序列化ReadObject
            }

            //put together as new serialize json string as server need
            json_srv_indetify json_to_srv = new json_srv_indetify();
            string ret_str;
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer seralizer = new DataContractJsonSerializer(typeof(json_srv_indetify));

                //assign one json to another json 
                json_to_srv.encMinutiae = json_from_WebAPI.data.encMinutiae;
                json_to_srv.eSkey = json_from_WebAPI.data.eSkey;
                json_to_srv.iv = json_from_WebAPI.data.iv;

                //write to stream
                seralizer.WriteObject(ms, json_to_srv);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                ret_str = sr.ReadToEnd();
                sr.Close();
            }
            return ret_str;
        }

        public string ReComposeJson_Delete(string json_in)
        {
            json_get_delete_data json_from_WebAPI;
            //de-serialize json string from web api.
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json_in)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(json_get_delete_data));
                json_from_WebAPI = (json_get_delete_data)deseralizer.ReadObject(ms);// //反序列化ReadObject
            }

            //put together as new serialize json string as server need
            json_srv_delete json_to_srv = new json_srv_delete();
            string ret_str;
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer seralizer = new DataContractJsonSerializer(typeof(json_srv_delete));

                //assign one json to another json 
                json_to_srv.clientUserId = json_from_WebAPI.data.clientUserId;
                json_to_srv.deleteData = json_from_WebAPI.data.deleteData;

                //write to stream
                seralizer.WriteObject(ms, json_to_srv);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                ret_str = sr.ReadToEnd();
                sr.Close();
            }
            return ret_str;
        }

        public string Srv_Enroll(string json_string, bool https_en, string ip, string port)
        {
            string route = "/redirect/enroll";

            string ret_str = PostJson2RedirectServer(https_en, ip, port, route, json_string);

            return ret_str;
        }

        public string Srv_Verify(string json_string, bool https_en, string ip, string port)
        {
            string route = "/redirect/verify";

            string ret_str = PostJson2RedirectServer(https_en, ip, port, route, json_string);

            return ret_str;
        }

        public string Srv_Identify(string json_string, bool https_en, string ip, string port)
        {
            string route = "/redirect/identify";

            string ret_str = PostJson2RedirectServer(https_en, ip, port, route, json_string);

            return ret_str;
        }

        public string Srv_Delete(string json_string, bool https_en, string ip, string port)
        {
            string route = "/redirect/delete";

            string ret_str = PostJson2RedirectServer(https_en, ip, port, route, json_string);

            return ret_str;
        }

        private string PostJson2RedirectServer(bool https_en, string SrvIp, string port, string route, string json_string)
        {
            string protocol = "";

            if (https_en)
            {
                protocol = "https://" + SrvIp + ":" + port;

                ServicePointManager.ServerCertificateValidationCallback =
                delegate { return true; };
            }
            else
            {
                protocol = "http://" + SrvIp + ":" + port;
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(protocol + route);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            httpWebRequest.UserAgent = "myapp";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = json_string;

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            string ret_str = "";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                ret_str = result.ToString();
            }

            return ret_str;

        }
    }

    [DataContract]
    public class json_data_get_minutiae
    {
        [DataMember(Name = "encMinutiae")]
        public string encMinutiae { get; set; }

        [DataMember(Name = "eSkey")]
        public string eSkey { get; set; }

        [DataMember(Name = "iv")]
        public string iv { get; set; }
    }

    [DataContract]
    public class json_get_minutiae
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "message")]
        public string message { get; set; }

        [DataMember(Name = "data")]
        public json_data_get_minutiae data { get; set; }

    }

    [DataContract]
    public class json_set_session_key
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "message")]
        public string message { get; set; }
    }

    [DataContract]
    public class json_data_get_delete_data
    {
        [DataMember(Name = "clientUserId")]
        public string clientUserId { get; set; }

        [DataMember(Name = "deleteData")]
        public string deleteData { get; set; }
    }

    [DataContract]
    public class json_get_delete_data
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "message")]
        public string message { get; set; }

        [DataMember(Name = "data")]
        public json_data_get_delete_data data { get; set; }

    }

    [DataContract]
    public class json_srv_enroll
    {
        [DataMember(Name = "encMinutiae")]
        public string encMinutiae { get; set; }

        [DataMember(Name = "eSkey")]
        public string eSkey { get; set; }

        [DataMember(Name = "iv")]
        public string iv { get; set; }

        [DataMember(Name = "clientUserId")]
        public string clientUserId { get; set; }

        [DataMember(Name = "fpIndex")]
        public int fpIndex { get; set; }

        [DataMember(Name = "privilege")]
        public int privilege { get; set; }
    }

    [DataContract]
    public class json_srv_indetify
    {
        [DataMember(Name = "encMinutiae")]
        public string encMinutiae { get; set; }

        [DataMember(Name = "eSkey")]
        public string eSkey { get; set; }

        [DataMember(Name = "iv")]
        public string iv { get; set; }
    }

    [DataContract]
    public class json_srv_delete
    {
        [DataMember(Name = "clientUserId")]
        public string clientUserId { get; set; }

        [DataMember(Name = "deleteData")]
        public string deleteData { get; set; }
    }
}
