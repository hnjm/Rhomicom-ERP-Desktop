   M S B A M L   `   `   `  ����E  ACommonCode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CommonCode.XAMLCommonCode.XAML  5      !    CommonCode.XAML.RegisterApp   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:CommonCode.XAML     5      -   5          �Name$  registerProduct��5   	   $	��339��5      $	��485��6,    � 
ResizeMode$ NoResize=�6X    � WindowStartupLocation$ CenterScreen=�6n    � SizeToContent$ WidthAndHeight=�6�   .��5      � #��� ��6   $��0,0,0,0q�6X   .+�5   
   � $��5,5,5,5q�6   
�������$	��306��63   $��Bottom=�6@    � ColumnDefinitions 5      �� 5      $	��50* �6#    � RowDefinitions 5      �� 5      $
��252* �6    .+�5      �� -   5         ��Name$ errormessage��5   Q   $��20��6   $
��Left=�6$   $��67,0,0,0q�6?   $	��Top=�6h   $	��247��6�    J� OpacityMask
 �<�����	#FFE5572C6�   �� 5      -   5        $��textBlockFirstname��5   R   $��23��6   $
��Left=�6$   $��	16,51,0,0q�6?   $��Name of Organisation:��6l   $	��Top=�6�   $	��123��6�   �� 5      -   5        $��textBlockLastName��5   R   $��23��6   $
��Left=�6$   $��	16,81,0,0q�6?   $��	Location:��6k   $	��Top=�6|   $	��123��6�   �� 5      -   5        $��textBlockEmailId��5   S   $��23��6   $
��Left=�6$   $��
16,111,0,0q�6?   $��Email Address:��6k   $	��Top=�6�   $	��123��6�   �� 5      -   5        $��textBlockAddress��5   S   $��23��6   $
��Left=�6$   $��
16,168,0,0q�6?   $��Support Code:��6k   $	��Top=�6�   $	��110��6�   �� 5      -   5        $��textBoxFirstName��5   Q   $��23��6   $
��Left=�6"   $��
141,51,5,0q�6=   $	��Top=�6i   $	��320��6�   $��2��6�   �� 5      -   5        $��textBoxLastName��5   Q   $��23��6   $
��Left=�6"   $��
140,81,5,0q�6=   $	��Top=�6h   $	��321��6�   $��2��6�   �� 5      -	   5        $��textBoxEmail��5   R   $��23��6   $
��Left=�6"   $��140,111,5,0q�6=   $	��Top=�6f   $	��321��6~   $��2��6�   �� 5      -
   5        $��textBoxAddress��5   Q   $��98��6   $
��Left=�6"   $��
140,0,5,41q�6=   $��Bottom=�6g   $	��321��6�   $��2��6�   �� 5      -   5         ��Name$ Submit_Registration��5   m   $��Submit Registration��6   $��23��63   $
��Left=�6?   $��	7,273,0,0q�6Z   $	��Top=�6�   $	��123��6�   �� 5      -   5        $��button2��5   o   $��Verify Registration��6   $��23��63   $
��Left=�6?   $��135,273,0,0q�6Z   $	��Top=�6~   $	��111��6�   �� 5       -   5        $��closeButton��5    `   $��Close��6   $��23��6%   $��Right=�61   $��	0,273,8,0q�6M   $	��Top=�6s   $��70��6�   	 J� RenderTransformOrigin$	 -1.209,0.439&�6�   6�   �� 5!      -   5        $ textBlockAddress_Copy��5!   S   $��23��6   $
��Left=�6$   $��
16,139,0,0q�6?   $��Support Type:��6r   $	��Top=�6�   $��84��6�   �� 5"      -   5        
 ��Name$
 comboBox��5"      $
��Left=�6)   $��141,140,0,0q�6D   $	��Top=�6Y   $	��117��6q   .@�5#      �� $��Premium Support��6   �� 5$      $��Standard Support��6   �� 5%      $��Basic Support��6   5&      �� 5'      -   5        $ impCodeLabel��5'   T   $��23��6   $
��Left=�6$   $��264,140,0,0q�6?   $��	IMP Code:��6j   $	��Top=�6{   $��66��6�   G� 5(      -   5         G�Name$ impCodeTextbox��5(   V   $��23��6   $
��Left=�6&   $��325,139,5,0q�6A   $	��Top=�6n   $	��136��6�   $��2��6�   �� 5)      -   5        $ textBlockHeading��5)   R   $��34��6   $
��Left=�6$   $��	13,16,0,0q�6?   $)��#Product Registration (For Support):��6l   $	��Top=�6�   $	��420��6�   $��17(�6�   $��ExtraCondensed&�6�   $
��Bold"�6�   $��	Underline{�6�   $��2��6  5*      5+      5,      