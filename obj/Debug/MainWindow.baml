   M S B A M L   `   `   `  ����  CoordinateMappingBasics-WPFA    ;Microsoft.Samples.Kinect.CoordinateMappingBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Coordinate Mapping Basics��5   	   $	��768��5   	   $
��1366��6    � WindowStyle$
 None=�5   	    � WindowState$ 	Maximized=�5   	    � 	Resources 5	         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  ScreenshotButton �� 
TargetType	 �� Orientation
 ��Name �� TextAlignment ��Name �� Triggers P� Property P� Value �� 
TargetName��%h  &	        &	 #     &	 F     '�� i     &	 �     7  7  �� 5
   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $	 
Horizontal=�6)   
������ .+�5   "   �� $
 ScreenshotText��6,   $��Capture��6D   $
 Left=�6S   $��Center=�6i   8��  $��15(�6�   � 5   "   $��9,0,0,0q�6'   .+�5   &   �� $ ScreenshotNormal��6,   $!��Images\ScreenshotNormal.png��6F   $
��None=�6k   $��Center=�6z   �� 5   &   $ ScreenshotHover��6,   $ ��Images\ScreenshotHover.png��6E   $
��None=�6i   $��Center=�6x   $y�	Collapsed=�6�   5   #   5      5       5      P� 5       �@J�IsMouseOver . . �5   "   �� $ ScreenshotNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5    "   $ ScreenshotHover��6O    � y�$ Visible=�6?   6O   �� 5!   "   8  $ ScreenshotText��6h    � ��5"      5$      5&      5'      5(      .��5)      � $��0 0 0 0q�6    � RowDefinitions 5*   
   �� 5+      $
��Auto �6   �� 5,      $��* �6   �� 5-      $
��Auto �6   �� 5.      $
��Auto �6   .+�52   
   � -   5         �Name$ background_select��52      $y�Hidden=�6*   $��1��6>   .+�53      �� -   5        $
 img1��53      $
��Fill=�6"   $��64,129,938,384q�6P   �� 54      -   5        $
 img2��54      $
��Fill=�6"   $��64,394,938,119q�6P   �� 55      -   5        $
 img3��55      $
��Fill=�6"   $��513,129,489,384q�6P   �� 56      -   5        $
 img4��56      $
��Fill=�6"   $��513,394,489,119q�6P   �� 57      -   5        $
 img5��57      $
��Fill=�6"   $��946,129,56,384q�6P   �� 58      -   5        $
 img6��58      $
��Fill=�6"   $��946,394,56,119q�6P   59      � 5;   
   -	   5        $ 	welcomeUI��5;      $y�Visible=�6"   $��1��67   .+�5<      �� -
   5        $ 
welcomeImg��5<      $
��Fill=�6(   �� 5=      -   5        $��
Click here��5=      $��36(�6*   $��Center=�68   $��Bottom=�6V   $��	0,0,0,100q�6q   $	��250��6�   $��64��6�   6�   5>      � 5?   
   -   5        $ 
thankyouUI��5?      $y�Hidden=�6#   $��1��67   .+�5@      �� -   5        $ thankyouImg��5@      $
��Fill=�6)   5A      (� 5C   
   -   5         (�Name$ 
MainScreen��5C      $��1��6&   $��Center=�63   $y�Hidden=�6P   .��5D      � -   5        $��CompositeImage��5D      $	��423��6)   .+�5E      �� -   5        $ BackgroundIMG��5E      $
��Fill=�6/   �� 5F      $��UniformToFill=�67   ��6   �� *ImageSource+67   5G      5H      � 5J   
   -   5        $ 	pageTagID��5J      $��1��6"   $y�Hidden=�6/   5L      �� 5N   
   -   5         ��Name$ Screenshot_btn��5N      $��2��6)   #��[ 66   $y�Hidden=�6`   $��
Screenshot��6t   $
��Auto��6�   $��Center=�6�   $��Center=�6�    J� RenderTransformOrigin$ 1.892,-2.219&�6�   �� 5O   
   -   5        $ Back_btn��5O      $��1��6B   $y�Hidden=�6O   $
��Left=�6c   $
��Back��6~   $��44(�6�   $
��Auto��6�   $��
25,653,0,0q�6�   $	��128��6�   6�   �� 5P   
   -   5        $��	statusBar��5P   ?   $��3��6   $��Stretch=�6!   $��Bottom=�6P   
�������#��[  6~   .@�5Q      �� ��6   �� *
StatusText+5R      5T      5U      