   M S B A M L   `   `   `  ����  CIS345FinalApplication'    !CIS345FinalApplication.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
XML Search��5   	   $	��422��6   $	��721��6)   .��5      � $��0,0,2,0q�6    � RowDefinitions 5   
   �� 5       � ColumnDefinitions 5	   
   �� 5
      $
��107* �6   �� 5      $
��593* �6   �� 5      $	��11* �6   .+�5   
   �� -   5         ��Name$ 
txtContent��5      $��23��6&   $��10,68,158,0q�62    �� TextWrapping$
 Wrap=�6G   $	��Top=�6[   $��1��6s   �� 5   
   -   5         ��Name$ lblTag��5      $��
Search Tag��6    $��	0,18,10,0q�65   $	��Top=�6H   $��Right=�6`   $��68��6|    J� RenderTransformOrigin$ 0.397,0.462&�6�   $��26��6�   �� 5   
   -   5        $ 
lblContent��5      $��Search Query��6$   $��	0,73,10,0q�6;   $	��Top=�6N   $ 0.658,0.192&�6f   $��Right=�6�   $��81��6�   $��26��6�   �� 5   
   -   5        $ 
lblResults��5      $��Results��6$   $��	0,99,10,0q�66   $	��Top=�6I   $��Right=�6a   $��47��6}   $��26��6�   �� 5   
   -   5         ��Name$ btnQuery��5      $��	Run Query��6#   $��	0,10,10,0q�67   $��1��6a   $��81��6q   $	��Top=�6}   $��Right=�6�   $	��140��6�   �� 5   
   -   5         ��Name$ cmbTag��5      $��10,14,344,0q�6#   $	��Top=�68   $��1��6P   $��22��6`   &   System.Windows.Controls.DataGrid  5   
   -   5        	  Name$	 dataResults��5      $��10,96,10,38q�6@   
   AutoGenerateColumns
 .     
IsReadOnly . $��1��6�   ��6(   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 File��6B      Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *
Filepath+6P     5      $ Element��6A   $	 100 6R    6%   �� *	Element+6R     5      $ Content��6A   $	 300 6R    6%   �� *	Content+6R     5         
Visibility$ Hidden=�6G    6%   �� *ContextString+6G   5      �� 5   
   -   5        $��	txtOutput��5      $��
10,0,10,10q�6#   $
 Wrap=�67   $�� ��6K    �� 
IsReadOnly . $��1��6e   $��23��6u   $��Bottom=�6�   �� 5   
   -	   5        $ cmbFile��5      $��10,41,392,0q�6$   $	��Top=�69   $��1��6Q   $��22��6a   �� 5   
   -
   5        $ lblFile��5      $��File to Search��6!   $
��Left=�6:   $��	17,38,0,0q�6U   $	��Top=�6h   $��26��6�   $��81��6�   �� 5   
   -   5        $ txtHits��5      $��1��6#   $
��Left=�63   $��23��6N   $��402.8,38,0,0q�6Z   $
 Wrap=�6p   $��10��6�   $	��Top=�6�   $��33��6�   �� 5    
   -   5        $ lblHits��5       $��Number of Results��6!   $��1��6=   $
��Left=�6M   $��288.8,35,0,0q�6h   $	��Top=�6~   5!      5"      