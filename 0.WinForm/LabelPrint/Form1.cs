using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabelPrint
{
    public partial class Form1 : Form
    {
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;


        public Form1()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case WM_DEVICECHANGE:
                            break;
                        case DBT_DEVICEARRIVAL://U盘插入
                            DriveInfo[] s = DriveInfo.GetDrives();
                            foreach (DriveInfo drive in s)
                            {
                                if (drive.DriveType == DriveType.Removable)
                                {
                                    MessageBox.Show("U盘已插入，盘符为:" + drive.Name.ToString());
                                    break;
                                }
                            }
                            break;
                        case DBT_CONFIGCHANGECANCELED:
                            break;
                        case DBT_CONFIGCHANGED:
                            break;
                        case DBT_CUSTOMEVENT:
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            break;
                        case DBT_DEVICEQUERYREMOVEFAILED:
                            break;
                        case DBT_DEVICEREMOVECOMPLETE: //U盘卸载
                            break;
                        case DBT_DEVICEREMOVEPENDING:
                            break;
                        case DBT_DEVICETYPESPECIFIC:
                            break;
                        case DBT_DEVNODES_CHANGED:
                            break;
                        case DBT_QUERYCHANGECONFIG:
                            break;
                        case DBT_USERDEFINED:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.WndProc(ref m);
        }

        //private void BackgroundWorkerPrint_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker = sender as BackgroundWorker;

        //    int i = 0, nextRemainder = 0, count = this._listBarcodeData.Count;
        //    bool flag = true;
        //    float pageWidth, pageHeight;
        //    int dpiX, dpiY, perPaperFactor;
        //    string reportPath = string.Format(@"{0}/Reports/{1}", Application.StartupPath, this._modelType);
        //    PrintLog printLog = new PrintLog() { Operater = LoginName };
        //    PrinterSettings printerSettings = new PrinterSettings() { PrinterName = PrintParam, Copies = 1 };

        //    using (StreamReader tr = new StreamReader(this.ModelFilePath))
        //    {
        //        XElement xe = XDocument.Load(tr).Root.Elements()
        //            .Elements(XName.Get("ModelType")).First(x => x.Value == this._modelType).Parent;
        //        pageWidth = float.Parse(xe.Elements(XName.Get("PageWidth")).First().Value);
        //        pageHeight = float.Parse(xe.Elements(XName.Get("PageHeight")).First().Value);
        //        dpiX = int.Parse(xe.Elements(XName.Get("DotPerInchX")).First().Value);
        //        dpiY = int.Parse(xe.Elements(XName.Get("DotPerInchY")).First().Value);
        //        perPaperFactor = int.Parse(xe.Elements(XName.Get("PerPaperFactor")).First().Value);
        //        this._no = int.Parse(xe.Elements(XName.Get("NO")).First().Value);
        //    }

        //    using (LocalReportHelper printHelper = new LocalReportHelper(reportPath))
        //    {
        //        printHelper.PrintTypeNO = this._no;
        //        printHelper.PrintLogInformation = printLog;
        //        printHelper.ExportImageDeviceInfo.DpiX = dpiX;
        //        printHelper.ExportImageDeviceInfo.DpiY = dpiY;
        //        printHelper.ExportImageDeviceInfo.PageWidth = pageWidth;
        //        printHelper.ExportImageDeviceInfo.PageHeight = pageHeight;
        //        foreach (BarcodeData bdCurrent in this._listBarcodeData)
        //        {
        //            if (worker.CancellationPending == true)
        //            {
        //                e.Cancel = true;
        //                break;
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    DataSet dsCurrent = this.GetDataForPrinterByBarcode(bdCurrent.Barcode, bdCurrent.IncreaseType);
        //                    DataSet dsNext = null, dsPrevious = dsCurrent.Copy();

        //                    int amount = this._printType == 0 ? 1 : bdCurrent.Amount - nextRemainder;
        //                    int copies = amount / perPaperFactor;
        //                    int remainder = nextRemainder = amount % perPaperFactor;

        //                    Action<DataSet, int, string, int> actPrint = (ds, duplicates, barcodes, amountForLog) =>
        //                    {
        //                        printHelper.PrintLogInformation.Barcode = barcodes;
        //                        printHelper.PrintLogInformation.Amount = amountForLog;
        //                        if (this.PrintType == 0 && DeviceType == DeviceType.DRV)
        //                        {
        //                            printerSettings.Copies = (short)duplicates;
        //                            printHelper.WindowsDriverPrint(printerSettings, dpiX, ds);
        //                        }
        //                        else
        //                        {
        //                            printHelper.OriginalPrint(DeviceType, ds, duplicates, PrintParam);
        //                        }
        //                    };

        //                    if (copies > 0)
        //                    {
        //                        int amountForCopy = copies;
        //                        if (perPaperFactor > 1)
        //                        {
        //                            DataSet dsCurrentCopy = dsCurrent.CopyForBarcode();
        //                            dsCurrent.Merge(dsCurrentCopy);
        //                            amountForCopy = copies * perPaperFactor;
        //                        }

        //                        actPrint.Invoke(dsCurrent, copies, bdCurrent.Barcode, amountForCopy);
        //                    }
        //                    if (remainder > 0)
        //                    {
        //                        int nextIndex = i + 1;
        //                        string barcodes = bdCurrent.Barcode;
        //                        if (nextIndex < count)
        //                        {
        //                            BarcodeData bdNext = this._listBarcodeData[nextIndex];
        //                            dsNext = this.GetDataForPrinterByBarcode(bdNext.Barcode, bdNext.IncreaseType);
        //                            dsPrevious.Merge(dsNext);
        //                            barcodes += "," + bdNext.Barcode;
        //                        }
        //                        actPrint.Invoke(dsPrevious, 1, barcodes, 1);
        //                    }
        //                    worker.ReportProgress(i++, string.Format("正在生成 {0} 并输送往打印机...", bdCurrent.Barcode));

        //                    if (this._printType == 0)
        //                    {
        //                        count = 1;
        //                        flag = true;
        //                        break;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    flag = false;
        //                    e.Result = ex.Message;
        //                    break;
        //                }
        //            }
        //        }
        //        if (!e.Cancel && flag)
        //        {
        //            e.Result = string.Format("打印操作成功完成，共处理条码 {0} 条", count);
        //        }
        //    }
        //}  
    }
}
