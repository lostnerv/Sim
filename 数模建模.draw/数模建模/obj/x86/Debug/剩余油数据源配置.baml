   M S B A M L   `   `   `  ����G  C数模建模, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:数模建模数模建模  5   	   +    %数模建模.剩余油数据源配置   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ,localclr-namespace:数模建模     5   	     � Title$  剩余油数据源配置��5   	   $	��400��6   $��558.209��6'   .��5      �  � RowDefinitions 5   
   �� 5      $��1* �6   �� 5	      $��1* �6   �� 5
      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   !   数模建模.SelectFilePath  -   5          Name$ fgrid��5         dataPath$ .FGRID 文件：��6.   $��0��6D   $��2��6Q   6c     5   
   -   5        $ prtinc��5      $( ".PRT (饱和度、孔隙度等)：��6/   $��1��6K   $��2��6X     5   
   -   5        $ schinc��5      $ SCH.INC (井点)：��6/   $��2��6H   $��2��6U   6g     5   
   -   5        $ gothinc��5      $ GOTH.INC (断层)：��60   $��3��6J   $��2��6W   6i     5   
   -   5        $ 	faciesinc��5      $ REG.INC (FACIES)：��62   $��4��6O   $��2��6\   6n     5   
   -   5        $ gproinc��5      $ GPRO.INC 文件：��60   $��5��6H   $��2��6U   6g     5   
   -   5        $ partinc��5      $ 分区文件 (可选)：��60   $��6��6F   $��2��6S   6e   �� 5   
   -   5         ��Name$ 
simaf_save��5      $��12��6%   $��7��63   $��保存��6C   6k   5      5      