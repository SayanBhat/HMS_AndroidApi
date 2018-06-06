using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DataAccess;
using HealthMonitoringSystem.Models;
using System.Data;
using System.Data.SqlClient;

namespace HealthMonitoringSystem.Controllers
{
    public class UserController : ApiController
    {
        private const string REQ_REGISTRATION = "user_save";
        private const string REQ_AUTHENTICATE = "user_auth";

        [HttpPost]
        public UserModel RegisterUser([FromBody]UserModel userMod)

        {

            UserModel resultMod = new UserModel();
            int result;
            using (DataManager odm = new DataManager())

            {
                odm.Add("@pUserId", SqlDbType.Int, ParameterDirection.InputOutput, userMod.userId);
                odm.Add("@pName", SqlDbType.VarChar, 20, userMod.name.Trim());
                odm.Add("@pEmail", SqlDbType.NVarChar, 20, userMod.email.Trim());
                odm.Add("@pPassword", SqlDbType.NVarChar, 20, userMod.password.Trim());
                odm.CommandType = CommandType.StoredProcedure;

                result = odm.ExecuteNonQuery("User_Save");
            }
            if (result == -1)
            {
                resultMod.Message = "Sign Up Successfull";
                resultMod.result = true;
                return resultMod;
            }
            else
            {
                resultMod.Message = "Sign Up Failed!";
                resultMod.result = false;
                return resultMod;
            }
        }

        [HttpGet]
        public List<ResultModel> GetUserResult(int userId, int categoryId)
        {
            List<ResultModel> resultList = new List<ResultModel>();
            using (DataManager odm = new DataManager())
            {
                odm.Add("@pUserId", SqlDbType.Int, userId);
                odm.Add("@pCategoryId", SqlDbType.Int, categoryId);
                odm.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = odm.ExecuteReader("Result_get");

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {


                        resultList.Add(new ResultModel
                        {
                            userId = (dr["UserId"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["UserId"]),
                            resultId = (dr["ResultId"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ResultId"]),
                            categoryId = (dr["CategoryId"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["CategoryId"]),
                            categoryName = (dr["CategoryName"] == DBNull.Value) ? "" : dr["CategoryName"].ToString().Trim(),
                            result = (dr["Result"] == DBNull.Value) ? "" : dr["Result"].ToString().Trim(),
                            date = (dr["Date"] == DBNull.Value) ? "" : dr["Date"].ToString().Trim(),
                            isNull = false
                        });
                    }
                }
                return resultList;
            }

        }
    }

}