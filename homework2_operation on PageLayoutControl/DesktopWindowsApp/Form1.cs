using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Output;

namespace DesktopWindowsApp
{
    public partial class Form1 : Form
    {
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
        //private ESRI.ArcGIS.Controls.AxMapControl axMapControl2;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
         
              
        ///////
        private const int WM_ENTERSIZEMOVE= 0x231;
        private const int WM_EXITSIZEMOVE= 0x232;
        //快捷菜单
        private IToolbarMenu m_toolbarmenu = new ToolbarMenu();

        private IEnvelope m_envelope;
        private Object m_fillsymbol;

        private ITransformEvents_VisibleBoundsUpdatedEventHandler visBoundsUpdatedE;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            //this.axMapControl2 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Location = new System.Drawing.Point(266, 56);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(525, 525);
            this.axPageLayoutControl1.TabIndex = 1;
            this.axPageLayoutControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IPageLayoutControlEvents_Ax_OnMouseDownEventHandler(this.axPageLayoutControl1_OnMouseDown);
             this.axPageLayoutControl1.OnPageLayoutReplaced += new ESRI.ArcGIS.Controls.IPageLayoutControlEvents_Ax_OnPageLayoutReplacedEventHandler(this.axPageLayoutControl1_OnPageLayoutReplaced);
            // 
            // axMapControl1
            //      
            this.axMapControl1.Location = new System.Drawing.Point(0, 319);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(200, 200);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.axMapControlEvents1_OnAfterDraw);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);//移动鹰眼
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);//点击一下，新建鹰眼

            //
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 28);
            this.axToolbarControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(2130, 928);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(0, 56);
            this.axTOCControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(200, 200);
            this.axTOCControl1.TabIndex = 4;
            //this.axTOCControl1.OnEndLabelEdit += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnEndLabelEditEventHandler(this.axTOCControl1_OnEndLabelEdit);

            this.Controls.Add(this.axPageLayoutControl1);
            //this.Controls.Add(this.axMapControl2);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axToolbarControl1);
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();

            //////////////////////
            //把地图文档装载到pagelayoutcontrol和mapcontrol
            //string filename = @"D:\1004145209\zujian\7\HA.mxd";
            //if (axPageLayoutControl1.CheckMxFile(filename))
            //{
            //    axPageLayoutControl1.LoadMxFile(filename, "");
            //}

            ////设置伙伴控件
            axTOCControl1.SetBuddyControl(axPageLayoutControl1);
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);

            //窗体调整的重绘
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);




            //向命令条添加命令项
            string progID;
            progID = "esriControlToolsGeneric.ControlsOpenDocCommand";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            //放大
             progID = "esriControlToolsPageLayout.ControlsPageZoomInTool";
             axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            //缩小
             progID = "esriControlToolsPageLayout.ControlsPageZoomOutTool";
             axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            //手
             progID = "esriControlToolsPageLayout.ControlsPagePanTool";
             axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            // 
             progID = "esriControlToolsPageLayout.ControlsPageZoomWholePageCommand";
             axToolbarControl1.AddItem(progID, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);

             progID = "esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentBackCommand";
             axToolbarControl1.AddItem(progID, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);

             progID = "esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentForwardCommand";
             axToolbarControl1.AddItem(progID, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);


             progID = "esriControlToolsMapNavigation.ControlsMapZoomInTool";
             axToolbarControl1.AddItem(progID, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);

             progID = "esriControlToolsMapNavigation.ControlsMapZoomOutTool";
             axToolbarControl1.AddItem(progID, -1, -1, false, 0,
             esriCommandStyles.esriCommandStyleIconOnly);

             progID = "esriControlToolsMapNavigation.ControlsMapPanTool";
             axToolbarControl1.AddItem(progID, -1, -1, false, 0,
             esriCommandStyles.esriCommandStyleIconOnly);

             progID = "esriControlToolsMapNavigation.ControlsMapFullExtentCommand";
             axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);



             //给pagelayout创建快捷菜单
             progID = "esriControls.ControlsPageZoomInFixedCommand";
             m_toolbarmenu.AddItem(progID,-1,-1,false,esriCommandStyles.esriCommandStyleIconOnly);
             progID = "esriControlToolsPageLayout.ControlsPageZoomInFixedCommand";
             m_toolbarmenu.AddItem(progID, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
             progID = "esriControlToolsPageLayout.ControlsPageZoomOutFixedCommand";
             m_toolbarmenu.AddItem(progID, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
             progID = "esriControlToolsPageLayout.ControlsPageZoomWholePageCommand";
             m_toolbarmenu.AddItem(progID, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
             progID = "esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentBackCommand";
             m_toolbarmenu.AddItem(progID, -1, -1, true, esriCommandStyles.esriCommandStyleIconAndText);
             progID = "esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentForwardCommand";
             m_toolbarmenu.AddItem(progID, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
             progID = "esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentForwardCommand";
             m_toolbarmenu.AddItem(progID, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
             //Set the hook to the PageLayoutControl
             m_toolbarmenu.SetHook(axPageLayoutControl1);
            //
             CreateOverviewSymbol();

        }
        protected override void OnNotifyMessage(System.Windows.Forms.Message m)
        {
            base.OnNotifyMessage(m);

        }


        //pagelayout的数据改变时
        private void axPageLayoutControl1_OnPageLayoutReplaced(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnPageLayoutReplacedEvent e)
        {
            IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            visBoundsUpdatedE = new ITransformEvents_VisibleBoundsUpdatedEventHandler(OnVisibleBoundsUpdated);
            ((ITransformEvents_Event)activeView.ScreenDisplay.DisplayTransformation).VisibleBoundsUpdated+=visBoundsUpdatedE;

            //得到focusmap中的内容
            m_envelope = activeView.Extent;
           
            //每当图层加载到pagelayout时，触发此事件将同一个文档加载到mapcontrol
            axMapControl1.LoadMxFile(axPageLayoutControl1.DocumentFilename,null,null);
            axMapControl1.Extent = axMapControl1.FullExtent;

        }
        //pagelayout范围改变时调用此函数，更新axMapControl1
        private void OnVisibleBoundsUpdated(IDisplayTransformation Sender, bool sizechanged)
        {
            
            m_envelope = Sender.VisibleBounds;
            
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }

        //在mapcontrol上显示创建的符号
        private void axMapControlEvents1_OnAfterDraw(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnAfterDrawEvent e )
        {
            if (m_envelope == null)
            {
                return;
            }

            esriViewDrawPhase viewDrawPhase = (esriViewDrawPhase)e.viewDrawPhase;
            if(viewDrawPhase==esriViewDrawPhase.esriViewForeground)
            {
                IGeometry geometry = m_envelope;
                axMapControl1.DrawShape(geometry,ref m_fillsymbol);
            }
        }

        //////////
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            
            if(e.button==1)
            {
                //点击一下左键，以此为中心新建鹰眼
                IPoint m_point = new PointClass();
                m_point.PutCoords(e.mapX, e.mapY);
                m_envelope.CenterAt(m_point);
                IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
                activeView.Extent = m_envelope;
                // axPageLayoutControl1刷新
                axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            if(e.button==2)
            {
                //点击右键追踪拖动生成新的鹰眼
                m_envelope = axMapControl1.TrackRectangle();
                IPoint m_point = new PointClass();
                m_point.PutCoords(e.mapX,e.mapY);
                m_envelope.CenterAt(m_point);
                IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
                activeView.Extent = m_envelope;
                // axPageLayoutControl1刷新
                axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

            }
        }
        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {

            if (e.button == 1)
            {
                //点击处在鹰眼内
                if (e.mapX > m_envelope.XMin && e.mapY > m_envelope.YMin && e.mapX < m_envelope.XMax && e.mapY < m_envelope.YMax)
                {
                    IPoint m_point = new PointClass();
                    m_point.PutCoords(e.mapX,e.mapY);
                    m_envelope.CenterAt(m_point);//以m_point为中心移动m_envelope
                   
                    IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
                    activeView.Extent = m_envelope;
                    // axPageLayoutControl1刷新
                   axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                
                }               
            }

        }


        //右键鼠标，显示快捷菜单
        private void axPageLayoutControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseDownEvent e)
        { 
            
            if(e.button==2)
            {
                m_toolbarmenu.PopupMenu(e.x,e.y,axPageLayoutControl1.hWnd);
            }
        }
        //颜色
        private void CreateOverviewSymbol()
        {
            
            IRgbColor color = new RgbColor();
            color.RGB = 255;
            
            //
            ILineSymbol outline = new SimpleLineSymbol();
            outline.Width = 1.5;
            outline.Color = color;

            //
            ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            simpleFillSymbol.Outline = outline;
            simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
            m_fillsymbol = simpleFillSymbol;
         }
        
   

        private void Form1_resizebegin(object sender, EventArgs e)
        {
            axMapControl1.SuppressResizeDrawing(true,0);
            axPageLayoutControl1.SuppressResizeDrawing(true,0);
        }

        private void Form1_resizeend(object sender, EventArgs e)
        {
            axMapControl1.SuppressResizeDrawing(false,0);
            axPageLayoutControl1.SuppressResizeDrawing(false,0);

        }

       
       
    }
}