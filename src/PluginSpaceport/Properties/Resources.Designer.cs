﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PluginSpaceport.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PluginSpaceport.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Jason (Null) Spafford.
        /// </summary>
        internal static string PluginAuthor {
            get {
                return ResourceManager.GetString("PluginAuthor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A spaceport IDE.
        /// </summary>
        internal static string PluginDescription {
            get {
                return ResourceManager.GetString("PluginDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 7b05efcc-d6e8-49c4-85b9-85ae9e22ead9.
        /// </summary>
        internal static string PluginGuid {
            get {
                return ResourceManager.GetString("PluginGuid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://spaceport.io.
        /// </summary>
        internal static string PluginHelp {
            get {
                return ResourceManager.GetString("PluginHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Spaceport Plugin.
        /// </summary>
        internal static string PluginName {
            get {
                return ResourceManager.GetString("PluginName", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap spaceportIcon {
            get {
                object obj = ResourceManager.GetObject("spaceportIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
