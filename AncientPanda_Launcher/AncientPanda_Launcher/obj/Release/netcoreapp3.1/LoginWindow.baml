   M S B A M L   `   `   `  ���� P  LAncientPanda_Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:AncientPanda_LauncherAncientPanda_Launcher  '    !AncientPanda_Launcher.LoginWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5local#clr-namespace:AncientPanda_Launcher     -     � WindowStartupLocation$  CenterScreen=� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � Title$ AncientLauncher Login��$	��450��$	��450�� � 	Resources    passwordbox �� 
TargetType ��Name �� CornerRadius J� SnapsToDevicePixels	 �� Triggers
 P� Property P� Value �� Value �� 
TargetName �� Property  TextBoxBaseControlTemplate ��Name��%;  &	        &	     �� " G�.R��� $ Bd��#��z��
���   �#��z��$ 10��._��� $��PART_ContentHost��# z 	 P� 
 � {�$ False��. ��� # � ��$ Bd�� � ���� # � �� � ��P� 
 � ��$
 Auto��. ���  � ��$	 100��P� 
 � ��$
 Auto��. ���  � ��$ 20���� " ��.R��� $ Bd��#��z��
���   �#��z��$ 10��._��� $ PART_ContentHost��	 P� 
 � {�$ False��. ��� # � ��$ Bd�� � ���� # � �� � ��P� 
 � ��$
 Auto��. ���  � ��$	 100��P� 
 � ��$
 Auto��. ���  � ��$ 20��.��� ����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Black��� $ 	#FF118AB2�� � Offset$ 1Q�� $ 	#FF073B4C��$ 0.004Q�.+��� -   $��DragBar��$��Center=�$��30��$	��Top=�$	��450�����	#33000000A� -    A�Name$ CloseOverride��$
��Left=�$��20��$��	427,5,0,0q�$	��Top=�$��20������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFB41919��� $ 	#FF971717��$ 1Q�A� -   $ MinimizeOverride��$
��Left=�$��20��$��	398,5,0,0q�$	��Top=�$��20������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFADB419��� $ 	#FF8B9717��$ 1Q��� -   $��UserTxt��$y�Visible=�$��Center=�$�� �� �� TextWrapping$
 Wrap=�$	��Top=�$��Center=�$��Center=�$	��277��#��[ $��43��$��	0,144,0,0q�$��Poppins)�$��20(���Z� �� $��
ExtraBlack"�$��
Username
��$
��Left=�$��33��$��
86,111,0,0q�$	��Top=�$	��122��$��Poppins)�$��20(�
��������� $
��Bold"�$��Ancient Panda
��$��Center=�$��63��$��0,30,0,0q�$��Center=�$��Center=�$	��Top=�$	��370��$��Poppins ExtraBold)�$��40(�
�������G� -    G�Name$ PassTxt��$y�Visible=�$��Center=� G� Password$  ��$	��Top=�$��Center=�$��Center=�$	��278��#��[  $��43��$��	0,230,0,0q�$��Poppins)�$��20(���Z� �� -   $��RememberCheck�� d� 	IsChecked$ FalseY�$��Remember Me��$��Center=�$��	0,286,0,0q�$	��Top=�$��Poppins)�
�������$��
UltraBlack"�$	��278��
��������� $��
ExtraBlack"�$��Password��$
��Left=�$��33��$��
86,195,0,0q�$	��Top=�$	��122��$��Poppins)�$��20(�
��������� -   $��ErrorTxt��$y�Hidden=�$��
ExtraBlack"�$$��Incorrect Password or Username��$��Center=�$��Center=�$��Center=�$��33��$��	0,381,0,0q�$	��Top=�$	��450��$��Poppins)�$��20(����	#FFEF476F�� -	    ��Name$ PlayBtn��$��Center=�$��50��$��	0,326,0,0q�$	��Top=�$	��277�� �� RadiusX$ 25�� �� RadiusY$ 25������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF049E76��$ 1Q�� $ 	#FF06D6A0��$ 0Q��� -
   $��Login��$��Center=�$��Center=�$��	0,326,0,0q�$	��Top=����	#FFF4F1DE J� RenderTransformOrigin$ 0.497,0.125&�$��50��$
��Bold"�$��Center=�$	��278��$��Poppins Black)�$��23(�