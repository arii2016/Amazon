using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon
{
    //--------------------------------------------------------------
    #region 列挙型
    /// <summary>
    /// 送り状種別要素
    /// </summary>
    public enum EnumInvoiceClass
    {
        /// <summary>DM便</summary>
        DM = 0,
        /// <summary>ゆうパケット</summary>
        POST_PACKET,
        /// <summary>宅急便</summary>
        DELIVERY,
        /// <summary>代引き</summary>
        CASH_ON_DELIVERY,

        MAX
    }
    /// <summary>
    /// amazon要素
    /// </summary>
    public enum EnumAmazonItem
    {
        /// <summary>注文番号</summary>
        ORDER_ID = 0,
        /// <summary>名前</summary>
        NAME = 16,
        /// <summary>郵便番号</summary>
        POST_NO = 22,
        /// <summary>県</summary>
        STATE = 21,
        /// <summary>住所1</summary>
        ADDRESS_1 = 17,
        /// <summary>住所2</summary>
        ADDRESS_2 = 18,
        /// <summary>住所3</summary>
        ADDRESS_3 = 19,
        /// <summary>電話番号</summary>
        PHONE_NO = 9,
        /// <summary>商品ID</summary>
        PRODUCT_ID = 10,
        /// <summary>商品名</summary>
        PRODUCT_NAME = 11,
        /// <summary>注文商品数</summary>
        QUANTITY_PURCHASED = 12,

        MAX
    }
    /// <summary>
    /// ゆうプリR要素
    /// </summary>
    public enum EnumYpprItem
    {
        /// <summary>お客様側管理番号</summary>
        USER_ID = 0,
        /// <summary>発送予定日</summary>
        SHIPPING_SCHEDULE_DATE,
        /// <summary>発送予定時間</summary>
        SHIPPING_SCHEDULE_TIME,
        /// <summary>郵便種別</summary>
        POST_CLASS,
        /// <summary>支払元</summary>
        PAYMENT_SOURCE,
        /// <summary>送り状種別</summary>
        INVOICE_CLASS,
        /// <summary>お届け先郵便番号</summary>
        TRANSPORT_POST_NO,
        /// <summary>お届け先住所1</summary>
        TRANSPORT_ADDRESS_1,
        /// <summary>お届け先住所2</summary>
        TRANSPORT_ADDRESS_2,
        /// <summary>お届け先住所3</summary>
        TRANSPORT_ADDRESS_3,
        /// <summary>お届け先名称1</summary>
        TRANSPORT_NAME_1,
        /// <summary>お届け先名称2</summary>
        TRANSPORT_NAME_2,
        /// <summary>お届け先敬称</summary>
        TRANSPORT_TITLE,
        /// <summary>お届け先電話番号</summary>
        TRANSPORT_TEL,
        /// <summary>お届け先メール</summary>
        TRANSPORT_MAIL,
        /// <summary>発送元郵便番号</summary>
        ORIGIN_POST_NO,
        /// <summary>発送元住所1</summary>
        ORIGIN_ADDRESS_1,
        /// <summary>発送元住所2</summary>
        ORIGIN_ADDRESS_2,
        /// <summary>発送元住所3</summary>
        ORIGIN_ADDRESS_3,
        /// <summary>発送元名称1</summary>
        ORIGIN_NAME_1,
        /// <summary>発送元名称2</summary>
        ORIGIN_NAME_2,
        /// <summary>発送元敬称</summary>
        ORIGIN_TITLE,
        /// <summary>発送元電話番号</summary>
        ORIGIN_TEL,
        /// <summary>発送元メール</summary>
        ORIGIN_MAIL,
        /// <summary>こわれもの</summary>
        BREAKABLE_FLG,
        /// <summary>逆さま厳禁</summary>
        WAY_UP_FLG,
        /// <summary>下積み厳禁</summary>
        DO_NOT_STACK_FLG,
        /// <summary>厚さ</summary>
        THICKNESS,
        /// <summary>お届け日</summary>
        DELIBERY_DATE,
        /// <summary>お届け時間</summary>
        DELIBERY_TIME,
        /// <summary>フリー項目</summary>
        FREE_ITEM,
        /// <summary>代引金額</summary>
        COD_PAY,
        /// <summary>代引消費税</summary>
        COD_TAX,
        /// <summary>商品名</summary>
        PRODUCT_NAME,

        MAX
    }
    /// <summary>
    /// ヤマト要素
    /// </summary>
    public enum EnumYamatoItem
    {
        /// <summary>お客様側管理番号</summary>
        USER_ID = 0,
        /// <summary>送り状種別</summary>
        INVOICE_CLASS,
        /// <summary>NULL</summary>
        NULL_1,
        /// <summary>NULL</summary>
        NULL_2,
        /// <summary>発送予定時間</summary>
        SHIPPING_SCHEDULE_TIME,
        /// <summary>配達指定日</summary>
        DELIBERY_DATE,
        /// <summary>配達時間帯区分</summary>
        DELIBERY_TIME,
        /// <summary>NULL</summary>
        NULL_3,
        /// <summary>お届け先電話番号</summary>
        TRANSPORT_TEL,
        /// <summary>NULL</summary>
        NULL_4,
        /// <summary>お届け先郵便番号</summary>
        TRANSPORT_POST_NO,
        /// <summary>お届け先住所1</summary>
        TRANSPORT_ADDRESS_1,
        /// <summary>お届け先住所2</summary>
        TRANSPORT_ADDRESS_2,
        /// <summary>お届け先会社・部門名1</summary>
        TRANSPORT_COMPANY_1,
        /// <summary>お届け先会社・部門名2</summary>
        TRANSPORT_COMPANY_2,
        /// <summary>お届け先名</summary>
        TRANSPORT_NAME,
        /// <summary>NULL</summary>
        NULL_5,
        /// <summary>お届け先敬称</summary>
        TRANSPORT_TITLE,
        /// <summary>ご依頼主コード</summary>
        ORIGIN_CODE,
        /// <summary>ご依頼主電話番号</summary>
        ORIGIN_TEL,
        /// <summary>NULL</summary>
        NULL_6,
        /// <summary>ご依頼主郵便番号</summary>
        ORIGIN_POST_NO,
        /// <summary>ご依頼主住所1</summary>
        ORIGIN_ADDRESS_1,
        /// <summary>ご依頼主住所2</summary>
        ORIGIN_ADDRESS_2,
        /// <summary>ご依頼主名</summary>
        ORIGIN_NAME,
        /// <summary>NULL</summary>
        NULL_7,
        /// <summary>品名コード1</summary>
        PRODUCT_NAME_CODE_1,
        /// <summary>品名1</summary>
        PRODUCT_NAME_1,
        /// <summary>品名コード2</summary>
        PRODUCT_NAME_CODE_2,
        /// <summary>品名2</summary>
        PRODUCT_NAME_2,
        /// <summary>荷扱い1</summary>
        FREIGHT_HANDLING_1,
        /// <summary>荷扱い2</summary>
        FREIGHT_HANDLING_2,
        /// <summary>記事</summary>
        ARTICLE,
        /// <summary>代引金額</summary>
        COD_PAY,
        /// <summary>代引消費税</summary>
        COD_TAX,
        /// <summary>NULL</summary>
        NULL_8,
        /// <summary>NULL</summary>
        NULL_9,
        /// <summary>発行枚数</summary>
        POST_NUM,
        /// <summary>個数口枠の印字</summary>
        NUMBER_FRAME,
        /// <summary>ご請求先顧客コード</summary>
        BILLING_CODE,
        /// <summary>NULL</summary>
        NULL_10,
        /// <summary>運賃管理番号</summary>
        FARE_NO,

        MAX
    }
    #endregion
    //--------------------------------------------------------------
    #region 構造体
    /// <summary>
    /// amazonデータクラス
    /// </summary>
    public class CAmazonData
    {
        //--------------------------------------------------------------
        /// <summary>有効無効Flg</summary>
        public bool bEnable;
        /// <summary>注文番号</summary>
        public string strOrderId;
        /// <summary>購入者名</summary>
        public string strName;
        /// <summary>製品名リスト</summary>
        public List<string> listProductName = new List<string>();
        /// <summary>送り状種別</summary>
        public EnumInvoiceClass enInvoiceClass;
        /// <summary>代引き金額</summary>
        public int iCodPay;

        /// <summary>郵便番号</summary>
        public string strPostNo;
        /// <summary>県</summary>
        public string strState;
        /// <summary>住所1</summary>
        public string strAddress1;
        /// <summary>住所2</summary>
        public string strAddress2;
        /// <summary>住所3</summary>
        public string strAddress3;
        /// <summary>電話番号</summary>
        public string strPhoneNo;
        
        //--------------------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CAmazonData()
        {
            listProductName = new List<string>();
            Init();
        }
        /// <summary>
        /// 初期化関数
        /// </summary>
        public void Init()
        {
            bEnable = true;
            strOrderId = "";
            strName = "";
            listProductName.Clear();
            enInvoiceClass = EnumInvoiceClass.DM;
            iCodPay = 0;
            strPostNo = "";
            strState = "";
            strAddress1 = "";
            strAddress2 = "";
            strAddress3 = "";
            strPhoneNo = "";
        }
    }
    #endregion
    //--------------------------------------------------------------
    /// <summary>
    /// 定数宣言クラス
    /// </summary>
    class CDefine
    {
        //--------------------------------------------------------------
        #region 定数
        #endregion
        //--------------------------------------------------------------
        #region 文字列定数
        /// <summary>送り状種別文字定数</summary>
        public static string[] SETTING_INVOICE_CLASS_STR = new string[(int)EnumInvoiceClass.MAX];
        #endregion
        //--------------------------------------------------------------
        /// <summary>コンストラクタ</summary>
        static CDefine()
        {
            SETTING_INVOICE_CLASS_STR[(int)EnumInvoiceClass.DM] = "DM便";
            SETTING_INVOICE_CLASS_STR[(int)EnumInvoiceClass.POST_PACKET] = "ゆうパケット";
            SETTING_INVOICE_CLASS_STR[(int)EnumInvoiceClass.DELIVERY] = "宅急便";
            SETTING_INVOICE_CLASS_STR[(int)EnumInvoiceClass.CASH_ON_DELIVERY] = "代引き";
        }
        //--------------------------------------------------------------
    }
    //--------------------------------------------------------------
}
