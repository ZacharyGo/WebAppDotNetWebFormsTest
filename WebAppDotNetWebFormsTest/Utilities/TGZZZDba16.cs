// <要約>
/// プロジェクト: お客さまポータルサイト
/// 機能       : 共通処理
/// クラス名    : ENEDO店舗情報取得（契約番号）													
/// 作成日      : 2023/03/02
/// Copyright 2015 FUJITSU LIMITED
/// </要約>

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDBFirstCient;

namespace WebAppDotNetWebFormsTest.Utilities
{
    public class TGZZZDba16 : TGZZZDba
    {
        public TGZZZDba16(TohogasDataContext context) : base(context)
        {
        }

        public class Dba16Accept2_1
        {
            // 使用場所番号
            public string SYB_NO { get; set; }
            //使用場所連番
            public string SYB_RNBN { get; set; }
            // 適用開始年月日
            public string TYK_YMD { get; set; }

        }
        public class Dba16Accept2_2
        {
            // 使用場所番号
            public string SYB_NO { get; set; }
            // 使用場所補足連番
            public string SYB_HSK_RNBN { get; set; }
            // 住所番号
            public string ZYUSY_NO { get; set; }

        }

        public class Dba16Accept2_3
        {
            // 市区町村コード
            public string SKTS_CD { get; set; }
            // 行政区コード
            public string GSK_CD { get; set; }
        }

        public class Dba16Accept4
        {
            // 組織番号
            public string SSK_NO { get; set; }
            // 適用開始年月日
            public string TYK_YMD { get; set; }
            // 適用終了年月日
            public string TYS_YMD { get; set; }
            // 申込開始年月日
            public string MSM_KNG_H { get; set; }
            // 申込終了年月日
            public string MSM_SYRY_YMD { get; set; }
            // マイショップ名
            public string MISYP_ME { get; set; }
            // ソート用マイショップ名かな
            public string ST_YU_MISYP_ME_KN { get; set; }
            // 行政区コード
            public string GSK_CD { get; set; }
            // 表示用マイショップ郵便番号
            public string HYUZYU_MISYP_YUBN_NO { get; set; }
            // 表示用マイショップ住所
            public string HYUZYU_MISYP_ZYUSY { get; set; }
            // 表示用マイショップ電話番号
            public string HYUZYU_MISYP_DWNO { get; set; }
            // 表示用マイショップURL
            public string HYUZYU_MISYP_URL { get; set; }
            // 独自特典有無
            public string DKZ_TKTN_UM { get; set; }
            // 行政区名称
            public string GSK_MS { get; set; }
        }


        /// <要約>
        /// ENEDO店舗情報取得（契約番号）
        /// </要約>
        /// <param name="kiykNo">契約番号</param>
        /// <param name="dateTime">システム日付</param>
        /// <param name="listFlag">リストフラグ</param>
        /// <param name="storeList">店舗情報（マイショップ管理マスタレコード+行政区名称）</param>
        /// <param name="areaStorePresenceFlag">エリア店有無フラグ</param>
        /// <リターン>正常／異常 およびアプリケーション番号</リターン>
        /// 
        public int AcquisitionOfENEDOStoreInformation(string kiykNo, DateTime dateTime, bool listFlag, out List<Dba16Accept4> storeList, out bool areaStorePresenceFlag)
        {
            storeList = new List<Dba16Accept4>();
            areaStorePresenceFlag = false;

            try
            {
                TGZZZLog.StartLog();
                // List<string> sskNoRec = new List<string>();
                
                string dateTimeYMD = dateTime.ToString("yyyyMMdd");
                string tgzSybNo = string.Empty;
                Dba16Accept2_1 dba16Accept2_1;
                Dba16Accept2_2 dba16Accept2_2;
                Dba16Accept2_3 dba16Accept2_3; 
                List<Dba16Accept4> dba16Accept3;

                int result = 0;
                // 1. 位置番号を取得
                result = GetLocationNo(kiykNo, dateTimeYMD, out tgzSybNo);
                if (result != TGZZZConstants.SUCCEESS)
                    return result;

                /* 2) シーケンス番号の最大値を取得します. */
                // 2-1) 位置シーケンス番号を取得する;
                result = GetLocationSequenceNo(tgzSybNo, out dba16Accept2_1);
                if (result != TGZZZConstants.SUCCEESS)
                    return result;

                // 2-2)Where Used補足シーケンス番号を取得する.
                result = GetWhereUsedSupplementSequenceNo(tgzSybNo, out dba16Accept2_2);
                if (result != TGZZZConstants.SUCCEESS)
                    return result;

                // 2-3)都市コードを取得する.
                result = GetCityCode(dba16Accept2_2.ZYUSY_NO, out dba16Accept2_3);
                if (result != TGZZZConstants.SUCCEESS)
                    return result;

                /* 3)周辺店舗情報の取得 */
                result = GetInformationOnNearENEDOStores(dba16Accept2_3, dateTimeYMD, listFlag, out dba16Accept3);
                if (result != TGZZZConstants.SUCCEESS)
                {
                    // DBアクセス異常：汎用ログに出力、イベントログに出力、異常値（”-11”）を返却する。
                    return result;
                }

                if (!listFlag)
                {
                    if (dba16Accept3.Count == 0)
                    {
                        areaStorePresenceFlag = false;
                        result = TGZZZConstants.NOTFOUND;
                    }
                    else
                    {
                        areaStorePresenceFlag = true;
                        storeList = dba16Accept3;
                        result = TGZZZConstants.SUCCEESS;
                    }
                }
                else // listflagはtrueです。                
                {
                    List<string> sskNoList = new List<string>();

                    if (dba16Accept3.Count == 0)
                    {
                        // エリア・ストア存在フラグをfalseに設定します。
                        areaStorePresenceFlag = false;
                        result = GetENEDOStoreInformationList(sskNoList, dateTimeYMD, out storeList);
                    }
                    else
                    {
                        // エリアストア存在フラグをtrueに設定する
                        areaStorePresenceFlag = true;
                        sskNoList = dba16Accept3.Select(x => x.SSK_NO).ToList();
                        result = GetENEDOStoreInformationList(sskNoList, dateTimeYMD, out storeList);
                        // 取得したデータをストア一覧に追加する,
                        storeList.AddRange(dba16Accept3);
                    }
                }
                /* 4)ENEDOストア情報リストの取得 */
                foreach (Dba16Accept4 dba16Accept4 in storeList)
                {
                    Console.WriteLine(dba16Accept4.SSK_NO);
                }
                if (result != TGZZZConstants.SUCCEESS)
                    return result;

            }
            catch (Exception e)
            {
                TGZZZLog.WriteLogFile_ERR(TGZZZConstants.LOG_ERROR, "", "", e);
                TGZZZLog.WriteEventLog_ERR(TGZZZConstants.EVENT_LOG_ERROR);

                return TGZZZConstants.ABNORMAL;
            }
            finally
            {
                TGZZZLog.EndLog();
            }

            return TGZZZConstants.SUCCEESS;
        }

        #region 1. Get Location Number
        public int GetLocationNo(string kiykNo, string dateTimeYMD, out string tgzSybNo)
        {
            tgzSybNo = string.Empty;

            StringBuilder sbSql1 = new StringBuilder();
            sbSql1.Append("select b.SYB_NO");
            sbSql1.Append(" from C_KIYK a");
            sbSql1.Append(" inner join C_KIYK_SYUSI b ");
            sbSql1.Append(" on b.KIYK_NO = a.KIYK_NO");
            sbSql1.Append(" where a.KIYK_NO = @p0");
            sbSql1.Append(" and a.TYK_YMD <= @p1");
            sbSql1.Append(" and a.TYS_YMD >= @p1");
            sbSql1.Append(" and a.KIYK_KNG_H <= @p1");
            sbSql1.Append(" and a.KIYK_SYRY_YMD >= @p1");
            sbSql1.Append(" and a.RNR_SKZY_FLG = '0'");
            sbSql1.Append(" and b.RNR_SKZY_FLG = '0'");
            sbSql1.Append(" order by b.KIYK_SYUSI_NO DESC");
            sbSql1.Append(" option(maxdop 1);");

            SqlParameter p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);
            SqlParameter p1 = new SqlParameter("@p1", System.Data.SqlDbType.Char);

            p0.Value = kiykNo;
            p1.Value = dateTimeYMD;

            var recs1 = context.Database.SqlQuery<string>
                (sbSql1.ToString()
                , p0, p1).ToList();
            if (recs1.Count == 0)
            {
                return TGZZZConstants.NOTFOUND;
            }
            tgzSybNo = recs1[0];
            return TGZZZConstants.SUCCEESS;
        }
        #endregion

        #region 2) Gets the maximum value of the sequence number. 
        #region 2-1) Get the location sequence number 
        public int GetLocationSequenceNo(string sybNo, out Dba16Accept2_1 dba16Accept2_1)
        {
            StringBuilder sbSql2_1 = new StringBuilder();
            sbSql2_1.Append("select SYB_NO");
            sbSql2_1.Append(" ,SYB_RNBN");
            sbSql2_1.Append(" ,TYK_YMD");
            sbSql2_1.Append(" from C_SYB");
            sbSql2_1.Append(" where SYB_NO = @p0");
            sbSql2_1.Append(" and TYK_YMD <= convert(varchar, getdate(), 112)");
            sbSql2_1.Append(" and TYS_YMD >= convert(varchar, getdate(), 112)");
            sbSql2_1.Append(" and RNR_SKZY_FLG = '0'");
            sbSql2_1.Append(" order by SYB_RNBN desc");
            sbSql2_1.Append(" option(maxdop 1);");
            SqlParameter p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);

            p0.Value = sybNo;

            var recs2_1 = context.Database.SqlQuery<Dba16Accept2_1>
                (sbSql2_1.ToString()
                , p0).ToList();
            dba16Accept2_1 = recs2_1.First();

            if (recs2_1.Count == 0)
            {
                return TGZZZConstants.NOTFOUND;
            }

            return TGZZZConstants.SUCCEESS;
        }
        #endregion

        #region 2-2)Obtain the Where Used supplement sequence number.
        public int GetWhereUsedSupplementSequenceNo(string sybNo, out Dba16Accept2_2 dba16Accept2_2)
        {
            StringBuilder sbSql2_2 = new StringBuilder();
            sbSql2_2.Append("select SYB_NO");
            sbSql2_2.Append(" , SYB_HSK_RNBN");
            sbSql2_2.Append(" , ZYUSY_NO");
            sbSql2_2.Append(" from C_SYB_HSK");
            sbSql2_2.Append(" where SYB_NO = @p0");
            sbSql2_2.Append(" and SISN_FLG = '1'");
            sbSql2_2.Append(" and RNR_SKZY_FLG = '0'");
            sbSql2_2.Append(" order by SYB_HSK_RNBN DESC");
            sbSql2_2.Append("  option(maxdop 1);");
            SqlParameter p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);
            p0.Value = sybNo;

            var recs2_2 = context.Database.SqlQuery<Dba16Accept2_2>
                (sbSql2_2.ToString()
                , p0).ToList();

            dba16Accept2_2 = recs2_2.First();

            if (recs2_2.Count == 0)
            {
                return TGZZZConstants.NOTFOUND;
            }
            return TGZZZConstants.SUCCEESS;
        }
        #endregion

        #region 2-3)Get the city code.
        public int GetCityCode(string zyusyNo, out Dba16Accept2_3 dba16Accept2_3)
        {
            StringBuilder sbSql2_3 = new StringBuilder();

            sbSql2_3.Append("select SKTS_CD");
            sbSql2_3.Append(" , GSK_CD");
            sbSql2_3.Append(" from C_ZYUSY");
            sbSql2_3.Append(" where ZYUSY_NO = @p0");
            sbSql2_3.Append(" and TYK_YMD <= convert(varchar, getdate(), 112)");
            sbSql2_3.Append(" and TYS_YMD >= convert(varchar, getdate(), 112)");
            sbSql2_3.Append(" and RNR_SKZY_FLG = '0'");
            sbSql2_3.Append(" order by ZYUSY_RNBN DESC");
            sbSql2_3.Append(" option(maxdop 1);");

            SqlParameter p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);
            p0.Value = zyusyNo;

            var recs2_3 = context.Database.SqlQuery<Dba16Accept2_3>
                (sbSql2_3.ToString()
                , p0).ToList();

            dba16Accept2_3 = recs2_3.First();

            if (recs2_3.Count == 0)
            {
                return TGZZZConstants.NOTFOUND;
            }

            return TGZZZConstants.SUCCEESS;
        }
        #endregion
        #endregion

        #region 3)Acquisition of information on near ENEDO stores
        public int GetInformationOnNearENEDOStores(Dba16Accept2_3 dba16Accept2_3, string dateTimeYMD, bool listFlag, out List<Dba16Accept4> dba16Accept3)
        {
            string tgzSgskNo = string.Empty;

            StringBuilder sbSql3 = new StringBuilder();
            sbSql3.Append("select a.SGSK_NO");
            sbSql3.Append(" from C_TEKY_MST a");
            sbSql3.Append(" inner join C_BRK_MST b");
            sbSql3.Append(" on a.BRK_CD = b.BRK_CD");
            sbSql3.Append(" where b.GSK_CD = @p0");
            sbSql3.Append(" and b.TYU_CD = @p1");
            sbSql3.Append(" and a.TYK_YMD <= @p2");
            sbSql3.Append(" and a.TYS_YMD > @p2");
            sbSql3.Append(" and b.TYK_YMD <= @p2");
            sbSql3.Append(" and b.TYS_YMD > @p2");
            sbSql3.Append(" and a.RNR_SKZY_FLG = '0'");
            sbSql3.Append(" and b.RNR_SKZY_FLG = '0'");

            SqlParameter p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);
            SqlParameter p1 = new SqlParameter("@p1", System.Data.SqlDbType.Char);
            SqlParameter p2 = new SqlParameter("@p2", System.Data.SqlDbType.Char);

            p0.Value = dba16Accept2_3.GSK_CD;
            p1.Value = dba16Accept2_3.SKTS_CD;
            p2.Value = dateTimeYMD;

            var recs3 = context.Database.SqlQuery<string>
                (sbSql3.ToString()
                , p0, p1, p2).ToList();

            if (recs3.Count == 0)
            {
                tgzSgskNo = "";
            }
            else
            {
                tgzSgskNo = recs3[0];
            }

            sbSql3 = new StringBuilder();

            sbSql3.Append("select a.SSK_NO");
            sbSql3.Append(" , a.TYK_YMD");
            sbSql3.Append(" , a.TYS_YMD");
            sbSql3.Append(" , a.MSM_KNG_H");
            sbSql3.Append(" , a.MSM_SYRY_YMD");
            sbSql3.Append(" , a.MISYP_ME");
            sbSql3.Append(" , a.ST_YU_MISYP_ME_KN");
            sbSql3.Append(" , a.GSK_CD");
            sbSql3.Append(" , a.HYUZYU_MISYP_YUBN_NO");
            sbSql3.Append(" , a.HYUZYU_MISYP_ZYUSY");
            sbSql3.Append(" , a.HYUZYU_MISYP_DWNO");
            sbSql3.Append(" , a.HYUZYU_MISYP_URL");
            sbSql3.Append(" , a.DKZ_TKTN_UM");
            sbSql3.Append(" , b.GSK_MS");
            sbSql3.Append(" from C_MISYP_KNR_MST a");
            sbSql3.Append(" inner join C_GSK_MST b");
            sbSql3.Append(" on b.GSK_CD = a.GSK_CD");
            sbSql3.Append(" where a.SSK_NO = @p0");
            sbSql3.Append(" and a.TYS_YMD > @p1");
            sbSql3.Append(" and a.MSM_KNG_H <= @p1");
            sbSql3.Append(" and a.MSM_SYRY_YMD >= @p1");
            sbSql3.Append(" and b.TYK_YMD <= @p1");
            sbSql3.Append(" and b.TYS_YMD > @p1");
            sbSql3.Append(" and a.RNR_SKZY_FLG= '0'");
            sbSql3.Append(" and b.RNR_SKZY_FLG = '0'");
            sbSql3.Append(" option(maxdop 1);");

            p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);
            p1 = new SqlParameter("@p1", System.Data.SqlDbType.Char);

            p0.Value = tgzSgskNo;
            p1.Value = dateTimeYMD;

            dba16Accept3 = context.Database.SqlQuery<Dba16Accept4>
                (sbSql3.ToString()
                , p0, p1).ToList();

            return TGZZZConstants.SUCCEESS;
        }
        #endregion

        #region 4)Acquisition of ENEDO Store Information List
        public int GetENEDOStoreInformationList(List<string> sskNoList, string dateTimeYMD, out List<Dba16Accept4> storeList)
        {
            // 4)ENEDOストア情報リストの取得

            StringBuilder sbSql4 = new StringBuilder();
            StringBuilder sbSql4List = new StringBuilder();

            sbSql4.Append("select a.SSK_NO");
            sbSql4.Append(", a.TYK_YMD");
            sbSql4.Append(", a.TYS_YMD");
            sbSql4.Append(", a.MSM_KNG_H");
            sbSql4.Append(", a.MSM_SYRY_YMD");
            sbSql4.Append(", a.MISYP_ME");
            sbSql4.Append(", a.ST_YU_MISYP_ME_KN");
            sbSql4.Append(", a.GSK_CD");
            sbSql4.Append(", a.HYUZYU_MISYP_YUBN_NO");
            sbSql4.Append(", a.HYUZYU_MISYP_ZYUSY");
            sbSql4.Append(", a.HYUZYU_MISYP_DWNO");
            sbSql4.Append(", a.HYUZYU_MISYP_URL");
            sbSql4.Append(", a.DKZ_TKTN_UM");
            sbSql4.Append(", b.GSK_MS");
            sbSql4.Append(" from C_MISYP_KNR_MST a");
            sbSql4.Append(" inner join C_GSK_MST b");
            sbSql4.Append(" on a.GSK_CD = B.GSK_CD");

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter($"@p0", dateTimeYMD));

            if (sskNoList.Count == 0)
            {
                // dba 16 Accept 3が空の場合はSSK_Noにフィルタを含めない
                sbSql4.Append(" where a.TYS_YMD > @p0");
            }
            else
            {
                sbSql4.Append(" where a.SSK_NO not in (");

                int index = 1;
                foreach (string sskNo in sskNoList)
                {
                    if (index > 1)
                    {
                        sbSql4List.Append(", ");
                    }
                    sbSql4List.Append($"@p{index}");
                    paramList.Add(new SqlParameter($"@p{index}", sskNo));
                    index++;
                }
                sbSql4.Append(sbSql4List);
                sbSql4.Append(")");
                sbSql4.Append(" and a.TYS_YMD > @p0");
            }

            sbSql4.Append(" and a.MSM_KNG_H <= @p0");
            sbSql4.Append(" and a.MSM_SYRY_YMD >= @p0");
            sbSql4.Append(" and b.TYK_YMD <= @p0");
            sbSql4.Append(" and b.TYS_YMD > @p0");
            sbSql4.Append(" and a.RNR_SKZY_FLG = '0'");
            sbSql4.Append(" and b.RNR_SKZY_FLG = '0'");
            sbSql4.Append(" order by b.GSK_HYUZ_ZYN asc, a.ST_YU_MISYP_ME_KN asc");
            sbSql4.Append(" option(maxdop 1);");

            storeList = context.Database.SqlQuery<Dba16Accept4>
                (sbSql4.ToString(), paramList.ToArray()).ToList();

            return TGZZZConstants.SUCCEESS;
        }
        #endregion
    }
}
