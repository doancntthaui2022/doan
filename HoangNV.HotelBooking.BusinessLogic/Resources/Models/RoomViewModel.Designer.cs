﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HoangNV.HotelBooking.BusinessLogic.Resources.Models {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RoomViewModel {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RoomViewModel() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HoangNV.HotelBooking.BusinessLogic.Resources.Models.RoomViewModel", typeof(RoomViewModel).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loại phòng.
        /// </summary>
        internal static string Display_RoomTypeName {
            get {
                return ResourceManager.GetString("Display_RoomTypeName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {PropertyName} đã tồn tại..
        /// </summary>
        internal static string E_A_004_01 {
            get {
                return ResourceManager.GetString("E_A_004_01", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Vui lòng nhập {PropertyName}..
        /// </summary>
        internal static string E_A002_02 {
            get {
                return ResourceManager.GetString("E_A002_02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {PropertyName} không được vượt quá {MaxLength} ký tự..
        /// </summary>
        internal static string E_C_008_02 {
            get {
                return ResourceManager.GetString("E_C_008_02", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không được bỏ trống loại phòng.
        /// </summary>
        internal static string E_R00_001 {
            get {
                return ResourceManager.GetString("E_R00_001", resourceCulture);
            }
        }
    }
}
