using iMotto.Web.Models;
using Rest.Net;
using Rest.Net.Interfaces;
using System;
using System.Collections.Generic;

namespace iMotto.Web.Services
{
    public class IMottoApi
    {
        IRestClient client = new RestClient("http://app.imotto.net/");

        private string SIGNATURE = "";
        private string USER_ID = "";
        private string USER_TOKEN = "";

        public RespBase TestSpeed()
        {
            try
            {
                var resp = client.Get<RespBase>("/Api/CMN1004");
                return resp.Data;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("An error occured: {0}", e);
                return new RespBase
                {
                    Code = "CMN1004",
                    State = -1,
                    Msg = e.Message
                };
            }
        }

        public void RegisterDevice(string uniqueId, Action<RegisterDeviceResp> callback)
        {

            var param = new
            {
                Brand = "Litter Producer",
                OS = "dotnet",
                UniqueId = uniqueId,
                TVersion = "1.0",
                Type = "Z"
            };

            DoRequest<RegisterDeviceResp>("CMN1001", param, (resp) => {
                if (resp.State == 0)
                {
                    SIGNATURE = resp.Sign;
                }

                callback(resp);
            });
        }

        /// <summary>
        /// 创作一个motto
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="content"></param>
        /// <param name="callback"></param>
        public void AddMotto(int rid, string content, Action<RespBase> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                UserId = USER_ID,
                Token = USER_TOKEN,
                RID = rid,
                Content = content
            };

            DoRequest("MOT1001", param, callback);
        }

        /// <summary>
        /// 添加投票
        /// </summary>
        /// <param name="mid">偶得ID</param>
        /// <param name="theDay">偶得发布日期 yyyyMMdd</param>
        /// <param name="vote">投票： 1:支持，-1：反对，0：中立</param>
        /// <param name="callback"></param>
        public void AddVote(long mid, int theDay, int vote, Action<RespBase> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                UserId = USER_ID,
                Token = USER_TOKEN,
                MID = mid,
                TheDay = theDay,
                Support = vote
            };

            DoRequest("MOT1002", param, callback);
        }

        /// <summary>
        /// 喜欢motto
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="theDay"></param>
        /// <param name="callback"></param>
        public void loveMotto(long mid, int theDay, Action<RespBase> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                UserId = USER_ID,
                Token = USER_TOKEN,
                MID = mid,
                TheDay = theDay
            };

            DoRequest("MOT1003", param, callback);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="theDay"></param>
        /// <param name="content"></param>
        /// <param name="callback"></param>
        public void AddReview(long mid, int theDay, string content, Action<RespBase> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                UserId = USER_ID,
                Token = USER_TOKEN,
                MID = mid,
                TheDay = theDay,
                Content = content
            };

            DoRequest("MOT1005", param, callback);
        }

        ///将Motto加入珍藏
        public void AddMottoToCollection(long mid, long cid, Action<RespBase> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                UserId = USER_ID,
                Token = USER_TOKEN,
                MID = mid,
                CID = cid
            };

            DoRequest("MOT2004", param, callback);
        }


        ///按天读取Motto
        public void ReadMottos(int theday, int pIndex, int pSize, Action<ReadResp<List<MottoModel>>> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                UserId = USER_ID,
                Token = USER_TOKEN,
                TheDay = theday,
                PIndex = pIndex,
                PSize = pSize
            };

            DoRequest("RED2001", param, callback);
        }

        public void UserLogin(string mobile, string password, Action<LoginResp> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                Mobile = mobile,
                Password = password
            };

            DoRequest<LoginResp>("USR1006", param, (resp) => {
                if (resp.State == 0)
                {
                    USER_ID = resp.UserId;
                    USER_TOKEN = resp.UserToken;
                }

                callback(resp);
            });
        }



        public void UserLogOut(string userId, string userToken, Action<RespBase> callback)
        {
            var param = new
            {
                Sign = SIGNATURE,
                UserId = userId,
                Token = userToken
            };

            DoRequest("USR1007", param, callback);
        }

        public void DoRequest<T>(string code, object param, Action<T> callback) where T : RespBase, new()
        {
            string errormsg;
            try
            {
                var resp = client.Post<T>($"/Api/{code}", param);
                if (!resp.IsError)
                {
                    callback(resp.Data);
                    return;
                }

                Console.WriteLine(resp.RawData);
                Console.WriteLine(resp.Exception.ToString());
                errormsg = $"[{resp.StatusCode}- {resp.Exception.Message}]";
            }
            catch (System.Exception e)
            {
                Console.WriteLine("An error occured while requst to code[{0}]: {1}", code, e);
                errormsg = e.Message;
            }

            var result = new T();
            result.Code = code;
            result.State = -1;
            result.Msg = errormsg;

            callback(result);
        }
    }
}