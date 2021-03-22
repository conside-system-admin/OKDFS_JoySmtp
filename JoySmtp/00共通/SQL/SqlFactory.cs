using System.Data.SqlClient;
using System.Collections;

namespace JoySmtp.SQL
{
    public class SqlFactory
    {
        private string i_SQL;
        private ArrayList i_SQLParam;
        private string i_StoredProcedureName;

        public SqlFactory()
        {
            i_SQL = string.Empty;
            i_SQLParam = new ArrayList();
            i_StoredProcedureName = string.Empty;
        }

        /// <summary>
        ///         ''' SQL文字列のプロパティ
        ///         ''' </summary>
        ///         ''' <value>SQLの文字列</value>
        ///         ''' <returns>SQLの文字列</returns>
        ///         ''' <remarks></remarks>
        public string SQL
        {
            set
            {
                i_SQL = value;
            }
            get
            {
                return i_SQL;
            }
        }

        /// <summary>
        ///         ''' ストアドプロシージャの名前のプロマティ
        ///         ''' </summary>
        ///         ''' <value></value>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public string StoredProcedureName
        {
            set
            {
                i_StoredProcedureName = value;
            }
            get
            {
                return i_StoredProcedureName;
            }
        }
        /// <summary>
        ///         ''' SQLのパラメータを取得します
        ///         ''' </summary>
        ///         ''' <value></value>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public SqlParameter[] SqlParam
        {
            get
            {
                return (SqlParameter[])i_SQLParam.ToArray(typeof(SqlParameter));
            }
        }

        /// <summary>
        ///         ''' パラメータを追加します
        ///         ''' </summary>
        ///         ''' <param name="objSqlParam">パラメータ</param>
        ///         ''' <remarks></remarks>
        public void AddParameter(SqlParameter objSqlParam)
        {
            i_SQLParam.Add(objSqlParam);
        }

        /// <summary>
        ///         ''' パラメータによって、SQL文を取得します
        ///         ''' </summary>
        ///         ''' <returns>SQL文</returns>
        ///         ''' <remarks></remarks>
        public string GetSQL()
        {
            //SqlParameter objParam = null;
            string strSQL = i_SQL;
            foreach (SqlParameter objParam in i_SQLParam)
            {
                string strSQLVal = objParam.Value.ToString();
                strSQL = strSQL.Replace(objParam.ParameterName, strSQLVal);
            }
            return strSQL;
        }
    }
}
