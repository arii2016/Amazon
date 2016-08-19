using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Amazon
{
    //--------------------------------------------------------------
    #region 宣言
    /// <summary>
    /// 表示項目要素
    /// </summary>
    public enum EnumShowItem
    {
        /// <summary>有効・無効</summary>
        ENABLE = 0,
        /// <summary>注文番号</summary>
        ORDER_ID,
        /// <summary>購入者名</summary>
        NAME,
        /// <summary>製品名リスト</summary>
        PRODUCT_NAME_LIST,
        /// <summary>送り状種別</summary>
        INVOICE_CLASS,
        /// <summary>代引き金額</summary>
        COD_PAY,

        MAX
    }
    #endregion
    //--------------------------------------------------------------
    public partial class Form2 : Form
    {
        //--------------------------------------------------------------
        // 読み込みデーター
        List<string[]> m_listReadData = new List<string[]>();
        // ゆうプリR出力ファイル名
        string m_YpprFileName;
        // ヤマト出力ファイル名
        string m_YamatoFileName;
        // ゆうプリRデーター
        string[] m_strYpprData = new string[(int)EnumYpprItem.MAX];
        // やまとデーター
        string[] m_strYamatoData = new string[(int)EnumYamatoItem.MAX];

        // 注文データーリスト
        List<CAmazonData> m_listOrderData = new List<CAmazonData>();

        /// <summary>グリッド列の幅</summary>
        int[] m_iGridColWidth = new int[(int)EnumShowItem.MAX];
        /// <summary>表示項目要素文字列定数</summary>
        string[] SETTING_SHOW_ITEM_STR = new string[(int)EnumShowItem.MAX];
        //--------------------------------------------------------------
        public Form2()
        {
            InitializeComponent();

            m_iGridColWidth[(int)EnumShowItem.ENABLE] = 50;
            m_iGridColWidth[(int)EnumShowItem.ORDER_ID] = 170;
            m_iGridColWidth[(int)EnumShowItem.NAME] = 140;
            m_iGridColWidth[(int)EnumShowItem.PRODUCT_NAME_LIST] = 320;
            m_iGridColWidth[(int)EnumShowItem.INVOICE_CLASS] = 100;
            m_iGridColWidth[(int)EnumShowItem.COD_PAY] = 100;

            SETTING_SHOW_ITEM_STR[(int)EnumShowItem.ENABLE] = "有効";
            SETTING_SHOW_ITEM_STR[(int)EnumShowItem.ORDER_ID] = "注文番号";
            SETTING_SHOW_ITEM_STR[(int)EnumShowItem.NAME] = "購入者名";
            SETTING_SHOW_ITEM_STR[(int)EnumShowItem.PRODUCT_NAME_LIST] = "製品名";
            SETTING_SHOW_ITEM_STR[(int)EnumShowItem.INVOICE_CLASS] = "送り状種別";
            SETTING_SHOW_ITEM_STR[(int)EnumShowItem.COD_PAY] = "代引き金額";
        }
        //--------------------------------------------------------------
        public void ShowDlg(string strFileName)
        {
            if (ExecLabelPrint(strFileName) == false)
            {
                MessageBox.Show("読み込みエラー");
                return;
            }

            // ゆうプリR出力ファイル名
            m_YpprFileName = Path.GetDirectoryName(strFileName) + "\\" + "ゆうプリR.csv";
            // ヤマト出力ファイル名
            m_YamatoFileName = Path.GetDirectoryName(strFileName) + "\\" + "ヤマト.csv";
            
            // 画面表示
            ShowDialog();
        }
        //--------------------------------------------------------------
        // 処理実行
        public bool ExecLabelPrint(string strFileName)
        {
            // リストをクリア
            m_listReadData.Clear();

            // ヘッダー
            string[] strHeader;
            // ファイルを読み込む
            StreamReader clsSr;
            try
            {
                clsSr = new StreamReader(strFileName, System.Text.Encoding.GetEncoding("shift_jis"));
                try
                {
                    string strLine;

                    // 1行目読み込み
                    if ((strLine = clsSr.ReadLine()) == null)
                    {
                        throw new FormatException();
                    }
                    // ヘッダーの文字列を読み、種類を判断する
                    strHeader = strLine.Split('\t');
                    if (strHeader[0] != "order-id")
                    {
                        throw new FormatException();
                    }
                    // データーを読み込む                
                    while ((strLine = clsSr.ReadLine()) != null)
                    {
                        if (strLine == "")
                        {
                            break;
                        }
                        // 全てのデーターを格納
                        m_listReadData.Add(strLine.Split('\t'));
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    clsSr.Close();
                }
            }
            catch
            {
                return false;
            }

            // 注文データーリストに追加する
            int iIDPos = (int)EnumAmazonItem.ORDER_ID;

            m_listOrderData.Clear();

            while (m_listReadData.Count > 0)
            {
                CAmazonData clsAmazonData = new CAmazonData();

                clsAmazonData.strOrderId = m_listReadData[0][(int)EnumAmazonItem.ORDER_ID];
                clsAmazonData.strName = m_listReadData[0][(int)EnumAmazonItem.NAME];
                clsAmazonData.listProductName.Add("(" + m_listReadData[0][(int)EnumAmazonItem.QUANTITY_PURCHASED] + ") " + m_listReadData[0][(int)EnumAmazonItem.PRODUCT_NAME]);

                clsAmazonData.strPostNo = m_listReadData[0][(int)EnumAmazonItem.POST_NO];
                clsAmazonData.strState = m_listReadData[0][(int)EnumAmazonItem.STATE];
                clsAmazonData.strAddress1 = m_listReadData[0][(int)EnumAmazonItem.ADDRESS_1];
                clsAmazonData.strAddress2 = m_listReadData[0][(int)EnumAmazonItem.ADDRESS_2];
                clsAmazonData.strAddress3 = m_listReadData[0][(int)EnumAmazonItem.ADDRESS_3];
                clsAmazonData.strPhoneNo = m_listReadData[0][(int)EnumAmazonItem.PHONE_NO];

                // データ削除
                string strID = m_listReadData[0][iIDPos];
                for (int i = m_listReadData.Count - 1; i >= 0; i--)
                {
                    if (strID == m_listReadData[i][iIDPos])
                    {
                        clsAmazonData.listProductName.Add("(" + m_listReadData[i][(int)EnumAmazonItem.QUANTITY_PURCHASED] + ") " + m_listReadData[i][(int)EnumAmazonItem.PRODUCT_NAME]);
                        m_listReadData.RemoveAt(i);
                    }
                }
                // 最後の製品名リストは重複しているので削除する
                clsAmazonData.listProductName.RemoveAt(clsAmazonData.listProductName.Count - 1);
                // データ追加
                m_listOrderData.Add(clsAmazonData);
            }

            return true;
        }
        //--------------------------------------------------------------
        private void Form2_Load(object sender, EventArgs e)
        {
            #region データグリッドの初期設定
            // 左端の三角のプロパティ
            Dgv_Data.RowHeadersVisible = false;
            // ユーザーが新しい行を追加できないようにする
            Dgv_Data.AllowUserToAddRows = false;
            // ユーザーが削除できないようにする
            Dgv_Data.AllowUserToDeleteRows = false;
            // 列の幅は自動調整しない
            Dgv_Data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            // セル内で文字列を折り返す
            Dgv_Data.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            // ヘッダーとすべてのセルの内容に合わせて、行の高さを自動調整する
            Dgv_Data.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // 列の幅をユーザーが変更できないようにする
            Dgv_Data.AllowUserToResizeColumns = false;
            // 行の高さをユーザーが変更できないようにする
            Dgv_Data.AllowUserToResizeRows = false;
            // セル、行、列が複数選択されないようにする
            Dgv_Data.MultiSelect = false;
            //全ての列の背景色を変更する
            Dgv_Data.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            //奇数行の色を変更する
            Dgv_Data.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            #endregion
            #region データグリッドの列設定
            // 列の設定
            Dgv_Data.Columns.Clear();
            Dgv_Data.Columns.Add(new DataGridViewCheckBoxColumn() { HeaderText = SETTING_SHOW_ITEM_STR[(int)EnumShowItem.ENABLE] });
            Dgv_Data.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = SETTING_SHOW_ITEM_STR[(int)EnumShowItem.ORDER_ID] });
            Dgv_Data.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = SETTING_SHOW_ITEM_STR[(int)EnumShowItem.NAME] });
            Dgv_Data.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = SETTING_SHOW_ITEM_STR[(int)EnumShowItem.PRODUCT_NAME_LIST] });
            Dgv_Data.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = SETTING_SHOW_ITEM_STR[(int)EnumShowItem.INVOICE_CLASS] });
            Dgv_Data.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = SETTING_SHOW_ITEM_STR[(int)EnumShowItem.COD_PAY] });

            for (int i = 0; i < (int)EnumShowItem.MAX; i++)
            {
                // ソートを全て無効
                Dgv_Data.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                // 文字列を左に表示
                Dgv_Data.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Dgv_Data.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                // 幅を設定
                Dgv_Data.Columns[i].Width = m_iGridColWidth[i];
            }
            // 列の読み取り専用を設定
            Dgv_Data.Columns[(int)EnumShowItem.ORDER_ID].ReadOnly = true;
            Dgv_Data.Columns[(int)EnumShowItem.NAME].ReadOnly = true;
            Dgv_Data.Columns[(int)EnumShowItem.PRODUCT_NAME_LIST].ReadOnly = true;
            #endregion

            // データを全てクリア
            Dgv_Data.RowCount = 0;
            // 行数設定
            Dgv_Data.RowCount = m_listOrderData.Count;

            for (int i = 0; i < m_listOrderData.Count; i++)
            {
                // 有効・無効の設定
                Dgv_Data.Rows[i].Cells[(int)EnumShowItem.ENABLE].Value = m_listOrderData[i].bEnable;
                // 注文番号の設定
                Dgv_Data.Rows[i].Cells[(int)EnumShowItem.ORDER_ID].Value = m_listOrderData[i].strOrderId;
                // 購入者名の設定
                Dgv_Data.Rows[i].Cells[(int)EnumShowItem.NAME].Value = m_listOrderData[i].strName;
                // 製品名リストの設定
                for (int j = 0; j < m_listOrderData[i].listProductName.Count; j++)
                {
                    string strProductName = m_listOrderData[i].listProductName[j];
                    if (strProductName.Length > 25)
                    {
                        strProductName = strProductName.Substring(0, 25);
                    }

                    Dgv_Data.Rows[i].Cells[(int)EnumShowItem.PRODUCT_NAME_LIST].Value += strProductName;
                    if (j != m_listOrderData[i].listProductName.Count - 1)
                    {
                        Dgv_Data.Rows[i].Cells[(int)EnumShowItem.PRODUCT_NAME_LIST].Value += "\n";
                    }
                }

                // 送り状種別の設定
                DataGridViewComboBoxCell Cel = new DataGridViewComboBoxCell();
                Cel.Items.AddRange(CDefine.SETTING_INVOICE_CLASS_STR);
                Dgv_Data.Rows[i].Cells[(int)EnumShowItem.INVOICE_CLASS] = Cel;
                ((DataGridViewComboBoxCell)Dgv_Data.Rows[i].Cells[(int)EnumShowItem.INVOICE_CLASS]).Value = ((DataGridViewComboBoxCell)Dgv_Data.Rows[i].Cells[(int)EnumShowItem.INVOICE_CLASS]).Items[(int)m_listOrderData[i].enInvoiceClass];

                // 代引き金額の設定
                Dgv_Data.Rows[i].Cells[(int)EnumShowItem.COD_PAY].Value = m_listOrderData[i].iCodPay.ToString();
            }
        
        }
        //--------------------------------------------------------------
        private void Dgv_Data_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        //--------------------------------------------------------------
        private void Dgv_Data_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].HeaderText == SETTING_SHOW_ITEM_STR[(int)EnumShowItem.INVOICE_CLASS])
            {
                SendKeys.Send("{F4}");
            }
        }
        //--------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            // データの取得
            for (int i = 0; i < m_listOrderData.Count; i++)
            {
                // 有効・無効の取得
                m_listOrderData[i].bEnable = (bool)Dgv_Data.Rows[i].Cells[(int)EnumShowItem.ENABLE].Value;
                // 送り状種別の設定
                for (int j = 0; j < ((DataGridViewComboBoxCell)Dgv_Data.Rows[i].Cells[(int)EnumShowItem.INVOICE_CLASS]).Items.Count; j++)
                {
                    if (((DataGridViewComboBoxCell)Dgv_Data.Rows[i].Cells[(int)EnumShowItem.INVOICE_CLASS]).Value.ToString() == ((DataGridViewComboBoxCell)Dgv_Data.Rows[i].Cells[(int)EnumShowItem.INVOICE_CLASS]).Items[j].ToString())
                    {
                        m_listOrderData[i].enInvoiceClass = (EnumInvoiceClass)j;
                    }
                }
                try
                {
                    m_listOrderData[i].iCodPay = int.Parse(((string)Dgv_Data.Rows[i].Cells[(int)EnumShowItem.COD_PAY].Value));
                }
                catch
                {
                    MessageBox.Show("代引き金額入力エラー");
                    return;
                }
            }

            // CSV出力
            for (int i = 0; i < m_listOrderData.Count; i++)
            {
                if (m_listOrderData[i].bEnable == false)
                {
                    continue;
                }

                if (m_listOrderData[i].enInvoiceClass == EnumInvoiceClass.POST_PACKET)
                {
                    // ゆうプリR
                    LoadYpprData(m_listOrderData[i]);
                    if (WriteYpprData() == false)
                    {
                        MessageBox.Show("出力エラー");
                        return;
                    }
                }
                else
                {
                    // ヤマト
                    LoadYamatoData(m_listOrderData[i]);
                    if (WriteYamatoData() == false)
                    {
                        MessageBox.Show("出力エラー");
                        return;
                    }
                }
            }

            Close();        
        }
        //--------------------------------------------------------------
        /// <summary>
        /// ゆうプリR読み込み
        /// </summary>
        public bool LoadYpprData(CAmazonData clsData)
        {
            for (int i = 0; i < m_strYpprData.Length; i++)
            {
                m_strYpprData[i] = "";
            }

            // お客様側管理番号
            m_strYpprData[(int)EnumYpprItem.USER_ID] = "";
            // 発送予定日
            m_strYpprData[(int)EnumYpprItem.SHIPPING_SCHEDULE_DATE] = DateTime.Now.ToString("yyyyMMdd");
            // 発送予定時間
            m_strYpprData[(int)EnumYpprItem.SHIPPING_SCHEDULE_TIME] = "02";
            // 郵便種別
            m_strYpprData[(int)EnumYpprItem.POST_CLASS] = "9";
            // 支払元
            m_strYpprData[(int)EnumYpprItem.PAYMENT_SOURCE] = "0";
            // 送り状種別
            m_strYpprData[(int)EnumYpprItem.INVOICE_CLASS] = "1800800001";
            // お届け先郵便番号
            m_strYpprData[(int)EnumYpprItem.TRANSPORT_POST_NO] = clsData.strPostNo;

            // お届け先住所
            m_strYpprData[(int)EnumYpprItem.TRANSPORT_ADDRESS_1] = clsData.strState;
            m_strYpprData[(int)EnumYpprItem.TRANSPORT_ADDRESS_2] = clsData.strAddress1;
            m_strYpprData[(int)EnumYpprItem.TRANSPORT_ADDRESS_3] = clsData.strAddress2;
            if (clsData.strAddress3 == "")
            {
                m_strYpprData[(int)EnumYpprItem.TRANSPORT_NAME_1] = clsData.strName;
            }
            else
            {
                m_strYpprData[(int)EnumYpprItem.TRANSPORT_NAME_1] = clsData.strAddress3;
                m_strYpprData[(int)EnumYpprItem.TRANSPORT_NAME_2] = clsData.strName;
            }
            // お届け先敬称
            m_strYpprData[(int)EnumYpprItem.TRANSPORT_TITLE] = "0";
            // お届け先電話番号
            m_strYpprData[(int)EnumYpprItem.TRANSPORT_TEL] = (clsData.strPhoneNo.Replace("-", "")).Replace("‐", "");
            // お届け先メール
            m_strYpprData[(int)EnumYpprItem.TRANSPORT_MAIL] = "";
            // 発送元郵便番号
            m_strYpprData[(int)EnumYpprItem.ORIGIN_POST_NO] = "4000306";
            // 発送元住所1
            m_strYpprData[(int)EnumYpprItem.ORIGIN_ADDRESS_1] = "山梨県";
            // 発送元住所2
            m_strYpprData[(int)EnumYpprItem.ORIGIN_ADDRESS_2] = "南アルプス市小笠原";
            // 発送元住所3
            m_strYpprData[(int)EnumYpprItem.ORIGIN_ADDRESS_3] = "1589-1";
            // 発送元名称1
            m_strYpprData[(int)EnumYpprItem.ORIGIN_NAME_1] = "ASSShop";
            // 発送元名称2
            m_strYpprData[(int)EnumYpprItem.ORIGIN_NAME_2] = "";
            // 発送元敬称
            m_strYpprData[(int)EnumYpprItem.ORIGIN_TITLE] = "0";
            // 発送元電話番号
            m_strYpprData[(int)EnumYpprItem.ORIGIN_TEL] = "05037867989";
            // 発送元メール
            m_strYpprData[(int)EnumYpprItem.ORIGIN_MAIL] = "shop@assystem.jp";
            // こわれもの
            m_strYpprData[(int)EnumYpprItem.BREAKABLE_FLG] = "1";
            // 逆さま厳禁
            m_strYpprData[(int)EnumYpprItem.WAY_UP_FLG] = "1";
            // 下積み厳禁
            m_strYpprData[(int)EnumYpprItem.DO_NOT_STACK_FLG] = "1";
            // 厚さ
            m_strYpprData[(int)EnumYpprItem.THICKNESS] = "10";
            // お届け日
            m_strYpprData[(int)EnumYpprItem.DELIBERY_DATE] = "";
            // お届け時間
            m_strYpprData[(int)EnumYpprItem.DELIBERY_TIME] = "00";
            // フリー項目
            m_strYpprData[(int)EnumYpprItem.FREE_ITEM] = "2" + clsData.strOrderId;
            // 代引金額
            m_strYpprData[(int)EnumYpprItem.COD_PAY] = "";
            // 代引消費税
            m_strYpprData[(int)EnumYpprItem.COD_TAX] = "";
            // 商品名設定
            m_strYpprData[(int)EnumYpprItem.PRODUCT_NAME] = clsData.listProductName[0];
            if (m_strYpprData[(int)EnumYpprItem.PRODUCT_NAME].Length > 25)
            {
                m_strYpprData[(int)EnumYpprItem.PRODUCT_NAME] = m_strYpprData[(int)EnumYpprItem.PRODUCT_NAME].Substring(0, 25);
            }

            return true;
        }
        //--------------------------------------------------------------
        /// <summary>
        /// ゆうプリR 書き込む
        /// </summary>
        public bool WriteYpprData()
        {
            StreamWriter clsSw;
            try
            {
                clsSw = new StreamWriter(m_YpprFileName, true, Encoding.GetEncoding("Shift_JIS"));
            }
            catch
            {
                return false;
            }

            for (int i = 0; i < (int)EnumYpprItem.MAX; i++)
            {
                clsSw.Write("{0},", m_strYpprData[i]);
            }
            clsSw.Write("\n");


            clsSw.Flush();
            clsSw.Close();

            return true;
        }
        //--------------------------------------------------------------
        /// <summary>
        /// ヤマト読み込み
        /// </summary>
        public bool LoadYamatoData(CAmazonData clsData)
        {
            for (int i = 0; i < m_strYamatoData.Length; i++)
            {
                m_strYamatoData[i] = "";
            }

            // お客様側管理番号
            m_strYamatoData[(int)EnumYamatoItem.USER_ID] = "2" + clsData.strOrderId;
            // 送り状種別
            if (clsData.enInvoiceClass == EnumInvoiceClass.CASH_ON_DELIVERY)
            {
                m_strYamatoData[(int)EnumYamatoItem.INVOICE_CLASS] = "2";
            }
            else if (clsData.enInvoiceClass == EnumInvoiceClass.DELIVERY)
            {
                m_strYamatoData[(int)EnumYamatoItem.INVOICE_CLASS] = "0";
            }
            else
            {
                m_strYamatoData[(int)EnumYamatoItem.INVOICE_CLASS] = "3";
            }
            // 発送予定日
            m_strYamatoData[(int)EnumYamatoItem.SHIPPING_SCHEDULE_TIME] = DateTime.Now.ToString("yyyy/MM/dd");
            // お届け日
            m_strYamatoData[(int)EnumYamatoItem.DELIBERY_DATE] = "";
            // お届け時間
            m_strYamatoData[(int)EnumYamatoItem.DELIBERY_TIME] = "";
            // お届け先電話番号
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_TEL] = (clsData.strPhoneNo.Replace("-", "")).Replace("‐", "");
            // お届け先郵便番号
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_POST_NO] = clsData.strPostNo;
            // お届け先住所
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_ADDRESS_1] = clsData.strState + clsData.strAddress1;
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_ADDRESS_2] = (clsData.strAddress2.Replace(" ", "")).Replace("　", "");
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_COMPANY_1] = clsData.strAddress3;
            // お届け先会社・部門名2
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_COMPANY_2] = "";
            // お届け先名
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_NAME] = clsData.strName;
            // お届け先敬称
            m_strYamatoData[(int)EnumYamatoItem.TRANSPORT_TITLE] = "様";
            // ご依頼主コード
            m_strYamatoData[(int)EnumYamatoItem.ORIGIN_CODE] = "";
            // ご依頼主電話番号
            m_strYamatoData[(int)EnumYamatoItem.ORIGIN_TEL] = "050-3786-7989";
            // ご依頼主郵便番号
            m_strYamatoData[(int)EnumYamatoItem.ORIGIN_POST_NO] = "4000306";
            // ご依頼主住所1
            m_strYamatoData[(int)EnumYamatoItem.ORIGIN_ADDRESS_1] = "山梨県南アルプス市小笠原1589-1";
            // ご依頼主住所2
            m_strYamatoData[(int)EnumYamatoItem.ORIGIN_ADDRESS_2] = "";
            // ご依頼主名
            m_strYamatoData[(int)EnumYamatoItem.ORIGIN_NAME] = "ASShop";
            // 品名コード1
            m_strYamatoData[(int)EnumYamatoItem.PRODUCT_NAME_CODE_1] = "";
            // 品名1
            m_strYamatoData[(int)EnumYamatoItem.PRODUCT_NAME_1] = clsData.listProductName[0];
            if (m_strYamatoData[(int)EnumYamatoItem.PRODUCT_NAME_1].Length > 25)
            {
                m_strYamatoData[(int)EnumYamatoItem.PRODUCT_NAME_1] = m_strYamatoData[(int)EnumYamatoItem.PRODUCT_NAME_1].Substring(0, 25);
            }
            // 品名コード2
            m_strYamatoData[(int)EnumYamatoItem.PRODUCT_NAME_CODE_2] = "";
            // 品名2
            m_strYamatoData[(int)EnumYamatoItem.PRODUCT_NAME_2] = "";
            // 荷扱い1
            m_strYamatoData[(int)EnumYamatoItem.FREIGHT_HANDLING_1] = "精密機器";
            // 荷扱い2
            m_strYamatoData[(int)EnumYamatoItem.FREIGHT_HANDLING_2] = "下積厳禁";
            // 記事
            m_strYamatoData[(int)EnumYamatoItem.ARTICLE] = "";
            // 代引金額
            if (clsData.enInvoiceClass == EnumInvoiceClass.CASH_ON_DELIVERY)
            {
                m_strYamatoData[(int)EnumYamatoItem.COD_PAY] = clsData.iCodPay.ToString();
            }
            else
            {
                m_strYamatoData[(int)EnumYamatoItem.COD_PAY] = "";
            }
            // 代引消費税
            m_strYamatoData[(int)EnumYamatoItem.COD_TAX] = "";
            // 発行枚数
            m_strYamatoData[(int)EnumYamatoItem.POST_NUM] = "1";
            // 個数口枠の印字
            m_strYamatoData[(int)EnumYamatoItem.NUMBER_FRAME] = "3";
            // ご請求先顧客コード
            m_strYamatoData[(int)EnumYamatoItem.BILLING_CODE] = "05037867989";
            // 運賃管理番号
            m_strYamatoData[(int)EnumYamatoItem.FARE_NO] = "01";
            // 空白
            m_strYamatoData[(int)EnumYamatoItem.NULL_1] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_2] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_3] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_4] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_5] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_6] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_7] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_8] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_9] = "";
            m_strYamatoData[(int)EnumYamatoItem.NULL_10] = "";

            return true;
        }
        //--------------------------------------------------------------
        /// <summary>
        /// ヤマト書き込む
        /// </summary>
        public bool WriteYamatoData()
        {
            bool bHeaderFlg = false;

            // ファイルが存在しない場合には、ヘッダーを追加
            if (System.IO.File.Exists(m_YamatoFileName) == false)
            {
                bHeaderFlg = true;
            }
            
            StreamWriter clsSw;
            try
            {
                clsSw = new StreamWriter(m_YamatoFileName, true, Encoding.GetEncoding("Shift_JIS"));
            }
            catch
            {
                return false;
            }

            // データ書き込み
            if (bHeaderFlg)
            {
                clsSw.Write("ヤマト運輸データ\n");
            }
            for (int i = 0; i < (int)EnumYamatoItem.MAX; i++)
            {
                clsSw.Write("{0},", m_strYamatoData[i]);
            }
            clsSw.Write("\n");


            clsSw.Flush();
            clsSw.Close();

            return true;
        }
        //--------------------------------------------------------------
    }
}
