   M S B A M L   `   `   `  ����G  C数模建模, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:数模建模数模建模  5   	       数模建模.ResByLayer   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   ,localclr-namespace:数模建模     5   	     � Title$  
ResByLayer��5   	   $	��400��6   $	��400��6)   .��5      �� .+�5	   
   !   数模建模.SelectFilePath  -   5          Name$ 
fgrid_file��5	         dataPath$ 请选择fgrid文件：��63   $	��300��6J   6Y     5
   
   -   5        $ prt_file��5
      $ 请选择prt文件：��61   $	��300��6F   $��0,30,0,0q�6S   6g   �� 5   
   -   5        $��时间解析��5      $��
305,30,0,0q�66   6M   �� 5   
   $��23��6   $
��Left=�6!   $��0,60,0,0q�6<   $	��Top=�5   !   $��50��69   $��时间��6D   $��0��6N   $��1��6[   �� 5   
   -   5        $��combo_soiltime��5      $
��Left=�6*   $	��Top=�6E   $��	25,60,0,0q�6^   $	��100��6q   $��0��6}   $��1��6�   $
��23px��6�   �� 5   
   $��原油密度:��6   $��
140,60,0,0q�6!   $��23��66   $��50��6B   $
��Left=�6M   �� 5   
   -   5        $��res_density��5      $��0.852��6&   $��
190,60,0,0q�63   $��23��6G   $��50��6S   $
��Left=�6^   �� 5   
   $��体积系数:��6   $��
260,60,0,0q�6!   $
��Left=�65   $��23��6Q   $��50��6]   �� 5   
   -   5        $��res_vol��5      $��
310,60,0,0q�6#   $��23��67   $��50��6C   $
��Left=�6N   �� 5   
   -   5        $��计算��5      $��0,90,0,0q�6/   6D   &   System.Windows.Controls.DataGrid  5   
   -   5        $��dataGridReservers��5         AutoGenerateColumns .  $	��240��6H      CanUserAddRows .   �� Left$ 0��6m   
 �� Top$	 118��6}      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 层号��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 80 61   
   
IsReadOnly
 . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   ��  �� Path$ ch� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�     5      $ 储量(万吨)��6%   $		 120 65    6A   �� $	 val�$ TwoWay=�$ PropertyChanged=�5      5      5      