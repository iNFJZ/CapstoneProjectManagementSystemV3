using Infrastructure.Entities;
using Newtonsoft.Json;

namespace CapstoneProjectManagementSystemV3UI.Models
{
    public class ResultObj
    {
        [JsonProperty("$id")]
        public string id { get; set; }
        public bool status { get; set; }
        public int totalPage { get; set; }
        public int pageIndex { get; set; }
        public string search { get; set; }
        public int role { get; set; }
        public List<Role> roles { get; set; }
        public List<User> users { get; set; }
    }

    public class Role
    {
        [JsonProperty("$id")]
        public string id { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
        public object createdAt { get; set; }
        public object updatedAt { get; set; }
        public object deletedAt { get; set; }
        public List<User> users { get; set; }
    }

    public class Roles
    {
        [JsonProperty("$id")]
        public string id { get; set; }

        [JsonProperty("$values")]
        public List<Value> values { get; set; }
    }

    public class ListUserViewModel
    {
        [JsonProperty("$id")]
        public string id { get; set; }
        public bool isSuccessed { get; set; }
        public object message { get; set; }
        public ResultObj resultObj { get; set; }
        public object affiliateAccount { get; set; }
    }

    public class Users
    {
        [JsonProperty("$id")]
        public string id { get; set; }

        [JsonProperty("$values")]
        public List<object> values { get; set; }
    }

    public class Value
    {
        [JsonProperty("$id")]
        public string id { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
        public DateTime createdAt { get; set; }
        public object updatedAt { get; set; }
        public object deletedAt { get; set; }
        public List<User> users { get; set; }
        public int rowNum { get; set; }
        public string userID { get; set; }
        public object userName { get; set; }
        public string fullName { get; set; }
        public string fptEmail { get; set; }
        public object avatar { get; set; }
        public object gender { get; set; }
        public object affiliateAccount { get; set; }
        public int roleID { get; set; }
        public object staff { get; set; }
        public object student { get; set; }
        public object supervisor { get; set; }
        public Role role { get; set; }
        public DateTime created_At { get; set; }
        public object notifications { get; set; }
    }
}
