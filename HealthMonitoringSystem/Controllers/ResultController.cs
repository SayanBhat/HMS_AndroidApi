using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using HealthMonitoringSystem.Models;
using DataAccess;
using System.Data;
using System.Data.SqlClient;

namespace HealthMonitoringSystem.Controllers
{
    public class ResultController : ApiController
    {
       
        public List<ResultModel> POST([FromBody] ResultModel reqMod)
        {
            
            List<ResultModel>ansList= reqResult(reqMod);
        
            return ansList;

        }

        private List<ResultModel> reqResult(ResultModel reqMod)
            
        {
            List<ResultModel> ansList = new List<ResultModel>() ;

            using (DataManager oDm = new DataManager())
            {
                oDm.Add("@pUserId", SqlDbType.VarChar, 50, reqMod.userId);
                oDm.Add("@pCategoryName", SqlDbType.VarChar, 50, reqMod.categoryName.Trim());
                oDm.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = oDm.ExecuteReader("Result_GetByCategory");

                if (dr.HasRows && dr!=null )
                {
                   
                    while (dr.Read())
                    {

                        ResultModel sendMod = new ResultModel();
                        sendMod.userId = (dr["UserId"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["UserId"]);
                        sendMod.result = (dr["Result"] == DBNull.Value) ? "" : dr["Result"].ToString().Trim();
                        sendMod.date = (dr["Date"] == DBNull.Value) ? "" : dr["Date"].ToString().Trim();
                        ansList.Add(sendMod);
                    }
                }

            }
            return ansList;
        }
    }
}