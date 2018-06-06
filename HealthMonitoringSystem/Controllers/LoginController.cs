using DataAccess;
using HealthMonitoringSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthMonitoringSystem.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public UserModel AuthUser([FromBody]UserModel model)
        {
            UserModel authUser = new UserModel();
            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pEmail", SqlDbType.VarChar, 50, model.email.Trim());
                oDm.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = oDm.ExecuteReader("User_Authenticate");

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        authUser.userId = (dr["UserId"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["UserId"]);
                        authUser.name = (dr["Name"] == DBNull.Value) ? "" : Convert.ToString(dr["Name"]);
                        authUser.password = (dr["Password"] == DBNull.Value) ? "" : dr["Password"].ToString().Trim();
                        authUser.email = (dr["Email"] == DBNull.Value) ? "" : dr["Email"].ToString().Trim();
                    }
                }
                try
                {
                    if (authUser != null && authUser.password == model.password.Trim()

                           && authUser.email == model.email.Trim())
                    {
                        authUser.result = true;
                        authUser.Message = "Logged in successfully";
                        return authUser;
                    }
                    else
                    {
                        authUser.Message = "No account found";
                        authUser.result = false;
                        return authUser;
                    }
                }
                catch (Exception e)
                {
                    authUser.Message = e.Message;
                    authUser = new UserModel();
                    authUser.result = false;
                    return authUser;
                }
            }
        }
    }
}
