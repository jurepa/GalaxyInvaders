﻿#pragma checksum "C:\Users\pjarana\Desktop\GitHub\GalaxyInvaders\GalaxyInvaders\GalaxyInvaders\SpaceInvaders\Game.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "38D51EBDEAC7B0A4E9256762F05C9638"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpaceInvaders
{
    partial class Game : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // Game.xaml line 23
                {
                    this.grid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    ((global::Windows.UI.Xaml.Controls.Grid)this.grid).Loaded += this.allowfocus_Loaded;
                }
                break;
            case 2: // Game.xaml line 32
                {
                    this.border = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 3: // Game.xaml line 33
                {
                    this.canvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 4: // Game.xaml line 37
                {
                    this.imagenPausa = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 5: // Game.xaml line 38
                {
                    this.player = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 6: // Game.xaml line 25
                {
                    this.vida1 = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 7: // Game.xaml line 26
                {
                    this.vida2 = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 8: // Game.xaml line 27
                {
                    this.vida3 = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 9: // Game.xaml line 28
                {
                    this.txtpuntuacion = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10: // Game.xaml line 29
                {
                    this.puntuacion = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

