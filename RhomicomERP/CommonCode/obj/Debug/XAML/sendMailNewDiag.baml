   M S B A M L   `   `   `  ����E  ACommonCode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CommonCode.XAMLCommonCode.XAML  5   
   %    CommonCode.XAML.sendMailNewDiag   5   	   'localclr-namespace:CommonCode.XAML  5   
   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          �Name$  MainWindow1��5   	    � Title$ Send Bulk Mails��5   	    � WindowStartupLocation$ CenterScreen=�6"    � Icon$/ )pack://siteoforigin:,,,/Resources/web.png��6^   $	��550��6�   $
��1020��6�   #��� ��6�   #��� ��6�   .��5      � .+�5	   
   #   CommonCode.XAML.WPFWebBrowser  -   5          Name$ webBrowserEditor��5	      $��1��69   $��	0,112,0,0q�6F   $��2��6Y   $��1q�6i   #��� ��6}    K� ZIndex$	 -99��6�   +  %System.Windows.Controls.Ribbon.Ribbon  5
   
   -   5          Name$ mainRibbon1��5
      ���#333333$��	0,-24,0,0q�6<   $��2��6O   $	 -99��6_   $��2��6r   .@�5      .  (System.Windows.Controls.Ribbon.RibbonTab  $��Editor��6   .@�5      0  *System.Windows.Controls.Ribbon.RibbonGroup  $��Files��6   $��80��6-   .@�5      1  +System.Windows.Controls.Ribbon.RibbonButton  -   5          Name$ RibbonButtonNew��5   #      Label$  ��6<   	   LargeImageSource$,	 &pack://siteoforigin:,,,/Images/new.png��6E   6     5      -   5        $ RibbonButtonOpen��5   #   $  ��6=   
   SmallImageSource$-
 'pack://siteoforigin:,,,/Images/open.png��6F   $
��Left=�6�   $��22��6�     5      -   5        $ RibbonButtonOpenweb��5   #   $  ��6@   $-
 'pack://siteoforigin:,,,/Images/link.png��6I   6�     5      -   5        $ RibbonButtonSave��5   #   $  ��6=   $-
 'pack://siteoforigin:,,,/Images/save.png��6F   6�   5        5      $��Editor��6   $	��350��6.   .@�5      �� .+�5      �� -   5         ��Name$ RibbonComboboxFormat��5   #   $	��160��6A   $��0,0,0,4q�6M   $
��Left=�6^   ��5      �� 5   "   .R�5   &   �� ��60   �� *Value+5   #   5      �� 5       �� Orientation$ 
Horizontal=�6%   .+�5      �� -	   5        $ RibbonComboboxFonts��5   (   $	��160��6E   6Q   �� 5      -
   5        $ RibbonComboboxFontHeight��5   '   $��50��6I   6T   5      �� 5      $ 
Horizontal=�6%   .+�5         -   5        $ SettingsBold��5    +   $-
 'pack://siteoforigin:,,,/Images/bold.png��6A   6|     5!      -   5        $ SettingsItalic��5!   +   $/
 )pack://siteoforigin:,,,/Images/italic.png��6C   6�     5"      -   5        $ SettingsUnderLine��5"   +   $2
 ,pack://siteoforigin:,,,/Images/UnderLine.png��6F   6�     5#      -   5        $ SettingsFontColor��5#   +   $.
 (pack://siteoforigin:,,,/Images/Color.png��6F   6�     5$      -   5        $ SettingsBackColor��5$   +   $2
 ,pack://siteoforigin:,,,/Images/backcolor.png��6F   6�     5%      -   5        $ SettingsAddLink��5%   +   $-
 'pack://siteoforigin:,,,/Images/link.png��6D   6     5&      -   5        $ SettingsAddImage��5&   +   $.
 (pack://siteoforigin:,,,/Images/image.png��6E   6�     5'      -   5        $ SettingsLeftAlign��5'   +   $2
 ,pack://siteoforigin:,,,/Images/LeftAlign.png��6F   6�     5(      -   5        $ SettingsCenter2��5(   +   $0
 *pack://siteoforigin:,,,/Images/center2.png��6D   6�     5)      -   5        $ SettingsRightAlign��5)   +   $3
 -pack://siteoforigin:,,,/Images/RightAlign.png��6G   6�     5*      -   5        $ SettingsCenter��5*   +   $/
 )pack://siteoforigin:,,,/Images/center.png��6C   6�     5+      -   5        $ SettingsNumbered��5+   +   $1
 +pack://siteoforigin:,,,/Images/Numbered.png��6E   6�     5,      -   5        $ SettingsBullets��5,   +   $0
 *pack://siteoforigin:,,,/Images/bullets.png��6D   6�     5-      -   5        $ SettingsOutIdent��5-   +   $1
 +pack://siteoforigin:,,,/Images/OutIdent.png��6E   6�     5.      -   5        $ SettingsIdent��5.   +   $.
 (pack://siteoforigin:,,,/Images/ident.png��6B   6~   5/      50      51        52      $��Custom��6   $	��144��6.   $	��0,1q�6:   .@�53      �� .+�54      �� $ 
Horizontal=�6%   .+�55        -   5        $ EditWeb1��55   +   $/	 )pack://siteoforigin:,,,/Images/Editor.png��6>   $��42��6{   $
��Left=�6�   6�     56      -   5        $ ViewHTML��56   +   $-	 'pack://siteoforigin:,,,/Images/html.png��6>   $��42��6y   $
��Left=�6�   6�     57      -   5        $ closeWindow��57   +   $��42��6@   $
��Left=�6L    �� IsCancel .  J� RenderTransformOrigin$ 0.682,0.619&�6�      ToolTipDescription$ Close Program��6�   $  ��6�   $.	 (pack://siteoforigin:,,,/Images/Close.png��6�   58      59      5:      5;      5<      �� 5>   
   $��0��6   
����  �$��5��6:   $��Stretch=�5?      $��Stretch=�6%   $��1q�6A   $��Stretch=�6U   $��Stretch=�6x   $
��auto��6�   $��
198,0,89,0q�6�   �� 5A   
   $��2��6   $��3��6!   }�. .@�5B      �� .��5C      �� -   5         ��Name$ lblCursorPosition��5C      $a��[http://www.rhomicom.com Credits: http://www.greencomponent.net, http://www.codeproject.com/��67   5D      5E          CommonCode.XAML.HtmlEditor  5F   
   -   5          Name$ HtmlEditor1��5F      $��1��61   $��2��6>   $��	0,112,0,0q�6N   &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�6a   $
��Auto��6�   $
��Auto��6�   $y�Hidden=�6�   �� 5G   
   $!��Destination Group/Addresses��6   $��1��68   $��5,1,5,0q�6E   $	��351��6V   $	��Top=�6c   $��2��6{   .��5H      � .+�5I      �� -   5         ��Name$ grpNmIDTextBox��5I      $
��Left=�62   $��6��6M   $��
162,72,0,0q�6X    �� TextWrapping$
 Wrap=�6l   $��-1��6�   $	��Top=�6�   $��25��6�   |�.   J� IsHitTestVisible .  ��.   J� 	AllowDrop .  $	 -11��6�    �� 
IsReadOnly . 6  �� 5J      $��Message Type:��6   $
��Left=�60   $	��Top=�6K   $��0,3,0,0q�6c   �� 5K      $��Group Type:��6   $
��Left=�6.   $��0,25,0,0q�6I   $	��Top=�6[   �� 5L      -    5         ��Name$ sendIndvdllyCheckBox��5L      $
��Left=�69   $��	267,3,0,0q�6T   $	��Top=�6g   $	��109��6   $��78��6�    d� 	IsChecked$
 TrueY�6�   $��Center=�6�   .��5M      �� $C��=Send Mails to Email Addresses Individually and NOT in Groups!��6    $
��Wrap=�6e   5N      �� 5O      -!   5        $ msgTypComboBox��5O      $
��Left=�63   $��85,3,0,0q�6N   $	��Top=�6`   $	��177��6x   .@�5P      �� -"   5         ��Name$
 cbi1��5P   #   $��Email��61   �� 5Q      -#   5        $
 cbi2��5Q   #   $	��SMS��61   �� 5R      -$   5        $
 cbi3��5R   #   $��Local Inbox (System)��61   5S      �� 5T      -%   5        $ grpComboBox��5T      $
��Left=�60   $��	85,27,0,0q�6K   $	��Top=�6^   $	��102��6v   .@�5U      �� -&   5        $	 cb1��5U   #   $��Everyone��60   �� 5V      -'   5        $	 cb2��5V   #   $��Divisions/Groups��60   �� 5W      -(   5        $	 cb3��5W   #   $��Grade��60   �� 5X      -)   5        $	 cb4��5X   #   $	��Job��60   �� 5Y      -*   5        $	 cb5��5Y   #   $��Position��60   �� 5Z      -+   5        $	 cb6��5Z   #   $��Site/Location��60   �� 5[      -,   5        $	 cb7��5[   #   $��Person Type��60   �� 5\      --   5        $	 cb8��5\   #   $��Single Person��60   �� 5]      -.   5        $	 cb9��5]   #   $��Companies/Institutions��60     5^      5_      �� 5`      $��Group Name:��6   $
��Left=�6.   $��0,50,0,0q�6I   $	��Top=�6[   �� 5a      $��
Workplace:��6   $
��Left=�6-   $��0,83,0,0q�6H   $	��Top=�6Z   �� 5b      $��Site:��6   $��Right=�6(   $��0,122,347,0q�6D   $	��Top=�6Y   �� 5c      $	��To:��6   $
��Left=�6&   $��	0,155,0,0q�6A   $	��Top=�6T   �� 5d      $	��Cc:��6   $
��Left=�6&   $��	0,217,0,0q�6A   $	��Top=�6T   �� 5e      $
��Bcc:��6   $
��Left=�6'   $��	0,269,0,0q�6B   $	��Top=�6U   �� 5f      -/   5         ��Name$ grpNmButton��5f      $	��...��6.   $
��Left=�6<   $��
237,52,0,0q�6W   $	��Top=�6k   $��25��6�   6�   �� 5g      -0   5        $ cstmrButton��5g      $	��...��6.   $
��Left=�6<   $��
237,91,0,0q�6W   $	��Top=�6k   $��25��6�   6�   �� 5h      -1   5        $ cstmrSiteButton��5h      $	��...��62   $
��Left=�6@   $��237,130,0,0q�6[   $	��Top=�6p   $��25��6�   6�   �� 5i      -2   5        $ 	toTextBox��5i      $
��Left=�6-   $��57��6H   $��
85,159,0,0q�6T   $
 Wrap=�6h   $	��Top=�6|   $	��291��6�    �� AcceptsReturn .  �� 
AcceptsTab .  �� AutoWordSelection . $ Display=�6�   �� 5j      -3   5        $ toButton��5j      $	��-->��6+   $
��Left=�6<   $��
57,160,0,0q�6W   $	��Top=�6k   $��25��6�   6�   �� 5k      -4   5        $ 	ccTextBox��5k      $
��Left=�6-   $��52��6H   $��
85,218,0,0q�6T   $
 Wrap=�6h   $	��Top=�6|   $	��291��6�   �� 5l      -5   5        $ ccButton��5l      $	��-->��6+   $
��Left=�6<   $��
57,219,0,0q�6W   $	��Top=�6k   $��25��6�   6�   �� 5m      -6   5        $ 
bccTextBox��5m      $
��Left=�6.   $��55��6I   $��
85,272,0,0q�6U   $
 Wrap=�6i   $	��Top=�6}   $	��291��6�   �� 5n      -7   5        $ 	bccButton��5n      $	��-->��6,   $
��Left=�6=   $��
57,273,0,0q�6X   $	��Top=�6l   $��25��6�   6�   �� 5o      -8   5        $ button5��5o      $
��Left=�6*   $��
267,90,0,0q�6E   $	��Top=�6Y   $	��109��6q   $��52��6}   .��5p      �� $
��Wrap=�6    $ ��Auto-Load Qualifying Mails��64   5q      �� 5r      -9   5        $ cstmrIDTextBox��5r      $
��Left=�62   $��23��6M   $��
162,97,0,0q�6Y   $
 Wrap=�6m   $��-1��6�   $	��Top=�6�   $��25��6�   |�.   .  ��.   .   . 6�   �� 5s      -:   5        $ cstmrSiteIDTextBox��5s      $
��Left=�66   $��23��6Q   $��162,132,0,0q�6]   $
 Wrap=�6r   $��-1��6�   $	��Top=�6�   $��25��6�   |�.   .  ��.   .  6�   �� 5t      -;   5        $ cstmrSiteTextBox��5t      $
��Left=�64   $��28��6O   $��
85,129,0,0q�6[   $
 Wrap=�6o   $	��Top=�6�   $	��147��6�   " �� VerticalScrollBarVisibility$
 Auto=�6�   $ 2��6�   �� 5u      -<   5        $ cstmrNmTextBox��5u      $
��Left=�62   $��36��6M   $��	85,91,0,0q�6Y   $
 Wrap=�6l   $	��Top=�6�   $	��147��6�   $
 Auto=�6�   $ 2��6�   �� 5v      -=   5        $ grpNmTextBox��5v      $
��Left=�60   $��38��6K   $��	85,51,0,0q�6W   $
 Wrap=�6j   $	��Top=�6~   $	��147��6�   $
 Auto=�6�   $ 2��6�   
�������6�   �� 5w      ->   5        $ grpSlctnComboBox��5w      $
��Left=�65   $��
192,27,0,0q�6P   $	��Top=�6d   $��70��6|   .@�5x      �� -?   5        $
 cb10��5x   #   $��contains��61   �� 5y      -@   5        $
 cb11��5y   #   $��is equal to��61   �� 5z      -A   5        $
 cb12��5z   #   $��starts with��61     5{      5|      5}      5~      �� 5   
   $��Subject/Attachments��6   $��	5,349,0,0q�60   $��1��6C   $	��Top=�6P   $��95��6h   $
��Left=�6t   $��2��6�   .��5�      � .+�5�      �� $��Subject:��6   $
��Left=�6+   $��5,0,0,0q�6F   $	��Top=�6W   �� 5�      -B   5        $ subjTextBox��5�      $
��Left=�6/   $��23��6J   $��86,4,0,0q�6V   $
 Wrap=�6h   $	��Top=�6|   $	��293��6�   �� 5�      $��Attachments:��6   $
��Left=�6/   $��6,29,0,0q�6J   $	��Top=�6\   �� 5�      -C   5        $ attchMntsTextBox��5�      $
��Left=�64   $��41��6O   $��	86,30,0,0q�6[   $
 Wrap=�6n   $	��Top=�6�   $	��266��6�   �� 5�      -D   5        $ browseButton��5�      $	��...��6/   $��
354,30,0,0q�6=   $	��Top=�6Q   $
��Left=�6�   $��25��6�   5�      5�      �� 5�   
   -E   5        $ okButton��5�      $
��Send��6#   $
��Left=�62   $��123,449,0,0q�6M   $��1��6b   $	��Top=�6o   $��75��6�   6�   �� 5�   
   -F   5        $ cancelButton��5�      $��Cancel��6'   $
��Left=�68   $��203,449,0,0q�6S   $��1��6h   $	��Top=�6u   $��75��6�   6�   �� 5�   
   -G   5          ��Name$  	mailLabel��5�      ���	#FF19B600$ 50��6<   $��Center=�6N   $��Times New Roman)�6j   $��24(�6�   $��Italic$�6�   #��� ��6�   $y�Visible=�6�   $��1��6
  $��Center=�6  $��2q�64  $��Center=�6@  $��Center=�6d  $��3��6�  $��27,222,25,196q�6�  $	��960��6�  $��77��6�  .��5�      �� $��WrapWithOverflow=�6   ! �� Padding$! 10q�68   ���	#FF19B600$
��auto��6\   $
��auto��6i   #!Sending Messages...Please Wait...5�      5�      " � ColumnDefinitions" 5�   
   �� 5�      $	��292 �6   �� 5�      $	��109 �6   �� 5�      $��* �6   # � RowDefinitions# 5�   
   �� 5�      $
��Auto �6   �� 5�      $��* �6   �� 5�      $
��Auto �6   5�      5�      