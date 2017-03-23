namespace iMotto.Web.Models
{
    public class RespBase
    {
        public string Code { get; set; }
        public int State { get; set; }
        public string Msg { get; set; }
    }

    public class LoginResp : RespBase
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserToken { get; set; }
        public string UserThumb { get; set; }
    }

    public class MottoModel
    {
        public long ID { get; set; }
        public string UID { get; set; }
        public double Score { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }
        public int Reviews { get; set; }
        public int Loves { get; set; }
        public int RecruitID { get; set; }
        public string RecruitTitle { get; set; }
        public string Content { get; set; }
        public string AddTime { get; set; }
        public string UserName { get; set; }
        public string UserThumb { get; set; }
        public int Reviewed { get; set; }
        public int State { get; set; }
        /// <summary>
        /// 喜欢状态 0:未喜欢，1：已喜欢
        /// </summary>
        public int Loved { get; set; }
        /// <summary>
        /// 投票状态： 1：已支持，-1：反对，0：中立，9：未投票
        /// </summary>
        public int Vote { get; set; }
        public int Collected { get; set; }
    }

    public class ReadResp<T> : RespBase
    {
        public T Data { get; set; }
    }

    public class RegisterDeviceResp : RespBase
    {
        public string Sign { get; set; }
    }
}
