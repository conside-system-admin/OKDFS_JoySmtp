using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.Data;
using System.Data.SqlClient;

using JoySmtp.Data;
using JoySmtp.CLogOut;
using JoySmtp.SQL;



namespace JoySmtp.DataBase
{
    /// <summary>
    /// データベース操作を管理するクラス
    /// </summary>
    public class DataBaseSQL
    {
        private SqlConnection i_Connection;
        private SqlTransaction i_Transaction;

        ///// <summary>
        ///// コンストラクタ（DB接続）
        ///// <para name="dbServer">DBサーバ名</para>
        ///// <para name="dbName">DB名</para>
        ///// <para name="dbUser">ユーザ名</para>
        ///// <para name="dbPass">パスワード</para>
        ///// </summary>
        //public DataBaseSQL(string dbServer, string dbName, string dbUser, string dbPass)
        //{
        //    // 接続文字列を生成
        //    string connectString =
        //    "Data Source = " + dbServer
        //    + ";Initial Catalog = " + dbName
        //    + ";User ID = " + dbUser
        //    + ";Password = " + dbPass
        //    + ";MultipleActiveResultSets=True";

        //    // i_Connection の新しいインスタンスを生成 (接続文字列を指定)
        //    this.i_Connection = new SqlConnection(connectString);

        //    // データベース接続を開く
        //    this.i_Connection.Open();
        //}

        /// <summary>
        /// DB切断
        /// </summary>
        public void Close()
        {
            this.i_Connection.Close();
            this.i_Connection.Dispose();
        }

        public void DBLogIn(string strDBConnect = "")
        {
            try
            {
                if (i_Connection == null)
                {
                    if (strDBConnect.Equals(string.Empty))
                        i_Connection = new SqlConnection(HPFData.DBConenctString);
                    else
                        i_Connection = new SqlConnection(strDBConnect);
                }

                // すでに開いています
                if (i_Connection.State == ConnectionState.Open)
                    return;

                i_Connection.Open();
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        public void DBLogOut()
        {
            try
            {
                if (i_Connection == null)
                    return;

                // 未閉じる状態で、閉じます
                if (i_Connection.State != ConnectionState.Closed)
                    i_Connection.Close();
                i_Connection.Dispose();
                i_Connection = null;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        
        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTransaction()
        {
            this.i_Transaction = this.i_Connection.BeginTransaction();
        }

        /// <summary>
        /// トランザクション　コミット
        /// </summary>
        public void Commit()
        {
            if (this.i_Transaction != null)
            {
                if (this.i_Transaction.Connection != null)
                {
                    this.i_Transaction.Commit();
                    this.i_Transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// トランザクション　ロールバック
        /// </summary>
        public void RollBack()
        {
            if (this.i_Transaction != null)
            {
                if (this.i_Transaction.Connection != null)
                {
                    this.i_Transaction.Rollback();
                    this.i_Transaction.Dispose();
                }
            }
        }

        /// <summary>
        ///         ''' レコードの取得
        ///         ''' </summary>
        ///         ''' <param name="objFactory"></param>
        ///         ''' <param name="strTableName"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public DataSet GetDataSet(SqlFactory objFactory, string strTableName = "", bool blnLogOut = true)
        {
            try
            {
                DataSet dsResult = null/* TODO Change to default(_) if this is not a reference type */;

                if (blnLogOut == true)
                    LogOut.SQLOut(objFactory.GetSQL(), "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);

                dsResult = SqlHelper.ExecuteDataset(i_Connection, CommandType.Text, objFactory.SQL, objFactory.SqlParam);
                if (string.Empty.Equals(strTableName) == false)
                    dsResult.Tables[0].TableName = strTableName;
                return dsResult;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' レコードの取得
        ///         ''' </summary>
        ///         ''' <param name="objFactory"></param>
        ///         ''' <param name="strTableName"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public DataTable GetDataTable(SqlFactory objFactory, string strTableName = "")
        {
            try
            {
                return GetDataSet(objFactory, strTableName).Tables[0];
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' レコードの取得
        ///         ''' </summary>
        ///         ''' <param name="strSQL"></param>
        ///         ''' <param name="strTableName"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public DataSet GetDataSet(string strSQL, string strTableName = "",  bool blnLogOut = true)
        {
            try
            {
                DataSet dsResult = null/* TODO Change to default(_) if this is not a reference type */;

                if (blnLogOut)
                    LogOut.SQLOut(strSQL, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);

                dsResult = SqlHelper.ExecuteDataset(i_Connection, CommandType.Text, strSQL);
                if (string.Empty.Equals(strTableName) == false)
                    dsResult.Tables[0].TableName = strTableName;
                return dsResult;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' レコードの取得
        ///         ''' </summary>
        ///         ''' <param name="strSQL"></param>
        ///         ''' <param name="strTableName"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public DataTable GetDataTable(string strSQL, string strTableName = "", bool blnLogOut = true)
        {
            try
            {
                return GetDataSet(strSQL, strTableName, blnLogOut).Tables[0];
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }


        /// <summary>
        /// クエリー実行(OUTPUT項目あり)
        /// <para name="query">SQL文</para>
        /// <para name="paramDict">SQLパラメータ</para>
        /// </summary>
        public SqlDataReader ExecuteQuery(string query, Dictionary<string, Object> paramDict)
        {
            SqlCommand sqlCom = new SqlCommand();
            try
            {
                LogOut.SQLOut(query, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);

                //クエリー送信先、トランザクションの指定
                sqlCom.Connection = this.i_Connection;
                sqlCom.Transaction = this.i_Transaction;

                sqlCom.CommandText = query;
                foreach (KeyValuePair<string, Object> item in paramDict)
                {
                    sqlCom.Parameters.Add(new SqlParameter(item.Key, item.Value));
                }

                // SQLを実行
                SqlDataReader reader = sqlCom.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        /// クエリー実行
        /// <para name="query">SQL文</para>
        /// </summary>
        public SqlDataReader ExecuteQuery(string query)
        {
            return this.ExecuteQuery(query, new Dictionary<string, Object>());
        }

        
        /// <summary>
        /// クエリー実行(OUTPUT項目なし)
        /// <para name="query">SQL文</para>
        /// <para name="paramDict">SQLパラメータ</para>
        /// </summary>
        public void ExecuteNonQuery(string query, Dictionary<string, Object> paramDict)
        {
            int intRet = 0;
            ExecuteNonQuery(query, paramDict, out intRet);
        }

        ///// <summary>
        ///// クエリー実行
        ///// <para name="query">SQL文</para>
        ///// <para name="paramDict">SQLパラメータ</para>
        ///// </summary>
        //public void ExecuteNonQuery(string query, Dictionary<string, Object> paramDict, out int intRet)
        //{
        //    SqlCommand sqlCom = new SqlCommand();
        //    try
        //    {
        //        LogOut.SQLOut(query, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);

        //        //クエリー送信先、トランザクションの指定
        //        sqlCom.Connection = this.i_Connection;
        //        sqlCom.Transaction = this.i_Transaction;

        //        sqlCom.CommandText = query;
        //        if (paramDict != null && paramDict.Count > 0)
        //        {
        //            foreach (KeyValuePair<string, Object> item in paramDict)
        //            {
        //                sqlCom.Parameters.Add(new SqlParameter(item.Key, item.Value));
        //            }
        //        }

        //        // SQLを実行
        //        intRet = sqlCom.ExecuteNonQuery();
        //    }
        //    catch (SqlException odbcEx)
        //    {
        //        LogOut.ErrorOut(odbcEx.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
        //        intRet = -odbcEx.ErrorCode;
        //    } 
        //    catch (Exception ex)
        //    {
        //        LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// クエリー実行
        /// <para name="query">SQL文</para>
        /// <para name="paramDict">SQLパラメータ</para>
        /// </summary>
        public void ExecuteNonQuery(string query, Dictionary<string, Object> paramDict, out int intRet)
        {
            int num = 10;
            int errcode = 0;
            intRet = 0;
            if (!string.IsNullOrWhiteSpace(query))
            {
                while (num > 0)
                {
                    try
                    {
                        ExecuteNonQueryBase(query, paramDict, out intRet, out errcode);
                        num = 0;
                    }
                    catch (SqlException odbcEx)
                    {
                        if (odbcEx.ErrorCode == 11 || odbcEx.ErrorCode == 1205)
                        {

                            num--;
                            System.Threading.Thread.Sleep(5000);
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        intRet = -1;
                        break;
                    }

                }
            }
        }
        /// <summary>
        /// クエリー本実行
        /// <para name="query">SQL文</para>
        /// <para name="paramDict">SQLパラメータ</para>
        /// </summary>
        public void ExecuteNonQueryBase(string query, Dictionary<string, Object> paramDict, out int intRet, out int intErrcode)
        {
            SqlCommand sqlCom = new SqlCommand();
            intErrcode = 0;
            try
            {
                LogOut.SQLOut(query, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);

                //クエリー送信先、トランザクションの指定
                sqlCom.Connection = this.i_Connection;
                sqlCom.Transaction = this.i_Transaction;

                sqlCom.CommandText = query;
                if (paramDict != null && paramDict.Count > 0)
                {
                    foreach (KeyValuePair<string, Object> item in paramDict)
                    {
                        sqlCom.Parameters.Add(new SqlParameter(item.Key, item.Value));
                    }
                }

                // SQLを実行
                intRet = sqlCom.ExecuteNonQuery();
            }
            catch (SqlException odbcEx)
            {
                LogOut.ErrorOut(odbcEx.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                intRet = 0;
                intErrcode = odbcEx.ErrorCode;
                throw odbcEx;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
        /// <summary>
        /// レコードの更新
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="blnUseTransaction">
        /// True :トランザクションがある
        /// False:トランザクションがなし
        /// </param>
        /// <param name="blnLogOut"></param>
        /// <returns></returns>
        public int ExecuteScalar(string strSQL, bool blnUseTransaction = false)
        {
            SqlCommand sqlCom = new SqlCommand();
            if (string.IsNullOrWhiteSpace(strSQL))
            {
                return -1;
            }

            try
            {
                LogOut.SQLOut(strSQL, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                if (blnUseTransaction == true)
                {
                    // トランザクションが空であれば、トランザクションを開始します
                    if (this.i_Transaction == null)
                    {
                        this.i_Transaction = this.i_Connection.BeginTransaction();

                    }
                }
                sqlCom.Connection = this.i_Connection;
                sqlCom.Transaction = this.i_Transaction;

                sqlCom.CommandType = System.Data.CommandType.Text;
                sqlCom.CommandText = strSQL;

                // ExecuteScalarで結果セットの最初の行にある最初の列を取得する
                var ret = sqlCom.ExecuteScalar();
                return Convert.ToInt32(ret);
            }
            catch (SqlException odbcEx)
            {
                LogOut.ErrorOut(odbcEx.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                return -1;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "HPFDataBaseSQL", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            //int n;
            //if (strSQL.Substring(strSQL.Length - 1) != ";") strSQL += ";";
            //strSQL += "SELECT CAST(scope_identity() AS int)";

            //SqlCommand cmd = new SqlCommand(strSQL, this.i_Connection);
            ////cmd.Parameters.Add("@Name", SqlDbType.VarChar);
            ////cmd.Parameters["@name"].Value = newName;

        }
    }


}