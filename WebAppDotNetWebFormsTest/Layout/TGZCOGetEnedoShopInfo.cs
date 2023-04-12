using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using TestDBFirstCient;
using TestDBFirstCient.Utilities;
using static TestDBFirstCient.Utilities.TGZZZDba17;

namespace WebAppDotNetWebFormsTest.API
{
    public partial class TGZCOGetEnedoShopInfo : System.Web.UI.UserControl
    {
        TGZZZDba17 tGZZZDba17;

        protected void Page_Load(object sender, EventArgs e)
        {
            string loaded = string.Empty;
            TGZZZLog.StartLog();
            string address = string.Empty;
            string phonenumber = null;
            string servicename = "-";
            string proprietaryservice = "-";
            int result;
            string organizationNumber;
            loaded = Request["loaded"];
            if (loaded == null) 
                return;
            using (var context = new TohogasDataContext())
            {
                tGZZZDba17 = new TGZZZDba17(context);
                organizationNumber = Request["organizationnumber"];
                try
                {
                    result = CheckTheFormatOfTheOrganizationNumber(organizationNumber);
 
                    List<Dba17Accept1> storeInformationList;
                    result = tGZZZDba17.GetENEDOStoreInformationWithOrganizationNumber(organizationNumber, DateTime.Now, false, out storeInformationList);
                    if (result==0 && storeInformationList.Count>0)
                    {
                        // Could not find Table: (Member Site) Code Definition M
                        address = "〒" + storeInformationList[0].HYUZYU_MISYP_YUBN_NO + " " + storeInformationList[0].HYUZYU_MISYP_ZYUSY;
                        phonenumber = storeInformationList[0].HYUZYU_MISYP_DWNO;
                        servicename = "(Member site) Code definition master. Display name" + storeInformationList[0].MISYP_ME;
                        proprietaryservice = storeInformationList[0].DKZ_TKTN_UM;
                    }
                }
                catch (Exception ex)
                {
                    // ★DBアクセス異常：汎用ログに出力、イベントログに出力。
                    TGZZZLog.WriteLogFile_ERR(TGZZZConstants.LOG_ERROR, "", "", ex);
                    TGZZZLog.WriteEventLog_ERR(TGZZZConstants.EVENT_LOG_ERROR);
                }
                finally
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/json; charset=utf-8";

                    string jsonout = this.getJson(address, phonenumber, servicename, proprietaryservice);

                    this.Response.Output.Write(jsonout);

                    // 汎用_終了ログを出力する。
                    TGZZZLog.EndLog();
                    this.Response.End();
                }
            }
        }

        public int CheckTheFormatOfTheOrganizationNumber(string organizationNumber)
        {
            if (organizationNumber == null || TGZZZCom08.CheckStringType(organizationNumber, TGZZZConstants.CHECK_STRING_TYPE_HALFALPHNUM) != 0 
                || !(organizationNumber.Trim().Length == 3 || organizationNumber.Trim().Length == 7))
                return TGZZZConstants.ABNORMAL;

            return TGZZZConstants.SUCCEESS;
        }

        private string getJson(string address, string phonenumber, string servicename, string proprietaryservice)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            // yubnNo1
            json.Append("\"address\":");
            json.Append("\"").Append(address).Append("\",");
            json.Append("\"phonenumber\":");
            json.Append("\"").Append(phonenumber).Append("\",");
            json.Append("\"servicename\":");
            json.Append("\"").Append(servicename).Append("\",");
            json.Append("\"proprietaryservice\":");
            json.Append("\"").Append(proprietaryservice).Append("\"");
            json.Append("}");
            return json.ToString();
        }
        
    }
}