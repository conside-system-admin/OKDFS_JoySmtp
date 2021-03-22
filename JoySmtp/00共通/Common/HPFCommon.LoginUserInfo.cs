using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
//using System.Reflection.MethodBase;
using System.Net;
using JoySmtp.CLogOut;//.HPFLogOut;
using JoySmtp.Data;//.HPFData;
using JoySmtp.DataBase;


namespace JoySmtp.JoyCommon
{
    public partial class Common
    {
    //    /// <summary>
    //    ///         ''' ﾛｸﾞｲﾝﾕｰｻﾞｰの必須情報(ユーザーIDとユーザー名前)を追加します
    //    ///         ''' </summary>
    //    ///         ''' <remarks></remarks>
    //    public void SetLoginUserDef(DataRow drUserInfo)
    //    {
    //        try
    //        {
    //            string StrBusho = "";  // 部署
    //            if (HPFData.LoginUserInfo == null || HPFData.LoginUserInfo.Rows.Count == 0)
    //            {
    //                HPFData.LoginUserInfo = new DataTable();
    //                HPFData.LoginUserInfo.Columns.Add(HPFData.LOGINUSERINFO_KEY, Type.GetType("System.String"));
    //                HPFData.LoginUserInfo.Columns.Add(HPFData.LOGINUSERINFO_VALUE, Type.GetType("System.String"));

    //                // ﾛｸﾞｲﾝﾕｰｻﾞｰID
    //                Add(HPFData.LOGINUSERINFO_KEY_ID, ConvertToString(drUserInfo.Item("LOGIN_ID")));

    //                // ﾛｸﾞｲﾝﾕｰｻﾞｰの名前
    //                Add(HPFData.LOGINUSERINFO_KEY_NAME, ConvertToString(drUserInfo.Item("SHAIN_NAME")));

    //                // ﾛｸﾞｲﾝﾕｰｻﾞｰの有効期限区分
    //                Add(HPFData.LOGINUSERINFO_KEY_PW_HENKO_FLG, ConvertToInteger(drUserInfo.Item("PW_HENKO_FLG")));

    //                Add(HPFData.LOGINUSERINFO_KEY_BUSHO_CD, ConvertToString(drUserInfo.Item("BUSHO_CD")));

    //                Add(HPFData.LOGINUSERINFO_KEY_SHAIN_KBN, ConvertToString(drUserInfo.Item("SHAIN_KBN")));


    //                Add(HPFData.LOGINUSERINFO_KEY_SHAIN_CD, ConvertToString(drUserInfo.Item("SHAIN_CD")));
    //                Add(HPFData.LOGINUSERINFO_KEY_SHAIN_BCD, ConvertToString(drUserInfo.Item("SHAIN_BCD")));
    //                Add(HPFData.LOGINUSERINFO_KEY_SHAIN_NAME_KN, ConvertToString(drUserInfo.Item("SHAIN_NAME_KN")));
    //                Add(HPFData.LOGINUSERINFO_KEY_SHAIN_NAME_RK, ConvertToString(drUserInfo.Item("SHAIN_NAME_RK")));
    //                Add(HPFData.LOGINUSERINFO_KEY_SHAIN_NAME_RK_KN, ConvertToString(drUserInfo.Item("SHAIN_NAME_RK_KN")));
    //                Add(HPFData.LOGINUSERINFO_KEY_LOGIN_PW, ConvertToString(drUserInfo.Item("LOGIN_PW")));
    //                Add(HPFData.LOGINUSERINFO_KEY_PW_UPDATE, ConvertToString(drUserInfo.Item("PW_UPDATE")));
    //                Add(HPFData.LOGINUSERINFO_KEY_WEB_ID, ConvertToString(drUserInfo.Item("WEB_ID")));
    //                Add(HPFData.LOGINUSERINFO_KEY_WEB_PW1, ConvertToString(drUserInfo.Item("WEB_PW1")));
    //                Add(HPFData.LOGINUSERINFO_KEY_WEB_PW2, ConvertToString(drUserInfo.Item("WEB_PW2")));
    //                Add(HPFData.LOGINUSERINFO_KEY_WEB_FLG, ConvertToString(drUserInfo.Item("WEB_FLG")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENGEN_GP_ID, ConvertToString(drUserInfo.Item("KENGEN_GP_ID")));
    //                Add(HPFData.LOGINUSERINFO_KEY_YUKO_START, ConvertToString(drUserInfo.Item("YUKO_START")));
    //                Add(HPFData.LOGINUSERINFO_KEY_YUKO_END, ConvertToString(drUserInfo.Item("YUKO_END")));
    //                Add(HPFData.LOGINUSERINFO_KEY_ZEN_BUMON_FLG, ConvertToString(drUserInfo.Item("ZEN_BUMON_FLG")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_01, ConvertToString(drUserInfo.Item("KENMU_BUSHO_01")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_02, ConvertToString(drUserInfo.Item("KENMU_BUSHO_02")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_03, ConvertToString(drUserInfo.Item("KENMU_BUSHO_03")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_04, ConvertToString(drUserInfo.Item("KENMU_BUSHO_04")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_05, ConvertToString(drUserInfo.Item("KENMU_BUSHO_05")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_06, ConvertToString(drUserInfo.Item("KENMU_BUSHO_06")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_07, ConvertToString(drUserInfo.Item("KENMU_BUSHO_07")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_08, ConvertToString(drUserInfo.Item("KENMU_BUSHO_08")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_09, ConvertToString(drUserInfo.Item("KENMU_BUSHO_09")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_10, ConvertToString(drUserInfo.Item("KENMU_BUSHO_10")));
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO_11, ConvertToString(drUserInfo.Item("KENMU_BUSHO_11")));
    //                Add(HPFData.LOGINUSERINFO_KEY_CRT_SHAIN_CD, ConvertToString(drUserInfo.Item("CRT_SHAIN_CD")));
    //                Add(HPFData.LOGINUSERINFO_KEY_CRT_DATE, ConvertToString(drUserInfo.Item("CRT_DATE")));
    //                Add(HPFData.LOGINUSERINFO_KEY_CRT_TANMATU_ID, ConvertToString(drUserInfo.Item("CRT_TANMATU_ID")));
    //                Add(HPFData.LOGINUSERINFO_KEY_UPD_SHAIN_CD, ConvertToString(drUserInfo.Item("UPD_SHAIN_CD")));
    //                Add(HPFData.LOGINUSERINFO_KEY_UPD_DATE, ConvertToString(drUserInfo.Item("UPD_DATE")));
    //                Add(HPFData.LOGINUSERINFO_KEY_UPD_TANMATU_ID, ConvertToString(drUserInfo.Item("UPD_TANMATU_ID")));
    //                Add(HPFData.LOGINUSERINFO_KEY_VERSION_NO, ConvertToInteger(drUserInfo.Item("VERSION_NO")));
    //                Add(HPFData.LOGINUSERINFO_KEY_MUKO_FLG, ConvertToString(drUserInfo.Item("MUKO_FLG")));
    //                Add(HPFData.LOGINUSERINFO_KEY_DELETE_FLG, ConvertToString(drUserInfo.Item("DELETE_FLG")));
    //                Add(HPFData.LOGINUSERINFO_KEY_HAITA_FLG, ConvertToString(drUserInfo.Item("HAITA_FLG")));
    //                Add(HPFData.LOGINUSERINFO_KEY_HAITA_NICHIJI, ConvertToString(drUserInfo.Item("HAITA_NICHIJI")));
    //                Add(HPFData.LOGINUSERINFO_KEY_HAITA_USER, ConvertToString(drUserInfo.Item("HAITA_USER")));

    //                Add(HPFData.LOGINUSERINFO_KEY_LOGINFLG, ConvertToString(drUserInfo.Item("LOGINFLG")));
    //                Add(HPFData.LOGINUSERINFO_KEY_LOGIN_TANMATU_ID, ConvertToString(drUserInfo.Item("LOGIN_TANMATU_ID")));
    //                Add(HPFData.LOGINUSERINFO_KEY_LOGINDT, ConvertToString(drUserInfo.Item("LOGINDT")));
    //                Add(HPFData.LOGINUSERINFO_KEY_OSHIRASE_NEW_FLG, ConvertToString(drUserInfo.Item("OSHIRASE_NEW_FLG")));
    //                Add(HPFData.LOGINUSERINFO_KEY_SOKO_CD, ConvertToString(drUserInfo.Item("SOKO_CD")));
    //                Add(HPFData.LOGINUSERINFO_KEY_SOKO_NAME, ConvertToString(drUserInfo.Item("SOKO_NAME")));

    //                // 所属部署を設定する 書式：'0001','0002','0003'.......
    //                if (!drUserInfo.Item("BUSHO_CD").ToString().Equals(""))
    //                {
    //                    if (StrBusho.Equals(""))
    //                        StrBusho = "'" + drUserInfo.Item("BUSHO_CD").ToString() + "'";
    //                    else
    //                        StrBusho = StrBusho + "," + drUserInfo.Item("BUSHO_CD").ToString();
    //                }
    //                for (int i = 1; i <= 11; i++)
    //                {
    //                    if (!drUserInfo.Item("KENMU_BUSHO_" + i.ToString("00")).ToString().Equals(""))
    //                    {
    //                        // If i = 1 AndAlso Not StrBusho.Equals("") Then
    //                        // StrBusho = "'" & StrBusho & "'"
    //                        // End If
    //                        if (StrBusho.Equals(""))
    //                            StrBusho = "'" + drUserInfo.Item("KENMU_BUSHO_" + i.ToString("00")).ToString() + "'";
    //                        else
    //                            StrBusho = StrBusho + ",'" + drUserInfo.Item("KENMU_BUSHO_" + i.ToString("00")).ToString() + "'";
    //                    }
    //                }
    //                StrBusho = GetBusyo(StrBusho);
    //                Add(HPFData.LOGINUSERINFO_KEY_KENMU_BUSHO, ConvertToString(StrBusho));

    //                // 端末PCのID
    //                Add(HPFData.LOGINUSERINFO_KEY_TANMATSUID, Dns.GetHostName());

    //                Add(HPFData.LOGINUSERINFO_KEY_HAVE_NOTYFY, string.Empty);
    //            }
    //            else
    //            {
    //                // 更新で、ﾛｸﾞｲﾝﾕｰｻﾞｰの必須情報を更新します
    //                // ﾛｸﾞｲﾝﾕｰｻﾞｰ名前
    //                Update(HPFData.LOGINUSERINFO_KEY_NAME, ConvertToString(drUserInfo.Item("SHAIN_NAME")));

    //                // ﾛｸﾞｲﾝﾕｰｻﾞｰの有効期限区分
    //                Update(HPFData.LOGINUSERINFO_KEY_PW_HENKO_FLG, ConvertToInteger(drUserInfo.Item("PW_HENKO_FLG")));

    //                Update(HPFData.LOGINUSERINFO_KEY_BUSHO_CD, ConvertToString(drUserInfo.Item("BUSHO_CD")));

    //                Update(HPFData.LOGINUSERINFO_KEY_SHAIN_KBN, ConvertToString(drUserInfo.Item("SHAIN_KBN")));
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            ErrorOut(ex.Message, "HPFCommon.LoginUserInfo", MethodBase.GetCurrentMethod.Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' ログインの全部の部署結果を戻ります
    //    ///         ''' </summary> 
    //    ///         ''' <param name="strBusyo">兼務部署と部署</param>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public string GetBusyo(string strBusyo)
    //    {
    //        HPFDataBaseSQL objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //        try
    //        {
    //            DataSet dsSearch = null/* TODO Change to default(_) if this is not a reference type */;
    //            objDb = new HPFDataBaseSQL();
    //            objDb.DBLogIn();
    //            StringBuilder strSQL = new StringBuilder();
    //            bool blnHasWhere = false;

    //            strSQL.AppendLine("  with [CTE] (BUSHO_CD, BUSHO_NAME, JOI_BUSHO_CD) as (    ");
    //            strSQL.AppendLine("  select BUSHO_CD,BUSHO_NAME,JOI_BUSHO_CD                                                              ");
    //            strSQL.AppendFormat("  from [M_BUSHO] where BUSHO_CD in ({0})  ", strBusyo).AppendLine();

    //            strSQL.AppendLine("  union all            ");
    //            strSQL.AppendLine("  select M_BUSHO.BUSHO_CD,M_BUSHO.BUSHO_NAME,M_BUSHO.JOI_BUSHO_CD    ");
    //            strSQL.AppendLine("  from M_BUSHO inner join [CTE] on (      ");
    //            strSQL.AppendLine("  M_BUSHO.JOI_BUSHO_CD  = [CTE].BUSHO_CD))                      ");
    //            strSQL.AppendLine("  select * from [CTE] union                     ");
    //            strSQL.AppendLine("  select busho_cd ,busho_name,joi_busho_cd from M_BUSHO     ");
    //            strSQL.AppendFormat("  where BUSHO_CD in ({0})  ", strBusyo).AppendLine();

    //            dsSearch = objDb.GetDataSet(strSQL.ToString());

    //            for (int i = 0; i <= dsSearch.Tables(0).Rows.Count - 1; i++)
    //            {
    //                if (i == 0)
    //                    strBusyo = "'" + dsSearch.Tables(0).Rows(i).Item("BUSHO_CD").ToString() + "'";
    //                else
    //                    strBusyo = strBusyo + ",'" + dsSearch.Tables(0).Rows(i).Item("BUSHO_CD").ToString() + "'";
    //            }

    //            return strBusyo;
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorOut(ex.Message, "PublicSearch", MethodBase.GetCurrentMethod.Name);
    //            throw ex;
    //        }
    //        finally
    //        {
    //            if (IsNothing(objDb) == false)
    //            {
    //                objDb.DBLogOut();
    //                objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //            }
    //        }
    //    }
    //    /// <summary>
    //    ///         ''' キーでﾛｸﾞｲﾝﾕｰｻﾞｰの基本情報を追加します
    //    ///         ''' </summary>
    //    ///         ''' <param name="strKey">キーの名前</param>
    //    ///         ''' <param name="strValue">値</param>
    //    ///         ''' <remarks></remarks>
    //    public bool SetLoginUserByKey(string strKey, string strValue)
    //    {
    //        try
    //        {
    //            if (IsNothing(LoginUserInfo) || LoginUserInfo.Rows.Count == 0)
    //            {
    //                LoginUserInfo = new DataTable();

    //                LoginUserInfo.Columns.Add(HPFData.LOGINUSERINFO_KEY, Type.GetType("System.String"));
    //                LoginUserInfo.Columns.Add(HPFData.LOGINUSERINFO_VALUE, Type.GetType("System.String"));
    //                // ﾛｸﾞｲﾝﾕｰｻﾞｰ情報の追加
    //                Add(strKey, strValue);
    //            }
    //            else
    //                // 更新で、ﾛｸﾞｲﾝﾕｰｻﾞｰの必須情報を更新します
    //                Update(strKey, strValue);

    //            LoginUserInfo.AcceptChanges();
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            ErrorOut(ex.Message, "HPFCommon.LoginUserInfo", MethodBase.GetCurrentMethod.Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' ﾛｸﾞｲﾝﾕｰｻﾞｰ情報の更新
    //    ///         ''' </summary>
    //    ///         ''' <param name="strKey">キー</param>
    //    ///         ''' <param name="strValue">値</param>
    //    ///         ''' <remarks></remarks>
    //    private void Update(string strKey, string strValue)
    //    {
    //        try
    //        {
    //            DataRow drUpdate = GetLoginUserInfoRow(strKey);

    //            if (IsNothing(drUpdate))
    //                Add(strKey, strValue);
    //            else
    //                drUpdate.Item(HPFData.LOGINUSERINFO_VALUE) = strValue;
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            ErrorOut(ex.Message, "HPFCommon.LoginUserInfo", MethodBase.GetCurrentMethod.Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' ﾛｸﾞｲﾝﾕｰｻﾞｰ情報のテーブルに、項目を追加します
    //    ///         ''' </summary>
    //    ///         ''' <param name="strKey">キーのな前</param>
    //    ///         ''' <param name="objValue">キーの値</param>
    //    ///         ''' <remarks></remarks>
    //    private void Add(string strKey, object objValue)
    //    {
    //        try
    //        {
    //            DataRow drNew = LoginUserInfo.NewRow;
    //            drNew.Item(HPFData.LOGINUSERINFO_KEY) = strKey;
    //            drNew.Item(HPFData.LOGINUSERINFO_VALUE) = objValue;
    //            LoginUserInfo.Rows.Add(drNew);
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            ErrorOut(ex.Message, "HPFCommon.LoginUserInfo", MethodBase.GetCurrentMethod.Name);
    //            throw ex;
    //        }
    //    }

        /// <summary>
        ///         ''' キーでﾛｸﾞｲﾝﾕｰｻﾞｰ情報の内容を取得します
        ///         ''' </summary>
        ///         ''' <param name="strKey">キー</param>
        ///         ''' <returns>取得した内容</returns>
        ///         ''' <remarks></remarks>
        public string GetLoginUserInfo(string strKey)
        {
            try
            {
                DataRow drRow = GetLoginUserInfoRow(strKey);
                if (DBNull.Value.Equals(drRow))
                    return string.Empty;

                return ConvertToString(drRow[HPFData.LOGINUSERINFO_VALUE]);
            }
            catch (Exception ex)
            {
                // エラー処理
                LogOut.ErrorOut(ex.Message, "HPFCommon.LoginUserInfo", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

    //    /// <summary>
    //    ///         ''' キーでﾛｸﾞｲﾝﾕｰｻﾞｰ情報の内容を取得します
    //    ///         ''' </summary>
    //    ///         ''' <param name="strKey">キー</param>
    //    ///         ''' <returns>取得した内容</returns>
    //    ///         ''' <remarks></remarks>
    //    public int GetLoginUserInfo2(string strKey)
    //    {
    //        try
    //        {
    //            DataRow drRow = GetLoginUserInfoRow(strKey);
    //            if (IsNothing(drRow))
    //                return string.Empty;

    //            return ConvertToString(drRow.Item(HPFData.LOGINUSERINFO_VALUE));
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            ErrorOut(ex.Message, "HPFCommon.LoginUserInfo", MethodBase.GetCurrentMethod.Name);
    //            throw ex;
    //        }
    //    }
        /// <summary>
        ///         ''' キーでﾛｸﾞｲﾝﾕｰｻﾞｰ情報のデータローを取得します
        ///         ''' </summary>
        ///         ''' <param name="strKey">キー</param>
        ///         ''' <returns>ﾛｸﾞｲﾝﾕｰｻﾞｰ情報のロー</returns>
        ///         ''' <remarks></remarks>
        private static DataRow GetLoginUserInfoRow(string strKey)
        {
            try
            {
                DataRow[] drRows = null;

                // ﾛｸﾞｲﾝﾕｰｻﾞｰ情報のテーブルにレコードがないとき、空を戻します
                if ((HPFData.LoginUserInfo == null) || HPFData.LoginUserInfo.Rows.Count == 0)
                    return null/* TODO Change to default(_) if this is not a reference type */;

                // キーが無いとき、空を戻します
                if (HPFData.LoginUserInfo.Columns.Contains(strKey))
                    return null/* TODO Change to default(_) if this is not a reference type */;

                // ﾛｸﾞｲﾝﾕｰｻﾞｰ情報のテーブルから、キーに対するレコードを取得します
                drRows = HPFData.LoginUserInfo.Select(string.Format("{0} = '{1}' ", HPFData.LOGINUSERINFO_KEY, strKey));

                // 取得した結果が0件であれば、空を戻します
                if (drRows.Length == 0)
                    return null/* TODO Change to default(_) if this is not a reference type */;

                return drRows[0];
            }
            catch (Exception ex)
            {
                // エラー処理
                LogOut.ErrorOut(ex.Message, "HPFCommon.LoginUserInfo", MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
    }
}
