   M S B A M L   `   `   `  ���� O  KVehicleFlowSimulator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:VaaaN.MLFF.VehicleFlowSimulatorVaaaN.MLFF.VehicleFlowSimulator  0    *VaaaN.MLFF.VehicleFlowSimulator.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ?local-clr-namespace:VaaaN.MLFF.VehicleFlowSimulator     -     � Title$!  MLFF Vehicle Flow Simulator��$	��482��$	��624�� � WindowStartupLocation$ CenterScreen=�.����  J� RenderTransformOrigin$ 0.503,0.499&�.+��� -    ��Name$ labelWebAPIAddress��$��API Address:�� �� Left$ 10��
 �� Top$ 16��$��Center=�$��80��$
��Bold"��� -    ��Name$ textBoxAPIAddress��$��23��$ 99�� �� TextWrapping$
 Wrap=�$��http://10.213.0.33:5555��$ 20��$	��221��$��Center=��� -   $ label1��$��Vehicle:��$ 10��$ 46��$��Center=�$
��Left=�$��80��$
��Bold"��� -    ��Name$ comboBoxVehicle��$ 99��$ 50��$	��460��$��Center=��� -   $ 
labelFront��$��
Front ANPR��$ 94��$	 156��$	��Top=�$
��Bold"��� -   $ labelCrossTalk1��$��CT Front��$	 214��$	 156��$	��Top=�$
��Bold"��� -   $ 	labelRear��$��	Rear ANPR��$	 333��$	 156��$ 0.455,0.423&�$	��Top=�$
��Bold"��� -	   $ labelCrossTalk��$��CT Rear��$	 495��$	 156��$	��Top=�$
��Bold"��� -
   	 ��Name$	 checkBoxFrontAttachement��$�� ��$ 99��$	 216��
 d� 	IsChecked$

 TrueY��� -   $&	  checkBoxCrossTalkAttachmentFront��$�� ��$	 219��$	 216��$

 TrueY��� -   $"	 checkBoxRearAttachementFront��$�� ��$	 339��$	 216��$

 TrueY��� -   $%	 checkBoxCrossTalkAttachmentRear��$�� ��$	 501��$	 216��$

 TrueY��� -    ��Name$ buttonSimulate��$��Simulate��$	 260��$	 351��$	��100���� -   $ labelCustomer��$��	Customer:��$ 10��$ 79��$��Center=�$
��Left=�$
��Bold"��� -   $ textBoxCustomer��$��23��$ 99��$�� ��$ 82��$	��301���� -   $ textBoxFrontOrder��$��23��$ 99��$
 Wrap=�$��1��$	 186��$��20��$	��Top=��� -   $  textBoxCrossTalkOrderFront��$��23��$	 219��$
 Wrap=�$��2��$	 186��$��20��$	��Top=��� -   $ textBoxRearOrder��$��23��$	 339��$
 Wrap=�$��3��$	 186��$��20��$	��Top=��� -   $ textBoxCrossTalkOrderRear��$��23��$	 501��$
 Wrap=�$��4��$	 186��$��20��$	��Top=��� -   $ labelVehicle��$��Vehicle:��$ 10��$	 115��$
��Bold"��� -   $ textBoxVehicle��$��23��$ 99��$�� ��$	 118��$	��301���� -   $ buttonGenerateNFHTML��$��Generate��$	 511��$	 416��$��75��$y�Visible=�{�.  �� -   $ buttonStartMLFF��$��
Start MLFF��$	 160��$	 415��$	��140��$y�Visible=�{�.  �� -   $ buttonStopMLFF��$��	Stop MLFF��$	 340��$	 415��$	��140��$y�Visible=�{�.  �� -   $ comboBoxFrontClass��$ 99��$	 276��$	��100��.@��� .��Not Detected�� .��two-wheeled�� .��small�� .��medium�� .��large�� -   $ comboBoxRearClass��$	 339��$	 276��$	��100��.@��� .��Not Detected�� .��two-wheeled�� .��small�� .��medium�� .��large�� -   $ textBoxNFFText��$��23��$ 99��$
 Wrap=�$�� ��$	 246��$	��100���� -   $ textBoxNFRText��$��23��$	 339��$
 Wrap=�$�� ��$	 246��$	��100���� -   $ label1_Copy��$��Packet:��$ 10��$	 156��$��Center=�$
��Left=�$��80��$
��Bold"��� -   $ label1_Copy1��$��Order:��$ 10��$	 182��$��Center=�$
��Left=�$��80��$
��Bold"��� -    $ label1_Copy2��$��Attach:��$ 10��$	 212��$��Center=�$
��Left=�$��80��$
��Bold"��� -!   $ label1_Copy3��$
��VRN:��$ 10��$	 243��$��Center=�$
��Left=�$
��Bold"�$��80���� -"   $ label1_Copy5��$��Class:��$ 10��$	 272��$��Center=�$
��Left=�$��80��$
��Bold"�