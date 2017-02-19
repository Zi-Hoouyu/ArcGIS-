using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;


namespace DesktopWindowsApp
{
    public partial class Form1 : Form
    {
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
 ////////以下为加载模板///////////
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(286, 56);
            this.axMapControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(300, 400);
            this.axMapControl1.TabIndex = 2;
            //this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            // 
            // axToolbarControl1 
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 28);
            this.axToolbarControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1125, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // axTOCControl1内容表控件
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl1.Location = new System.Drawing.Point(4, 56);
            this.axTOCControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(50, 469);
            this.axTOCControl1.TabIndex = 4;

            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axToolbarControl1);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();



           
            
            /////添加遥感
           //IWorkspaceFactory pWorkSpaceFactory=new RasterWorkspaceFactory();
           //IWorkspace pWorkspace = pWorkSpaceFactory.OpenFromFile("D:\\1004145209\\zujian\\1004145209周瑜琦_作业1_加载DEM和遥感\\DesktopWindowsApp\\北京遥感", 0);
           //IRasterWorkspace pRasterWorkspace=pWorkspace as IRasterWorkspace;           
           //IRasterDataset prd=pRasterWorkspace.OpenRasterDataset("123-3206");
           // IRasterLayer player = new RasterLayerClass();
           // player.CreateFromDataset(prd);
           // IMap pmap = axMapControl1.Map;
           // pmap.AddLayer(player);
           // axMapControl1.ActiveView.Refresh();

            ////加载dem
            //D:\1004145209\zujian\1004145209周瑜琦_作业1_加载DEM和遥感\DesktopWindowsApp\dem\dem\dem
            IWorkspaceFactory pWorkSpaceFactory = new RasterWorkspaceFactory();
            IWorkspace pWorkspace = pWorkSpaceFactory.OpenFromFile("D:\\1004145209\\zujian\\1004145209周瑜琦_作业1_加载DEM和遥感\\DesktopWindowsApp\\dem\\dem\\dem", 0);
            IRasterWorkspace rWorkspace = pWorkspace as IRasterWorkspace;

            IRasterLayer pLayer = new RasterLayerClass();
            pLayer.CreateFromFilePath("D:\\1004145209\\zujian\\1004145209周瑜琦_作业1_加载DEM和遥感\\DesktopWindowsApp\\dem\\dem\\dem");
            IMap pMap = axMapControl1.Map;
            pMap.AddLayer(pLayer);
            axMapControl1.ActiveView.Refresh();

        }
    }
}