using Dapper;
using NPOI.SS.Formula.Functions;
using RentSystem.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RentSystem.Repository
{
    public static class DBContext
    {

        private static string connectionStrings = AppConfigurtaionServices.Configuration.GetSection("Connections").GetSection("ConnectiongStrings").Value;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> Query(string sql,object param = null)
        {
            using(var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                return Db.Query<T>(sql, param).ToList();

            }
            
        }

        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QueryFirst(string sql, object param = null)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                return Db.Query<T>(sql, param).ToList().First();
            }
           

        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>

        public static T QueryFirstOrDefault<T>(string sql,object param = null)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                return Db.QueryFirstOrDefault<T>(sql, param);
            }
          
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QuerySingle(string sql,object param = null)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                return Db.Query<T>(sql, param).ToList().Single();
            }
        }

        /// <summary>
        /// 获取单条数据没有就返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QuerySingleOrDefault(string sql,object param = null)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                return Db.Query<T>(sql, param).ToList().SingleOrDefault();
            }
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int Execute(string sql,object param = null)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                return Db.Execute(sql, param);
            }
        }

        /// <summary>
        /// Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(string sql,object param)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                return Db.ExecuteReader(sql, param);
            }
        }

        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> ExecutePro(string proc,object param)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                List<T> list = Db.Query<T>(proc, param, null, true, null, CommandType.StoredProcedure).ToList();
                return list;
            }
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="sqlArr"></param>
        /// <returns></returns>
        public static int ExecuteTransition(string[] sqlArr)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                using (var transition = Db.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in sqlArr)
                        {
                            result += Db.Execute(sql, null, transition);
                        }
                        transition.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transition.Rollback();
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 带参数的事务
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static int ExecuteTransition(Dictionary<string,object> dic)
        {
            using (var Db = new SqlConnection(connectionStrings))
            {
                if (Db.State != ConnectionState.Open)
                {
                    Db.Open();
                }
                using (var transition = Db.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in dic)
                        {
                            result += Db.Execute(sql.Key, sql.Value, transition);
                        }
                        transition.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transition.Rollback();
                        return 0;
                    }
                }
            }
        }

    }
}
