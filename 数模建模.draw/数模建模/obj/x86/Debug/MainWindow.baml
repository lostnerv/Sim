   M S B A M L   `   `   `  ����`  \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35tNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary!Microsoft.Windows.Controls.Ribbon  5   	   G C数模建模, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:数模建模数模建模 5   	      数模建模.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   aribbonNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary     5   	   ,localclr-namespace:数模建模     5   	   -   5        4   .Microsoft.Windows.Controls.Ribbon.RibbonWindow   Name$  RibbonWindow��5       � Title$ 
MainWindow��5   	   $
��Auto��5      $	��840��6   .��5
      � -   5         �Name$ 
LayoutRoot��5
       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   .   (Microsoft.Windows.Controls.Ribbon.Ribbon  -   5          Name$ Ribbon��5      $��0��6(      ApplicationMenu 5      =   7Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenu  5      8   2Microsoft.Windows.Controls.Ribbon.RibbonMenuButton   SmallImageSource$ Images\SmallIcon.png��6/   .@�5      A   ;Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -   5          Name$ 	MenuItem1��5   7   $��Hello_Ribbon��5   7   6   0Microsoft.Windows.Controls.Ribbon.RibbonMenuItem   ImageSource$ Images\LargeIcon.png��5   7   5      .@�5      1   +Microsoft.Windows.Controls.Ribbon.RibbonTab  -   5        	  Name$	 Reservor��5      $��剩余油分析��61   .@�5      3   -Microsoft.Windows.Controls.Ribbon.RibbonGroup  $��剩余油分析��6%   .@�5      4	   .Microsoft.Windows.Controls.Ribbon.RibbonButton	  -   5        
 	 Name$
 
bt_wellLoc��5   *    	  LargeImageSource$ Images\LargeIcon.png��5   *   $��5q�5   *    	  Label$ 剩余油数据配置��5   *   6:   	  5      -   5        $
 btn_showResByLayer��5   *   $ Images\LargeIcon.png��5   *   $��5q�5    *   $ 各层储量��5!   *   67   	  5#      -   5        $
 bt_prt��5#   *   $ Images\LargeIcon.png��5$   *   $��5q�5%   *   $ 对比展示��5&   *   67   	  5'      -	   5        $
 bt_convertFacies��5'   *   $ Images\LargeIcon.png��5(   *   $��5q�5)   *   $ 相图文件生成��5*   *   69   5+      5,      5-      � 5.   
   $��1��6    � ColumnDefinitions 5/      �� 50      $��* �6#   .+�52      ]� ��. $��1��6-   .+�53      #
  数模建模.MainRightTabPage
  -
   5         
 Name$ 	RightMain��53   )   6>   54      55      56      57      