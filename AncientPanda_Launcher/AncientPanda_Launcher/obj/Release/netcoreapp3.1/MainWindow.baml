   M S B A M L   `   `   `  ���� P  LAncientPanda_Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AncientPanda_LauncherAncientPanda_Launcher  &     AncientPanda_Launcher.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  Z VEO.WebBrowser.Wpf, Version=21.0.18.0, Culture=neutral, PublicKeyToken=e92353a6bf73fffc1eo(http://schemas.essentialobjects.com/wpf/  /local#clr-namespace:AncientPanda_Launcher  -     � WindowStartupLocation$  CenterScreen=� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � Title$ AncientPanda Games��$	��450��$	��800�� � 	Resources    TextBoxBaseControlTemplate �� 
TargetType ��Name �� CornerRadius ��Name	 �� Triggers
 P� Property P� Value �� Value �� 
TargetName �� Property��%  &	        �� " ��.R��� $ Bd��#��z��
���   �#��z��$ 10��._��� $ PART_ContentHost��	 P� 
 � {�$ False��. ��� # � ��$ Bd�� � ���� # � �� � ��P� 
 � ��$
 Auto��. ���  � ��$	 100��P� 
 � ��$
 Auto��. ���  � ��$ 20��.��� ����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Black��� $ 	#FF118AB2�� � Offset$ 1Q�� $ 	#FF073B4C��$ 0.004Q�.+��� -   $��
VersionTxt��$��Bottom=�$��Right=�$��20(�$��2.0.0��
�������$��0,0,3,0q�� -   $��DownloadBar�� � Value$ 50Q�$��228,381,35,63q�
�������$y�Hidden=����	#FFFFD166��Z� �� -   $��DownloadTxt��$��Right=�$y�Hidden=�$
��100%��$	��Top=�$��228,358,0,0q����	#FFF4F1DE J� RenderTransformOrigin$ 0.497,0.125&�$
��Bold"�$
��Left=�$	��537��$��23��$	��Top=��� -   $��DownloadAmount��$y�Hidden=�$��0 of 1024mb��$	��Top=�$��228,388,0,0q�$��Center=�$	��Top=����	#FFF4F1DE$ 0.497,0.125&�$��25��$��Light"�$
��Left=�$	��537��$��Poppins SemiBold)��� -    ��Name$ PlayBtn��$
��Left=�$��50��$��
17,381,0,0q�$	��Top=�$	��206�� �� RadiusX$ 25�� �� RadiusY$ 25������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF049E76��$ 1Q�� $ 	#FF06D6A0��$ 0Q��� -   $��StateTxt��$��Downloading��$��Center=�$��Center=�$��
17,381,0,0q�$	��Top=����	#FFF4F1DE$ 0.497,0.125&�$��50��$
��Bold"�$
��Left=�$	��206��$��Poppins Black)�$��23(��� -   $��DragBar��$
��Left=�$��30��$	��Top=�$	��804�����	#33000000A� -	    A�Name$ CloseOverride��$
��Left=�$��20��$��	770,5,0,0q�$	��Top=�$��20������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFB41919��� $ 	#FF971717��$ 1Q�A� -
   $ MinimizeOverride��$
��Left=�$��20��$��	741,5,0,0q�$	��Top=�$��20������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFADB419��� $ 	#FF8B9717��$ 1Q�� -   $��SettingsMenu��$y�Hidden=�$��-10,-13,10,13q�.+��� -   $ PlayBtn_Copy��$��10,43,584,212q����	#35000000�� $��Settings��$��Center=�$��Center=�$��	10,42,0,0q�$	��Top=����	#FFF4F1DE$ 0.497,0.125&�$��50��$
��Bold"�$
��Left=�$	��206��$��Poppins Black)�$��23(��� -   $��ipConfig��$y�Visible=�$
��Left=�$��255.255.255.255�� �� TextWrapping$
 Wrap=�$	��Top=�$��Center=�$��Center=�$	��154��#��[  $��24��$��
36,113,0,0q�$��Poppins)��� $��	Server Ip��$��Center=�$��Center=�$��0,0,684,337q�$��Bottom=����	#FFF4F1DE$ 0.497,0.125&�$��26��$
��Bold"�$��Poppins Black)�$��13(�$��Right=�$��80���� -   $��	LogoutVis��$	��Top=�$	��Top=�$��Logout��$y�Visible=�$
��Left=�$��Poppins)�$��
ExtraBlack"�$��
76,207,0,0q�$��69�����	#FFDA0B0B$��15(�$��28����Z� ��Z� �� -    ��Name$ ipConfig_Copy��$y�Visible=�$
��Left=�$�� ��$
 Wrap=�$	��Top=�$��Center=�$��Center=�$	��154��#��[  $��26��$��
36,167,0,0q�$��Poppins)��� $��	Arguments��$��Center=�$��Center=�$��
36,0,0,277q����	#FFF4F1DE$ 0.497,0.125&�$
��Bold"�$
��Left=�$��90��$��Poppins Black)�$��13(�$��30��$��Bottom=��� -   $��ConfigSett_Btn��$
��Left=�$��25��$��10,2,0,0q�$	��Top=�$��25��$ 
32.65,-0.2&�����  �� ImageSource$ /images/Avatar.png���� -    ��Name$ 	UserLabel��$�� ��$��Center=�$
��Left=�$��0,0,502,423q�
�������$��Poppins ExtraBold)�$
��Bold"�$��15(�$��Right=�$	��263��$��30��$��Bottom=���Z� ��Z� A� -   $��LaunchGitHub��$
��Left=�$��32��$��715,420,0,0q�$	��Top=�$��32������ $ /images/GitHub.png��� -   $��	ErrorGrid��$y�Hidden=�.+��� $��180,123,180,123q�$ 5��$ 5������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFEF4756��$ 0Q�� $ 	#FFF12A33��$ 1Q��� -   $ 	CancelBtn��$��Center=�$��29��$��	0,298,0,0q�$	��Top=�$	��440�����	#33000000$ 5��$ 5���� -   $��	CancelTxt��$��WARNING

��$��Center=�$��Center=�$��	0,141,0,0q�$	��Top=����	#FFF4F1DE$ 0.497,0.125&�$��50��$
��Bold"�$��Center=�$	��440��$��Poppins Black)�$��30(��� $��Center=�$��82��$��0,68,0,0q�$	��Top=�$��80������ $ /images/warning.png���� -   $��OkActionWarningBtn��$��OK��$��Center=�$��Center=�$��	0,296,0,0q�$	��Top=����	#FFF4F1DE$ 0.497,0.125&�$��29��$
��Bold"�$��Center=�$	��440��$��Poppins Black)�$��15(��� -   $��ErrorContentTxt��$��Center=� �� TextAlignment$ Center=�$��	0,202,0,0q�$a��[AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA.��$
��Wrap=�$	��Top=�$	��412��$��91��$��Poppins)�$��15(�
�������