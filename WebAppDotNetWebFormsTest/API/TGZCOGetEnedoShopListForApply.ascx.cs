using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TestDBFirstCient;
using WebAppDotNetWebFormsTest.Utilities;

namespace WebAppDotNetWebFormsTest.API
{
    public partial class TGZCOGetEnedoShopListForApply : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void ObtainTheontractNumberFromTheCustomerNumber()
        {
            C_KIYK contract;
            string datenow = DateTime.Now.ToString("yyyyMMdd");
            List<TGZZZDba16.Dba16Accept4> myShopList;
            bool areaStorePresenceFlag;
            TGZZZDba16 tGZZZDba16 = new TGZZZDba16(new TohogasDataContext());
            int result;
            string customerNumber = Request["customerNumber"];
            if (customerNumber == null)
                return;
            using (var context = new TohogasDataContext())
            {
                // お客さま番号より契約番号を取得する。
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select a.* ");
                sbSql.Append(" from C_KIYK a");
                sbSql.Append(" inner join C_KIYK_SYUSI b");
                sbSql.Append(" on a.KIYK_NO = b.KIYK_NO");
                sbSql.Append(" where b.SYB_NO = @p0");
                sbSql.Append(" and a.SBS_SYBT = '01'");
                sbSql.Append(" and a.TYK_YMD <= @p1");
                sbSql.Append(" and a.TYS_YMD >= @p1 ");
                sbSql.Append(" and a.KIYK_KNG_H <=  @p1");
                sbSql.Append(" and a.KIYK_SYRY_YMD >=  @p1");
                sbSql.Append(" and a.RNR_SKZY_FLG = '0'");
                sbSql.Append(" and b.RNR_SKZY_FLG = '0'");
                sbSql.Append(" order by b.KIYK_NO desc");
                SqlParameter p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);
                SqlParameter p1 = new SqlParameter("@p1", System.Data.SqlDbType.Char);
                p0.Value = customerNumber;
                p1.Value = datenow;

                List<C_KIYK> rec = context.Database.SqlQuery<C_KIYK>
                    (sbSql.ToString()
                    , p0, p1).ToList();
                if (rec.Count == 0)
                {
                    // ★NotFound：汎用ログに出力、イベントログに出力。
                    TGZZZLog.WriteEventLog(TGZZZLog.EVENT_LOG_LEVEL_INFO, "Not Found");
                    TGZZZLog.WriteLogFile_INFO("Not Found", "", "");
                    return;
                }
                else
                {
                    contract = rec.First();
                }
                // ★データ取得できた場合、処理を継続する。

                // 契約番号よりENEDO店舗情報取得共通関数を呼び出す＜共通関数：ENEDO店舗情報取得（契約番号）＞
                result = tGZZZDba16.AcquisitionOfENEDOStoreInformation(contract.KIYK_NO, DateTime.Now, true, out myShopList, out areaStorePresenceFlag);
            }
        }
        public int ShowItemsRelatedToEnedoStores()
        {
            return TGZZZConstants.SUCCEESS;
        }
    }
}